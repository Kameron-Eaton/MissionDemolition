/****
 * Created by: Kameron Eaton
 * Date Created: Feb 14, 2022
 * 
 * Last Edited by: NA
 * Last Edited: Feb 14, 2022
 * 
 * Description: Creates multiple clouds
 ****/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("SET IN INSPECTOR")]
    public int numClouds = 40; //number of clouds to make
    public GameObject cloudPrefab; //prefab for the clouds
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10); 
    public float cloudScaleMin = 1; //min scale of cloud
    public float cloudScaleMax = 3; //max scale of cloud
    public float cloudSpeedMult = 0.5f; //adjust speed of clouds

    private GameObject[] cloudInstances;

    void Awake()
    {
        //Make an array large enough to hold all the cloud instances
        cloudInstances = new GameObject[numClouds];
        //Find CloudAnchor parent
        GameObject anchor = GameObject.Find("CloudAnchor");
        //Iterate through and make Clouds
        GameObject cloud;
        for(int i = 0; i < numClouds; i++)
        {
            //Make Instance of cloud prefab
            cloud = Instantiate<GameObject>(cloudPrefab);
            //Position cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //Scale cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //Smaller clouds should be nearer the ground
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //Smaller clouds should be further away
            cPos.z = 100 - 90 * scaleU;
            //Apply these transforms to cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //Make cloud a child of anchor
            cloud.transform.SetParent(anchor.transform);
            //Add the cloud to cloudInstance
            cloudInstances[i] = cloud;
        }//end for
    }//end Awake

    void Update()
    {
        //Iterate over each cloud that was created
        foreach(GameObject cloud in cloudInstances)
        {
            //Get cloud scale and position
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //Move larger clouds faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //If a cloud has moved too far to the left
            if(cPos.x <= cloudPosMin.x)
            {
                //Move it to the far right
                cPos.x = cloudPosMax.x;
            }//end if
            //Apply the new position to cloud
            cloud.transform.position = cPos;
        }//end foreach
    }//end Update
}//end CloudCrafter
