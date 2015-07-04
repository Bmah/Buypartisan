using UnityEngine;
using System.Collections;

public class ToolTipScript : MonoBehaviour {

	//holds the UI Controller so this script can alter the text box
	public UI_Script uiController;

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
		uiController.alterTextBox (origninalMessage);
	}

	/// <summary>
	/// Clearses the text box when you click the cancel button.
	/// </summary>
	public void ClearsTextBox()
	{
		//clears the text box by loading in a blank message
		uiController.alterTextBox ("");
	}

	//these functions load their message on mouse on.
	public void LoadMessageAction0()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;

		//puts the help message for action 0 into the maint text box
		uiController.alterTextBox (messageAction0);
	}

	public void LoadMessageAction1()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for action 1 into the maint text box
		uiController.alterTextBox (messageAction1);
	}

	public void LoadMessageAction2()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for action 2 into the maint text box
		uiController.alterTextBox (messageAction2);
	}

	public void LoadMessageAction3()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for action 3 into the maint text box
		uiController.alterTextBox (messageAction3);
	}

	public void LoadMessageAction4()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for action 4 into the maint text box
		uiController.alterTextBox (messageAction4);
	}

	public void LoadMessageAction5()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for action 5 into the maint text box
		uiController.alterTextBox (messageAction5);
	}

	public void LoadMessageAction6()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for action 6 into the maint text box
		uiController.alterTextBox (messageAction6);
	}

	public void LoadMessageAction7()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for action 7 into the maint text box
		uiController.alterTextBox (messageAction7);
	}

	public void LoadMessageAction8()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for action 8 into the maint text box
		uiController.alterTextBox (messageAction8);
	}

	public void LoadMessageAction9()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for action 9 into the maint text box
		uiController.alterTextBox (messageAction9);
	}

	public void LoadMessageHelp()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for the help button into the maint text box
		uiController.alterTextBox (messageHelp);
	}

	public void LoadMessageXPlus()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for the X+ button into the maint text box
		uiController.alterTextBox (messageXPlus);
	}

	public void LoadMessageXMinus()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for the X- button into the maint text box
		uiController.alterTextBox (messageXMinus);
	}

	public void LoadMessageYPlus()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for the Y+ button into the maint text box
		uiController.alterTextBox (messageYPlus);
	}
	
	public void LoadMessageYMinus()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for the Y- button into the maint text box
		uiController.alterTextBox (messageYMinus);
	}

	public void LoadMessageZPlus()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for the Z+ button into the maint text box
		uiController.alterTextBox (messageZPlus);
	}
	
	public void LoadMessageZMinus()
	{
		//saves off the original message
		origninalMessage = uiController.visualText.text;
		
		//puts the help message for the Z- button into the maint text box
		uiController.alterTextBox (messageZMinus);
	}
}