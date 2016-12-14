using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusUpdate {

    public string content;

    public StatusUpdate(string _content) {
        content = _content;
    }
}
