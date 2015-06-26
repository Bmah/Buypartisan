using UnityEngine;
using System.Collections;

/// <summary>
/// SFX controller.
/// Brian Mah
/// </summary>
public class SFXController : MonoBehaviour {

	public AudioClip[] SFXList;
	public AudioSource[] AudioChannels;

	// Use this for initialization
	void Start () {
		AudioChannels = this.GetComponents<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Plays the audio clip specified by Clip number and on the Audio channel specified by Source number
	/// Brian Mah
	/// </summary>
	/// <param name="clipNumber">Clip number.</param>
	/// <param name="sourceNumber">Source number.</param>
	public void PlayAudioClip(int clipNumber, int sourceNumber){
		if (clipNumber >= SFXList.Length) {
			Debug.LogError ("Attempted to access SFX clip " + clipNumber + " outside SFXList's range");
		} else if (sourceNumber >= AudioChannels.Length) {
			Debug.LogError ("Attempted to access audio channel " + sourceNumber + " outside AudioChannel's range");
		} else {
			AudioChannels [sourceNumber].PlayOneShot (SFXList [clipNumber]);
		}
	}

	/// <summary>
	/// Plays the audio clip specified by Clip number and on the Audio channel specified by Source number
	/// at a volume scaled on the float passed in.
	/// Brian Mah
	/// </summary>
	/// <param name="clipNumber">Clip number.</param>
	/// <param name="sourceNumber">Source number.</param>
	/// <param name="volume">volume.</param>
	public void PlayAudioClip(int clipNumber, int sourceNumber, float volume){
		if (clipNumber >= SFXList.Length) {
			Debug.LogError ("Attempted to access SFX clip " + clipNumber + " outside SFXList's range");
		} else if (sourceNumber >= AudioChannels.Length) {
			Debug.LogError ("Attempted to access audio channel " + sourceNumber + " outside AudioChannel's range");
		} else {
			AudioChannels [sourceNumber].PlayOneShot (SFXList [clipNumber], volume);
		}
	}

}
