using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Period {
    public string codeName;
    public int periodNumber;
    public List<Question> questions = new List<Question>();
    public List<StatusUpdate> statusUpdates = new List<StatusUpdate>();
}
