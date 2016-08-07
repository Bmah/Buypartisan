using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour
{
    //Created by Michael, with lots of edits by Chris.
    //EDITED BY TYLER FOR 2D ADAPTION

    //Main Board Game Controller
    public BoardGameController gameController;
    //The Camera Target/Look at
    public Transform CamTarget;
    //Current Zoom Distance
    private float ScrollDistance = 5.0f;
    //The Camera's rigid body
    private Rigidbody cameraRigidbody;

    //Radius from midpoint position
    public float radius = 5.0f;
    //Speed of X rotation
    public float xSpeed = 80.0f;
    //Speed of Y rotation
    public float ySpeed = 80.0f;
    //Minimum Zoom distance
    public float minScrollDistance = .5f;
    //Maximum Scroll distance
    public float maxScrollDistance = 15f;

    //Current Camera X Angle
    private float CamAngle_X = 0.0f;
    //Current Camera Y Angle
    private float CamAngle_Y = 0.0f;
    //Original Pivot/Look at position
    private Vector3 pivotOriginalPosition;
    //Original camera transform position
    private Vector3 cameraOriginalPosition;
    //Starting point of the main game board
    private Vector3 gridStartingPoint;
    //Camera Rotater boolean is for whether the camera should rotate freely around the board or be player controller
    //Used for the BeforePlay state
    [HideInInspector]
    public bool CamRotater = false;

    // Use this for initialization
    void Start()
    {
        CamAngle_X = this.transform.eulerAngles.x;
        CamAngle_Y = this.transform.eulerAngles.y;
        cameraRigidbody = GetComponent<Rigidbody>();
        // Make the rigid body not change rotation
        if (cameraRigidbody != null)
        {
            cameraRigidbody.freezeRotation = true;
        }

        //This is the calculation to find where the pivot point should be according to grid size.
        float midPoint = (gameController.BoardSize) / 2.0f;
        pivotOriginalPosition = new Vector3(midPoint, 0, midPoint);
        CamTarget.transform.position = pivotOriginalPosition;

        //This is the calculation to find where the Camera should be according to grid size.
        float rad = Vector3.Distance(pivotOriginalPosition, gridStartingPoint);
        rad += radius;
        ScrollDistance = rad;
        float xPos = rad * Mathf.Sin(225.0f * Mathf.Deg2Rad) * Mathf.Sin(45.0f * Mathf.Deg2Rad);
        float yPos = rad * Mathf.Cos(45.0f * Mathf.Deg2Rad);
        float zPos = rad * Mathf.Cos(225.0f * Mathf.Deg2Rad) * Mathf.Sin(45.0f * Mathf.Deg2Rad);
        cameraOriginalPosition = new Vector3(xPos, yPos, zPos) + pivotOriginalPosition;
        this.transform.position = cameraOriginalPosition;
        gameController.CamPositions[0].position = cameraOriginalPosition;

        //float margin = 360f / Screen.width;
        //camController.rect = new Rect(margin, 0f, 1-margin, 1f);
    }

    void LateUpdate()
    {
        Vector3 negScrollDistance;
        Vector3 NewCamPos;

        //If player has control of the camera
        if (!CamRotater)
        {
            //Rotation
            if (Input.GetMouseButton(1))
            {
                if (CamTarget)
                {
                    CamAngle_X += Input.GetAxis("Mouse X") * xSpeed * ScrollDistance * 0.02f;
                    CamAngle_Y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                    //Y and X angle is swapped here, I forget why. 
                    Quaternion NewCamRot = Quaternion.Euler(Mathf.Clamp(CamAngle_Y, 10.0f, 90.0f), CamAngle_X, 0);

                    this.transform.rotation = NewCamRot;
                    CamTarget.transform.rotation = NewCamRot;
                }
            }
            //Zoom In/Out
            ScrollDistance = Mathf.Clamp(ScrollDistance - (Input.GetAxis("Mouse ScrollWheel") / 2), minScrollDistance, maxScrollDistance);
            negScrollDistance = new Vector3(0.0f, 0.0f, -ScrollDistance);
            NewCamPos = this.transform.rotation * negScrollDistance + CamTarget.position;
            this.transform.position = NewCamPos;
        }
        else
        {
            //If Camera is being computer controlled
            //Same code as above minus the player input
            if (CamTarget)
            {
                CamAngle_X -=  0.2f;
                //CamAngle_Y -= ySpeed * 0.02f;

                Quaternion NewCamRot = Quaternion.Euler(Mathf.Clamp(CamAngle_Y, 10.0f, 90.0f), CamAngle_X, 0);

                this.transform.rotation = NewCamRot;
                CamTarget.transform.rotation = NewCamRot;
            }
            negScrollDistance = new Vector3(0.0f, 0.0f, -ScrollDistance);
            NewCamPos = this.transform.rotation * negScrollDistance + CamTarget.position;
            this.transform.position = NewCamPos;
        }
    }    
}
