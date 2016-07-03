using UnityEngine;
using System.Collections;

public class GridFacePlayerZ : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.LookAt (new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z), Vector3.up);
	}
}
