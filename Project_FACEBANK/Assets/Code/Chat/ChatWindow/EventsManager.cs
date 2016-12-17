using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EventsManager : MonoBehaviour
{

    public Canvas canvas;

    [Header("Activate Events")]
    public bool recieveMessage;
    public bool updateStatus;

    [Header("Set Events")]
    public string characterName;
    public string chatMessage;
    public string statusUpdate;

    [Header("")]
    public GetCharacters_B getCharacters;
    public NotificationManager notificationManager;

    /*
    [Header("Chat Manager")]
    public List<ChatBubble> activeQuestions;
    public List<ChatBubble> pastQuestions;
    public GameObject chatBubble;

    public GameObject[] chatBubbleArray;

    public GameObject chatWindow;
    public float spacing;
    public Vector2 offset;
    */




    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (recieveMessage)
        {


            //ExecuteRecieveMessage();
            recieveMessage = false;

            //CheckForDuplicates();

        }

        if (updateStatus)
        {
            ExecuteUpdateStatus();
            updateStatus = false;
        }

    }

    /*
    public void ExecuteRecieveMessage()
    {
        for (int c = 0; c < getCharacters.characters.Count; c++)
        {
            for (int q = 0; q < getCharacters.characters[c].questions.Count; q++)
            {
                if (getCharacters.characters[c].name.Contains(characterName) && getCharacters.characters[c].questions[q].Q.Contains(chatMessage) && getCharacters.characters[c].questions[q].send != true)
                {
                    chatBubbleArray = GameObject.FindGameObjectsWithTag("ChatBubble");
                    foreach (GameObject g in chatBubbleArray)
                        DestroyImmediate(g);

                    foreach (ChatBubble i in activeQuestions)
                        pastQuestions.Insert(0, i);
                    activeQuestions.Clear();
                    getCharacters.characters[c].questions[q].send = true;

                    print("Recieved Message from  " + getCharacters.characters[c].name + ": " + getCharacters.characters[c].questions[q].Q);
                    activeQuestions.Insert(activeQuestions.Count, new ChatBubble(getCharacters.characters[c].questions[q].Q));
                    //activeQuestions.Add( new ChatBubble(getCharacters.characters[c].questions[q].Q));
                    UpdateActiveQuestion(c, q);
                    checkFollowthrough(c, q);

                }
            }
        }
    }

    public void checkFollowthrough(int c, int q)
    {
        print("Checking FollowThrough for  " + q + ": " + getCharacters.characters[c].questions[q].Q);
        if (getCharacters.characters[c].questions[q].followThrough != 666 && getCharacters.characters[c].questions[q].answered != true)
        {
            print("FollowThrough is " + getCharacters.characters[c].questions[q].followThrough);
            getCharacters.characters[c].questions[q].send = true;

            activeQuestions.Add(new ChatBubble(getCharacters.characters[c].questions[getCharacters.characters[c].questions[q].followThrough - 1].Q));
            getCharacters.characters[c].questions[q].answered = true;
            UpdateActiveQuestion(c, getCharacters.characters[c].questions[q].followThrough - 1);

            checkFollowthrough(c, getCharacters.characters[c].questions[q].followThrough - 1);

        }
    }
    */

    public void ExecuteUpdateStatus()
    {
        for (int c = 0; c < getCharacters.characters.Count; c++)
        {
            for (int p = 0; p < getCharacters.characters[c].periods.Count; p++)
            {
                for (int su = 0; su < getCharacters.characters[c].periods[p].statusUpdates.Count; su++)
                {
                    if (getCharacters.characters[c].name.Contains(characterName) && getCharacters.characters[c].periods[p].statusUpdates[su].content.Contains(statusUpdate))
                    {
                        print(getCharacters.characters[c].name + " just updated their status: " + getCharacters.characters[c].periods[p].statusUpdates[su].content + " at " + System.DateTime.Now.ToString());
                        notificationManager.notifications.Insert(0, new Notification(getCharacters.characters[c].name, getCharacters.characters[c].periods[p].statusUpdates[su].content, System.DateTime.Now, getCharacters.characters[c].profilePic));

                    }
                }
            }
        }
    }
    /*
    public void UpdateActiveQuestion(int c, int q)
    {
        print("UpdateActiveQuestion " + c + " " + q);
        CheckForDuplicates();
        for (int i = activeQuestions.Count - 1; i >= 0; i--)
        {
            GameObject clone = Instantiate(chatBubble, transform.position, transform.rotation);
            clone.transform.SetParent(chatWindow.transform);
            clone.GetComponent<ChatBubbleScript>().content.text = activeQuestions[i].message;
            clone.GetComponent<ChatBubbleScript>().profilePic.sprite = getCharacters.characters[c].profilePic;
            clone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            //clone.GetComponent<RectTransform>().transform.position = new Vector3(chatWindow.transform.position.x + offset.x + ((chatWindow.GetComponent<RectTransform>().rect.width * canvas.scaleFactor) / 2), -offset.y - (0) + (i * spacing), 0);
            clone.GetComponent<RectTransform>().transform.localPosition = new Vector3(offset.x,-chatWindow.GetComponent<RectTransform>().rect.height + offset.y + (i * spacing), 0);
            //clone.GetComponent<RectTransform>().transform.position = new Vector3(chatWindow.transform.position.x + (chatWindow.transform.position.x / 2) , chatWindow.transform.position.y - (i * spacing), 0);
            clone.name = "Chat Message " + i.ToString() + ": " + activeQuestions[i].message;

        }

        //UpdateOldQuestions(c,q);
    }

    public void UpdateOldQuestions(int c, int q) {
        for (int i = 0; i < activeQuestions.Count; i++)
        {
            GameObject clone = Instantiate(chatBubble, transform.position, transform.rotation);
            clone.transform.SetParent(chatWindow.transform);
            clone.GetComponent<ChatBubbleScript>().content.text = activeQuestions[i].message;
            clone.GetComponent<ChatBubbleScript>().profilePic.sprite = getCharacters.characters[c].profilePic;
            clone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            //clone.GetComponent<RectTransform>().transform.position = new Vector3(chatWindow.transform.position.x + offset.x + ((chatWindow.GetComponent<RectTransform>().rect.width * canvas.scaleFactor) / 2), -offset.y - (0) + (i * spacing), 0);
            clone.GetComponent<RectTransform>().transform.localPosition = new Vector3(offset.x, -chatWindow.GetComponent<RectTransform>().rect.height + offset.y, 0);
            //clone.GetComponent<RectTransform>().transform.position = new Vector3(chatWindow.transform.position.x + (chatWindow.transform.position.x / 2) , chatWindow.transform.position.y - (i * spacing), 0);
        }
    }

    public void CheckForDuplicates() {
        chatBubbleArray = GameObject.FindGameObjectsWithTag("ChatBubble");
        foreach (GameObject i in chatBubbleArray)
        {
            foreach (GameObject j in chatBubbleArray)
            {
                if (i.name == j.name && i != j)
                    print("Duplicate " + i.name);
                    //DestroyImmediate(j);
            }
        }
    }
    */
}
