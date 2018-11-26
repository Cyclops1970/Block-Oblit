using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour {
    public static BallControl bc;

    public Camera myCamera;
    public GameObject ball;
    public Renderer targetSprite;
    [HideInInspector]
    public Vector3 aimAt;
    Vector3 direction;
    float startPowerTime, endPowerTime, timeInterval;
    float ballForce = 500;
    float minTimeInterval = 0.2f;
    float aimTime = 0.15f;
    [HideInInspector]
    public bool aimed;

    Renderer ts; //targetsprite

    private void Start()
    {
        aimed = false;
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
            if ((Input.GetMouseButtonUp(1))&&(aimed==true))
            {
                endPowerTime = Time.time;

                ThrowBallViaMouse();
                aimed = false;
            }
        }
        #endregion
        #region Touch Controls
        else
        {
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
                                ThrowBallViaTouch();
                            }
                        }
                        break;
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

        GameObject ballInPlay = Instantiate(ball, myCamera.transform.localPosition, Quaternion.identity);
        ballInPlay.GetComponent<Rigidbody>().AddForce((direction / timeInterval * ballForce));

        //set aimed to false since shot is done
        aimed = false;
    }

    void ThrowBallViaMouse()
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
            print("Shortest time reached.");
        }

        GameObject ballInPlay = Instantiate(ball, myCamera.transform.localPosition, Quaternion.identity);
        ballInPlay.GetComponent<Rigidbody>().AddForce((direction / timeInterval * ballForce));
        //ballInPlay.GetComponent<Rigidbody>().AddTorque((direction / timeInterval * ballForce));
    }
}
