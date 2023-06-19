using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

public class AudioVideoStates
{
    public bool subAudio = true;
    public bool pubAudio = false;
    public bool muteAllAudio = false;
}

public class VoiceManager
{

    //Micro Sprites
    public Sprite MicroOff;
    public Sprite MicroOn;

    public AudioVideoStates AudioVideoState = new AudioVideoStates();

    public Recorder recorder;

    public void GetGameObjects()
    {
        MicroOff = Resources.Load<Sprite>("Sprites/UI/micro_off");
        MicroOn = Resources.Load<Sprite>("Sprites/UI/micro_on");
        recorder = GameObject.Find("Network runner").GetComponent<Recorder>();
    }

    /// <summary>
    /// Mute and Unmute and change the sprite
    /// </summary>
    /// <param name="estado"></param>
    public void MuteAudio(UserStatus estado)
    {
        if (AudioVideoState.muteAllAudio == false)
        {
            if (AudioVideoState.pubAudio == true)
            {
                recorder.TransmitEnabled = false;
                AudioVideoState.pubAudio = false;
                Debug.Log("[PhotonVoice] Micro Off");

                if (estado == UserStatus.InGame)
                    GameObject.Find("Micro").GetComponent<Image>().sprite = MicroOff;
            }
            else
            {
                recorder.TransmitEnabled = true;
                AudioVideoState.pubAudio = true;
                Debug.Log("[PhotonVoice] Micro On");

                if (estado == UserStatus.InGame)
                    GameObject.Find("Micro").GetComponent<Image>().sprite = MicroOn;
            }
        }

    }

    public void MuteAllPlayersAudio(UserStatus estado, bool mute)
    {
        AudioVideoState.muteAllAudio = mute;

        if (AudioVideoState.muteAllAudio == false)
        {

            if (AudioVideoState.pubAudio == true)
            {
                recorder.TransmitEnabled = true;
                Debug.Log("[PhotonVoice] Micro On");

                if (estado == UserStatus.InGame)
                    GameObject.Find("Micro").GetComponent<Image>().sprite = MicroOn;
            }
        }
        else
        {
            recorder.TransmitEnabled = false;
            Debug.Log("[PhotonVoice] Micro Off");
            if (estado == UserStatus.InGame)
                GameObject.Find("Micro").GetComponent<Image>().sprite = MicroOff;
        }
    }
}
