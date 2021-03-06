﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour {
    public static BallControl bc;

    public Camera myCamera;
    public GameObject ball;
    public GameObject ballKilledParticle;
    public Renderer targetSprite;
    public GameObject textReady;
    public GameObject textWaiting;

    [HideInInspector]
    public Vector3 aimAt;
    Vector3 direction;
    float startPowerTime, endPowerTime, timeInterval;
    float ballForce = 500;
    float minTimeInterval = 0.2f;
    float aimTime = 0.15f;
    float minMovementThreshold = 0.3f;

    [HideInInspector]
    public bool aimed;
    GameObject ballInPlay;

    bool moving;
    Renderer ts; //targetsprite

    private void Start()
    {
        //set ball force dependant on mass of ball
        ballForce *= ball.GetComponent<Rigidbody>().mass;

        aimed = false;
        moving = false;
    }
    
    // Update is called once per frame
    void Update ()
    {
        //aimed may be set to false if camera moves after aiming, this removes the target
        if(aimed==false)
        {
            if (ts != null)
            {
                Destroy(ts);
            }
        }

        #region Mouse Controls
        if (GameController.gc.debug == true)
        {
            //If nothing is moving, we are ready to go
            if (moving == false)
            {
                textReady.SetActive(true);
                textWaiting.SetActive(false);

                //register aiming has commenced
                if (Input.GetMouseButtonUp(0))
                {
                    aimAt = myCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 2)); //need to add a z amount to it otherwise it doesn't work
                                                                                                     //Delete old target if there is one
                    if (ts != null)
                    {
                        Destroy(ts);
                    }
                    //Display new target sprite at selected location
                    ts = Instantiate(targetSprite, aimAt, Quaternion.identity);
                    ts.transform.LookAt(myCamera.transform.position, -Vector3.up);
                    aimed = true;
                }

                //Get the swipe...
                if (Input.GetMouseButtonDown(1))
                {
                    startPowerTime = Time.time;
                }
                if ((Input.GetMouseButtonUp(1)) && (aimed == true))
                {
                    endPowerTime = Time.time;

                    ThrowBall();
                    aimed = false;
                }
            }
        }
        #endregion
        #region Touch Controls
        
        else
        {
            if (moving == false) // if nothing moving, ready for next shot
            {
                textReady.SetActive(true);
                textWaiting.SetActive(false);

                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);

                    // Handle finger movements based on touch phase.
                    switch (touch.phase)
                    {
                        // Record initial touch position.
                        case TouchPhase.Began:
                            startPowerTime = Time.time;
                            break;

                        case TouchPhase.Ended:
                            endPowerTime = Time.time;
                            if (endPowerTime - startPowerTime < aimTime)
                            {
                                aimAt = myCamera.ScreenToWorldPoint((Vector3)(Input.GetTouch(0).position) + new Vector3(0, 0, 2)); //need to add a z amount to it otherwise it doesn't work 
                                                                                                                                   //Delete old target if there is one
                                if (ts != null)
                                {
                                    Destroy(ts);
                                }
                                //Display new target sprite at selected location
                                ts = Instantiate(targetSprite, aimAt, Quaternion.identity);
                                ts.transform.LookAt(myCamera.transform.position, -Vector3.up);

                                aimed = true;
                            }
                            else
                            {
                                if (aimed == true)
                                {
                                    ThrowBall();
                                    aimed = false;
                                }
                            }
                            break;
                    }
                }
            }
        }
        
        #endregion
    }

    void ThrowBallViaTouch()
    {
        timeInterval = endPowerTime - startPowerTime;
        direction = (aimAt - (myCamera.transform.localPosition));

        // Remove the target
        if (ts != null)
        {
            Destroy(ts);
        }

        //Prevent ball from tavelling too fast
        if (timeInterval < minTimeInterval)
        {
            timeInterval = minTimeInterval;
        }

        ballInPlay = Instantiate(ball, myCamera.transform.localPosition, Quaternion.identity);
        ballInPlay.GetComponent<Rigidbody>().AddForce((direction / timeInterval * ballForce));

        //set aimed to false since shot is done
        aimed = false;
    }

    void ThrowBall()
    {
        timeInterval = endPowerTime - startPowerTime;
        direction = (aimAt - (myCamera.transform.localPosition));

        // Remove the target
        if (ts != null)
        {
            Destroy(ts); 
        }

        //Prevent ball from tavelling too fast
        if(timeInterval < minTimeInterval)
        {
            timeInterval = minTimeInterval;
        }

        //instantiate the ball and apply the force
        ballInPlay = Instantiate(ball, myCamera.transform.localPosition, Quaternion.identity);
        ballInPlay.GetComponent<Rigidbody>().AddForce((direction / timeInterval * ballForce));
        
        StartCoroutine(CheckIfBlocksMoving());
    }

    IEnumerator CheckIfBlocksMoving() //called from the ball throw
    {
        Rigidbody[] blocks;

        //Get all the rigidbodies in the scene
        blocks = GameObject.FindObjectsOfType<Rigidbody>();

        //give the ball time to get up to speed so this doesn't fail immediately
        yield return new WaitForSeconds(0.25f);

        //set moving to true as we have thrown ball.
        moving = true;
        textWaiting.SetActive(true);
        textReady.SetActive(false);

        //keep moving set to true whilst things are in motion. This is used to prevent player
        //taking another shot while things are still in play
        while (moving == true)
        {
            moving = false;

            foreach (Rigidbody rb in blocks)
            {
                if (rb != null)
                {
                    if (rb.velocity.magnitude > minMovementThreshold)
                    {
                        moving = true;
                    }
                }
            }
            yield return null;
        }

        //once everything has stopped moving, destroy the ball
        if (ballInPlay != null)
        {
            Instantiate(ballKilledParticle, ballInPlay.transform.localPosition, Quaternion.identity);
            ballInPlay.GetComponent<Renderer>().enabled = false;
            Destroy(ballInPlay.GetComponent<Rigidbody>());
            Destroy(ballInPlay, 3);
        }
        yield return null;
    }
}
