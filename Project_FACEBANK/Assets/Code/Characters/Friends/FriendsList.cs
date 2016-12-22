using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FriendsList : MonoBehaviour {

    public List<string> friends;
    public string tempText;
    public Text friendsList;

    public void UpdateFriendsList() {
        foreach (Character c in GetComponent<GetCharacters_C>().characters) {
            string cleanName = c.name.Replace("name:", "");
            friends.Add(cleanName);
            tempText = tempText + cleanName + " | " + c.value + "\n";
        }
        
        friendsList.text = tempText;
    }
}
