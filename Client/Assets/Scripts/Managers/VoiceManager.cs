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

    public void GetGameObjects()
    {
        MicroOff = Resources.Load<Sprite>("Sprites/UI/micro_off");
        MicroOn = Resources.Load<Sprite>("Sprites/UI/micro_on");
        recorder = GameObject.Find("Network runner").GetComponent<Photon.Voice.Unity.Recorder>();
    }

    public void MuteAudio(UserStatus estado)
    {
        
        if (AudioVideoState.pubAudio == true)
        {
            recorder.TransmitEnabled = false;
            AudioVideoState.pubAudio = false;
            Debug.Log("[PhotonVoice-VoiceManager] Micro Off");

            if (estado==UserStatus.InGame)
                GameObject.Find("Micro").GetComponent<Image>().sprite = MicroOff;
        }
        else
        {
            recorder.TransmitEnabled = true;
            AudioVideoState.pubAudio = true;
            Debug.Log("[PhotonVoice-VoiceManager] Micro On");

            if (estado == UserStatus.InGame)
                GameObject.Find("Micro").GetComponent<Image>().sprite = MicroOn;
        }
    }
}
