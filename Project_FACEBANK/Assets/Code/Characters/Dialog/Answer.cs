using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Answer {
    public string A;
    public int nextQuestion;
    public float valueChange;

    
    public Answer(string _A, int _nextQuestion, float _valueChange) {
        A = _A;
        nextQuestion = _nextQuestion;
        valueChange = _valueChange;
    }
    
	
}
