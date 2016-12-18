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
        UpdatePeriod();
    }

    void UpdatePeriod()
    {
        if (prevPeriod != activePeriod)
        {
            print("Updating periods from " + prevPeriod + " to " + activePeriod);
            GetChat();
            GetStatusUpdates();
            GenerateStatusUpdates();
            prevPeriod = activePeriod;
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
                }
            }
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

                            foreach (Comment com in su.comments) {
                                GameObject comment = (GameObject)Instantiate(commentPrefab, statusUpdate.GetComponent<SUScript>()._commentSection.transform.position, statusUpdate.GetComponent<SUScript>()._commentSection.transform.rotation);
                                comment.transform.SetParent(statusUpdate.GetComponent<SUScript>()._commentSection.transform);
                                comment.name = com.content;

                                comment.transform.localScale = new Vector3(1, 1, 1);
                                comment.GetComponent<CScript>()._content.text = com.content;
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
