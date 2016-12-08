using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question {
    public string Q;
    public bool hasBeenAnswered;
    public List<Answer> answers;
	
}
