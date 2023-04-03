using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class AudioVideoStates
{
    public bool subAudio = true;
    public bool pubAudio = false;
}

public class VoiceManager
{

    //Micro Sprites
    public Sprite MicroOff;
    public Sprite MicroOn;

    private AudioVideoStates AudioVideoState = new AudioVideoStates();

    public Photon.Voice.Unity.Recorder recorder;
    // Start is called before the first frame update

    public void GetGameObjects()
    {
        MicroOff = Resources.Load<Sprite>("Sprites/micro_off");
        MicroOn = Resources.Load<Sprite>("Sprites/micro_on");
        recorder = GameObject.Find("Network runner").GetComponent<Photon.Voice.Unity.Recorder>();
    }

    public void MuteAudio(UserStatus estado)
    {
        Debug.Log("mute");
        if (AudioVideoState.pubAudio == true)
        {
            recorder.TransmitEnabled = false;
            //mRtcEngine.MuteLocalAudioStream(true);
            AudioVideoState.pubAudio = false;
            Debug.Log("MicroOff");

            if (estado==UserStatus.InGame)
                GameObject.Find("Micro").GetComponent<Image>().sprite = MicroOff;
        }
        else
        {
            recorder.TransmitEnabled = true;
            //mRtcEngine.MuteLocalAudioStream(false);
            AudioVideoState.pubAudio = true;
            Debug.Log("MicroOn");

            if (estado == UserStatus.InGame)
                GameObject.Find("Micro").GetComponent<Image>().sprite = MicroOn;
        }
    }
}
