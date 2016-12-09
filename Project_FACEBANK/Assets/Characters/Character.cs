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
    public List<Question> questions;



}
