using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Notification  {

    public string title;

    public string content;
    public bool show;
    public string time;
    public Sprite profilePic;

    public Notification()
    {

    }

    public Notification(string _title, string _content, DateTime _time, Sprite _profilePic) {
        title = _title;
        content = _content;
        time = _time.ToString();
        profilePic = _profilePic;
    }
}
