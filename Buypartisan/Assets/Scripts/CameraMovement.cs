using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private InputManagerScript inputManager;

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody cameraRigidbody;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
		inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManagerScript>();

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        cameraRigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (cameraRigidbody != null)
        {
            cameraRigidbody.freezeRotation = true;
        }
    }
	
	void Update () {

        // These movements include the camera AND THE camera's rotation anchor
        // F key forces camera back to default position and angle
        // The default position is set manually, must change if the grid changes
		if (inputManager.fButtonDown == true)
        {
            transform.position = new Vector3(-1.11696f, 4.865548f, 4.112394f);
            transform.eulerAngles = new Vector3(45f, 135f, 0f);
            target.transform.position = new Vector3(1.5f, 1.5f, 1.5f);
            target.transform.eulerAngles = new Vector3(45f, 135f, 0f);
        }

        // Camera pans left
		if (inputManager.leftButtonHold == true)
        {
            transform.Translate(Vector3.left * Time.deltaTime);
            target.transform.Translate(Vector3.left * Time.deltaTime);
        }
        // Camera pans right
		if (inputManager.rightButtonHold == true)
        {
            transform.Translate(Vector3.right * Time.deltaTime);
            target.transform.Translate(Vector3.right * Time.deltaTime);
        }
        // Camera pans forward
		if (inputManager.upButtonHold == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
            target.transform.Translate(Vector3.forward * Time.deltaTime);
        }
        // Camera pans backwards
		if (inputManager.downButtonHold == true)
        {
            transform.Translate(Vector3.back * Time.deltaTime);
            target.transform.Translate(Vector3.back * Time.deltaTime);
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
                x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                // This clamps the y axis - not needed for now?
                //y = ClampAngle(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(y, x, 0);

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

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
