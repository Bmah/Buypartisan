using UnityEngine;
using System.Collections;

public class ToolTipScript : MonoBehaviour {

	//holds the UI Controller so this script can alter the text box
	public PopUpTVScript popupUIController;

	//These are the messages that will be displayed
	//on the TV when you hover over the aciton buttons
	public string messageAction0;
	public string messageAction1;
	public string messageAction2;
	public string messageAction3;
	public string messageAction4;
	public string messageAction5;
	public string messageAction6;
	public string messageAction7;
	public string messageAction8;
	public string messageAction9;

	//a help message for the scrolling and zooming
	public string messageHelp;

	//messages for the dirctional movement buttons
	public string messageXPlus;
	public string messageXMinus;
	public string messageYPlus;
	public string messageYMinus;
	public string messageZPlus;
	public string messageZMinus;

	//messages for the parties
	public string party1;
	public string party2;
	public string party3;
	public string party4;
	public string party5;

	//holds the original message from the text box so
	//doesn't get over written
	private string origninalMessage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Resets the text box on mouse off.
	/// </summary>
	public void ResetTextBox()
	{
		popupUIController.SetPopupTextBox (origninalMessage);
		popupUIController.ExitUIToolTip ();
	}

	/// <summary>
	/// Clearses the text box when you click the cancel button.
	/// </summary>
	public void ClearsTextBox()
	{
		//clears the text box by loading in a blank message
		popupUIController.SetPopupTextBox ("");
	}

	//these functions load their message on mouse on.
	public void LoadMessageAction0()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;

		//puts the help message for action 0 into the maint text box
		popupUIController.SetPopupTextBox (messageAction0);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageAction1()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for action 1 into the maint text box
		popupUIController.SetPopupTextBox (messageAction1);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageAction2()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for action 2 into the maint text box
		popupUIController.SetPopupTextBox (messageAction2);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageAction3()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for action 3 into the maint text box
		popupUIController.SetPopupTextBox (messageAction3);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageAction4()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for action 4 into the maint text box
		popupUIController.SetPopupTextBox (messageAction4);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageAction5()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for action 5 into the maint text box
		popupUIController.SetPopupTextBox (messageAction5);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageAction6()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for action 6 into the maint text box
		popupUIController.SetPopupTextBox (messageAction6);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageAction7()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for action 7 into the maint text box
		popupUIController.SetPopupTextBox (messageAction7);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageAction8()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for action 8 into the maint text box
		popupUIController.SetPopupTextBox (messageAction8);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageAction9()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for action 9 into the maint text box
		popupUIController.SetPopupTextBox (messageAction9);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageHelp()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the help button into the maint text box
		popupUIController.SetPopupTextBox (messageHelp);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageXPlus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the X+ button into the maint text box
		popupUIController.SetPopupTextBox (messageXPlus);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageXMinus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the X- button into the maint text box
		popupUIController.SetPopupTextBox (messageXMinus);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageYPlus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Y+ button into the maint text box
		popupUIController.SetPopupTextBox (messageYPlus);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}
	
	public void LoadMessageYMinus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Y- button into the maint text box
		popupUIController.SetPopupTextBox (messageYMinus);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageZPlus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z+ button into the maint text box
		popupUIController.SetPopupTextBox (messageZPlus);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}
	
	public void LoadMessageZMinus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (messageZMinus);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageParty1()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (party1);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageParty2()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (party2);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageParty3()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (party3);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageParty4()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (party4);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageParty5()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (party5);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

}