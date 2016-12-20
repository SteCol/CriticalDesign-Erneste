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


    void Start()
    {
        value = Random.Range(70.0f, 130.0f).ToString();
        possibleProfilePics = GetComponent<GetCharacters_B>().profilePics;

        UpdateInfo();
        

    }

    void Update()
    {
        if (GetComponent<GetCharacters_B>().GetCharactersComplete)
        {
            UpdateProfilePic();
            if (name != nameInput.text || age != ageInput.text || info != infoInput.text)
            {
                UpdateInfo();
            }
        }
    }

    void UpdateInfo()
    {
        name = nameInput.text;
        age = ageInput.text;
        info = infoInput.text;
    }

    public void UpdateProfilePic()
    {
        profilePicChoice.options.Clear();
        profilePicChoice.AddOptions(possibleProfilePics);

        for (int s = 0; s < possibleProfilePics.Count; s++)
        {
            profilePic = possibleProfilePics[profilePicChoice.value];
        }
    }

    public void updateValue()
    {
        values.Insert(0, int.Parse(value));
    }
}
