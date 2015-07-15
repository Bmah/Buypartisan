using UnityEngine;
using System.Collections;

public class VoterAnimator : MonoBehaviour {

	public void RandomizeIdle() {
		float saved = Mathf.Round (Random.Range (0f, 6f));
		GetComponent<Animator> ().SetFloat ("IdleFloat", saved);
	}
}
