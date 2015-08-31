using UnityEngine;
using System.Collections;

public class VoterAnimator : MonoBehaviour {
	public float numberOfAnimations;


	public void RandomizeIdle() {
		float saved = Mathf.Round (Random.Range (0f, numberOfAnimations - 1f));
		GetComponent<Animator> ().SetFloat ("IdleFloat", saved);
	}
}
