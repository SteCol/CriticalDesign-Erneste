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
            print(splitString.Length.ToString());
            
            
            

            for (int j = 0; j < splitString.Length; j++)
            {
                //print(splitString[i]);
                if (splitString[j].Contains("[Info]"))
                {
                    //print("Found [Info] for: " + splitString[j+1]);
                    character[i].name = splitString[j+1];
                    character[i].value = splitString[j+2];
                    character[i].info = splitString[j+3];
                }

                if (splitString[j].Contains("[Dialog]"))
                {
                    print("Found [Dialog] for: " + splitString[j + 1]);
                    foreach (string d in splitString) {
                        if (d.Contains("1")) {
                            print(d);
                        }

                        if (d.Contains("2"))
                        {
                            print(d);
                        }
                        if (d.Contains("3"))
                        {
                            print(d);
                        }




                    }
                }
            }

        }




    }

    // Update is called once per frame
    void Update()
    {

    }
}
