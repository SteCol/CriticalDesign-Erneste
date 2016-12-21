using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EventManager_B : MonoBehaviour
{

    [Header("General")]
    public GetCharacters_C getCharacter_B;
    public Character activeCharacter;
    public Character oldActiveCharacter;
    public PlayerProfile playerProfile;
    private float timer;

    [Header("Debugging")]
    public bool debug;
    public bool inculeIdentifiers;

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
    public ScrollRect myScrollRect;
    public Dropdown answerSelect;
    public GameObject questionBubblePrefab;
    public GameObject answerBubblePrefab;

    public Question activeQuestion;
    public List<Question> possibleQuestions;
    public List<Question> pastQuestions;
    public List<GameObject> pastQA;

    public bool UpdateEverything;


    [Header("Notifications")]
    public GridLayoutGroup feed;
    public GameObject statusUpdatePrefab;
    public GameObject commentPrefab;
    public List<StatusUpdate> statusUpdatesToShow;


    // Use this for initialization
    void Start()
    {
        Debug1("//////////////////////////////////////////////////////////////////////// EVENT MANAGER START ////////////////////////////////////////////////////////////////////////");

        activePeriod = 1;
        UpdateChat();
        GetStatusUpdates();
        GenerateStatusUpdates();

        Debug1("////////////////////////////////////////////////////////////////////////");
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePeriod();

        if (UpdateEverything == true)
        {
            GetQuestions();
            GenerateQuestions();

            ClearChatBubbles();
            if (possibleQuestions.Count > 0)
            {
                InstantiateChatBubbles(possibleQuestions[0]);
            }

            GenerateOptions(activeQuestion);

            GetStatusUpdates();
            GenerateStatusUpdates();

            myScrollRect.verticalNormalizedPosition = 0.0f;

            UpdateEverything = false;
        }
    }


    public void UpdateChat()
    {
        DebugSplit("UPDATING CHAT");
        UpdateEverything = true;
    }

    void UpdatePeriod()
    {
        if (prevPeriod != activePeriod)
        {
            GetStatusUpdates();
            GenerateStatusUpdates();
            if (possibleQuestions.Count > 0)
            {
                DebugSplit2("Updating periods from " + prevPeriod + " to " + activePeriod);

                
                UpdateEverything = true;
            }

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
        DebugSplit2("GenerateFriendsListOPtions");
        friendsChat.ClearOptions();

        foreach (Character c in getCharacter_B.characters)
        {
            //Generate an option to chat with each character.
            string newName = c.name.Replace("name: ", "");


            friendsChat.options.Add(new Dropdown.OptionData() { image = c.profilePic, text = newName });
        }

        GetQuestions();
        GenerateQuestions();

        friendsChat.value = 1;
        friendsChat.value = 0;

        if (possibleQuestions.Count > 0)
            InstantiateChatBubbles(possibleQuestions[0]);
    }

    public void GetQuestions()
    {
        DebugSplit2("GetQuestions");
        activeCharacter = getCharacter_B.characters[friendsChat.value];

        int c = friendsChat.value;

        foreach (Period p in getCharacter_B.characters[c].periods)
        {
            string[] periodIndexArray = p.codeName.Split('_');
            string periodIndex = periodIndexArray[1];

            if (int.Parse(periodIndex) == activePeriod)
            {
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

        DebugSplit2("GenerateQuestions");
        if (possibleQuestions.Count == 0)
        {
            Debug2("Possible questions is empty");
            activeQuestion = null;
            answerSelect.ClearOptions();
        }
        else
        {
            foreach (Question q in possibleQuestions)
            {
                Debug2("Checking question " + q.Q + " | send: " + q.send + " | checkForAnswered: " + q.checkForAnswered + " | followThrough: " + q.followThrough + " | hasBeenAnswered: " + q.answered);

                if (q.Q.Contains("+Q1 ") && q.answered != true) //Starting question.
                {
                    Debug3("!! Found '+Q1 ' in " + q.Q);
                    activeQuestion = q;
                    activeQuestion.send = true;

                    Debug6("SET ACTIVE QUESTION TO " + q.Q);
                    break;
                }
                else if (q.answered == false && q.send == true) {
                    activeQuestion = q;
                    activeQuestion.send = true;

                    Debug6("SET ACTIVE QUESTION TO " + q.Q);

                    if (q.followThrough != 666) {
                        activeQuestion.answered = true;
                        foreach (Question que in possibleQuestions) {
                            if (que.Q.Contains("+Q" + activeQuestion.followThrough + " ")) {
                                activeQuestion = que;
                                activeQuestion.send = true;
                                Debug6("SET ACTIVE QUESTION TO " + que.Q);
                                GenerateQuestions();
                                break;
                            }
                        }
                    }

                    break;
                }
                else
                {
                    Debug4("NOPE");
                }
            }
        }
        GenerateOptions(activeQuestion);

    }

    void GenerateOptions(Question q)
    {
        DebugSplit2("GenerateOptions");
        answerSelect.ClearOptions();
        if (q != null)
        {
            if (q.answers != null)
            {
                if (q.answers.Count > 0)
                {
                    foreach (Answer a in q.answers)
                    {
                        if (!inculeIdentifiers)
                        {
                            //Clean sentence
                            string sentence = a.A;
                            string[] sentenceArray = sentence.Split(' ');
                            string newSentence = "";

                            foreach (string s in sentenceArray)
                            {
                                if (s.Contains("+A") || s.Contains("["))
                                {
                                    //print("Sorry, s contains bad char");
                                }
                                else {
                                    newSentence = newSentence + s + " ";
                                }
                            }

                            answerSelect.options.Add(new Dropdown.OptionData { text = newSentence });
                        }
                        else {
                            answerSelect.options.Add(new Dropdown.OptionData { text = a.A });
                        }



                    }
                }
            }
        }
    }

    void ClearChatBubbles()
    {
        DebugSplit("ClearCHatBubbles");
        foreach (GameObject cb in GameObject.FindGameObjectsWithTag("ChatBubble"))
        {
            Destroy(cb);
        }
    }

    public void SendButton()
    {

        
        DebugSplit("SendButton");
        for (int a = 0; a < activeQuestion.answers.Count; a++)
        {
            if (answerSelect.value == a)
            {

                //doTheValueStuff.
                if (activeQuestion.answers[a].valueChange != 666)
                {
                    print("ADDING VALUE " + activeQuestion.answers[a].valueChange + " WOOOOOOOOOO!!!!");

                    playerProfile.value = playerProfile.value + activeQuestion.answers[a].valueChange;
                    playerProfile.AddValue(activeQuestion.answers[a].valueChange);
                    playerProfile.AddValue(playerProfile.value);
                    playerProfile.UpdateInfo();

                    //playerProfile.addNewValue = true;
                }

                activeQuestion.answers[a].wasUsed = true;
                activeQuestion.answered = true;

                //Get the new question
                foreach (Question q in possibleQuestions)
                {
                    if (q.Q.Contains("+Q" + activeQuestion.answers[a].nextQuestion + " "))
                    {
                        activeQuestion.answered = true;
                        activeQuestion.checkForAnswered = false;
                        activeQuestion.send = true;

                        activeQuestion = q;
                        Debug6("Set ActiveQuestion to " + q.Q);
                        GenerateOptions(activeQuestion);
                        break;
                    }
                }

                
            }
        }

        GetQuestions();
        GenerateOptions(activeQuestion);

        if (possibleQuestions.Count > 0)
            InstantiateChatBubbles(possibleQuestions[0]);

        if (activeQuestion.valueChange != 666)
        {
            print("ADDING VALUE " + activeQuestion.valueChange + " WOOOOOOOOOO!!!!");

            playerProfile.value = playerProfile.value + activeQuestion.valueChange;
            playerProfile.AddValue(activeQuestion.valueChange);
            playerProfile.AddValue(playerProfile.value);
            //playerProfile.UpdateInfo();

            //playerProfile.addNewValue = true;
        }

    }

    void InstantiateChatBubbles(Question _question)
    {
        Debug3("Instantiate Question" + _question.Q);
        //print("Instantiating" + q.Q + " from pastQuestions");

        GameObject chatBubble = Instantiate(questionBubblePrefab, transform.position, transform.rotation);
        chatBubble.transform.SetParent(chatWindow.transform);
        chatBubble.transform.localScale = new Vector3(1, 1, 1);
        chatBubble.name = _question.Q;

        if (!inculeIdentifiers)
        {
            //Clean sentence
            string sentence = _question.Q;
            string[] sentenceArray = sentence.Split(' ');
            string newSentence = "";

            foreach (string s in sentenceArray) {
                if (s.Contains("+Q") || s.Contains("["))
                {
                    //print("Sorry, s contains bad char");
                }
                else {
                    newSentence = newSentence + s + " ";
                }
            }

            chatBubble.GetComponent<ChatBubbleScript>().content.text = newSentence;
        }
        else {
            chatBubble.GetComponent<ChatBubbleScript>().content.text = _question.Q;
        }
        chatBubble.GetComponent<ChatBubbleScript>().profilePic.sprite = activeCharacter.profilePic;

        if (_question.followThrough != 666)
        {
            foreach (Question que in possibleQuestions)
                if (que.Q.Contains("+Q" + _question.followThrough + " "))
                {
                    activeQuestion = que;

                    InstantiateChatBubbles(que);
                    break;
                }
        }
        else if (_question.followThrough == 666)
        {
            foreach (Answer a in _question.answers)
            {
                if (a.wasUsed)
                {
                    InstantiateAnswerbubble(a);
                    foreach (Question que in possibleQuestions)
                        if (que.Q.Contains("+Q" + a.nextQuestion.ToString() + " "))
                        {
                            activeQuestion = que;
                            InstantiateChatBubbles(que);
                            break;
                        }
                    break;
                }
            }
        }
    }

    void InstantiateAnswerbubble(Answer _answer)
    {
        Debug6("Instantiate Answer " + _answer.A);
        GameObject chatBubble = Instantiate(answerBubblePrefab, transform.position, transform.rotation);
        chatBubble.transform.SetParent(chatWindow.transform);
        chatBubble.transform.localScale = new Vector3(1, 1, 1);
        chatBubble.name = _answer.A;

        if (!inculeIdentifiers)
        {
            //Clean sentence
            string sentence = _answer.A;
            string[] sentenceArray = sentence.Split(' ');
            string newSentence = "";

            foreach (string s in sentenceArray)
            {
                if (s.Contains("+A") || s.Contains("["))
                {
                    //print("Sorry, s contains bad char");
                }
                else {

                    newSentence = newSentence + s + " ";
                }
            }

            chatBubble.GetComponent<ChatBubbleScript>().content.text = newSentence;
        }
        else {
            chatBubble.GetComponent<ChatBubbleScript>().content.text = _answer.A;

        }

        chatBubble.GetComponent<ChatBubbleScript>().profilePic.sprite = playerProfile.profilePic;
    }

    void compareQuestions(Question q1, Question q2)
    {
        print("Comparing " + q1.Q + " and " + q2.Q);

    }

    void GetStatusUpdates()
    {
        DebugSplit("GetStatusUpdates");
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
        print("GenerateStatusUpdates");
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


                            if (!inculeIdentifiers)
                            {
                                //Clean sentence
                                string sentence = su.content;
                                string[] sentenceArray = sentence.Split(' ');
                                string newSentence = "";

                                foreach (string s in sentenceArray)
                                {
                                    if (s.Contains("+SU") || s.Contains("["))
                                    {
                                        //print("Sorry, s contains bad char");
                                    }
                                    else {
                                        newSentence = newSentence + s + " ";
                                    }
                                }

                                statusUpdate.GetComponent<SUScript>()._content.text = newSentence;
                            }
                            else {
                                statusUpdate.GetComponent<SUScript>()._content.text = su.content;

                            }

                            //statusUpdate.GetComponent<SUScript>()._content.text = su.content;

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
        print("MoveForwardPeriod");
        activePeriod++;
    }

    public void MoveBackPeriod()
    {
        print("MoveBackPeriod");
        activePeriod--;

    }

    public void Debug1(string message)
    {
        if (debug)
            print(message);
    }

    public void Debug2(string message)
    {
        if (debug)
            print("     |--- " + message);
    }

    public void Debug3(string message)
    {
        if (debug)
            print("             |--- " + message);
    }

    public void Debug4(string message)
    {
        if (debug)
            print("                     |--- " + message);
    }

    public void Debug5(string message)
    {
        if (debug)
            print("                             |--- " + message);
    }

    public void Debug6(string message)
    {
        if (debug)
            print("                                       |--- " + message);
    }

    public void DebugSplit(string message)
    {
        if (debug)
            print("-------------------------" + message + "--------------------------------");
    }

    public void DebugSplit2(string message)
    {
        if (debug)
            print("==========" + message + "==============");
    }
}
