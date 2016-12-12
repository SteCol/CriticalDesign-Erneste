using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EventsManager : MonoBehaviour {

    [Header("Activate Events")]
    public bool recieveMessage;
    public bool updateStatus;

    [Header("Set Events")]
    public string characterName;
    public string chatMessage;
    public string statusUpdate;

    [Header("")]
    public GetCharacters getCharacters;
    public NotificationManager notificationManager;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (recieveMessage) {
            ExecuteRecieveMessage();
            recieveMessage = false;
        }

        if (updateStatus)
        {
            ExecuteUpdateStatus();
            updateStatus = false;
        }

    }

    public void ExecuteRecieveMessage() {
        for (int c = 0; c < getCharacters.characters.Count; c++)
        {
            for (int q = 0; q < getCharacters.characters[c].questions.Count; q++)
            {
                if (getCharacters.characters[c].name.Contains(characterName) && getCharacters.characters[c].questions[q].Q.Contains(chatMessage))
                {
                    print("Recieved Message from  " + getCharacters.characters[c].name + ": " + getCharacters.characters[c].questions[q].Q);
                }
            }
        }
    }

    public void ExecuteUpdateStatus() {
        for (int c = 0; c < getCharacters.characters.Count; c++) {
            for (int su = 0; su < getCharacters.characters[c].statusUpdates.Count; su++)
            {
                if (getCharacters.characters[c].name.Contains(characterName) && getCharacters.characters[c].statusUpdates[su].content.Contains(statusUpdate)) {
                    print(getCharacters.characters[c].name +  " just updated their status: " + getCharacters.characters[c].statusUpdates[su].content + " at " + System.DateTime.Now.ToString());
                    notificationManager.notifications.Add(new Notification(getCharacters.characters[c].name, getCharacters.characters[c].statusUpdates[su].content, System.DateTime.Now, getCharacters.characters[c].profilePic));
                }
            }
        }
    }
}
