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
    //Mouse Sensitivity for panning
    public float MouseSensitivity = 1.0f;

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
    //Boolean to control whether control of the camera should be enabled
    public bool ControlEnabled = true;
    //Boolean to allow the cam to free rotate
    private bool RotateCam = false;
    //Last mouse click position
    private Vector3 LastMousePos;

    private Vector3 negScrollDistance;
    private Vector3 NewCamPos;

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


        //If player has control of the camera
        if (ControlEnabled)
        { 
            if (Input.GetMouseButtonDown(0))
            {
                LastMousePos = Input.mousePosition;
            }
            if(Input.GetMouseButton(0))
            {
                Vector3 deltaMovement = Camera.main.ScreenToViewportPoint(Input.mousePosition - LastMousePos);
                Vector3 camMove = new Vector3(deltaMovement.x * MouseSensitivity, 0, deltaMovement.y * MouseSensitivity);
                this.transform.Translate(camMove, Space.World);
                LastMousePos = Input.mousePosition;
            }

            //NEED TO INCORPORATE ABOVE CODE WITH THE BELOW CODE


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
        else if(RotateCam)
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
    /// <summary>
    /// Toggles the camera control. Control turns camera control on/off. Rotate turns on/off auto rotate.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="rotate"></param>
    public void ToggleCamControls(bool control, bool rotate)
    {
        Debug.Log("SETTING CONTROL TO: " + control + " SETTING ROTATE TO: " + rotate);
        ControlEnabled = control;
        RotateCam = rotate;
    }

}
