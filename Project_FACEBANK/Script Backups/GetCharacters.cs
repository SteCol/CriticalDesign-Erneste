﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

//[ExecuteInEditMode]
public class GetCharacters : MonoBehaviour
{
    public bool GetCharactersComplete = false;

    [Header("A Whole Bunch Of Lists")]
    public string[] allPaths;
    public List<string> paths;
    public List<string> profilePicPaths;

    //public List<string> words;
    public List<TextAsset> characterFiles;
    public List<Sprite> profilePics;

    [Header("Controls")]
    public bool getFiles;
    public bool getCharacters;
    public bool getInfo;
    public bool getDialog;
    public bool getQuestions;
    public bool getAnswers;
    public bool getStatusUpdates;
    public bool getFriendsList;
    public bool getProfilePic;
    public bool debug;

    [Header("Final Characters List")]
    public List<Character> characters = new List<Character>();


    // Use this for initialization
    void Start()
    {

        Debug("==========GETCHARACTERS START=============");

        //Gets the character.txt files.
        if (getFiles)
            GetFiles();


        //Start of the descipherer.
        if (getFiles)
            GetCharacter();

        //Get the profile pictures.
        if (getProfilePic)
            GetProfilePic();

        CheckFollowThrough();

        Debug("==========GETCHARACTERS END=============");
        GetCharactersComplete = true;

        GetComponent<FriendsList>().UpdateFriendsList();


    }

    public void GetFiles()
    {
        paths.Clear();
        allPaths = Directory.GetFiles(Application.dataPath, "*", SearchOption.AllDirectories); //ALL the files in the assets folder.

        foreach (string p in allPaths)
        {
            if (p.Contains("Character_") && p.Contains(".txt") && !p.Contains("meta")) //Narrows down ALL the files to only the Character.txt files.
            {
                paths.Add(p);
                Debug("Found character file at " + p);
            }

            if (p.Contains("ProfilePic_") && !p.Contains("meta")) //Narrows down ALL the files to only the Profile pics sprites (any image file type) files.
            {
                profilePicPaths.Add(p);
                Debug("Found profile pic at" + p);
            }
        }
    }


