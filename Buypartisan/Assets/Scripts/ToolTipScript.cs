using UnityEngine;
using System.Collections;

public class ToolTipScript : MonoBehaviour
{

    //holds the UI Controller so this script can alter the text box
    public PopUpTVScript popupUIController;

    //All tooltip messages appear in the upper right TV screen to display help messages for the players

    //Help messages to detail what the actions do
    [TextArea]
    public string[] ActionMessages;

    //messages for the dirctional movement buttons
    //0: Neg X, 1: Pos X
    //2: Neg Y, 3: Pos Y
    //4: Neg Z, 5: Pos Z
    [TextArea]
    public string[] DirectionalMessages = new string[6];

    //messages for the parties
    [TextArea]
    public string[] PartyMessages = new string[5];


    //a help message for the scrolling and zooming
    [TextArea]
    public string HelpMessage;

    //Holds the original (last) message in the screen so that it does not get overwritten
    private string origninalMessage;


    /// <summary>
    /// Resets the text box on mouse off.
    /// </summary>
    public void ResetTextBox()
    {
        popupUIController.SetPopupTextBox(origninalMessage);
        popupUIController.ExitUIToolTip();
    }

    /// <summary>
    /// Clearses the text box when you click the cancel button.
    /// </summary>
    public void ClearsTextBox()
    {
        //clears the text box by loading in a blank message
        popupUIController.SetPopupTextBox("");
    }

    public void LoadActionMessage(int curAction)
    {
        //saves off the original message
        origninalMessage = popupUIController.popUpText.text;

        //puts the help message for action 0 into the maint text box
        popupUIController.SetPopupTextBox(ActionMessages[curAction]);
        //starts the timer that waits for the ui tooltip down function
        popupUIController.StartWaitingForUIToolTip();

    }

    public void LoadMessageHelp()
    {
        //saves off the original message
        origninalMessage = popupUIController.popUpText.text;

        //puts the help message for the help button into the maint text box
        popupUIController.SetPopupTextBox(HelpMessage);
        //starts the timer that waits for the ui tooltip down function
        popupUIController.StartWaitingForUIToolTip();
    }

    public void LoadDirectionMessage(int dir)
    {
        //0: Neg X, 1: Pos X
        //2: Neg Y, 3: Pos Y
        //4: Neg Z, 5: Pos Z

        //saves off the original message
        origninalMessage = popupUIController.popUpText.text;

        //puts the help message for the X+ button into the maint text box
        popupUIController.SetPopupTextBox(DirectionalMessages[dir]);
        //starts the timer that waits for the ui tooltip down function
        popupUIController.StartWaitingForUIToolTip();
    }

    public void LoadPartyMessage(int partyNum)
    {
        //saves off the original message
        origninalMessage = popupUIController.popUpText.text;

        //puts the help message for the Z- button into the maint text box
        popupUIController.SetPopupTextBox(PartyMessages[partyNum]);
        //starts the timer that waits for the ui tooltip down function
        popupUIController.StartWaitingForUIToolTip();

        //0: Apple Party
        //1: Espresso
        //2: Drone
        //3: Windy
        //4: Providence
    }

}




////these functions load their message on mouse on.
//public void LoadMessageAction0()
//{
//	//saves off the original message
//	origninalMessage = popupUIController.popUpText.text;

//	//puts the help message for action 0 into the maint text box
//	popupUIController.SetPopupTextBox (ActionMessages[0]);
//	//starts the timer that waits for the ui tooltip down function
//	popupUIController.StartWaitingForUIToolTip ();
//}

//public void LoadMessageAction1()
//{
//	//saves off the original message
//	origninalMessage = popupUIController.popUpText.text;

//	//puts the help message for action 1 into the maint text box
//	popupUIController.SetPopupTextBox (ActionMessages[1]);
//	//starts the timer that waits for the ui tooltip down function
//	popupUIController.StartWaitingForUIToolTip ();
//}

