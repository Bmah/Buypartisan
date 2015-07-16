using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIResolutionScale : MonoBehaviour {
	
	public GameObject panel1, panel2;

	// Use this for initialization
	void Start () {
	
		fitScreenResolution ();
		//Screen.width

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void fitScreenResolution()
	{
		// Referencing UI RectTransform Element
		RectTransform panelSize1 = panel1.GetComponent<RectTransform> ();
		RectTransform panelSize2 = panel2.GetComponent<RectTransform> ();

		// Storing old width values to determine percentage change
		float screenWidthSave1 = panelSize1.sizeDelta.x;
		float screenWidthSave2 = panelSize2.sizeDelta.x;
		float screenLengthSave1 = panelSize1.sizeDelta.y;
		float screenLengthSave2 = panelSize2.sizeDelta.y;
	
		float screenWidthPercent1;
		float screenWidthPercent2;
		float screenLengthPercent1;
		float screenLengthPercent2;

		// Set panel width according to screen's resolution (20% for panel 1 atm)
		panelSize1.sizeDelta = new Vector2((Screen.width / 5), Screen.height);
		panelSize1.sizeDelta = new Vector2((Screen.width / 5), Screen.height);

		Debug.Log (panel1.GetComponent<RectTransform> ().sizeDelta.x);
		Debug.Log (panel1.GetComponent<RectTransform> ().sizeDelta.y);

		// Get percent change
		screenWidthPercent1 = (panelSize1.sizeDelta.x - screenWidthSave1) / screenWidthSave1;


	}
}
