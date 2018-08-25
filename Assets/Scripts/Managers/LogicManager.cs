using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;
using UnityEngine.UI;

public class LogicManager : MonoBehaviour
{
    public static string[] tagsReference = File.ReadAllLines(@"C:\Users\marsh\Documents\RogueLike\Assets\References\Tags.txt");
    public static string[] scriptListReference = File.ReadAllLines(@"C:\Users\marsh\Documents\RogueLike\Assets\References\ScriptList.txt");

    public void LogicController(int lmid)                                               // destroys necessary game object based on logic.Tags
    {

        Debug.Log("LogicController: lmid = " + lmid);

        // set logic from reference
        string tags = tagsReference[lmid];
        string scriptList = scriptListReference[lmid];
        Logic logic = gameObject.AddComponent<Logic>();
        logic.Tags = tags;
        logic.ScriptsList = scriptList;
       
        if (logic.ScriptsList.Contains("BoardController"))
        {
            GameManager.boardScript.BoardController(lmid);
        }
        if (logic.ScriptsList.Contains("TextController"))
        {
            GameManager.textScript.TextController(lmid);
        }
        if (logic.ScriptsList.Contains("Save"))
        {

        }
        if (logic.ScriptsList.Contains("Load"))
        {

        }
        if (logic.ScriptsList.Contains("Sweeper"))
        {
            logic.Sweeper(logic.Tags);
        }
    }
}

public class Logic : MonoBehaviour
{
    public string Tags { get; set; }
    public string ScriptsList { get; set; }


    public void Sweeper(string tags)
    {
        Debug.Log("Sweeping: " + tags);

        GameObject[] toDestroy;
        string[] myStringArray = tags.Split(',');

        foreach (string tag in myStringArray)
        {
            toDestroy = GameObject.FindGameObjectsWithTag(tag);

            for (var i = 0; i < toDestroy.Length; i++)
            {
                Destroy(toDestroy[i]);
            }
        }
    }
}
