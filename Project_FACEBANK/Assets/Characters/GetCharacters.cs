using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

public class GetCharacters : MonoBehaviour
{

    public string[] allPaths;
    public List<string> paths;
    //public List<string> words;
    public List<TextAsset> characterFiles;
    public List<Character> character;



    // Use this for initialization
    void Start()
    {

        paths.Clear();
        allPaths = Directory.GetFiles(Application.dataPath, "*", SearchOption.AllDirectories);

        foreach (string p in allPaths)
        {
            if (p.Contains("Character_") && !p.Contains("meta"))
            {
                paths.Add(p);
                print(p);
            }
        }

        for (int i = 0; i < paths.Count; i++)
        {

            string output = paths[i].Substring(paths[i].IndexOf(',') + 1);
            print(output);
            output = output.Substring(0, output.Length - 4);
            print(output);

            characterFiles.Add((TextAsset)Resources.Load("," + output, typeof(TextAsset)));

            character.Add(new Character());
            string[] splitString = characterFiles[i].text.Split(new string[] { "\n" }, StringSplitOptions.None);
            print("Amount of lines in character doc: " + splitString.Length.ToString());




            for (int j = 0; j < splitString.Length; j++)
            {
                //print(splitString[i]);
                if (splitString[j].Contains("[Info]"))
                {
                    print("Found [Info] for: " + splitString[j+1]);
                    character[i].name = splitString[j + 1];
                    character[i].value = splitString[j + 2];
                    character[i].info = splitString[j + 3];
                }

                if (splitString[j].Contains("[Dialog]"))
                {
                    print("Found [Dialog] for: " + splitString[j + 1]);

                    for (int q = 0; q < splitString.Length; q++) {
                        if (splitString[q].Contains("1"))
                        {
                            print("Found Question: " + splitString[q]);
                            character[i].questions.Add(new Question(splitString[q],false));

                            for (int a = 0; a < character[i].questions.Count; a++)
                            {
                                if (character[i].questions[a].Q.Contains(splitString[q]))
                                {
                                    for (int k = 0; k < 5; k++)
                                    if (splitString.Length < k && splitString[q + k].Contains("2"))
                                    {
                                        print("Found Answer: " + splitString[q+k]);
                                        //character[i].questions[a].answers.Add(new Answer(splitString[q+1], 0));

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

