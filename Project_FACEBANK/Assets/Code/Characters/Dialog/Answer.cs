using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Answer {
    public string A;
    public int nextQuestion;

    
    public Answer(string _A, int _nextQuestion) {
        A = _A;
        nextQuestion = _nextQuestion;
    }
    
	
}
