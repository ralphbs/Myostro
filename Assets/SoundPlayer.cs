using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SoundPlayer : MonoBehaviour {
    public AudioSource[] tracks;
    public Grapher2[] trackHolders;
    public Text textOverlay; 
    public static int bpm = 127;
    private int selectedIndex;
    private bool textVisible;
    private double oneBeat,startTime,textTimeout;
	// Use this for initialization
	void Start () {
        oneBeat = 60.0 / bpm;
        trackHolders = new Grapher2[6];
        tracks = new AudioSource[6];
        textOverlay = GameObject.Find("Canvas/Text").GetComponent<Text>();
        for (int i = 0; i < tracks.Length; i++) {
            trackHolders[i] = GameObject.Find("Graph "+(i+1)).GetComponent<Grapher2>();
            tracks[i] = GameObject.Find("Graph " + (i + 1)).GetComponent<AudioSource>();
            tracks[i].loop = true;
        }
        startTime = AudioSettings.dspTime;
        selectedIndex = 2;
        Debug.Log("Selected Index: " + selectedIndex);
        Debug.Log("bruh " + startTime + oneBeat * 4);
        //tracks[0].PlayScheduled(startTime+oneBeat*4);
        //tracks[1].PlayScheduled(startTime+oneBeat*8);
        UpdateVisuals();
    }

    //KEY EVENTS
    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && e.type.ToString() == "KeyUp") {

            if (e.keyCode.ToString() == "Return")
                toggleTrack(selectedIndex);
            if (selectedIndex > 0 && e.keyCode.ToString() == "RightArrow")
            {
                selectedIndex--;
                UpdateVisuals();
                Debug.Log("Selected Index: "+ selectedIndex);
            }
            if (selectedIndex < tracks.Length - 1 && e.keyCode.ToString() == "LeftArrow")
            {
                selectedIndex++;
                UpdateVisuals();
                Debug.Log("Selected Index: " + selectedIndex);
            }
            if (e.keyCode.ToString() == "UpArrow") {              
                AudioSource audio = tracks[selectedIndex];
                Debug.Log("Increasing Volume, volume= " + audio.volume);
                audio.mute = false;
                if (audio.volume < 0.90)
                {
                    audio.volume += 0.1f;
                }
                else
                {
                    audio.volume = 1.0f;
                }
                Debug.Log("Increasing Volume, volume= " + audio.volume);
                SetText("Volume: " + Math.Round((audio.volume) * 100) + "%");
            }
            if (e.keyCode.ToString() == "DownArrow")
            {               
                AudioSource audio = tracks[selectedIndex];
                Debug.Log("Reducing Volume, volume= " + audio.volume);
                if (audio.volume > 0.1)
                {
                    audio.volume -= 0.1f;
                }
                else
                {
                    audio.mute = true;
                }
                Debug.Log("Reduced Volume, volume= " + audio.volume);
                SetText("Volume: "+ Math.Round((audio.volume) * 100)+"%");
            }
        }

    }
    void UpdateVisuals() {
        Debug.Log("Updating Visuals");
        for(int i = 0; i < trackHolders.Length; i++)
        {
            if (i == selectedIndex)
                trackHolders[i].Highlight();
            else
                trackHolders[i].Unhighlight();
        }
    }
    void SetText(String text) {
        textOverlay.text = text;
        textTimeout = AudioSettings.dspTime;
        textVisible = true;
    }
    void toggleTrack(int index) {

		if (tracks [index].isPlaying == false) {
			for(int i = 0; i<6; i++){

			tracks [i].Play ();
				if(i!=index) tracks[i].mute = true;
			}
		} else if (tracks [index].mute == false) {
        //if (tracks[index].isPlaying)
			//tracks[index].Stop();
			tracks [index].mute = true;

			SetText ("Stop");
		} else {
			SetText ("Play");
			int beatNumber = getBeatNumber ();
			double nextTime = (startTime + oneBeat * beatNumber);
			//tracks[index].Stop();
			// tracks[index].PlayScheduled(nextTime);
			tracks [index].mute = false;

		}
	}

        //Debug.Log(nextTime);
    
    int getBeatNumber() {
        double time = AudioSettings.dspTime;
        double delta = time - startTime;
        return Convert.ToInt32(Math.Ceiling(delta / (oneBeat)));
    }
    // Update is called once per frame
    void Update () {
        if (textVisible && AudioSettings.dspTime - textTimeout > 1)
        {
            textOverlay.text = "";
            textVisible = false;
        }
	}
}
