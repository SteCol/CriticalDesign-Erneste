using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EventManager_B : MonoBehaviour
{

    [Header("General")]
    public GetCharacters_B getCharacter_B;

    [Header("Periods")]
    public int activePeriod;
    private int prevPeriod;

    [Header("Periods")]
    public List<Character> characters;

    [Header("Chat")]
    //public List<Character> friends;
    public Dropdown friendsChat;
    public Question activeQuestion;
    public List<Question> possibleQuestions;


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
        GetChat();
        UpdatePeriod();
    }

    void UpdatePeriod()
    {
        if (prevPeriod != activePeriod)
        {
            print("Updating periods from " + prevPeriod + " to " + activePeriod);
            GetStatusUpdates();
            GenerateStatusUpdates();
            prevPeriod = activePeriod;
            GenerateFriendsListOPtions();

        }
    }

    void GetChat()
    {
        foreach (Character c in getCharacter_B.characters)
        {
            foreach (Period p in c.periods)
            {
                foreach (Question q in p.questions)
                {
                    //print("Found Q " + q.Q);
                    //Each characters needs a start question, active question and past questions.
                    //Q1 is the starting question
                    //past questions are all the questions that have been answered.

                    //Let's do this.

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

    void GenerateFriendsListOPtions()
    {
        friendsChat.ClearOptions();

        foreach (Character c in getCharacter_B.characters)
        {
            //Generate an option to chat with each character.
            friendsChat.options.Add(new Dropdown.OptionData() { image = c.profilePic, text = c.name });
        }

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
                                print("CommentorName " + commentorName);

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
