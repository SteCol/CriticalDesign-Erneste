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
    //public string value;
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


        //for (int i = 0; i < GameObject.FindGameObjectsWithTag("ProfilePicDropdown").Length; i++)
        //    GameObject.FindGameObjectsWithTag("ProfilePicDropdown")[i].GetComponent<Image>().sprite = possibleProfilePics[i];
        //foreach (GameObject g in GameObject.FindGameObjectsWithTag("ProfilePicDropdown")){
        //}

        //profilePicChoice.AddOptions(GetComponent<GetCharacters>().profilePicPaths);

        foreach (Sprite s in possibleProfilePics) {
            //profilePicChoice.options.Add(Dropdown.OptionData);

        }
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
}
