using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question {
    public string Q;
    public bool send;
    public bool answered;
    public int followThrough;
    public List<Answer> answers = new List<Answer>();

    public Question(string _Q, bool _hasBeenAnswered)
    {
        Q = _Q;
        this.answered = _hasBeenAnswered;
        
    }
}
