/****
 * Created by: Kameron Eaton
 * Date Created: Feb 16, 2022
 * 
 * Last Edited by: NA
 * Last Edited: Feb 16, 2022
 * 
 * Description: Create trailing lines behind projectile
 ****/using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //singleton

    [Header("SET IN INSPECTOR")]
    public float minDistance = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake()
    {
        S = this; //set the singleton

        line = GetComponent<LineRenderer>(); //reference the line renderer
        line.enabled = false; //disable line renderer
        points = new List<Vector3>(); //new list

    }//end Awake

    public GameObject poi
    {
        get { return (_poi); }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoints();
            }
        }
    }
}//end ProjectileLine
