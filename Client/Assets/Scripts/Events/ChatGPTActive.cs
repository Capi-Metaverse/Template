using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ChatGPTActive : MonoBehaviour, IMetaEvent
{
    public GameObject CanvasChatGPT;
    PlayerSpawn playerSpawn;
   
    private void Start() 
    {
        playerSpawn = GameObject.FindObjectOfType<PlayerSpawn>();
    }
    public void activate(bool host)
    {
        if (host == true)
        {
            CanvasChatGPT.SetActive(true);    
            playerSpawn.DesactiveALL();
            Cursor.visible = true;
            playerSpawn.setState(1);
            playerSpawn.playerToSpawn.GetComponent<SC_FPSController>().eventText.SetActive(false); 

            //Start Animator
            playerSpawn.animator.speed = 0;
            object[] content =new object[] {playerSpawn.playerToSpawn.GetComponent<PhotonView>().Owner.NickName,playerSpawn.animator.speed,"Stop&Replay"};
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(2,content,raiseEventOptions,SendOptions.SendReliable);     
        }  
        else 
        {
            CanvasChatGPT.SetActive(false); 
            playerSpawn.ActiveALL();
             playerSpawn.setState(0);
            playerSpawn.estado = 0; 

            //Stop Animation
            playerSpawn.animator.speed = 1;
            object[] content = new object[] {playerSpawn.playerToSpawn.GetComponent<PhotonView>().Owner.NickName,playerSpawn.animator.speed,"Stop&Replay"};
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(2,content,raiseEventOptions,SendOptions.SendReliable);
        } 
    }
    /*public void PressReturn()
    {
       CanvasChatGPT.SetActive(false); 
       playerSpawn.ActiveALL();
    }*/
}
