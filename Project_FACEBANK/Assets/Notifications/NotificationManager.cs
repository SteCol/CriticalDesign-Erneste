using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class NotificationManager : MonoBehaviour
{
    [Header("Generating Content")]
    public GameObject feedBubble;
    public List<Notification> notifications ;
    public List <GameObject> StatusUpdates;
    public GameObject feedContent;
    GameObject[] StatusUpdatesArray;

    [Header("Generating Layout")]
    public bool updateFeed;
    public float spacing;
    private float oldSpacing;
    public bool debug;
    public Vector2 offset;
    private Vector2 oldOffset;

    // Use this for initialization
    void Start()
    {
        UpdateFeed();
        oldSpacing = spacing;
    }

    // Update is called once per frame
    void Update()
    {
        if (spacing != oldSpacing) {
            UpdateFeed();
            oldSpacing = spacing;
        }

        if (offset != oldOffset) {
            UpdateFeed();
            oldOffset = offset;
        }

        if (updateFeed) {
            UpdateFeed(); ;
            updateFeed = false;
        }

        if (StatusUpdatesArray.Length != notifications.Count)
            UpdateFeed();


        if (debug)
        {
            Debug.Log("StatusList: " + notifications.Count.ToString() + " | StatusArray: " + StatusUpdatesArray.Length.ToString());
            debug = false;
        }

    }

    void UpdateFeed() {
        StatusUpdatesArray = GameObject.FindGameObjectsWithTag("StatusUpdate");
        foreach (GameObject g in StatusUpdatesArray)
            DestroyImmediate(g);

        StatusUpdates.Clear();
        for (int i = 0; i < notifications.Count; i++)
        {
            GameObject clone = Instantiate(feedBubble, transform.position, transform.rotation);
            clone.transform.SetParent(feedContent.transform);
            clone.GetComponent<Bubble>().title.text = notifications[i].title;
            clone.GetComponent<Bubble>().content.text = notifications[i].content;
            clone.GetComponent<Bubble>().profilePic.sprite = notifications[i].profilePic;
            clone.GetComponent<Bubble>().time.text = notifications[i].time;


            clone.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            clone.GetComponent<RectTransform>().transform.position = new Vector3(feedContent.transform.position.x + (feedContent.transform.position.x / 2) + offset.x, - offset.y + feedContent.transform.position.y - (i * spacing), 0);
            //clone.transform.SetParent(feed.transform);

            clone.name = "Notification " + i.ToString() + ": " + notifications[i].title;
            StatusUpdates.Add(clone);
        }
    }
}
