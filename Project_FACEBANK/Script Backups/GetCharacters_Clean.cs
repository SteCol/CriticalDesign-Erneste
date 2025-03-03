﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

//[ExecuteInEditMode]
public class GetCharacters_B : MonoBehaviour
{
    public bool GetCharactersComplete = false;

    [Header("A Whole Bunch Of Lists")]
    public string[] allPaths;
    public List<string> characterPaths;
    public List<string> profilePicPaths;

    //public List<string> words;
    public List<TextAsset> characterFiles;
    public List<Sprite> profilePics;

    [Header("Controls")]
    public bool getPaths;
    public bool getCharacters;
    public bool getInfo;
    public bool getPeriods;
    public bool getDialog;
    public bool getQuestions;
    public bool getAnswers;
    public bool getStatusUpdates;
    public bool getFriends;
    public bool getProfilePic;
    public bool debug, cycleDebug;

    [Header("Final Characters List")]
    public List<Character> characters = new List<Character>();


    // Use this for initialization
    void Start()
    {
        //Gets the character.txt files.
        if (getPaths)
        {
            GetPaths();
            GetFiles();
        }

        //Start of the descipherer.
        if (getPaths)
            GetCharacters();

        //Get the profile pictures.
        if (getProfilePic)
            GetProfilePic();

        GetCharactersComplete = true;

        GetComponent<FriendsList>().UpdateFriendsList();
    }

    public void GetPaths()
    {
        characterPaths.Clear();
        allPaths = Directory.GetFiles(Application.dataPath, "*", SearchOption.AllDirectories); //ALL the files in the assets folder.

        foreach (string p in allPaths)
        {
            if (p.Contains("Character_") && p.Contains(".txt") && !p.Contains("meta")) //Get the Character files.
                characterPaths.Add(p);

            if (p.Contains("ProfilePic_") && !p.Contains("meta")) //Get the profile pics.
                profilePicPaths.Add(p);
        }
    }

    public void GetFiles()
    {

        foreach (string p in characterPaths)   //Get the character files
        {
            string output = p.Substring(p.IndexOf(',') + 1); //to get a 'Characters/Characters.txt path' (to be ready by Resoures.Load) I placed a ',' (not used in any other path).
            output = output.Substring(0, output.Length - 4); //Removes '.txt'
            characterFiles.Add((TextAsset)Resources.Load("," + output, typeof(TextAsset))); //Takes the paths generated by 'output' and gets the Character.txt files.
        }

        foreach (string p in profilePicPaths)   //Get the character Profile pics
        {
            string output = p.Substring(p.IndexOf(',') + 1); //to get a 'Characters/ProfilePic.psd' path
            output = output.Substring(0, output.Length - 4); //Removes '.psd'
            profilePics.Add((Sprite)Resources.Load("," + output, typeof(Sprite))); //Takes the paths generated by 'output' and gets the profilePic_.psd files.
        }
    }

