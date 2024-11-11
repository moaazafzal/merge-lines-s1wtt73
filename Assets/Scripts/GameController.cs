using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private General generalScript;
    private Admob admobScript;

    public Spot lastObj;
    public GameObject spotPrefab;
    
    public bool touchStart;
    public bool touching;
    public int curNum = 1;
    private Spot[] allSpots;
    private Lines linesScript;
    public GameObject canvasObject;
    private UI_Script uiScript;

    public bool getCode;
    [TextArea(5,5)] public string generatedCode = "";

    public string[] levels;
    
    
    private void Start()
    {
        generalScript = FindObjectOfType<General>();
        admobScript = FindObjectOfType<Admob>();

        allSpots = FindObjectsOfType<Spot>();
        linesScript = FindObjectOfType<Lines>();
        uiScript = FindObjectOfType<UI_Script>();
        
        canvasObject.SetActive(false);
        uiScript.SetLevelText();
        
        if (getCode)
        {
            //generate level code if Get Code ticked
            GenerateCode();
        }
        else
        {
            //create level based on the level code if Get Code isn't ticked
            CreateFromCode(levels[generalScript.level]);
        }
    }
    
    public void GenerateCode()
    {
        // getting position of each Spot and save in generatedCode variable
        generatedCode = "";
        Spot[] spots = FindObjectsOfType<Spot>();
        foreach (var spot in spots)
        {
            var position = spot.transform.position;
            generatedCode += (Mathf.Round(position.x *100) /100) + "," + (Mathf.Round(position.y *100) /100) + "|";
        }
    }
    
    
    void CreateFromCode(string code)
    {
        // destroy existing Spots
        Spot[] spots = FindObjectsOfType<Spot>();
        foreach (var spot in spots)
        {
            Destroy(spot.gameObject);
        }
        
        // get back to menu if there is no code for level
        if (code == "")
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            // create Spots based on the level code and move them to AllSpots game object
            GameObject spotsParent = new GameObject("AllSpots");
    
            string[] mainToken = code.Split('|');
            for (int i = 0; i < mainToken.Length-1; i++)
            {
                string[] token = mainToken[i].Split(',');
                
                float x = float.Parse(token[0]);
                float y = float.Parse(token[1]);
                Vector3 pos = new Vector3(x,y,0);
                
                Spot newObj = FindObjectOfType<Spot>();
                
                GameObject newSpot = Instantiate(newObj.gameObject, pos, transform.rotation);
                newSpot.name = "Spot";
                newSpot.transform.parent = spotsParent.transform;
                newSpot.GetComponent<Spot>().enabled = true;
                newSpot.GetComponent<BoxCollider2D>().enabled = true;
    
            }
        }
    }
    
    // this function calls from Spot.cs
    public void ClearSpots(int number)
    {
        // deselecting all Spots with num => number
        allSpots = FindObjectsOfType<Spot>();
        
        foreach (var curSpot in allSpots)
        {
            if (curSpot.num >= number)
            {
                curSpot.num = 0;
                curSpot.colored = false;
                curNum -= 1;
                curSpot.AfterSetColor();
                linesScript.DrawLines();
            }
        }

        // getting lastobj after deselecting
        float v = -Mathf.Infinity;
        foreach (var curSpot in allSpots)
        {
            if (curSpot.colored && curSpot.num > v)
            {
                v = curSpot.num;
                lastObj = curSpot;
            }
        }
    }
    

    public void CheckLevelDone()
    {
        // check if all spots selected(colored)
        bool allFull = true;
        Spot[] spots = FindObjectsOfType<Spot>();
        foreach (var curSpot in spots)
        {
            if (!curSpot.colored)
            {
                allFull = false;
            }
        }

        // if all spots are selected, levels is complete
        if (allFull)
        {
            canvasObject.SetActive(true);
            
            // save new passed level in PlayerPrefs
            generalScript.level++;
            if (!generalScript.passedLevels.Contains("," +generalScript.level+ ","))
            {
                PlayerPrefs.SetInt("lvl", generalScript.level);
                
                generalScript.passedLevels += generalScript.level + ",";
                PlayerPrefs.SetString("PassedLevels", generalScript.passedLevels);
            }

            generalScript.PlaySound("LevelComplete");
            StartCoroutine(WaitThenLoadScene(1.5f));
        }
        
    }

    private IEnumerator WaitThenLoadScene(float time)
    {
        // show InterstitialAd every 3 levels
        if (generalScript.level%3 == 0)
        {
            admobScript.ShowInterstitialAd();
        }
        
        // rerun Game scene (runs next level)
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Game");
    }
    
}
