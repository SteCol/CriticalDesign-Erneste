using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Comment  {

    public string content;
    public string commentor;

    public Comment(string _content, string _commentor) {
        content = _content;
        commentor = _commentor;
    }
}
