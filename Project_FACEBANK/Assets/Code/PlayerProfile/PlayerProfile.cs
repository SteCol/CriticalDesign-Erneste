using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerProfile : MonoBehaviour
{

    [Header("General Info")]
    public string playerName;
    public string age;
    public Sprite profilePic;
    public float value;
    public string info;

    [Header("Updates")]
    public bool updateValue;
    public bool addNewValue;

    [Header("controls")]
    public Dropdown profilePicChoice;
    public InputField nameInput;
    public InputField ageInput;
    //public Sprite profilePic;
    public Text valueText;
    public List<float> values;
    public InputField infoInput;
    public List<Sprite> possibleProfilePics;


    void Start()
    {
        value = Random.Range(70.0f, 130.0f);
        AddValue(value);
        possibleProfilePics = GetComponent<GetCharacters_B>().profilePics;

        UpdateInfo();
        

    }

    void Update()
    {
        if (GetComponent<GetCharacters_B>().GetCharactersComplete)
        {
            UpdateProfilePic();
            if (playerName != nameInput.text || age != ageInput.text || info != infoInput.text)
            {
                UpdateInfo();
            }
        }

        if (updateValue)
        {
            UpdateInfo();
            updateValue = false;
        }

        if (addNewValue) {
            AddValue(value);
            updateValue = true;
            addNewValue = false;
        }
    }

    void UpdateInfo()
    {
        playerName = nameInput.text;
        age = ageInput.text;
        info = infoInput.text;

        string valueString = "";
        foreach (float v in values) {
            valueString = valueString + " \n" + v.ToString("000.00");
        }

        valueText.text = valueString;
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

    public void AddValue(float _value) {
        values.Insert(0, _value);
    }

    public void UpdateValue()
    {
        values.Insert(0, value);
    }
}
