using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Period {
    public string codeName;
    public int periodNumber;
    public int periodLineIndex;
    public List<Question> questions = new List<Question>();
    public List<StatusUpdate> statusUpdates = new List<StatusUpdate>();

    public Period(string _codeName, int _periodNumber, int _periodLineIndex) {
        codeName = _codeName;
        periodNumber = _periodNumber;
        periodLineIndex = _periodLineIndex;
    }


    public Period(string _codeName, int _periodNumber, int _periodLineIndex, List<Question> _questions, List<Answer> _answers)
    {
        codeName = _codeName;
        periodNumber = _periodNumber;
        periodLineIndex = _periodLineIndex;
        questions = _questions;
        questions[0].answers = _answers;
    }
}
