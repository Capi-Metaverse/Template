using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PhotonEventManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject fileExplorer;//Object in scene map, is the manager for the FileExploring system
    public TMP_Text loadingPressCanvas;
    private Compressor compressor = new Compressor();//Class used to compress video when presenting


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
           
            //Change animations
            case 2:
                {
                    object[] data = (object[]) photonEvent.CustomData;

                    GameObject[] playersInGame = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject player in playersInGame)
                    {
                        if (player.GetComponent<PhotonView>().Owner.NickName == (string) data[0])
                        {
                            switch ((string) data[2])
                            {
                                case "Walking":
                                {
                                    player
                                        .transform
                                        .GetChild(0)
                                        .GetComponent<Animator>()
                                        .SetBool("Walking",(bool) data[1]);
                                    break;
                                }
                                case "Running":
                                {
                                    player
                                        .transform
                                        .GetChild(0)
                                        .GetComponent<Animator>()
                                        .SetBool("Running",(bool) data[1]);
                                    break;
                                }
                                case "Stop&Replay":
                                {
                                    player
                                        .transform
                                        .GetChild(0)
                                        .GetComponent<Animator>()
                                        .speed = (float) data[1];
                                    break;
                                }
                                case "SpeedAnim":
                                {
                                    player
                                        .transform
                                        .GetChild(0)
                                        .GetComponent<Animator>()
                                        .SetFloat("Speed",(float) data[1]);
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            //Light Events
            case 21:
                {
                    object[] data = (object[]) photonEvent.CustomData;

                    GameObject eventObject = GameObject.Find((string) data[0]);

                    eventObject.GetComponent<Lamp>().activate(false);
                    break;
                }
            //Event FileExplorer(GET)
            case 22:
                {
                    loadingPressCanvas = GameObject.Find("LoadingText").GetComponent<TMP_Text>();
                    loadingPressCanvas.enabled = true;
                    loadingPressCanvas.text = "Loading";
                    object[] data = (object[]) photonEvent.CustomData;
                    fileExplorer = GameObject.Find("ChooseFile");
                    fileExplorer.GetComponent<FileExplorer>().downloadImages((string) data[0]);
                    break;
                }
            //Event to move slide
            case 23:
                {
                    object[] data = (object[]) photonEvent.CustomData;

                    if ((string) data[0] == "Back")
                    {
                        //Back presentation
                        GameObject eventObject = GameObject.Find("Back");
                        eventObject.GetComponent<BackPresentation>().activate(false);
                    }
                    else
                    {
                        //Advance presentation
                        GameObject eventObject = GameObject.Find("Advance");
                        eventObject.GetComponent<AdvancePresentation>().activate(false);
                    }
                    break;
                }
            //Event FileExplorer Video
            case 24:
                {
                    loadingPressCanvas = GameObject.Find("LoadingText").GetComponent<TMP_Text>();
                    loadingPressCanvas.enabled = true;
                    loadingPressCanvas.text = "Loading";
                    object[] data = (object[]) photonEvent.CustomData;
                    fileExplorer = GameObject.Find("ChooseFile");
                    fileExplorer.GetComponent<FileExplorer>().SetVideo((string) data[0],(string) data[1],compressor.Decompress((byte[]) data[2]));
                    break;
                }
            //Event FileExplorer Image
            case 25:
                {
                    loadingPressCanvas = GameObject.Find("LoadingText").GetComponent<TMP_Text>();
                    loadingPressCanvas.enabled = true;
                    loadingPressCanvas.text = "Loading";
                    object[] data = (object[]) photonEvent.CustomData;
                    fileExplorer = GameObject.Find("ChooseFile");
                    fileExplorer.GetComponent<FileExplorer>().SetImage((byte[]) data[0]);
                    break;
                }
        }
    }
    
   

   
}
