// Michael Lee

using UnityEngine;
using System.Collections;

public class GridFacePlayer : MonoBehaviour { 
	public Material defaultMat;
	public Material lowerClassMat;
	public Material middleClassMat;
	public Material upperClassMat;

	public Material currentMat;
	public float currentOpacity = 255f;

	public float lowerClassPercent = 0.5f;
	public float upperClassPercent = 0.2f;

	private int voterLayerMask = 1 << 11;

	public int lowerClassLimit;
	public int upperClassLimit;

	void Start() {
		currentMat = defaultMat;

		GameObject gameCont = GameObject.Find ("GameController");
		float maxMoney = gameCont.GetComponent<GameController> ().voterMaxMoney;

		lowerClassLimit = Mathf.RoundToInt(maxMoney * lowerClassPercent);
		upperClassLimit = Mathf.RoundToInt(maxMoney * (1 - upperClassPercent));
	}

    void Update() 
    { 
		//transform.LookAt(Camera.main.transform.position, Vector3.up); 
		//Debug.DrawRay (transform.position - new Vector3(0f,0.2f,0f), new Vector3 (0f, 0.3f, 0f), Color.green);

		RaycastHit hit;
		Physics.Raycast (transform.position - new Vector3 (0f, 0.2f, 0f), new Vector3 (0f, 1f, 0f), out hit, 0.3f, voterLayerMask);

		if (hit.transform != null) {
			if (hit.transform.GetComponent<VoterVariables>().money <= lowerClassLimit) {
				currentMat = lowerClassMat;
			} else if (hit.transform.GetComponent<VoterVariables>().money > lowerClassLimit && hit.transform.GetComponent<VoterVariables>().money <= upperClassLimit) {
				currentMat = middleClassMat;
			} else if (hit.transform.GetComponent<VoterVariables>().money > upperClassLimit) {
				currentMat = upperClassMat;
			}
		} else {
			currentMat = defaultMat;
		}

		Color mater = GetComponent<MeshRenderer>().material.color;
		GetComponent<MeshRenderer> ().material = currentMat;
		GetComponent<MeshRenderer> ().material.color = new Color(mater.r, mater.g, mater.b, currentOpacity);
    } 

}