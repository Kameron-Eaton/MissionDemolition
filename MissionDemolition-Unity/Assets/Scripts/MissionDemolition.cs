/****
 * Created by: Kameron Eaton
 * Date Created: Feb 21, 2022
 * 
 * Last Edited by: NA
 * Last Edited: Feb 21, 2022
 * 
 * Description: Game manager script
 ****/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}//end GameMode

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; //Singleton

    [Header("SET IN INSPECTOR")]
    public Text uitLevel; // The UIText_Level Text
    public Text uitShots; // UIText_Shots Text
    public Text uitButton; // Text on UIButton_View
    public Vector3 castlePos; // The place to put castles
    public GameObject[] castles; // Array of the castles

    [Header("SET DYNAMICALLY")]
    public int level; // current level
    public int levelMax; // number of levels
    public int shotsTaken;
    public GameObject castle; // current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // FollowCam mode
    
    void Start()
    {
        S = this; // Define singleton

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        // Get rid of the old castle if one exists
        if(castle != null)
        {
            Destroy(castle);
        }//end if

        // Destroy old projectiles if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }//end if

        // Instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        // Reset camera
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        // Reset goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }//end Start

    void UpdateGUI()
    {
        // Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }//end UpdateGUI

    
    void Update()
    {
        UpdateGUI();

        // Check for level end
        if((mode == GameMode.playing) && Goal.goalMet)
        {
            // Change mode to stop checking for level end
            mode = GameMode.levelEnd;
            // Zoom out
            SwitchView("Show Both");
            // Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }//end if
    }//Update

    void NextLevel()
    {
        level++;
        if(level == levelMax)
        {
            level = 0;
        }//end if
        StartLevel();
    }//end NextLevel

    public void SwitchView(string eView = "")
    {
        if(eView == "")
        {
            eView = uitButton.text;
        }// end if
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }//end switch
    }//end SwitchView

    // Static method that allows code anywhere to increment shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }//end ShotFired
}//end MissionDemolition
