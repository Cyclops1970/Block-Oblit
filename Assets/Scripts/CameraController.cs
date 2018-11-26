using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController cc;
    public BallControl bc;

    [Tooltip("What the camer will look at and rotate around.")]
    public GameObject pivot; // what the camer will look at and rotate around
    public Camera myCamera;

    private Vector3 pivotPosition;

    private Vector2 deltaPosition;
    private float speed = 50;
    private float keyboardSpeed = -8;

    // Use this for initialization
	void Start ()
    {
        //setup what the camera is looking at
        pivotPosition = pivot.transform.localPosition;
        myCamera.transform.LookAt(pivotPosition);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        myCamera.transform.LookAt(pivotPosition);
        if (GameController.gc.debug == true)
        {
            #region Mouse Controls
            if (Input.GetKey(KeyCode.S)) //down (was up, need to change all the code instead of just the input)
            {
                //works
                //transform.RotateAround(pivotPosition, transform.right, -2 * speed * Time.deltaTime);

                float upY = transform.localPosition.y + keyboardSpeed * Time.deltaTime;
                myCamera.transform.localPosition = new Vector3(transform.localPosition.x, upY, transform.localPosition.z);
                bc.aimed = false;
            }
            if (Input.GetKey(KeyCode.W)) //up (was down, need to change all the code instead of just the input)
            {
                //works
                //transform.RotateAround(pivotPosition, -transform.right, -2 * speed * Time.deltaTime);

                float downY = transform.localPosition.y + -keyboardSpeed * Time.deltaTime;
                myCamera.transform.localPosition = new Vector3(transform.localPosition.x, downY, transform.localPosition.z);
                bc.aimed = false;
            }
            if (Input.GetKey(KeyCode.A)) //left
            {
                transform.RotateAround(pivotPosition, Vector3.up, speed * Time.deltaTime);
                bc.aimed = false;
            }
            if (Input.GetKey(KeyCode.D)) //right
            {
                transform.RotateAround(pivotPosition, Vector3.down, speed * Time.deltaTime);
                bc.aimed = false;
            }
            #endregion
        }
        else
        {
            #region Touch Controlls
            if ((Input.touchCount == 2) && (Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                deltaPosition = Input.GetTouch(0).deltaPosition;

                if (deltaPosition.x > 0)
                {
                    transform.RotateAround(pivotPosition, Vector3.up, -deltaPosition.x * speed * Time.deltaTime);
                }
                else
                {
                    transform.RotateAround(pivotPosition, Vector3.down, deltaPosition.x * speed * Time.deltaTime);
                }

                if (deltaPosition.y > 0)
                {
                    //myCamera.transform.RotateAround(pivotPosition, transform.right, -deltaPosition.y * speed * Time.deltaTime);

                    float upY = transform.localPosition.y + deltaPosition.y * Time.deltaTime;
                    myCamera.transform.localPosition = new Vector3(transform.localPosition.x, upY, transform.localPosition.z);
                }
                else
                {
                    //myCamera.transform.RotateAround(pivotPosition, transform.right, deltaPosition.y * speed * Time.deltaTime);

                    float downY = transform.localPosition.y + deltaPosition.y * Time.deltaTime;
                    myCamera.transform.localPosition = new Vector3(transform.localPosition.x, downY, transform.localPosition.z);
                }

                //make sure camera is still looking at pivot point
                myCamera.transform.LookAt(pivotPosition);
                //force a retargeting
                bc.aimed = false;
                #endregion
            }
        }
    }
}
