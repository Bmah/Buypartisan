using UnityEngine;
using System.Collections;

public class VisualAidAxisManangerScript : MonoBehaviour {

	public GameObject VisualAidManager;
	private SpriteRenderer[] spriteChildren;

	// Use this for initialization
	void Start () { 

		VisualAidManager = GameObject.FindGameObjectWithTag("VisualAidManager");

		spriteChildren = this.transform.GetComponentsInChildren<SpriteRenderer>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void Attach(GameObject parent){

		for (int i = 0; i < spriteChildren.Length; i++)
		{
			spriteChildren[i].enabled = true;
		}
		transform.SetParent(parent.transform);
		transform.localPosition = new Vector3(0, 0, 0);
	}

	void Detach(){

		for (int i = 0; i < spriteChildren.Length; i++)
		{
			spriteChildren[i].enabled = false;
		}
		transform.parent = null;
	}

	void MoveTo(Vector3 position){

		transform.position = position;
	}

}
