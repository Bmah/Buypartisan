//Alex Jungroth
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using GameController;

public class UI_Script : MonoBehaviour {
	
	// Use this for initialization
	void Start () {

		//used for the increment and decrement of X,Y, and Z
		Vector3 ppMove = Vector3.zero;
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
		
	}

	/// <summary>
	/// These six functions increment and decrement
	/// currentPlayer's X,Y, and Z variables.
	/// 
	/// PP = Player Placement
	/// </summary>
	
	public void PP_X_Plus()
	{
		//ppMove = currentPlayer.transform.position;
		//ppMove = pp_Move + Vector3.right;
		//currentPlayer.transform.position = ppMove;
		Debug.Log ("X+ Clicked");
	}
	
	public void PP_X_Minus()
	{
		//ppMove = currentPlayer.transform.position;
		//ppMove = pp_Move + Vector3.left;
		//currentPlayer.transform.position = ppMove;
		Debug.Log ("X- Clicked");
	}
	
	public void PP_Y_Plus()
	{
		//ppMove = currentPlayer.transform.position;
		//ppMove = pp_Move + Vector3.up;
		//currentPlayer.transform.position = ppMove;
		Debug.Log ("Y+ Clicked");
	}
	
	public void PP_Y_Minus()
	{
		//ppMove = currentPlayer.transform.position;
		//ppMove = pp_Move + Vector3.down;
		//currentPlayer.transform.position = ppMove;
		Debug.Log ("Y- Clicked");
	}
	
	public void PP_Z_Plus()
	{
		//ppMove = currentPlayer.transform.position;
		//ppMove = pp_Move + Vector3.forward;
		//currentPlayer.transform.position = ppMove;
		Debug.Log ("Z+ Clicked");
	}
	
	public void PP_Z_Minus()
	{
		//ppMove = currentPlayer.transform.position;
		//ppMove = pp_Move + Vector3.back;
		//currentPlayer.transform.position = ppMove;
		Debug.Log ("Z- Clicked");
	}

	//calls the confirm function
	public void PP_Confirm()
	{
		//Confirm ();
		Debug.Log ("Confirm Clicked");
	}

}