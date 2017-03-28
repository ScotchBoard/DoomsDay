﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Vector3 distForGravity;

    // The, uhm.. Earth
    public GameObject earth;

    public Vector3 earthToPlanetDist;

    // The strength of the gravitational pull
    public float gravitationalPull;

    // Used to calibrate planet's gravity
    public float powerPerFrame;

    // The radius where gravity start to roll
    public float maxRadius;

    // Checks whether the player is attracted or not
    private bool planetAttraction = false;

    private Vector3 offset;
    private Vector3 direction;

    // Checks if the player got passed the planet
    private bool beyond2Souls = false;

    // Checks if the player is trying to escape the planetary gravity
    private bool runYouFool = false;

    // The positions of the planet's center
    private float xC;
    private float yC;
    private float zC;

    float gravity = 0.0f;

    private void Start()
    {
        offset = Vector3.zero;
        direction = Vector3.zero;

        xC = this.transform.position.x;
        yC = this.transform.position.y;
        zC = this.transform.position.z;

        distForGravity = Vector3.zero;
    }

    void Update()
    {
        offset = earth.transform.position - this.transform.position;
        earthToPlanetDist = offset;

        direction = offset;
        direction.z = 0;

        //Debug.Log(offset.magnitude + " " + ((maxRadius - this.transform.localScale.x) / 2 + 10));

        var x = Mathf.Pow((earth.transform.position.x - xC), 2);
        var y = Mathf.Pow((earth.transform.position.y - yC), 2);
        var z = Mathf.Pow((earth.transform.position.z - zC), 2);

        ///////
        if (earth.transform.position.z >= (this.transform.position.z + this.transform.localScale.z / 2) - 20)
        {
            beyond2Souls = true;
        }

        ///////
        if (gravity >= 0 && planetAttraction && ((Input.GetKey(GameManager.GM.leftFP) && this.transform.position.x > earth.transform.position.x)
            || (Input.GetKey(GameManager.GM.rightFP) && this.transform.position.x < earth.transform.position.x)
            || (Input.GetKey(GameManager.GM.upwardFP) && this.transform.position.y < earth.transform.position.y)
            || (Input.GetKey(GameManager.GM.downwardFP) && this.transform.position.y > earth.transform.position.y)))
        {
            //gravity -= 0.5f*1 / (Mathf.Abs(offset.magnitude - this.transform.localScale.x));//Time.deltaTime;
            //gravity = Mathf.Abs(gravity);
            gravity -= powerPerFrame * 0.02f;// Time.deltaTime;
            runYouFool = true;
        }

        if (gravity <= gravitationalPull && planetAttraction && !runYouFool)
        {
            //gravity += 0.3f*1 / (Mathf.Abs(offset.magnitude - this.transform.localScale.x));//Time.deltaTime;
            gravity += powerPerFrame * 0.02f;// Time.deltaTime;
        }

        //Debug.Log(Mathf.Abs(1 / (offset.magnitude - this.transform.localScale.x))); 
        //Debug.Log(gravity);

        ///////
        if ((x + y + z) <= Mathf.Pow(maxRadius - this.transform.localScale.x, 2) && !beyond2Souls)
        {
            planetAttraction = true;
            earth.GetComponent<Rigidbody>().AddForce(-direction * gravity, ForceMode.Acceleration);

            if (earth.transform.position.z >= this.transform.position.z && PlayerStatus.isAlive)
            {
                if (earth.GetComponent<Rigidbody>().velocity.z <= Controller.staticForwardSpeed + 30)
                    earth.GetComponent<Rigidbody>().AddForce(Vector3.forward * 100, ForceMode.Acceleration);
            }
        }
        else
        {
            gravity = 0.0f;
            planetAttraction = false;
            beyond2Souls = false;
        }

        runYouFool = false;
        //Debug.Log(offset.magnitude + " " + ((maxRadius - this.transform.localScale.x) / 2 + 10));
        /*
        Debug.Log("Planet: " + this.name + " " +
            "Attracted: " + planetAttraction + " " + 
            "Warning: " + PlayerStatus.warning + " " +
            "Alive: " + PlayerStatus.isAlive + " " + 
            "Camera: " + PlayerStatus.cameraFollow + " " +
            "Key: " + Controller.ignoreKey);
            */
        //////
        if (planetAttraction)
        {
            //Debug.Log(offset.magnitude);
            PlayerStatus.planetName = this.name;
            if (offset.magnitude <= (/*1 / 2.0 **/ (maxRadius - this.transform.localScale.x) / 2 + 10) && (this.transform.position.z + this.transform.localScale.z / 2) - 25 >= earth.GetComponent<Rigidbody>().transform.position.z)
            {
                PlayerStatus.warning = true;
                PlayerStatus.itsAGo = true;
            }
            else
            {
                PlayerStatus.warning = false;
            }

            if (offset.magnitude <= ((maxRadius - this.transform.localScale.x) / 3 + 7) && this.transform.position.z >= earth.GetComponent<Rigidbody>().transform.position.z)//(this.transform.position.z + this.transform.localScale.z / 2) >= earth.GetComponent<Rigidbody>().transform.position.z)//(1 / 3.0 * (maxRadius - this.transform.localScale.x)))// 
            {
                PlayerStatus.cameraFollow = false;
                Controller.ignoreKey = true;
            }
            if(!PlayerStatus.cameraFollow && PlayerStatus.isAlive)
            {
                earth.GetComponent<Rigidbody>().AddForce(-direction * 5, ForceMode.Acceleration);
            }

        //(this.transform.position.z + this.transform.localScale.z))//
        }
    }
    /*
    void SetGravity()
    {
        var behind = false;
        var front = false;

        if (offset.z <= 0 && offset.z >= -maxRadius)
        {
            behind = true;
            front = false;
        }
        else
        {
            if (offset.z >= 0 && offset.z <= maxRadius)
            {
                front = true;
                behind = false;
            }
        }
        if (offset.z > maxRadius || offset.z < -maxRadius)
        {
            front = false;
            behind = false;
        }
        var isInZRange = behind || front;

        // for case offset.x <= maxRadius && offset.y >= -maxRadius
        if (offset.x > maxRadius && offset.y >= -maxRadius)
        {
            isAttracted = false;
        }
        else
            if (offset.x <= maxRadius && offset.y < -maxRadius)
        {
            isAttracted = false;
        }
        else
                if (offset.x <= maxRadius && offset.y >= -maxRadius)
        {
            isAttracted = true;
        }

        // for case offset.x >= -maxRadius && offset.y >= -maxRadius
        if (offset.x < -maxRadius && offset.y >= -maxRadius)
        {
            isAttracted = false;
        }
        else
            if (offset.x >= -maxRadius && offset.y < -maxRadius)
        {
            isAttracted = false;
        }
        else
                if (offset.x >= -maxRadius && offset.y >= -maxRadius)
        {
            isAttracted = true;
        }

        //for case offset.x >= -maxRadius && offset.y <= maxRadius
        if (offset.x < -maxRadius && offset.y <= maxRadius)
        {
            isAttracted = false;
        }
        else
        if (offset.x >= -maxRadius && offset.y > maxRadius)
        {
            isAttracted = false;
        }
        else
        if (offset.x >= -maxRadius && offset.y <= maxRadius)
        {
            isAttracted = true;
        }

        //for case offset.x <= maxRadius && offset.y <= maxRadius
        if (offset.x > maxRadius && offset.y <= maxRadius)
        {
            isAttracted = false;
        }
        else
            if (offset.x <= maxRadius && offset.y > maxRadius)
        {
            isAttracted = false;
        }
        else
                if (offset.x <= maxRadius && offset.y <= maxRadius)
        {
            isAttracted = true;
        }

        if (offset.x <= -maxRadius && offset.y >= -maxRadius)
        {
            isAttracted = false;
        }

        if (!isInZRange)
            isAttracted = false;
    }
    */
}