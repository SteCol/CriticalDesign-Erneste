using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EventManager_B : MonoBehaviour
{

    [Header("General")]
    public GetCharacters_B getCharacter_B;
    public Character activeCharacter;

    [Header("Periods")]
    public int activePeriod;
    private int prevPeriod;

    [Header("Periods")]
    public List<Character> characters;

    [Header("Chat Friends")]
    //public List<Character> friends;
    public bool generateFriendsDropdown;
    public Dropdown friendsChat;

    [Header("Chat Actual")]
    public GameObject chatWindow;
    public Dropdown answerSelect;
    public GameObject chatBubblePrefab;
    public Question activeQuestion;
    public List<Question> possibleQuestions;
    public List<Question> pastQuestions;
    public List<GameObject> pastQA;


    [Header("Notifications")]
    public GridLayoutGroup feed;
    public GameObject statusUpdatePrefab;
    public GameObject commentPrefab;
    public List<StatusUpdate> statusUpdatesToShow;


    // Use this for initialization
    void Start()
    {
        activePeriod = 1;

    }

    // Update is called once per frame
    void Update()
    {
        //UpdateQuestionSequence();
        GenerateQuestions();
        UpdatePeriod();

        ClearChatBubbles();
        if (possibleQuestions.Count > 0)
            InstantiateChatBubbles(possibleQuestions[0]);
    }

    void UpdatePeriod()
    {
        if (prevPeriod != activePeriod)
        {
            print("Updating periods from " + prevPeriod + " to " + activePeriod);
            GetStatusUpdates();
            GenerateStatusUpdates();
            GetQuestions();
            GenerateQuestions();
            if (possibleQuestions.Count > 0)
                InstantiateChatBubbles(possibleQuestions[0]);

            bool periodExists = false;

            foreach (Period p in activeCharacter.periods)
            {
                if (p.codeName.Contains("Period_" + activePeriod))
                {
                    periodExists = true;
                    break;
                }
            }

            if (!periodExists)
                possibleQuestions = new List<Question>();


            prevPeriod = activePeriod;
        }

        if (generateFriendsDropdown && getCharacter_B.GetCharactersComplete)
        {
            GenerateFriendsListOptions();
            GenerateQuestions();
            generateFriendsDropdown = false;
        }

    }

    void UpdateQuestionSequence()
    {
        foreach (Character c in getCharacter_B.characters)
        {
            foreach (Period p in c.periods)
            {
                foreach (Question q in p.questions)
                {
                    //gets the pastQA.
                    if (q.Q.Contains("+Q1"))
                    {
                        q.send = true;
                    }

                    if (q.send == true && q.followThrough != 666)
                    {
                        q.answered = true;
                        foreach (Question q2 in p.questions)
                        {
                            if (q2.Q.Contains("+Q" + q.followThrough))
                            {
                                q2.send = true;


                            }
                        }
                    }
                }
            }
        }
    }

    void GenerateFriendsListOptions()
    {
        friendsChat.ClearOptions();

        foreach (Character c in getCharacter_B.characters)
        {
            //Generate an option to chat with each character.
            friendsChat.options.Add(new Dropdown.OptionData() { image = c.profilePic, text = c.name });
        }

        friendsChat.value = 1;
        friendsChat.value = 0;

        if (possibleQuestions.Count > 0)
            InstantiateChatBubbles(possibleQuestions[0]);


    }

    public void GetQuestions()
    {
        activeCharacter = getCharacter_B.characters[friendsChat.value];

        int c = friendsChat.value;
        //print("Fetching questions for " + getCharacter_B.characters[c].name);
       

        foreach (Period p in getCharacter_B.characters[c].periods)
        {
            string[] periodIndexArray = p.codeName.Split('_');
            string periodIndex = periodIndexArray[1];

            if (int.Parse(periodIndex) == activePeriod)
            {
                //foreach (Question q in p.questions)
                //   print("Fetsching question " + q.Q);
                possibleQuestions = p.questions;
            }
        }
        if (getCharacter_B.characters[c].periods.Count == 0)
        {
            possibleQuestions = new List<Question>();
        }

    }

    public void GenerateQuestions()
    {

        if (possibleQuestions.Count == 0)
        {
            activeQuestion = null;
            answerSelect.ClearOptions();
        }

        foreach (Question q in possibleQuestions)
        {

            //bool hasBeenAnswered = false;

            if (q.Q.Contains("+Q1 ") && q.answered != true) //Starting question.
            {
                activeQuestion = q;
                //answerSelect.captionText.text = q.Q;
                answerSelect.ClearOptions();
                foreach (Answer a in q.answers)
                    answerSelect.options.Add(new Dropdown.OptionData { text = a.A });

                activeQuestion.send = true;
            }
            else if (activeQuestion.answered && activeQuestion.followThrough != 666)
            {
                if (q.Q.Contains("+Q" + activeQuestion.followThrough + " "))
                {
                    activeQuestion = q;
                    activeQuestion.send = true;
                    activeQuestion.answered = true;

                    answerSelect.ClearOptions();
                    foreach (Answer a in q.answers)
                        answerSelect.options.Add(new Dropdown.OptionData { text = a.A });

                }
            }
            else if (activeQuestion.answered && activeQuestion.followThrough == 666)
            {
                answerSelect.ClearOptions();
                foreach (Answer a in q.answers)
                    answerSelect.options.Add(new Dropdown.OptionData { text = a.A });
            }

            if (activeQuestion.checkForAnswered && activeQuestion.followThrough != 666)
            {
                //print("waiting on answer");
                pastQuestions.Add(activeQuestion);
                activeQuestion.answered = true;
                activeQuestion.checkForAnswered = false;

            }

            //answerSelect.ClearOptions();
            //foreach (Answer a in q.answers)
            //    answerSelect.options.Add(new Dropdown.OptionData { text = a.A });
        }

    }

    void ClearChatBubbles()
    {
        foreach (GameObject cb in GameObject.FindGameObjectsWithTag("ChatBubble"))
        {
            Destroy(cb);
        }
    }



    void InstantiateChatBubbles(Question q)
    {

        //print("Instantiating" + q.Q + " from pastQuestions");
        GameObject chatBubble = Instantiate(chatBubblePrefab, transform.position, transform.rotation);
        chatBubble.transform.SetParent(chatWindow.transform);
        chatBubble.transform.localScale = new Vector3(1, 1, 1);
        chatBubble.name = q.Q;
        chatBubble.GetComponent<ChatBubbleScript>().content.text = q.Q;
        chatBubble.GetComponent<ChatBubbleScript>().profilePic.sprite = activeCharacter.profilePic;


        if (q.followThrough != 666)
        {
            foreach (Question que in possibleQuestions)
                if (que.Q.Contains("+Q" + q.followThrough + " "))
                {
                    InstantiateChatBubbles(que);
                }
        }
        else if (q.followThrough == 666)
        {
            foreach (Answer a in q.answers)
            {
                if (a.wasUsed)
                {
                    foreach (Question que in possibleQuestions)
                        if (que.Q.Contains("+Q" + a.nextQuestion.ToString() + " "))
                        {
                            InstantiateChatBubbles(que);
                        }
                }
            }
        }


    }

    void compareQuestions(Question q1, Question q2)
    {
        print("Comparing " + q1.Q + " and " + q2.Q);

    }

    void GetStatusUpdates()
    {
        statusUpdatesToShow.Clear();
        foreach (Character c in getCharacter_B.characters)
        {
            foreach (Period p in c.periods)
            {
                //Get Period Index.
                string[] periodIndexArray = p.codeName.Split('_');
                string periodIndex = periodIndexArray[1];

                if (int.Parse(periodIndex) == activePeriod)
                {
                    foreach (StatusUpdate su in p.statusUpdates)
                    {
                        statusUpdatesToShow.Add(su);
                    }
                }
            }
        }
    }

    void GenerateStatusUpdates()
    {
        foreach (GameObject su in GameObject.FindGameObjectsWithTag("StatusUpdate"))
        {
            Destroy(su);
        }

        //compare SU in the list to SU in GetCharacters_B
        //This is to get the poster info
        foreach (Character c in getCharacter_B.characters)
        {
            foreach (Period p in c.periods)
            {
                foreach (StatusUpdate su in p.statusUpdates)
                {
                    foreach (StatusUpdate suts in statusUpdatesToShow)
                    {
                        if (suts.content == su.content)
                        {
                            GameObject statusUpdate = (GameObject)Instantiate(statusUpdatePrefab, feed.transform.position, feed.transform.rotation);
                            statusUpdate.transform.SetParent(feed.transform);
                            statusUpdate.name = su.content;

                            //Get poster name
                            string[] posterNameArray = c.name.Split(' ');
                            string posterName = posterNameArray[1];

                            statusUpdate.GetComponent<SUScript>()._name.text = posterName;
                            statusUpdate.GetComponent<SUScript>()._profilePic.sprite = c.profilePic;
                            statusUpdate.GetComponent<SUScript>()._content.text = su.content;

                            statusUpdate.transform.localScale = new Vector3(1, 1, 1);

                            foreach (Comment com in su.comments)
                            {
                                GameObject comment = (GameObject)Instantiate(commentPrefab, statusUpdate.GetComponent<SUScript>()._commentSection.transform.position, statusUpdate.GetComponent<SUScript>()._commentSection.transform.rotation);
                                comment.name = com.content;
                                comment.transform.SetParent(statusUpdate.GetComponent<SUScript>()._commentSection.transform);

                                comment.GetComponent<CScript>()._content.text = com.content;

                                //Get commentor profilepic.
                                string[] commentorNameArray = com.content.Split(' ');
                                string commentorName = commentorNameArray[1].Replace("[", "");
                                commentorName = commentorName.Replace("]", "");
                                //print("CommentorName " + commentorName);

                                //Get profilePic
                                foreach (Character cha in getCharacter_B.characters)
                                {
                                    if (cha.name.Contains(commentorName))
                                    {
                                        comment.GetComponent<CScript>()._profilePic.sprite = cha.profilePic;
                                        break;
                                    }
                                    else
                                    {
                                        foreach (Sprite pic in getCharacter_B.profilePics)
                                        {
                                            if (pic.name.Contains("Missing"))
                                            {
                                                c.profilePic = pic;
                                                break;
                                            }
                                        }
                                    }
                                }
                                comment.transform.localScale = new Vector3(1, 1, 1);
                            }
                        }
                    }
                }
            }
        }
    }

    public void MoveForwardPeriod()
    {
        activePeriod++;
    }

    public void MoveBackPeriod()
    {
        activePeriod--;

    }
}
