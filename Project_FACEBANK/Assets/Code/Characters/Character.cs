using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character  {

    [Header("General Info")]
    public string name;
    public string age;
    public Sprite profilePic;
    public string value;
    public string info;
    public int infoLineIndex;


    [Header("Friends")]
    //public List<Friend> friends = new List<Friend>();
    public int friendsLineIndex;
    public List<string> friends = new List<string>();

    [Header("Dialog")]
    public int dialofLineIndex;

    [Header("StatusUpdates")]
    public int statusUpdatesLineIndex;

    [Header("Periods")]
    public List<Period> periods = new List<Period>();


    //[Header("Dialog")]
    //public List<Question> questions = new List<Question>();

    //[Header("StatusUpdates")]
    //public List<StatusUpdate> statusUpdates = new List<StatusUpdate>();


    /*
    public Character()
    {
    }


    public Character(string _name, string _value, string info) {
        name = _name;
        value = _value;
        info = _value;
    }
    */



}
