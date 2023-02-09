using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomItem : MonoBehaviour
{
  /*---------------------VARIABLES-----------------*/
  public TMP_Text roomName;

  public RoomInfo RoomInfo { get; private set; }
  LobbyManager manager;

  /*---------------------METHODS------------------*/
  private void Start()
  {
    manager = FindObjectOfType<LobbyManager>();//We get the LobbyManager from scene
  }
  public void SetRoomName(string _roomName)
  //Set name of the room
  {
    roomName.text = _roomName;
  }
  public void OnClickItem()
  //Enter into LobbyRoom
  {
    manager.OnClickJoinRoom(roomName.text);
  }

  public void SetRoomInfo(RoomInfo roomInfo)
  {
    RoomInfo = roomInfo;
    roomName.text = roomInfo.Name;
  }
}
