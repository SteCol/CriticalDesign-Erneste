using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusUpdate {

    public string content;
    public List<Comment> comments = new List<Comment>();

    public StatusUpdate(string _content)
    {
        content = _content;
    }


    public StatusUpdate(string _content, List<Comment> _comments) {
        content = _content;
        comments = _comments;
    }
}
