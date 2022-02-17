/****
 * Created by: Kameron Eaton
 * Date Created: Feb 16, 2022
 * 
 * Last Edited by: NA
 * Last Edited: Feb 16, 2022
 * 
 * Description: Create trailing lines behind projectile
 ****/
using System.Collections;
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

    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }//end Clear

    public void AddPoints()
    {
        Vector3 pt = _poi.transform.position;
        if(points.Count > 0 && (pt-lastPoint).magnitude < minDistance)
        {
            return;
        }//end if

        if(points.Count == 0)
        {
            Vector3 launchPointDiff = pt - Slingshot.LAUNCH_POS;
            points.Add(pt + launchPointDiff);
            points.Add(pt);
            line.positionCount = 2;
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true;
        }//end if
        else
        {
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }//end AddPoints

    public Vector3 lastPoint
    {
        get
        {
            if(points == null)
            {
                return (Vector3.zero);
            }//end if
            return (points[points.Count - 1]);
        }   
    }//end lastPoint

    void FixedUpdate()
    {
        if(poi == null)
        {
            if(FollowCam.POI != null)
            {
                if(FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }//end if
                else
                {
                    return;
                }//end else
            }//end if
            else
            {
                return;
            }//end else
        }//end if
        AddPoints();
        if(FollowCam.POI == null)
        {
            poi = null;
        }//end if
    }//end FixedUpdate
}//end ProjectileLine


