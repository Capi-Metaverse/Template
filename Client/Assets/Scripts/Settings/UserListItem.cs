
using System;
using UnityEngine;
using UnityEngine.UI;

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

        if (gameManager.GetUserRole() == UserRole.Admin)
        {
            kickButton.gameObject.SetActive(true);
        }

        
    }
    public void KickPlayer() { 
        GameManager.RPC_onKick(gameManager.GetRunner(),NumActor);  
    }

    public void ChangeAudioPlayer()
    {
        Debug.Log(slider.value);
        GameObjectPlayer.GetComponentInChildren<AudioSource>().volume = slider.value; 
    }

    public void MutePlayer()
    {
        Debug.Log(muteToggle.isOn);
        GameObjectPlayer.GetComponentInChildren<AudioSource>().mute = muteToggle.isOn;
    }



}