    public void GetCharacter()
    {
        for (int c = 0; c < paths.Count; c++) //Amount of paths = amount of files found = amount of characters.
        {
            Debug("_________________________________________________________________________________________________");
            Debug("Start of Loop: " + c);

            //Gets the file names from the paths and reformats them to be readable by Resource.Load.
            string output = paths[c].Substring(paths[c].IndexOf(',') + 1); //to get a 'Characters/Characters.txt path' (to be ready by Resoures.Load) I placed a ',' (not used in any other path).
            Debug(output);
            output = output.Substring(0, output.Length - 4); //Removes '.txt'
            //print(output);

            characterFiles.Add((TextAsset)Resources.Load("," + output, typeof(TextAsset))); //Takes the paths generated by 'output' and gets the Character.txt files.

            characters.Add(new Character()); //For each Characters.txt file found make a new entry in the list.
            string[] splitString = characterFiles[c].text.Split(new string[] { "\n" }, StringSplitOptions.None); //Get each seperate line from the Characters.txt files.
            Debug("Amount of lines in " + characterFiles[c].name + "'s file: " + splitString.Length.ToString());


            for (int s = 0; s < splitString.Length; s++) // 's' = amount of sentences.
            {
                //Debug("Line " + s + ": " + splitString[s]);

                //This box gets all the info per line. Might change later.
                if (getInfo && splitString[s].Contains("[Info]"))
                {
                    Debug("Found [Info] for: " + splitString[s + 1]);

                    Level1Debug("Found name: " + splitString[s + 1]);
                    characters[c].name = splitString[s + 1];
                    Level2Debug("Name for Character " + c + " (" + characters[c].name + ") " + "is now: " + characters[c].name);

                    Level1Debug("Found age: " + splitString[s + 2]);
                    characters[c].age = splitString[s + 2];
                    Level2Debug("Age for Character " + c + " (" + characters[c].name + ") " + "is now: " + characters[c].age);

                    Level1Debug("Found value: " + splitString[s + 3]);
                    characters[c].value = splitString[s + 3];
                    Level2Debug("Value for Character " + c + " (" + characters[c].name + ") " + "is now: " + characters[c].value);

                    Level1Debug("Found info: " + splitString[s + 4]);
                    characters[c].info = splitString[s + 4];
                    Level2Debug("Info for Character " + c + " (" + characters[c].name + ") " + "is now: " + characters[c].info);

                }

                //Here comes the dialog.

                if (getDialog && splitString[s].Contains("[Dialog]"))
                {
                    Debug("Found [Dialog] for: " + characters[c].name);

                    for (int r = 0; r < splitString.Length; r++)
                    {
                        for (int q = 0; q < 30; q++) //Amount of questions to check.
                        {
                            if (splitString[r].Contains("+Q" + q.ToString()))
                            {

                                for (int que = 0; que < characters[c].questions.Count; que++)

                                {
                                    if (characters[c].questions[que].Q == (splitString[r])){
                                       Level5Debug("OH NO (" + splitString[r] +  ")");
                                        characters[c].questions.RemoveAt(que);
                                    }
                                }

                                //Here is where the values for Q need to be calculated
                                //The: the question can be added to the questions list with an added variable.
                                
                                characters[c].questions.Add(new Question(splitString[r], false)); //Adds a question to the list with the string d and a 'answered' value of false. So, an unaswered question.
                                Level2Debug("Added question " + q.ToString() + " '" + splitString[r] + "' to " + characters[c].name);

                                for (int a = 1; a < 10; a++) //Amount of answers to check.
                                {
                                    if (splitString.Length > (r + a))
                                    {
                                        if (splitString[r + a].Contains("+A" + q.ToString() + "." + a.ToString())) //CHeck is the line it's checking has an answer
                                        {

                                            if (splitString[r + a].Contains("[Q")) //Checks if the dialoug has a [Qx] (next dialoug) identifier. If not it'll give 666 value (I doubt we'll ever have more than 666 questions)
                                                                                   //If the value will be 666 there won't be a next dialog or a timer will start for when you recieve the next chat message.
                                            {
                                                //Get the int value out of the [Qx] identifier
                                                string nextQ = splitString[r + a].Substring(splitString[r + a].IndexOf('[') + 2);
                                                string[] nextQArray = nextQ.Split(']');
                                                Level4Debug("Found value for NextQuestion for " + splitString[r + a] + " = " + nextQArray[0]);


                                                if (splitString[r + a].Contains("[V"))
                                                {

                                                    //Get polarty
                                                    Level3Debug("Checking polarity for " + splitString[r + a]);
                                                    string polarity = splitString[r + a].Substring(splitString[r + a].IndexOf("[V") + 2);
                                                    string[] polarityArray = polarity.Split(' ');
                                                    Level4Debug("Found polarity " + polarityArray[0]);

                                                    //Get value
                                                    Level3Debug("Checking Value for " + splitString[r + a]);
                                                    //string valueString = polarity.Substring(polarity.IndexOf("]") - 1);
                                                    string[] valueStringArray = polarity.Split(']');
                                                    string[] tempValueStringArray = valueStringArray[0].Split(' ');
                                                    Level4Debug("valuestringarray.length: " + valueStringArray.Length);

                                                    if (valueStringArray.Length > 1)
                                                    {
                                                        Level4Debug("Found value " + tempValueStringArray[1]);
                                                    }
                                                    else {
                                                        Level4Debug("Found value " + tempValueStringArray[0]);
                                                    }

                                                    if (polarity.Contains("+")) //if positive
                                                    {
                                                        characters[c].questions[q - 1].answers.Add(new Answer(splitString[r + a], int.Parse(nextQArray[0]), float.Parse(tempValueStringArray[1])));
                                                    }
                                                    else if (polarity.Contains("-")) //if negztive
                                                    {
                                                        characters[c].questions[q - 1].answers.Add(new Answer(splitString[r + a], int.Parse(nextQArray[0]), -float.Parse(tempValueStringArray[1])));
                                                    }
                                                    else //if nothing
                                                    {
                                                        characters[c].questions[q - 1].answers.Add(new Answer(splitString[r + a], int.Parse(nextQArray[0]), float.Parse(tempValueStringArray[1])));
                                                    }

                                                    //characters[c].questions[q - 1].answers.Add(new Answer(splitString[r + a], int.Parse(valueString), 666));
                                                }
                                                else {

                                                    characters[c].questions[q - 1].answers.Add(new Answer(splitString[r + a], int.Parse(nextQArray[0]), 666));
                                                }
                                            }
                                            else
                                            {
                                                characters[c].questions[q - 1].answers.Add(new Answer(splitString[r + a], 666, 666));
                                            }
                                            Level3Debug("Added answer '" + splitString[r + a] + "' to " + characters[c].name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //Status Updates
                if (getStatusUpdates && splitString[s].Contains("[StatusUpdates]"))
                {
                    Debug("Found " + splitString[s]);
                    for (int i = 0; i < 20; i++) //amount of lines to check for status updates;
                    {
                        if (splitString.Length > (s + i))
                        {
                            //Level1Debug("Looking for SU cycle " + i);

                            if (splitString[s + i].Contains("+SU")) //If the line is a Status Update
                            {
                                Level2Debug("Found SU at " + i + ": " + splitString[s + i]);
                                //new List<Comment> comments = new List<Comment>(); 
                                characters[c].statusUpdates.Add(new StatusUpdate(splitString[s + i]));

                                for (int com = 0; com < 20; com++) {
                                    //Level2Debug("Looking for C cycle " + com);

                                    //Level3Debug("Comment splitstring is good for " + splitString[s + i + com]);
                                    if (splitString.Length > (s + i + com))
                                    {
                                        Level3Debug("Splittring length: " + splitString.Length + " | s+i+com: " + (s + i + com));

                                        if (splitString[s + i  + com].Contains("+C" ))
                                        {
                                            Level4Debug("Found C at " + (i + com) + ": " + splitString[s + i + com] + "  --  | i: " + i + " | s: " + s + " | com:" + com);

                                            string[] polarityArray = splitString[s + i + com].Split('.');
                                            string polarity = polarityArray[0].Substring(polarityArray[0].IndexOf(".") + 2);
                                            //Level4Debug(polarity);

                                            char _v = polarity[1];
                                            int v = int.Parse(_v.ToString());

                                            Level4Debug(v.ToString() + " " + (v-1).ToString());

                                            string _commentor = splitString[s + i + com].Substring(splitString[s + i + com].IndexOf(" ") + 2);
                                            string[] commentorArray = _commentor.Split(']');
                                            _commentor = commentorArray[0];


                                            Level5Debug(_commentor);

                                            //if (splitString[s + i + com]) 
                                            foreach (Character cha in characters)
                                            {
                                                foreach (StatusUpdate su in cha.statusUpdates)
                                                {
                                                    Level5Debug("Checking for " + su.content);
                                                    if (su.content.Contains("+SU" + (v ).ToString()))
                                                    {
                                                        Level5Debug("Adding '" + splitString[s + i + com] + "' to '" + su.content + "'.");
                                                        su.comments.Add(new Comment(splitString[s + i + com], _commentor));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //Friends
                if (getFriendsList && splitString[s].Contains("[Friends]"))
                {
                    Debug("Found " + splitString[s]);
                    for (int i = 0; i < 20; i++) //amount of lines to check for friends;
                    {
                        if (splitString.Length > (s + i))

                            if (splitString[s + i].Contains("+F"))
                            {
                                characters[c].friends.Add(splitString[s + i].ToString());
                            }
                    }
                }
            }
        }
    }
    public void CheckValue()
    {

    }


    public void GetProfilePic()
    {
        for (int p = 0; p < profilePicPaths.Count; p++)
        {

            Debug("_________________________________________________________________________________________________");
            Debug("Start of Profile Pic Loop: " + p);

            //Gets the file names from the paths and reformats them to be readable by Resource.Load.
            string output = profilePicPaths[p].Substring(profilePicPaths[p].IndexOf(',') + 1); //to get a 'Characters/Characters.txt path' (to be ready by Resoures.Load) I placed a ',' (not used in any other path).
            Level1Debug(output);
            output = output.Substring(0, output.Length - 4); //Removes '.txt'
                                                             //print(output);

            Level2Debug(output);

            profilePics.Add((Sprite)Resources.Load("," + output, typeof(Sprite))); //Takes the paths generated by 'output' and gets the ProfilePic.psd files.

            for (int c = 0; c < paths.Count; c++)
            {
                //string charName = paths[c].Substring(paths[p].IndexOf(',') + 22); //to get a 'Characters/Characters_xyxyxy.txt' path (to be ready by Resoures.Load) I placed a ',' (not used in any other path).
                //Level3Debug(charName);
                //charName = charName.Substring(0, charName.Length - 4); //Removes '.txt'
                //print(output);

                //Level3Debug(charName);

                Level3Debug("Getting First Name for " + characters[c].name);

                //string charName = characters[c].name.Substring(characters[c].name.IndexOf(' ') + 1); //to get a 'Characters/Characters_xyxyxy.txt' path (to be ready by Resoures.Load) I placed a ',' (not used in any other path).
                //Level4Debug(charName);

                string[] chars = characters[c].name.Split(' ');
                string charName = chars[0];

                Level4Debug("Get name: " + charName);


                if (profilePicPaths[p].Contains(charName))
                {
                    characters[c].profilePic = profilePics[p];
                }

            }

            //characters.Add(new Character()); //For each Characters.txt file found make a new entry in the list.
        }


    }

    public void CheckFollowThrough()
    {
        Debug("________________________________________________________________________________");
        Debug("CheckFollowthrough");

        for (int c = 0; c < characters.Count; c++)
        {
            for (int q = 0; q < characters[c].questions.Count; q++)
            {
                Debug("Checking: " + characters[c].questions[q].Q);
                if (characters[c].questions[q].Q.Contains("[FollowThrough [Q"))
                {
                    //string nextQ = characters[c].questions[q].Q.Substring(characters[c].questions[q].Q.IndexOf(']') + 2);
                    string[] nextQArray = characters[c].questions[q].Q.Split(']');
                    for (int n = 0; n < nextQArray.Length; n++)
                    {
                        Level1Debug("nextQArray pos " + n + ": " + nextQArray[n]);
                    }

                    Level2Debug(nextQArray[0].Substring(nextQArray[0].LastIndexOf('[') + 2));

                    Level4Debug("Found FollowThrough value for " + characters[c].questions[q].Q + " in  " + characters[c].name + ": " + nextQArray[0].Substring(nextQArray[0].LastIndexOf('[') + 2));


                    characters[c].questions[q].followThrough = int.Parse(nextQArray[0].Substring(nextQArray[0].LastIndexOf('[') + 2));
                }
                else
                {
                    characters[c].questions[q].followThrough = 666;
                }
            }
        }
    }


    public void Debug(string message)
    {
        if (debug)
            print(message);
    }

    public void Level1Debug(string message)
    {
        if (debug)
            print("     |--- " + message);
    }

    public void Level2Debug(string message)
    {
        if (debug)
            print("             |--- " + message);
    }

    public void Level3Debug(string message)
    {
        if (debug)
            print("                     |--- " + message);
    }

    public void Level4Debug(string message)
    {
        if (debug)
            print("                             |--- " + message);
    }

    public void Level5Debug(string message)
    {
        if (debug)
            print("                                       |--- " + message);
    }



}