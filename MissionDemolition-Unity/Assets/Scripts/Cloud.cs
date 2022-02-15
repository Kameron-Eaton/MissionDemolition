/****
 * Created by: Kameron Eaton
 * Date Created: Feb 14, 2022
 * 
 * Last Edited by: NA
 * Last Edited: Feb 14, 2022
 * 
 * Description: Create a randomly generated cloud
 ****/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    /**** VARIABLES ****/
    [Header("SET IN INSPECTOR")]

    public GameObject cloudSphere;

    public int numberSphereMin = 6;
    public int numberSphereMax = 10;
    public Vector2 sphereScaleRangeX = new Vector2(4, 8);
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1);
    public float scaleYMin = 2f;

    private List<GameObject> spheres;

    // Start is called before the first frame update
    void Start()
    {
        spheres = new List<GameObject>();
        int num = Random.Range(numberSphereMin, numberSphereMax);

        for(int i = 0; i<num; i++)
        {
            GameObject sp = Instantiate<GameObject>(cloudSphere);
            spheres.Add(sp);

            Transform spTrans = sp.transform;
            spTrans.SetParent(this.transform);

            //Randomly assign a position
            Vector3 offset = Random.insideUnitSphere;
            offset.x *= sphereOffsetScale.x;
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            spTrans.localPosition = offset;

            //Randomly assign scale
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.y = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            //Adjust y scale by x distance from core
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);
            spTrans.localScale = scale;
        }//end for
    }//end Start

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
       */
    }//end Update

    void Restart()
    {
        // Clear out old spheres
        foreach (GameObject sp in spheres)
        {
            Destroy(sp);
        }//end foreach
        Start();
    }//end Restart
}//end Cloud
