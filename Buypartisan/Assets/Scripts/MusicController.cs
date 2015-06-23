using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	public AudioClip[] musicTracks;
	public AudioSource[] audioChannels;

	// Use this for initialization
	void Start () {
		audioChannels = this.GetComponents<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Loads the track onto the specified audioChannel
	/// </summary>
	/// <param name="trackNumber">Track number.</param>
	/// <param name="channelNumber">Channel number.</param>
	void LoadTrack(int trackNumber, int channelNumber){
		if (trackNumber >= musicTracks.Length) {
			Debug.LogError ("Attempted to access Music track " + trackNumber + " outside musicTracks range");
		} else if (channelNumber >= audioChannels.Length) {
			Debug.LogError ("Attempted to access audio channel " + channelNumber + " outside AudioChannel's range");
		} else {
			audioChannels [channelNumber].clip = musicTracks [trackNumber];
		}
	}
}
