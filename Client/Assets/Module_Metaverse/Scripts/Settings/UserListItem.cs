
using System;
using UnityEngine;
using UnityEngine.UI;
using Manager;
public class UserListItem : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Slider slider;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private Button kickButton;
    private int numActor;
    private GameObject gameObjectPlayer;

    public int NumActor { get => numActor; set => numActor = value; }
    public GameObject GameObjectPlayer { get => gameObjectPlayer; set => gameObjectPlayer = value; }

    public void Start()
    {
        gameManager = GameManager.FindInstance();
        UserManager userManager = UserManager.FindInstance();

        if ( userManager.UserRole == UserRole.Admin)
        {
            kickButton.gameObject.SetActive(true);
        }

        
    }
    /// <summary>
    /// Kick Other players from de APP
    /// </summary>
    public void KickPlayer() { 
        RPCManager.RPC_onKick(PhotonManager.FindInstance().Runner,NumActor);  
    }
    /// <summary>
    /// Change the Audio Level
    /// </summary>
    public void ChangeAudioPlayer()
    {
        Debug.Log(slider.value);
        GameObjectPlayer.GetComponentInChildren<AudioSource>().volume = slider.value; 
    }
    /// <summary>
    /// Mute the audio from other users
    /// </summary>
    public void MutePlayer()
    {
        Debug.Log(muteToggle.isOn);
        GameObjectPlayer.GetComponentInChildren<AudioSource>().mute = muteToggle.isOn;
    }



}