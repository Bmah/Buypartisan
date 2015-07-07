using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Pop up TV script.
/// Brian Mah
/// </summary>
public class PopUpTVScript : MonoBehaviour {

	private float yLocation = -1f;
	private float downyloaction = -1f;

	public float scrollSpeed = 1000f;
	public float timeTillToolTip = 1.5f;
	private float TimeOfToolTip = -1f;

	private bool mouseIsOnButton = false;

	public bool bringPopupDown = false;

	public Text popUpText;
	
	/// <summary>
	/// gets y location of the popupTV
	/// sets the location for it when it moves down.
	/// </summary>
	void Start () {
		yLocation = this.transform.position.y;
		downyloaction = yLocation - 250;
	}
	
	// Update is called once per frame
	void Update () {
		if (mouseIsOnButton && Time.time > TimeOfToolTip) {
			bringPopupDown = true;
		}
		else if (!mouseIsOnButton) {
			bringPopupDown = false;
		}

		if (bringPopupDown && this.transform.position.y > downyloaction) {
			this.transform.Translate(new Vector3(0,-scrollSpeed * ((this.transform.position.y - downyloaction)/250),0)*Time.deltaTime);
		}
		else if(!bringPopupDown && this.transform.position.y < yLocation){
			this.transform.Translate(new Vector3(0,scrollSpeed * ((yLocation - this.transform.position.y)/250),0)*Time.deltaTime);
		}
	}

	public void SetPopupTextBox(string inputText){
		popUpText.text = inputText;
	}

	public void StartWaitingForUIToolTip(){
		mouseIsOnButton = true;
		TimeOfToolTip = Time.time + timeTillToolTip;
	}

	public void ExitUIToolTip(){
		mouseIsOnButton = false;
	}
}
