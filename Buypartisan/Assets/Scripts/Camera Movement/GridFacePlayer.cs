﻿// Michael Lee

using UnityEngine;
using System.Collections;

public class GridFacePlayer : MonoBehaviour {
	public bool occupied = false;
	public Material defaultMat;
	public Material lowerClassMat;
	public Material middleClassMat;
	public Material upperClassMat;

	public Material currentMat;
	public float defCurrentOpacity = 0f;
	public float currentOpacity = 255f;
	private Transform currentVoter;

	public float lowerClassPercent = 0.5f;
	public float upperClassPercent = 0.2f;

	private int voterLayerMask = 1 << 11;

	public int lowerClassLimit;
	public int upperClassLimit;

    //Determines if the pedestals are being used (Alex Jungroth)
    public bool usePedestals = false;
    
	void Start() {
		currentMat = defaultMat;

		GameObject gameCont = GameObject.Find ("GameController");
		float maxMoney = gameCont.GetComponent<GameController> ().voterMaxMoney;

		lowerClassLimit = Mathf.RoundToInt(maxMoney * lowerClassPercent);
		upperClassLimit = Mathf.RoundToInt(maxMoney * (1 - upperClassPercent));

        //Sets usePedestals to the value gameController has (Alex Jungroth)
        usePedestals = gameCont.GetComponent<GameController>().usePedestals;
	}

    void Update() 
    { 
		//transform.LookAt(Camera.main.transform.position, Vector3.up); 
		//Debug.DrawRay (transform.position - new Vector3(0f,0.2f,0f), new Vector3 (0f, 0.3f, 0f), Color.green);

		RaycastHit hit;
		Physics.Raycast (transform.position - new Vector3 (0f, 0.2f, 0f), new Vector3 (0f, 1f, 0f), out hit, 0.3f, voterLayerMask);

		if ((hit.transform != currentVoter) && (usePedestals == true)) {
			occupied = true;
			if (hit.transform != null && hit.transform.GetComponent<VoterVariables>().money <= lowerClassLimit) {
				currentMat = lowerClassMat;
			} else if (hit.transform != null && hit.transform.GetComponent<VoterVariables>().money > lowerClassLimit && hit.transform.GetComponent<VoterVariables>().money <= upperClassLimit) {
				currentMat = middleClassMat;
			} else if (hit.transform != null && hit.transform.GetComponent<VoterVariables>().money > upperClassLimit) {
				currentMat = upperClassMat;
			} else if (hit.transform == null) {
				currentMat = defaultMat;
				occupied = false;
			}
			GetComponent<MeshRenderer> ().material = currentMat;
			if (occupied) {
				GetComponent<MeshRenderer> ().material.color = new Color(1f,1f,1f,currentOpacity/255f);
			} else {
				GetComponent<MeshRenderer> ().material.color = new Color(255f/255f,255f/255f,255f/255f,defCurrentOpacity/255f);
			}
			currentVoter = hit.transform;
		}//if
        else
        {
            //If pedestals are not being used every grid space gets the default material (Alex Jungroth)
            currentMat = defaultMat;
        }//else
    }

}