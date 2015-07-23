using UnityEngine;
using System.Collections;

/// <summary>
/// Music controller.
/// Brian Mah
/// </summary>
public class MusicController : MonoBehaviour {

	public AudioClip[] musicTracks;
	public AudioSource[] audioChannels;
	public bool[] fadingIn;
	public bool[] fadingOut;
	public float musicVolume = 0.5f;
	private float newVolume;
	private TitleScreenSettings titleScreenSettings;

	private float ElectionThemeTime = 111.449f;
	private float NewPlayTime;
	private bool CustomLoopForElectionTheme = false;
	//private bool PlayedElectionRecently = false;

	// Use this for initialization
	void Start () {
		audioChannels = this.GetComponents<AudioSource> ();
		
		GameObject TitleSettingGetter = GameObject.FindGameObjectWithTag ("TitleSettings");
		if(TitleSettingGetter != null){
			titleScreenSettings = TitleSettingGetter.GetComponent<TitleScreenSettings>();
		}
		if (titleScreenSettings != null) {
			musicVolume = titleScreenSettings.musicVolume;
			audioChannels[0].volume = musicVolume;
			audioChannels[1].volume = musicVolume;
			audioChannels[2].volume = musicVolume;
		}

		//play level song to start out
		LoadTrack(0,0);
		audioChannels[0].Play();
		audioChannels [0].loop = true;

		fadingIn =  new bool[audioChannels.Length];
		fadingOut =  new bool[audioChannels.Length];

		for (int i = 0; i < audioChannels.Length; i++) {
			fadingIn[i] = false;
			fadingOut[i] = false;
		}
	}
	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < audioChannels.Length; i++) {
			if(fadingIn[i] && audioChannels[i].volume < musicVolume){
				audioChannels[i].volume += musicVolume*(0.8f) * Time.deltaTime;
			}
			else{
				fadingIn[i] = false;
			}

			if(fadingOut[i] && audioChannels[i].volume > 0f){
				audioChannels[i].volume -= musicVolume*(0.8f) * Time.deltaTime;
			}
			else{
				fadingOut[i] = false;
			}
		}


		if (CustomLoopForElectionTheme && Time.time >= NewPlayTime) {
			NewPlayTime += ElectionThemeTime;
			audioChannels[1].PlayOneShot(musicTracks[2],musicVolume);
		} 
	}

	public void FadeIn(int channel){
		if (channel >= 0 && channel < fadingIn.Length) {
			fadingIn [channel] = true;
		} else {
			Debug.LogError("Index out of range error on input: "+channel);
		}
	}

	public void FadeOut(int channel){
		Debug.Log ("start fadeout: " + Time.time);
		if (channel >= 0 && channel < fadingOut.Length) {
			fadingOut [channel] = true;
		} else {
			Debug.LogError("Index out of range error on input: "+channel);
		}
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

	public void PlayElectionTheme(){
		audioChannels[1].PlayOneShot(musicTracks[2],musicVolume);
		NewPlayTime = Time.time + ElectionThemeTime;
		CustomLoopForElectionTheme = true;
	}
	public void StopElectionTheme(){
		CustomLoopForElectionTheme = false;
	}
}
