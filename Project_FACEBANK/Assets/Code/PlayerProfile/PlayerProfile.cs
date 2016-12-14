using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerProfile : MonoBehaviour
{

    [Header("General Info")]
    public string name;
    public string age;
    public Sprite profilePic;
    public string value;
    public string info;

    [Header("controls")]
    public Dropdown profilePicChoice;
    public InputField nameInput;
    public InputField ageInput;
    //public Sprite profilePic;
    public Text valueText;
    public List<int> values;
    public InputField infoInput;
    public List<Sprite> possibleProfilePics;


    // Use this for initialization
    void Start()
    {
        value = Random.Range(70.0f, 130.0f).ToString();
        possibleProfilePics = GetComponent<GetCharacters>().profilePics;

        //UpdateInfo();
        profilePicChoice.options.Clear();
        profilePicChoice.AddOptions(possibleProfilePics);
        
    }
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<GetCharacters>().GetCharactersComplete)
        if (name != nameInput.text || age != ageInput.text || info != infoInput.text)
            UpdateInfo();
    }

    void UpdateInfo()
    {
        name = nameInput.text;
        age = ageInput.text;
        info = infoInput.text;
    }

    public void UpdateProfilePic() {
        //profilePicChoice.GetComponent<Image>().sprite = possibleProfilePics[profilePicChoice.GetComponent<Dropdown>().value];
    }

    public void updateValue() {
        values.Insert(0, int.Parse(value));
    }
}
