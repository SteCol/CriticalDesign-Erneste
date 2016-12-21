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
    public GetCharacters_C getCharacters;
    public NotificationManager notificationManager;

    [Header("Period Control")]
    public int activePeriod;
    private int prevPeriod;


    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePeriod();

        if (recieveMessage)
        {
            recieveMessage = false;
        }

        if (updateStatus)
        {
            ExecuteUpdateStatus();
            updateStatus = false;
        }

    }

    void UpdatePeriod() {
        if (prevPeriod != activePeriod) {
            print("Updating periods from " + prevPeriod + " to " + activePeriod);
            prevPeriod = activePeriod;
        }

    }

    

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
    
}
