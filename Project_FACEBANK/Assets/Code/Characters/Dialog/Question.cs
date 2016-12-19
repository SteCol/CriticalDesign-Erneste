using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question {
    public string Q;
    public bool send;
    public bool checkForAnswered = true;
    public bool answered;
    public int followThrough;
    public float valueChange;

    public List<Answer> answers = new List<Answer>();

    public Question(string _Q, bool _hasBeenAnswered, int _followThrough ,float _valueChange)
    {
        Q = _Q;
        answered = _hasBeenAnswered;
        valueChange = _valueChange;
        followThrough = _followThrough;
    }
}
