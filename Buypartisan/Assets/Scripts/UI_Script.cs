//Alex Jungroth
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using GameController;

public class UI_Script : MonoBehaviour {

	private GameController controller;

	//used for the increment and decrement of X,Y, and Z
	Vector3 ppMove = Vector3.zero;

	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
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
		ppMove = controller.currentPlayer.transform.position;
		ppMove = ppMove + Vector3.right;
		controller.currentPlayer.transform.position = ppMove;
		Debug.Log ("X+ Clicked");
	}
	
	public void PP_X_Minus()
	{
		ppMove = controller.currentPlayer.transform.position;
		ppMove = ppMove + Vector3.left;
		controller.currentPlayer.transform.position = ppMove;
		Debug.Log ("X- Clicked");
	}
	
	public void PP_Y_Plus()
	{
		ppMove = controller.currentPlayer.transform.position;
		ppMove = ppMove + Vector3.up;
		controller.currentPlayer.transform.position = ppMove;
		Debug.Log ("Y+ Clicked");
	}
	
	public void PP_Y_Minus()
	{
		ppMove = controller.currentPlayer.transform.position;
		ppMove = ppMove + Vector3.down;
		controller.currentPlayer.transform.position = ppMove;
		Debug.Log ("Y- Clicked");
	}
	
	public void PP_Z_Plus()
	{
		ppMove = controller.currentPlayer.transform.position;
		ppMove = ppMove + Vector3.forward;
		controller.currentPlayer.transform.position = ppMove;
		Debug.Log ("Z+ Clicked");
	}
	
	public void PP_Z_Minus()
	{
		ppMove = controller.currentPlayer.transform.position;
		ppMove = ppMove + Vector3.back;
		controller.currentPlayer.transform.position = ppMove;
		Debug.Log ("Z- Clicked");
	}

	//calls the confirm function
	public void PP_Confirm()
	{
		controller.playerConfirmsPlacment = true;
		Debug.Log ("Confirm Clicked");
	}

}