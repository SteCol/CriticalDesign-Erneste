using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character  {

    [Header("General Info")]
    public string name;
    public string value;
    public string info;

    [Header("Dialog")]
    public List<Question> questions = new List<Question>();

    [Header("StatusUpdates")]
    public List<StatusUpdate> statusUpdates = new List<StatusUpdate>();


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