//public void LoadMessageAction2()
//{
//	//saves off the original message
//	origninalMessage = popupUIController.popUpText.text;

//	//puts the help message for action 2 into the maint text box
//	popupUIController.SetPopupTextBox (ActionMessages[2]);
//	//starts the timer that waits for the ui tooltip down function
//	popupUIController.StartWaitingForUIToolTip ();
//}

//public void LoadMessageAction3()
//{
//	//saves off the original message
//	origninalMessage = popupUIController.popUpText.text;

//	//puts the help message for action 3 into the maint text box
//	popupUIController.SetPopupTextBox (ActionMessages[3]);
//	//starts the timer that waits for the ui tooltip down function
//	popupUIController.StartWaitingForUIToolTip ();
//}

//public void LoadMessageAction4()
//{
//	//saves off the original message
//	origninalMessage = popupUIController.popUpText.text;

//	//puts the help message for action 4 into the maint text box
//	popupUIController.SetPopupTextBox (ActionMessages[4]);
//	//starts the timer that waits for the ui tooltip down function
//	popupUIController.StartWaitingForUIToolTip ();
//}

//public void LoadMessageAction5()
//{
//	//saves off the original message
//	origninalMessage = popupUIController.popUpText.text;

//	//puts the help message for action 5 into the maint text box
//	popupUIController.SetPopupTextBox (ActionMessages[5]);
//	//starts the timer that waits for the ui tooltip down function
//	popupUIController.StartWaitingForUIToolTip ();
//}

//public void LoadMessageAction6()
//{
//	//saves off the original message
//	origninalMessage = popupUIController.popUpText.text;

//	//puts the help message for action 6 into the maint text box
//	popupUIController.SetPopupTextBox (ActionMessages[6]);
//	//starts the timer that waits for the ui tooltip down function
//	popupUIController.StartWaitingForUIToolTip ();
//}

//public void LoadMessageAction7()
//{
//	//saves off the original message
//	origninalMessage = popupUIController.popUpText.text;

//	//puts the help message for action 7 into the maint text box
//	popupUIController.SetPopupTextBox (ActionMessages[7]);
//	//starts the timer that waits for the ui tooltip down function
//	popupUIController.StartWaitingForUIToolTip ();
//}

//public void LoadMessageAction8()
//{
//	//saves off the original message
//	origninalMessage = popupUIController.popUpText.text;

//	//puts the help message for action 8 into the maint text box
//	popupUIController.SetPopupTextBox (ActionMessages[8]);
//	//starts the timer that waits for the ui tooltip down function
//	popupUIController.StartWaitingForUIToolTip ();
//}

/*
	public void LoadMessageXPlus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the X+ button into the maint text box
		popupUIController.SetPopupTextBox (DirectionalMessages[1]);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageXMinus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the X- button into the maint text box
		popupUIController.SetPopupTextBox (DirectionalMessages[0]);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageYPlus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Y+ button into the maint text box
		popupUIController.SetPopupTextBox (DirectionalMessages[3]);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}
	
	public void LoadMessageYMinus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Y- button into the maint text box
		popupUIController.SetPopupTextBox (DirectionalMessages[2]);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageZPlus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z+ button into the maint text box
		popupUIController.SetPopupTextBox (DirectionalMessages[5]);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}
	
	public void LoadMessageZMinus()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (DirectionalMessages[4]);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}
    	public void LoadMessageParty1()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (PartyMessages[0]);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageParty2()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (PartyMessages[1]);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageParty3()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (PartyMessages[2]);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageParty4()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (PartyMessages[3]);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

	public void LoadMessageParty5()
	{
		//saves off the original message
		origninalMessage = popupUIController.popUpText.text;
		
		//puts the help message for the Z- button into the maint text box
		popupUIController.SetPopupTextBox (PartyMessages[4]);
		//starts the timer that waits for the ui tooltip down function
		popupUIController.StartWaitingForUIToolTip ();
	}

}

    //

    */

