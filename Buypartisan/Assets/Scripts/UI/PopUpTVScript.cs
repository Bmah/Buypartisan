using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Pop up TV script.
/// Brian Mah
/// </summary>
public class PopUpTVScript : MonoBehaviour {


	//public float downyloaction = -1f;
    [Header("TV Settings")]
    public float timeTillToolTip = 1.5f;
    public float timeTillDecay = 1.0f;
    public float scrollSpeed = 2f;

    //public float yLocation = -1f;
    private float TimeOfToolTip = -1f;
    private float TimeOfDecay = -1f;
    private bool prevConfirm, prevCancel;
    private bool ConfirmCancelButtonLock = false;

    private bool mouseIsOnButton = false;
    [HideInInspector]
	public bool bringPopupDown = false;

    [Header("UI Elements")]
    public GameObject TVObject;
    public Text popUpText;
	public GameObject Confirm, Cancel;

    [Header("TV Locations")]
    public Transform TVStartLoc;
    public Transform TVDropLoc;

    private Transform OriginalLocation;
    private Transform DropdownLocation;

    /// <summary>
    /// gets y location of the popupTV
    /// sets the location for it when it moves down.
    /// </summary>
    void Start () {

        //yLocation = this.GetComponent<RectTransform>().localPosition.y;
        //OriginalLocation = this.GetComponent<RectTransform>();
		//downyloaction = yLocation - 210.17f;
        //-21 | 250
		prevConfirm = Confirm.activeSelf;
		prevCancel = Cancel.activeSelf;
        OriginalLocation = TVStartLoc;
        DropdownLocation = TVDropLoc;
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
			} else if (!mouseIsOnButton && Time.time > TimeOfDecay) {
				bringPopupDown = false;
			}
		}

		if (bringPopupDown && this.transform.position.y > DropdownLocation.position.y)
        {
            Debug.Log("DROPDOWN");
			//this.transform.Translate (new Vector3 (0, -scrollSpeed * ((this.transform.position.y - downyloaction) / 250), 0) * Time.deltaTime);
            this.transform.Translate(new Vector3(0f, (this.transform.position.y - DropdownLocation.position.y) * -scrollSpeed, 0f)  * Time.deltaTime);
		}
        else if (!bringPopupDown && this.transform.position.y < OriginalLocation.position.y)
        {
            Debug.Log("GO UP");
			//this.transform.Translate (new Vector3 (0, scrollSpeed * ((yLocation - this.transform.position.y) / 250), 0) * Time.deltaTime);
            this.transform.Translate ( new Vector3 (0f, (OriginalLocation.position.y - this.transform.position.y) * scrollSpeed, 0f)  * Time.deltaTime);
		}

        //Debug.Log(this.transform.position.y + " " + OriginalLocation.position.y);
	}

	public void SetPopupTextBox(string inputText)
    {
		popUpText.text = inputText;
	}

	public void StartWaitingForUIToolTip()
    {
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

	public void ExitUIToolTip()
    {
		mouseIsOnButton = false;
        TimeOfDecay = Time.time + timeTillDecay;
	}

/*    public void UpdateTVPos(int Loc)
    {
        //LOC = 0: Original Tooltip Positions
        //LOC = 1: Action Tooltip Positions
        if (Loc == 1)
        {
            OriginalLocation = ActionStartLoc;
            DropdownLocation = ActionDropLoc;
            TVObject.GetComponent<Transform>().position = OriginalLocation.position;
        }
        else
        {
            OriginalLocation = TooltipStartLoc;
            DropdownLocation = TooltipDropLoc;
        }

        TVObject.GetComponent<Transform>().position = OriginalLocation.position;


    }*/
}
