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

        Debug1("//////////////////////////////////////////////////////////////////////// GETCHARACTERS START ////////////////////////////////////////////////////////////////////////");
        Debug1("//////////////////////////////////////////////////////////////////////// GETTING PATHS ////////////////////////////////////////////////////////////////////////");

        //Gets the character.txt files.
        if (getPaths)
        {
            GetPaths();
            GetFiles();
        }

        Debug1("//////////////////////////////////////////////////////////////////////// GETTING CHARACTERS ////////////////////////////////////////////////////////////////////////");
        //Start of the descipherer.
        if (getPaths)
            GetCharacters();

        Debug1("//////////////////////////////////////////////////////////////////////// GETTING PROFILE PICS ////////////////////////////////////////////////////////////////////////");
        //Get the profile pictures.
        if (getProfilePic)
            GetProfilePic();


        Debug1("//////////////////////////////////////////////////////////////////////// GETCHARACTERS END ////////////////////////////////////////////////////////////////////////");
        GetCharactersComplete = true;

        //GetComponent<FriendsList>().UpdateFriendsList();


    }

    public void GetPaths()
    {
        characterPaths.Clear();
        allPaths = Directory.GetFiles(Application.dataPath, "*", SearchOption.AllDirectories); //ALL the files in the assets folder.

        Debug2("Getting Paths");
        foreach (string p in allPaths)
        {
            if (p.Contains("Character_") && p.Contains(".txt") && !p.Contains("meta")) //Get the Character files.
            {
                characterPaths.Add(p);
                Debug3("Found character file at " + p);
                //Debug3("Attempting to add Character " + p);
            }

            if (p.Contains("ProfilePic_") && !p.Contains("meta")) //Get the profile pics.
            {
                profilePicPaths.Add(p);
                Debug4("Found profile pic at" + p);
            }
        }
    }

    public void GetFiles()
    {

        foreach (string p in characterPaths)   //Get the character files

        {
            string output = p.Substring(p.IndexOf(',') + 1); //to get a 'Characters/Characters.txt path' (to be ready by Resoures.Load) I placed a ',' (not used in any other path).
            Debug3(output);
            output = output.Substring(0, output.Length - 4); //Removes '.txt'
                                                             //print(output);
            characterFiles.Add((TextAsset)Resources.Load("," + output, typeof(TextAsset))); //Takes the paths generated by 'output' and gets the Character.txt files.

        }

        foreach (string p in profilePicPaths)   //Get the character Profile pics

        {
            string output = p.Substring(p.IndexOf(',') + 1); //to get a 'Characters/ProfilePic.psd' path
            Debug3(output);
            output = output.Substring(0, output.Length - 4); //Removes '.psd'
                                                             //print(output);
            profilePics.Add((Sprite)Resources.Load("," + output, typeof(Sprite))); //Takes the paths generated by 'output' and gets the profilePic_.psd files.
        }
    }

    public void GetCharacters()
    {
        for (int c = 0; c < characterFiles.Count; c++) //for each path is characterPaths (character files).'f' being the index.
        {
            DebugSplit2("(" + c + ") Getting character from file: " + characterPaths[c]);

            //Get each line in the file.

            characters.Add(new Character()); //For each Characters.txt file found make a new entry in the list.
            string[] line = characterFiles[c].text.Split(new string[] { "\n" }, StringSplitOptions.None); //Get each seperate line from the Characters.txt files.
            Debug3("Amount of lines in " + characterFiles[c].name + "'s file: " + line.Length.ToString());

            GetMainLines(c, line);


            //[INFO]
            if (getInfo == true)
            {
                DebugSplit("Info Start");
                Debug1("Getting [Info] from line " + characters[c].infoLineIndex + " on.");
                for (int l = characters[c].infoLineIndex + 1; l < characters[c].friendsLineIndex; l++) //every line between [Info] and [Friends]
                {

                    CycleDebug("Info cycle", "line ", l);
                    if (line[l].Contains("name:"))
                    {
                        characters[c].name = line[l];
                        Debug3("Name for '" + characterFiles[c].name + "' is now '" + characters[c].name + "'.");
                    }

                    if (line[l].Contains("value:"))
                    {
                        characters[c].value = line[l];
                        Debug3("Value for '" + characterFiles[c].name + "' is now '" + characters[c].value + "'.");
                    }

                    if (line[l].Contains("info:"))
                    {
                        characters[c].info = line[l];
                        Debug3("Info for '" + characterFiles[c].name + "' is now '" + characters[c].info + "'.");
                    }

                    if (line[l].Contains("age:"))
                    {
                        characters[c].age = line[l];
                        Debug3("Age for '" + characterFiles[c].name + "' is now '" + characters[c].age + "'.");
                    }

                }
                DebugSplit("Info Ended");
            }
            else
            {
                Debug3("GetInfo is off");
            }

            //[FRIENDS]
            if (getFriends == true)
            {
                DebugSplit("Friends Start");

                for (int l = characters[c].friendsLineIndex + 1; l < characters[c].dialofLineIndex; l++) //Every line between [Friends] and [Dialog]
                {
                    CycleDebug("Friends cycle ", "checking line ", l);

                    if (line[l].Contains("+F"))
                    {
                        Debug5("Found Friends '" + line[l] + "' in Friends for '" + characterFiles[c].name + "' on line " + (l));
                        characters[c].friends.Add(line[l]);
                    }
                }

                DebugSplit("Friends Ended");

            }
            else
            {
                Debug3("GetFriends is off");
            }

            //[DIALOG]
            if (getDialog == true)
            {
                DebugSplit("Dialog Start");

                for (int l = characters[c].dialofLineIndex + 1; l < characters[c].statusUpdatesLineIndex; l++) //Check each line between [Dialog] and [StatusUpdates]
                {

                    CycleDebug("GetDialog", "line", l);


                    if (line[l].Contains("[Period"))
                    {
                        Debug5("Found Period in Dialog for '" + characterFiles[c].name + "' on line " + (l));

                        //Get name
                        Debug6(line[l]);
                        string name = line[l];
                        name = line[l].Replace("[", "");
                        name = name.Replace("]", "");

                        Debug6(name);

                        //Get int
                        string[] strArr = line[l].Split('_');
                        string num = name.Replace("Period", "");
                        num = num.Replace("_", "");
                        Debug6(num);

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
                            Debug2("Check text in between lines " + characters[c].periods[p].periodLineIndex + " " + characters[c].periods[p + 1].periodLineIndex);
                            for (int q = characters[c].periods[p].periodLineIndex; q < characters[c].periods[p + 1].periodLineIndex; q++)//Read the lines between each period
                            {
                                Debug5(" Checking between line " + characters[c].periods[p].periodLineIndex + " & " + characters[c].periods[p + 1].periodLineIndex + ", line " + q);

                                CycleDebug(characters[c].periods[p].codeName, "checking line", q);
                                GetQuestions(line, c, q, p);
                                GetAnswers(line, c, q, p);
                            }

                            for (int q = characters[c].periods[characters[c].periods.Count - 1].periodLineIndex; q < characters[c].statusUpdatesLineIndex; q++)//read the lines between last period and [statusUpdates]
                            {
                                Debug5(" Checking between line " + characters[c].periods[p].periodLineIndex + " & " + characters[c].periods[p + 1].periodLineIndex + ", line " + q);

                                CycleDebug(characters[c].periods[characters[c].periods.Count - 1].codeName, "checking line", q);
                                GetQuestions(line, c, q, (characters[c].periods.Count - 1));
                                GetAnswers(line, c, q, (characters[c].periods.Count - 1));
                            }
                        }
                    }
                    else if (characters[c].periods.Count == 1) //if there's only one period
                    {
                        Debug4("BBBBBBBBBRG " + characters[c].periods[0].periodLineIndex);
                        for (int q = characters[c].periods[0].periodLineIndex; q < characters[c].statusUpdatesLineIndex; q++)//read the lines between last period and [statusUpdates]
                        {
                            CycleDebug(characters[c].periods[characters[c].periods.Count - 1].codeName, "checking line", q);
                            GetQuestions(line, c, q, 0);
                            GetAnswers(line, c, q, 0);
                        }
                    }
                    else //if there's no periods
                    {
                        Debug4("No periods");
                    }
                }
                else
                {
                    Debug3("GetQuestions is off");
                }


                DebugSplit("Dialog Ended");
            }
            else
            {
                Debug3("GetDialog is off");
            }

            //[PERIOD_X]
            //SU
            //C

            if (getPeriods)
            {
                DebugSplit("StatusUpdate getPeriods Start");
                for (int l = characters[c].statusUpdatesLineIndex + 1; l < line.Length; l++) //Check each line between [StatusUpdate] and End of file
                {

                    CycleDebug("GetStatusUpdate", "line", l);

                    if (line[l].Contains("[Period"))
                    {
                        Debug5("Found Period in StatusUpdates for '" + characterFiles[c].name + "' on line " + (l));

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
                            Debug6("Comparing " + name + " to " + p.codeName);

                            if (p.codeName == name)
                            {
                                Debug6("Period with name '" + name + "' already exists");
                                periodFound = true;
                                Debug6("Changing " + p.codeName + "'s lineIndex from " + p.periodLineIndex + " to " + l);
                                p.periodLineIndex = l;
                                break;

                            }
                        }

                        if (periodFound == false) //If not, add the period and add the statusUpdates
                        {
                            Debug6("Adding period '" + name + "'.");

                            characters[c].periods.Add(new Period(name, int.Parse(num), l));
                        }
                    }

                    

                    
                }

                for (int l = characters[c].statusUpdatesLineIndex + 1; l < line.Length; l++) //Check each line between [StatusUpdate] and End of file
                {
                    //Get the Status Updates
                    if (line[l].Contains("+SU"))
                    {
                        Debug2("Found SU in " + characters[c].name + ", line " + l + ", '" + line[l] + "'.");

                        for (int i = l; i > 0; i--)
                        {//Count from SU upwards
                            Debug3("Checking from " + l + "( " + line[l] + ") " + i);
                            if (line[i].Contains("[Period_"))
                            {
                                Debug3(line[l] + " belongs in " + line[i]);

                                name = line[i].Replace("[", "");
                                name = name.Replace("]", "");
                                foreach (Period p in characters[c].periods)
                                {
                                    if (p.codeName.Contains(name))
                                    {

                                        p.statusUpdates.Add(new StatusUpdate(line[l]));
                                        Debug4("Added " + line[l] + " to " + p.codeName);
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
                        Debug2("Found C in " + characters[c].name + ", line " + l + ", '" + line[l] + "'.");

                        for (int i = l; i > 0; i--)
                        {//Count from C upwards
                            Debug3("Checking from " + l + "( " + line[l] + ") " + i);
                            if (line[i].Contains("+SU"))
                            {
                                Debug3(line[l] + " belongs in " + line[i]);

                                foreach (Period p in characters[c].periods)
                                {
                                    Debug5("Checking Period " + p.codeName);
                                    foreach (StatusUpdate su in p.statusUpdates)
                                    {
                                        Debug6("Comparing SU '" + su.content + "' to '" + line[i] + "'.");

                                        if (su.content.Contains(line[i]))
                                        {
                                            su.comments.Add(new Comment(line[l], "Jane Doe"));
                                            Debug6("Added " + line[l] + " to " + su.content);
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }

                    DebugSplit("StatusUpdate getPeriods End");
            }
            
            if (getStatusUpdates)
            {
                DebugSplit("StatusUpdate getStatusUpdates End");



                DebugSplit("StatusUpdate getStatusUpdates End");
            }
            else
            {
                Debug3("GetStatusUpdates is off");
            }

    
        }
    }

    public void GetStatusUpdate(string[] line, int c, int q, int p)
    {
        //GetQuestions
        if (line[q].Contains("+SU"))
        {
            Debug3("Found SU on line " + q + ": " + line[q]);

            Debug4("Adding SU '" + line[q] + "' to '" + characters[c].periods[p].codeName + "'.");

            characters[c].periods[p].statusUpdates.Add(new StatusUpdate(line[q])); //add question with the proper values, 666 is there was no variable.


            //Get ValueChange value

        }
    }

    public void GetQuestions(string[] line, int c, int q, int p)
    {
        //GetQuestions
        if (line[q].Contains("+Q"))
        {
            Debug3("Found question on line " + q + ": " + line[q]);

            //Get Variables value
            //Debug6(line[q]);
            string[] followThrough = line[q].Split('[');

            string followThroughValue = "666";
            string valueChangeValue = "666";

            for (int cha = 0; cha < line[q].Length; cha++) //for each character in the question
            {
                if (line[q][cha] == '[') //Get the first '['
                {
                    //Debug6("found [  at " + cha + " on line " + line[q]);

                    for (int ss = cha; ss < line[q].Length; ss++) //From the first '[' onwards
                    {
                        if (line[q][ss] == ']') //Count untill ']'
                        {
                            //Debug6("found ] at " + cha + " on line " + line[q]);
                            string subString = line[q].Substring(cha + 1, (ss - cha - 1)); //get the string inbetween '[' and ']';

                            if (!subString.Contains("[") || !subString.Contains("]")) //If the substring contains '[' or ']', this for when there are muntiple Variables described in the question
                            {
                                //Debug6(subString);

                                //Check which kind of Variable it is and change it's value
                                if (subString.Contains("FollowThrough"))
                                {
                                    followThroughValue = subString.Replace("FollowThrough_Q", "");
                                    //Debug6(followThroughValue);
                                }

                                if (subString.Contains("V"))
                                {
                                    valueChangeValue = subString.Replace("V", "");
                                    //Debug6(valueChangeValue);
                                }
                            }
                        }
                    }
                }
            }

            Debug4("Adding question '" + line[q] + "' with values f:" + followThroughValue + ", v: " + valueChangeValue + " to '" + characters[c].periods[p].codeName + "'.");


            characters[c].periods[p].questions.Add(new Question(line[q], false, int.Parse(followThroughValue), float.Parse(valueChangeValue))); //add question with the proper values, 666 is there was no variable.


            //Get ValueChange value

        }
    }

    public void GetAnswers(string[] line, int c, int q, int p)
    {
        //Get Answers
        if (getAnswers)
        {
            if (line[q].Contains("+A"))
            {
                Debug4("Found answer on line " + q + ": " + line[q]);

                char i = line[q][2]; //Get the +A' '.x value
                //Debug5(i.ToString());

                foreach (Question question in characters[c].periods[p].questions)
                {
                    //Debug5("Cheking if answer goes to " + question.Q);

                    if (question.Q.Contains("+Q" + i))//check if the value of A is the same value of Q
                    {

                        //Get Variables value
                        //Debug6(line[q]);
                        string[] followThrough = line[q].Split('[');

                        string qValue = "666";
                        string vValue = "666";
                        string suValue = "666";


                        for (int cha = 0; cha < line[q].Length; cha++) //for each character in the question
                        {
                            if (line[q][cha] == '[') //Get the first '['
                            {
                                //Debug6("found [  at " + cha + " on line " + line[q]);

                                for (int ss = cha; ss < line[q].Length; ss++) //From the first '[' onwards
                                {
                                    if (line[q][ss] == ']') //Count untill ']'
                                    {
                                        //Debug6("found ] at " + cha + " on line " + line[q]);
                                        string subString = line[q].Substring(cha + 1, (ss - cha - 1)); //get the string inbetween '[' and ']';

                                        if (!subString.Contains("[") || !subString.Contains("]")) //If the substring contains '[' or ']', this for when there are muntiple Variables described in the question
                                        {
                                            //Debug6(subString);

                                            //Check which kind of Variable it is and change it's value
                                            if (subString.Contains("Q"))
                                            {
                                                qValue = subString.Replace("Q", "");
                                                //Debug6(qValue);
                                            }

                                            if (subString.Contains("V"))
                                            {
                                                vValue = subString.Replace("V", "");
                                                //Debug6(vValue);
                                            }

                                            if (subString.Contains("SU"))
                                            {
                                                suValue = subString.Replace("SU", "");
                                                //Debug6(suValue);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        Debug6("Adding '" + line[q] + "' with values q:" + qValue + ", v:" + vValue + ", su: " + suValue + " to '" + question.Q + "'.");


                        question.answers.Add(new Answer(line[q], int.Parse(qValue), float.Parse(vValue), int.Parse(suValue)));
                    }
                }
            }
        }
    }


    public void GetMainLines(int c, string[] line)
    {

        DebugSplit("GetMainLines");
        if (getStatusUpdates == true)
        {
            for (int l = 0; l < line.Length; l++)
            {
                if (line[l].Contains("[Info]"))
                {
                    characters[c].infoLineIndex = l;
                    Debug3("Found [Info] for " + characterFiles[c].name + " on line " + l);
                    break;
                }
                else
                {
                    //characters[c].infoLineIndex = 666;
                }
            }

            for (int l = 0; l < line.Length; l++)
            {
                if (line[l].Contains("[Friends]"))
                {
                    characters[c].friendsLineIndex = l;
                    Debug3("Found [Friends] for " + characterFiles[c].name + " on line " + l);
                    break;
                }
                else
                {
                    //characters[c].friendsLineIndex = 666;
                }
            }

            for (int l = 0; l < line.Length; l++)
            {
                if (line[l].Contains("[Dialog]"))
                {
                    characters[c].dialofLineIndex = l;
                    Debug3("Found [Dialog] for " + characterFiles[c].name + " on line " + l);
                    break;
                }
                else
                {
                    //characters[c].dialofLineIndex = 666;
                }
            }

            for (int l = 0; l < line.Length; l++)
            {
                if (line[l].Contains("[StatusUpdates]"))
                {
                    characters[c].statusUpdatesLineIndex = l;
                    Debug3("Found [StatusUpdates] for " + characterFiles[c].name + " on line " + l);
                    break;
                }
                else
                {
                    //characters[c].statusUpdatesLineIndex = 666;
                }
            }
        }

    }


    public void GetProfilePic()
    {

    }

    public void CheckFollowThrough()
    {

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