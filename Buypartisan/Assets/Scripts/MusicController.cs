using UnityEngine;
using System.Collections;

/// <summary>
/// Music controller.
/// Brian Mah
/// </summary>
public class MusicController : MonoBehaviour {

	public AudioClip[] musicTracks;
	public AudioSource[] audioChannels;

	// Use this for initialization
	void Start () {
		audioChannels = this.GetComponents<AudioSource> ();

		//play level song to start out
		LoadTrack(0,0);
		audioChannels[0].Play();
		audioChannels [0].loop = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Loads the track onto the specified audioChannel
	/// Brian Mah
	/// </summary>
	/// <param name="trackNumber">Track number.</param>
	/// <param name="channelNumber">Channel number.</param>
	public void LoadTrack(int trackNumber, int channelNumber){
		if (trackNumber >= musicTracks.Length) {
			Debug.LogError ("Attempted to access Music track " + trackNumber + " outside musicTracks range");
		} else if (channelNumber >= audioChannels.Length) {
			Debug.LogError ("Attempted to access audio channel " + channelNumber + " outside AudioChannel's range");
		} else {
			audioChannels [channelNumber].clip = musicTracks [trackNumber];
		}
	}
}
