/****
 * Created by: Kameron Eaton
 * Date Created: Feb 9, 2022
 * 
 * Last Edited by: NA
 * Last Edited: Feb 9, 2022
 * 
 * Description: Manages the behavior of the slingshot
 ****/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    //*** VARIABLES ***//
    [Header("SET IN INSPECTOR")]
    public GameObject prefabProjectile;
    public float velocityMultiplier = 8f;

    [Header("SET DYNAMICALLY")]
    public GameObject launchPoint;
    public Vector3 launchPos; //launch positon of projectile
    public GameObject projectile; //instance of projectile
    public bool aimingMode; //is player aiming
    public Rigidbody projectileRB; //rigid body of projectile

    private void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint"); //find child transform
        launchPoint = launchPointTrans.gameObject; //the game object of child object
        launchPoint.SetActive(false); //disable game object
        launchPos = launchPointTrans.position;
    }//end Awake

    private void Update()
    {
        if (!aimingMode)
            return;

        //get current mouse position in 2d screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos; //pixel amount of change between mouse3d and launchPos

        //limit mouseDelta to slingshot collider radius

        float maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize(); //sets the vector to the same direction, but length is 1.0
            mouseDelta *= maxMagnitude;
        }//end if

        //move projectile to new position

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            projectileRB.isKinematic = false;
            projectileRB.velocity = -mouseDelta * velocityMultiplier;
            projectile = null; //forgets the last instance of projectile
        }
        
    }//end Update

    private void OnMouseEnter()
    {
        launchPoint.SetActive(true); //enable game object
    }//end OnMouseEnter

    private void OnMouseExit()
    {
        launchPoint.SetActive(false); //disable game object
    }//end OnMouseExit

    private void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;
        projectileRB = projectile.GetComponent<Rigidbody>();
        projectileRB.isKinematic = true;
    }//end OnMouseDown
}//end Slingshot
