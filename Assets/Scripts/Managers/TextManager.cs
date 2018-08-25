using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Canvas canvas;                                                                                                                       
    public Text screenText;
    public RectTransform rectTransform;
    public int menuSelection;

    public static string[] xOffSetReference = File.ReadAllLines(@"C:\Users\marsh\Documents\RogueLike\Assets\References\XOffSet.txt");           // xOffSet Reference
    public static string[] yOffSetReference = File.ReadAllLines(@"C:\Users\marsh\Documents\RogueLike\Assets\References\YOffSet.txt");           // yOffSet Reference
    public static string[] redirectReference = File.ReadAllLines(@"C:\Users\marsh\Documents\RogueLike\Assets\References\Redirect.txt");         // redirect Reference
    public static string[] textReference = File.ReadAllLines(@"C:\Users\marsh\Documents\RogueLike\Assets\References\Text.txt");                 // text Reference
    public static string[] formatReference = File.ReadAllLines(@"C:\Users\marsh\Documents\RogueLike\Assets\References\Format.txt");                 
    
    private void Start()                                                                                                                        // component references
    {
        canvas.GetComponent<Canvas>();
        screenText.GetComponent<Text>();
        rectTransform.GetComponent<RectTransform>();
        canvas.worldCamera = Camera.main;
    }

    public void TextController(int lmid)                                                        // given id for reference lookup displays appropriate text on screen, triggers MenuController if needed
    {
        Debug.Log("TextController: lmid = " + lmid);

        // set menu from reference
        float xOffSet = float.Parse(xOffSetReference[lmid]);                                    // set xOffSet from reference, convert to float
        float yOffSet = float.Parse(yOffSetReference[lmid]);                                    // set yOffset from reference, convert to float
        int[] redirect = DelimitedStringsToInt(redirectReference[lmid], ',');                   // set redirect from reference using DelimitedStringsToInt method to separate and parse values                
        string text = textReference[lmid];                                                      // set text from reference
        string format = formatReference[lmid];                                                  // set format from reference, chooses optional formatting, unity default if -1
        Menu menu = new Menu(xOffSet, yOffSet, redirect, text, format);                         // declare new menu

        // If we're going to be redirected, 
        // send information to the menuSelector.
        if (menu.Redirect[0] != -1)                                                             // if menu.Redirect isn't -1, we need a menu, the user will make a selection to progress
        {
            GameManager.inMenu = true;
            menuSelection = 0;
            StartCoroutine(menu.MenuSelector(GameManager.inMenu, menu.Redirect, menuSelection));
        }

        // If we're not going to be redirected,
        // display text on the screen.
        if (menu.Format != "-1")                                                                // if menu.Format isn't -1, we want to change the format from Unity default
        {
            // vertical text if menu.Format = 0
            if (menu.Format == "0")
            {
                menu.Text = VerticalTextBuilder("\n", menu.Text); 
            }
        }

        // set text from menu.Text, instantiate canvas, set canvas position
        screenText.text = menu.Text;                                                            // place the text
        Canvas instance = Instantiate(canvas, new Vector3(0, 0, 0f), Quaternion.identity);      
        screenText.rectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        screenText.rectTransform.transform.localPosition = new Vector3
            (screenText.rectTransform.transform.position.x + menu.XoffSet,
            screenText.transform.position.y + menu.YOffSet,
            screenText.rectTransform.transform.position.z);
    }

    // takes comma delimited string of integers and parses to int[]
    public static int[] DelimitedStringsToInt(string phrase, char delimiter)                    // given a delimiter and phrase, parses values to int[]
    {
        List<int> myList = new List<int>();
        string[] mySplitPhrase = phrase.Split(delimiter);

        foreach (string word in mySplitPhrase)
        {
            int myInt = int.Parse(word);
            myList.Add(myInt);
        }

        int[] myIntArray = myList.ToArray();
        return myIntArray;
    }
    // takes string of reference text, and separates it into multiple lines
    public static string VerticalTextBuilder(string separator, string reference)
    {
        string[] myStringArray = reference.Split(',');
        string result = "";
        for (int i = 0; i < myStringArray.Length; i++)
        {
            if (i > 0)            
                result += separator;            
                result += myStringArray[i];
        }
        return result;
    }

}

public class Menu
{
    public float XoffSet { get; set; }
    public float YOffSet { get; set; }
    public int[] Redirect { get; set; }
    public string Text { get; set; }
    public string Format { get; set; }
    public Menu(float xOffSet, float yOffSet, int[] redirect, string text, string format)
    {
        XoffSet = xOffSet;
        YOffSet = yOffSet;
        Redirect = redirect;
        Text = text;
        Format = format;
    }

    public IEnumerator MenuSelector(bool wait, int[] redirect, int selection)                       // lets the user choose an option to progress based on on screen cues
    {                                                                            
        while (wait == true)                                                     
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (selection < redirect.Length - 1)
                {
                    selection++;
                    Debug.Log("Increased Selection");
                    Debug.Log(redirect[selection]);
                }

                else
                {
                    Debug.Log("Array index out of range: end of array.");
                    Debug.Log(redirect[selection]);
                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (selection > 0)
                {
                    selection--;
                    Debug.Log("Decreased Selection");
                    Debug.Log(redirect[selection]);
                }

                else
                {
                    Debug.Log("Array index out of range: start of array.");
                    Debug.Log(redirect[selection]);
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {

                Debug.Log("Final selection = " + redirect[selection]);
                GameManager.inMenu = false;
                GameManager.logicScript.LogicController(redirect[selection]);
                wait = false;
                GameManager.inMenu = false;
            }
            yield return null;
        }
        Debug.Log("Coroutine: MenuSelector - Complete");
    }
}
