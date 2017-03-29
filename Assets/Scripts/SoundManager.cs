using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour 
{
	public AudioSource efxSource;					//Drag a reference to the audio source which will play the sound effects.
	public AudioSource musicSource;					//Drag a reference to the audio source which will play the music.

	public AudioMixer audioMixer;					// Mixes the sounds for playback

	public float lowPitchRange = .95f;				//The lowest a sound effect will be randomly pitched.
	public float highPitchRange = 1.05f;			//The highest a sound effect will be randomly pitched.

	public AudioClip pathStart;
	public AudioClip pathExtend;
	public AudioClip pathAbort;
	public AudioClip pathSuccess;

	public bool efxEnabled = true;

	public bool musicEnabled = false;

	public void EnableEfx(bool enabled) {
		Debug.Log ("Enable EFX: " + enabled);
		efxEnabled = enabled;
	}

	public void EnableMusic(bool enabled) {
		musicEnabled = enabled;
	}

	public void PlayPathStart() {
		PlayEfxClip(pathStart);
	}

	public void PlayPathExtend(){
		PlayEfxClip(pathExtend);
	}

	public void PlayPathAbort(){
		PlayEfxClip(pathAbort);
	}

	public void PlayPathSuccess(){
		PlayEfxClip(pathSuccess);
	}

	private void PlayEfxClip(AudioClip clip) {
		if (efxEnabled) {
			efxSource.clip = clip;
			efxSource.Play ();
		}
	}
	
	void Awake() { 
		Debug.Log ("efxSource is enabled: " + efxSource.enabled);
	}


	//Used to play single sound clips.
	public void PlaySingle(AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		efxSource.clip = clip;

		//Play the clip.
		efxSource.Play ();
	}


	//RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
	public void RandomizeSfx (params AudioClip[] clips)
	{
		//Generate a random number between 0 and the length of our array of clips passed in.
		int randomIndex = Random.Range(0, clips.Length);

		//Choose a random pitch to play back our clip at between our high and low pitch ranges.
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		//Set the pitch of the audio source to the randomly chosen pitch.
		efxSource.pitch = randomPitch;

		//Set the clip to the clip at our randomly chosen index.
		efxSource.clip = clips[randomIndex];

		//Play the clip.
		efxSource.Play();
	}
}

