using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	private InputManagerScript inputManager;
	private GameController gameController;
	
	public Transform target;
	private float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float distanceMin = .5f;
	public float distanceMax = 15f;

    public float WASDQEspeed = 2f;
	
	private Rigidbody cameraRigidbody;
	
	float x = 0.0f;
	float y = 0.0f;
	
	private Vector3 pivotOriginalPosition;
	private Vector3 cameraOriginalPosition;
	private Vector3 gridStartingPoint;

	private Camera camController;
	
	// Use this for initialization
	void Start()
	{
		inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManagerScript>();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		gridStartingPoint = GameObject.Find ("GridStartingPoint").transform.position;
		
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		cameraRigidbody = GetComponent<Rigidbody>();
		
		// Make the rigid body not change rotation
		if (cameraRigidbody != null)
		{
			cameraRigidbody.freezeRotation = true;
		}

		//This is the calculation to find where the pivot point should be according to grid size.
		float midPoint = (gameController.gridSize - 1.0f) / 2.0f;
		pivotOriginalPosition = new Vector3 (midPoint, midPoint, midPoint);
		target.transform.position = pivotOriginalPosition;

		//This is the calculation to find where the Camera should be according to grid size.
		float rad = Vector3.Distance(pivotOriginalPosition, gridStartingPoint);
		rad += 2.0f;
		distance = rad;
		float xPos = rad * Mathf.Sin (225.0f * Mathf.Deg2Rad) * Mathf.Sin (45.0f * Mathf.Deg2Rad);
		float yPos = rad * Mathf.Cos (45.0f * Mathf.Deg2Rad);
		float zPos = rad * Mathf.Cos (225.0f * Mathf.Deg2Rad) * Mathf.Sin (45.0f * Mathf.Deg2Rad);
		cameraOriginalPosition = new Vector3 (xPos, yPos, zPos) + pivotOriginalPosition;
		this.transform.position = cameraOriginalPosition;

		camController = this.GetComponent<Camera> ();
		float margin = 360f / Screen.width;
		camController.rect = new Rect(margin, 0f, 1-margin, 1f);
	}
	
	void Update () {
		
		// These movements include the camera AND THE camera's rotation anchor
		// F key forces camera back to default position and angle
		// THIS IS NOT DYNAMIC, the default position is set manually, must change if the grid changes
		if (inputManager.fButtonDown == true)
		{
			transform.position = cameraOriginalPosition;
			transform.eulerAngles = new Vector3(45f, 45f, 0f);
			target.transform.position = pivotOriginalPosition; //new Vector3(1.5f, 1.5f, 1.5f);
			target.transform.eulerAngles = new Vector3(45f, 45f, 0f);
			
			//Resets the camera scrolling back to origin
			x = 45f;
			y = 45f;
		}
		
		// Camera pans left
		if (inputManager.leftButtonHold == true)
		{
			transform.Translate(Vector3.left * Time.deltaTime * WASDQEspeed);
			target.transform.Translate(Vector3.left * Time.deltaTime * WASDQEspeed);
		}
		// Camera pans right
		if (inputManager.rightButtonHold == true)
		{
            transform.Translate(Vector3.right * Time.deltaTime * WASDQEspeed);
            target.transform.Translate(Vector3.right * Time.deltaTime * WASDQEspeed);
		}
		// Camera pans forward
		if (inputManager.upButtonHold == true)
		{
            transform.Translate(Vector3.forward * Time.deltaTime * WASDQEspeed);
            target.transform.Translate(Vector3.forward * Time.deltaTime * WASDQEspeed);
		}
		// Camera pans backwards
		if (inputManager.downButtonHold == true)
		{
            transform.Translate(Vector3.back * Time.deltaTime * WASDQEspeed);
            target.transform.Translate(Vector3.back * Time.deltaTime * WASDQEspeed);
		}
		// Camera pans up
		if (inputManager.eButtonHold == true)
		{
            transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * WASDQEspeed);
            target.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * WASDQEspeed);
		}
		// Camera pans down
		if (inputManager.qButtonHold == true)
		{
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * WASDQEspeed);
            target.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * WASDQEspeed);
		}
	}
	
	void LateUpdate()
	{
		
		// Late update for camera; if right click is held, it rotates AND moves the camera's position individually to the anchor. This means the anchor CANNOT be parented/child.
		// Right clicking will override default's angle, despite being hard set.
		if (inputManager.rightClickHold == true)
		{
			if (target)
			{
				x += inputManager.mouseAxisX * xSpeed * distance * 0.02f;
				y -= inputManager.mouseAxisY * ySpeed * 0.02f;
				
				// This clamps the y axis - not needed for now?
				//y = ClampAngle(y, yMinLimit, yMaxLimit);
				
				Quaternion rotation = Quaternion.Euler(y, x, 0);
				
				distance = Mathf.Clamp(distance - (Input.GetAxis("Mouse ScrollWheel") / 2), distanceMin, distanceMax);
				
				//This contains the zoomin function from a raycast. Disabled for now - if we need it in the future, we can enable it.
				//RaycastHit hit;
				//if (Physics.Linecast(target.position, transform.position, out hit))
				//{
				//    distance -= hit.distance;
				//}
				
				Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
				Vector3 position = rotation * negDistance + target.position;
				
				transform.rotation = rotation;
				target.transform.rotation = rotation;
				transform.position = position;
			}
		}
	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}
