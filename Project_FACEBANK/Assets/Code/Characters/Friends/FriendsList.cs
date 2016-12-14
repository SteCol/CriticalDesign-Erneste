using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsList : MonoBehaviour {

    public List<string> friend;

    public void UpdateFriendsList() {
        foreach (Character c in GetComponent<GetCharacters>().characters) {
            c.friends.Add(c.name);
        }
    }
}
