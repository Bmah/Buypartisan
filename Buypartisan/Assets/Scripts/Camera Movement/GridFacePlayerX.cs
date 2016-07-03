using UnityEngine;
using System.Collections;

public class GridFacePlayerX : MonoBehaviour {

	//private Vector3 tmpVector;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update() 
	{ 
		//transform.LookAt(Camera.main.transform.position, Vector3.forward); 
		transform.LookAt (new Vector3 (transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z), Vector3.up);
		//transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, 0, 0);

		//transform.LookAt (new Vector3 (Camera.main.transform.position.x, 0, 0), Vector3.up);
	} 
}