    public void GetCharacters()
    {
        for (int c = 0; c < characterFiles.Count; c++) //for each path is characterPaths (character files).'f' being the index.
        {
            //Get each line in the file.
            characters.Add(new Character()); //For each Characters.txt file found make a new entry in the list.
            string[] line = characterFiles[c].text.Split(new string[] { "\n" }, StringSplitOptions.None); //Get each seperate line from the Characters.txt files.

            GetMainLines(c, line);

            //[INFO]
            if (getInfo == true)
            {
                for (int l = characters[c].infoLineIndex + 1; l < characters[c].friendsLineIndex; l++) //every line between [Info] and [Friends]
                {
                    if (line[l].Contains("name:"))
                        characters[c].name = line[l];

                    if (line[l].Contains("value:"))
                        characters[c].value = line[l];

                    if (line[l].Contains("info:"))
                        characters[c].info = line[l];

                    if (line[l].Contains("age:"))
                        characters[c].age = line[l];
                }
            }

            //[FRIENDS]
            if (getFriends == true)
            {
                for (int l = characters[c].friendsLineIndex + 1; l < characters[c].dialofLineIndex; l++) //Every line between [Friends] and [Dialog]
                {
                    if (line[l].Contains("+F"))
                        characters[c].friends.Add(line[l]);
                }
            }

            //[DIALOG]
            if (getDialog == true)
            {
                for (int l = characters[c].dialofLineIndex + 1; l < characters[c].statusUpdatesLineIndex; l++) //Check each line between [Dialog] and [StatusUpdates]
                {
                    if (line[l].Contains("[Period"))
                    {
                        //Get name
                        Debug6(line[l]);
                        string name = line[l];
                        name = line[l].Replace("[", "");
                        name = name.Replace("]", "");

                        //Get int
                        string[] strArr = line[l].Split('_');
                        string num = name.Replace("Period", "");
                        num = num.Replace("_", "");

                        //Add period
                        characters[c].periods.Add(new Period(name, int.Parse(num), l));
                    }
                }

                if (getQuestions)
                {
                    //Adding questions per period
                    //the if statement is needed because it needt to be able to find the space between one period and the next
                    if (characters[c].periods.Count > 1) //If there is more than 1 period
                    {
                        for (int p = 0; p < characters[c].periods.Count - 1; p++) //for each period in dialog exept the last one
                        {
                            for (int q = characters[c].periods[p].periodLineIndex; q < characters[c].periods[p + 1].periodLineIndex; q++)//Read the lines between each period
                            {
                                GetQuestions(line, c, q, p);
                                GetAnswers(line, c, q, p);
                            }

                            for (int q = characters[c].periods[characters[c].periods.Count - 1].periodLineIndex; q < characters[c].statusUpdatesLineIndex; q++)//read the lines between last period and [statusUpdates]
                            {
                                GetQuestions(line, c, q, (characters[c].periods.Count - 1));
                                GetAnswers(line, c, q, (characters[c].periods.Count - 1));
                            }
                        }
                    }
                    else if (characters[c].periods.Count == 1) //if there's only one period
                    {
                        for (int q = characters[c].periods[0].periodLineIndex; q < characters[c].statusUpdatesLineIndex; q++)//read the lines between last period and [statusUpdates]
                        {
                            GetQuestions(line, c, q, 0);
                            GetAnswers(line, c, q, 0);
                        }
                    }
                }
            }

            if (getPeriods)
            {
                for (int l = characters[c].statusUpdatesLineIndex + 1; l < line.Length; l++) //Check each line between [StatusUpdate] and End of file
                {
                    if (line[l].Contains("[Period"))
                    {
                        //Get name
                        Debug6(line[l]);
                        string name = line[l];
                        name = line[l].Replace("[", "");
                        name = name.Replace("]", "");

                        //Get int
                        string[] strArr = line[l].Split('_');
                        string num = name.Replace("Period", "");
                        num = num.Replace("_", "");

                        //Check if period already exists
                        bool periodFound = false;

                        foreach (Period p in characters[c].periods)
                        {
                            if (p.codeName == name)
                            {
                                periodFound = true;
                                p.periodLineIndex = l;
                                break;
                            }
                        }

                        if (periodFound == false) //If not, add the period and add the statusUpdates
                            characters[c].periods.Add(new Period(name, int.Parse(num), l));
                    }
                }

                for (int l = characters[c].statusUpdatesLineIndex + 1; l < line.Length; l++) //Check each line between [StatusUpdate] and End of file
                {
                    //Get the Status Updates
                    if (line[l].Contains("+SU"))
                    {
                        for (int i = l; i > 0; i--)
                        {//Count from SU upwards
                            if (line[i].Contains("[Period_"))
                            {
                                name = line[i].Replace("[", "");
                                name = name.Replace("]", "");
                                foreach (Period p in characters[c].periods)
                                {
                                    if (p.codeName.Contains(name))
                                    {
                                        p.statusUpdates.Add(new StatusUpdate(line[l]));
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }

                for (int l = characters[c].statusUpdatesLineIndex + 1; l < line.Length; l++) //Check each line between [StatusUpdate] and End of file
                {
                    //Get the Comments
                    if (line[l].Contains("+C"))
                    {
                        //Get commentor
                        string commentor = line[l].Substring(line[l].IndexOf('[') + 1, line[l].IndexOf(']') - 7);

                        for (int i = l; i > 0; i--)
                        {//Count from C upwards
                            if (line[i].Contains("+SU"))
                            {
                                foreach (Period p in characters[c].periods)
                                {
                                    foreach (StatusUpdate su in p.statusUpdates)
                                    {
                                        if (su.content.Contains(line[i]))
                                        {
                                            su.comments.Add(new Comment(line[l], commentor));
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public void GetStatusUpdate(string[] line, int c, int q, int p)
    {
        //GetQuestions
        if (line[q].Contains("+SU"))
            characters[c].periods[p].statusUpdates.Add(new StatusUpdate(line[q])); //add question with the proper values, 666 is there was no variable.
    }


    public void GetQuestions(string[] line, int c, int q, int p)
    {
        //GetQuestions
        if (line[q].Contains("+Q"))
        {
            //Get Variables value
            string[] followThrough = line[q].Split('[');

            string followThroughValue = "666";
            string valueChangeValue = "666";

            for (int cha = 0; cha < line[q].Length; cha++) //for each character in the question
            {
                if (line[q][cha] == '[') //Get the first '['
                {

                    for (int ss = cha; ss < line[q].Length; ss++) //From the first '[' onwards
                    {
                        if (line[q][ss] == ']') //Count untill ']'
                        {
                            string subString = line[q].Substring(cha + 1, (ss - cha - 1)); //get the string inbetween '[' and ']';

                            if (!subString.Contains("[") || !subString.Contains("]")) //If the substring contains '[' or ']', this for when there are muntiple Variables described in the question
                            {
                                //Check which kind of Variable it is and change it's value
                                if (subString.Contains("FollowThrough"))
                                {
                                    followThroughValue = subString.Replace("FollowThrough_Q", "");
                                }

                                if (subString.Contains("V"))
                                {
                                    valueChangeValue = subString.Replace("V", "");
                                }
                            }
                        }
                    }
                }
            }
            characters[c].periods[p].questions.Add(new Question(line[q], false, int.Parse(followThroughValue), float.Parse(valueChangeValue))); //add question with the proper values, 666 is there was no variable.
        }
    }

    public void GetAnswers(string[] line, int c, int q, int p)
    {
        //Get Answers
        if (getAnswers)
        {
            if (line[q].Contains("+A"))
            {
                char i = line[q][2]; //Get the +A' '.x value

                foreach (Question question in characters[c].periods[p].questions)
                {
                    if (question.Q.Contains("+Q" + i))//check if the value of A is the same value of Q
                    {
                        //Get Variables value
                        string[] followThrough = line[q].Split('[');

                        string qValue = "666";
                        string vValue = "666";
                        string suValue = "666";

                        for (int cha = 0; cha < line[q].Length; cha++) //for each character in the question
                        {
                            if (line[q][cha] == '[') //Get the first '['
                            {
                                for (int ss = cha; ss < line[q].Length; ss++) //From the first '[' onwards
                                {
                                    if (line[q][ss] == ']') //Count untill ']'
                                    {
                                        string subString = line[q].Substring(cha + 1, (ss - cha - 1)); //get the string inbetween '[' and ']';

                                        if (!subString.Contains("[") || !subString.Contains("]")) //If the substring contains '[' or ']', this for when there are muntiple Variables described in the question
                                        {
                                            //Check which kind of Variable it is and change it's value
                                            if (subString.Contains("Q"))
                                            {
                                                qValue = subString.Replace("Q", "");
                                            }

                                            if (subString.Contains("V"))
                                            {
                                                vValue = subString.Replace("V", "");
                                            }

                                            if (subString.Contains("SU"))
                                            {
                                                suValue = subString.Replace("SU", "");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        question.answers.Add(new Answer(line[q], int.Parse(qValue), float.Parse(vValue), int.Parse(suValue)));
                    }
                }
            }
        }
    }


    public void GetMainLines(int c, string[] line)
    {
        if (getStatusUpdates == true)
        {
            for (int l = 0; l < line.Length; l++)
            {
                if (line[l].Contains("[Info]"))
                {
                    characters[c].infoLineIndex = l;
                    break;
                }
            }

            for (int l = 0; l < line.Length; l++)
            {
                if (line[l].Contains("[Friends]"))
                {
                    characters[c].friendsLineIndex = l;
                    break;
                }
            }

            for (int l = 0; l < line.Length; l++)
            {
                if (line[l].Contains("[Dialog]"))
                {
                    characters[c].dialofLineIndex = l;
                    break;
                }
            }

            for (int l = 0; l < line.Length; l++)
            {
                if (line[l].Contains("[StatusUpdates]"))
                {
                    characters[c].statusUpdatesLineIndex = l;
                    break;
                }
            }
        }

    }


    public void GetProfilePic()
    {
        foreach (Character c in characters)
        {
            foreach (Sprite p in profilePics)
            {
                //CharacterName
                string[] characterNameArray = c.name.Split(' ');
                string characterName = characterNameArray[1];

                //profilePicName
                string[] profilePicNameArray = p.name.Split('_');
                string profilePicName = profilePicNameArray[1];

                if (characterName.Contains(profilePicName))
                {
                    Debug4("Name matches " + characterName + " " + profilePicName);
                    c.profilePic = p;
                }
            }
        }
    }


    public void Debug1(string message)
    {
        if (debug)
            print(message);
    }

    public void Debug2(string message)
    {
        if (debug)
            print("     |--- " + message);
    }

    public void Debug3(string message)
    {
        if (debug)
            print("             |--- " + message);
    }

    public void Debug4(string message)
    {
        if (debug)
            print("                     |--- " + message);
    }

    public void Debug5(string message)
    {
        if (debug)
            print("                             |--- " + message);
    }

    public void Debug6(string message)
    {
        if (debug)
            print("                                       |--- " + message);
    }

    public void DebugSplit(string message)
    {
        if (debug)
            print("-------------------------" + message + "--------------------------------");
    }

    public void DebugSplit2(string message)
    {
        if (debug)
            print("==========" + message + "==============");
    }

    public void CycleDebug(string _location, string _info, int _number)
    {
        if (cycleDebug)
            print("                                               " + _location + " | " + _info + " | " + _number + "");
    }

}