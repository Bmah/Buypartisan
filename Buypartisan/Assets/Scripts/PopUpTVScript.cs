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

	private float scrollSpeed = 1000f;
	private float timeTillToolTip = 1.5f;
	private float TimeOfToolTip = -1f;

	private bool mouseIsOnButton = false;

	public bool bringPopupDown = false;

	public Text popUpText;

	public GameObject Confirm, Cancel;
	private bool prevConfirm, prevCancel;
	private bool ConfirmCancelButtonLock = false;

	/// <summary>
	/// gets y location of the popupTV
	/// sets the location for it when it moves down.
	/// </summary>
	void Start () {
		yLocation = this.transform.position.y;
		downyloaction = yLocation - 250;
		prevConfirm = Confirm.activeSelf;
		prevCancel = Cancel.activeSelf;
	}
	
	// Update is called once per frame
	void Update () {
		if (Confirm.activeSelf && !prevConfirm) {
			bringPopupDown = true;
			ConfirmCancelButtonLock = true;
		}
		else if (!Confirm.activeSelf && prevConfirm) {
			bringPopupDown = false;
			ConfirmCancelButtonLock = false;
		}
		else if (Cancel.activeSelf && !prevCancel) {
			bringPopupDown = true;
			ConfirmCancelButtonLock = true;
		}
		else if (!Cancel.activeSelf && prevCancel) {
			bringPopupDown = false;
			ConfirmCancelButtonLock = false;
		}

		prevCancel = Cancel.activeSelf;
		prevConfirm = Confirm.activeSelf;


		if (!ConfirmCancelButtonLock) {
			if (mouseIsOnButton && Time.time > TimeOfToolTip) {
				bringPopupDown = true;
			} else if (!mouseIsOnButton) {
				bringPopupDown = false;
			}
		}

		if (bringPopupDown && this.transform.position.y > downyloaction) {

			this.transform.Translate (new Vector3 (0, -scrollSpeed * ((this.transform.position.y - downyloaction) / 250), 0) * Time.deltaTime);
		} else if (!bringPopupDown && this.transform.position.y < yLocation) {

			this.transform.Translate (new Vector3 (0, scrollSpeed * ((yLocation - this.transform.position.y) / 250), 0) * Time.deltaTime);
		}
	}

	public void SetPopupTextBox(string inputText){
		popUpText.text = inputText;
	}

	public void StartWaitingForUIToolTip(){
		mouseIsOnButton = true;
		TimeOfToolTip = Time.time + timeTillToolTip;
	}

	/// <summary>
	/// I need this for when you mouse over voters and players.
	/// </summary>
	public void ShortWaitForUIToolTip()
	{
		mouseIsOnButton = true;
		TimeOfToolTip = Time.time + timeTillToolTip * 0.5f;
	}

	public void ExitUIToolTip(){
		mouseIsOnButton = false;
	}
}
