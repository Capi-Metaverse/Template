using Fusion;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RPCManager : SimulationBehaviour
{



    //Static function to get the singleton
    public static RPCManager FindInstance()
    {
        return FindObjectOfType<RPCManager>();
    }

    //Initialization
    private void Awake()
    {
        //When this component awake, it get the others game managers
        RPCManager[] managers = FindObjectsOfType<RPCManager>();

        //Check if there is more managers
        if (managers != null && managers.Length > 1)
        {
            // There should never be more than a single App container in the context of this sample.
            Destroy(gameObject);
            return;

        }
    }


    /// <summary>
    /// Kick oter users by the ID
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="numActor"></param>
    [Rpc]
    public static async void RPC_onKick(NetworkRunner runner, int numActor)
    {



        Debug.Log(numActor);

        int PlayerID = GameManager.FindInstance().GetCurrentPlayer().GetComponent<NetworkPlayer>().ActorID;
        Debug.Log(PlayerID);
        if (numActor == PlayerID)
        {
            await GameManager.FindInstance().Disconnect();

            SceneManager.LoadSceneAsync("Lobby");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }

    }

    [Rpc]
    public static void RPC_MuteAllPlayers(NetworkRunner runner, bool mute, int numActor)
    {
        int PlayerID = GameManager.FindInstance().GetCurrentPlayer().GetComponent<NetworkPlayer>().ActorID;

        //If is not the player who started the muteAll
        if (numActor != PlayerID)
        {
            VoiceManager voiceChat = GameManager.FindInstance().GetCurrentPlayer().GetComponent<CharacterInputHandler>().voiceChat;
            UserStatus userStatus = GameManager.FindInstance().UserStatus;

            voiceChat.MuteAllPlayersAudio(userStatus, mute);
        }
    }

    /// <summary>
    /// All the people Download the Images of PDF
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="routes"></param>
    [Rpc]
    public static async void RPC_DownloadImages(NetworkRunner runner, string routes)
    {
        JObject JsonRoutes = JObject.Parse(routes);
        Debug.Log(JsonRoutes.ToString());

        FileSelector fileSelector = GameObject.Find("ChooseFile").GetComponent<FileSelector>();
        fileSelector.PresentationUpload._json = JsonRoutes;
        fileSelector.PresentationUpload.ClearPresentation();

    }
    /// <summary>
    /// All the people can see Back in the presentation
    /// </summary>
    /// <param name="runner"></param>
    [Rpc]
    public static void RPC_BackPress(NetworkRunner runner)
    {
        Presentation presentation = GameObject.Find("Presentation").GetComponent<Presentation>();
        presentation.OnReturn();

    }
    /// <summary>
    /// All the people can see Advance in the presentation
    /// </summary>
    /// <param name="runner"></param>
    [Rpc]
    public static void RPC_AdvancePress(NetworkRunner runner)
    {
        Presentation presentation = GameObject.Find("Presentation").GetComponent<Presentation>();
        presentation.OnAdvance();

    }
    /// <summary>
    /// All the people can see Open the door
    /// </summary>
    /// <param name="runner"></param>
    [Rpc]
    public static void RPC_OpenDoor(NetworkRunner runner)
    {
        TriggerEntranceDoor door = GameObject.Find("GlassEntrance").GetComponentInChildren<TriggerEntranceDoor>();
        if (door.membersInside == 0) door.OpenDoor();
        door.membersInside++;

    }
    /// <summary>
    /// All the people can see close the door
    /// </summary>
    /// <param name="runner"></param>
    [Rpc]
    public static void RPC_CloseDoor(NetworkRunner runner)
    {
        TriggerEntranceDoor door = GameObject.Find("GlassEntrance").GetComponentInChildren<TriggerEntranceDoor>();
        door.membersInside--;
        if (door.membersInside == 0)
        {
            door.CloseDoor();
        }

    }
    /// <summary>
    /// Increases the distance at which a certain user can be listened to
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="actorID"></param>
    [Rpc]
    public static void RPC_PrimarySpeaker(NetworkRunner runner, int actorID)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkPlayer>().ActorID == actorID)
            {
                player.GetComponentInChildren<AudioSource>().maxDistance = 500;
            }
        }

    }
    /// <summary>
    /// All the people can see, the lines was paint by others
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="Lines"></param>
    /// <param name="NumMaterial"></param>
    /// <param name="gross"></param>
    [Rpc(RpcSources.All, RpcTargets.All, InvokeLocal = false)]
    public static void RPC_LinesSend(NetworkRunner runner, Vector3[] Lines,float gross, int orderInLayer, Color color)
    {


        Debug.Log(Lines.Length);

        DrawLinesOnPlane drawLinesOnPlane = GameObject.Find("Plane").GetComponent<DrawLinesOnPlane>();
        drawLinesOnPlane.dibujoetc(Lines, gross, orderInLayer, color);
    }

    [Rpc]
    public static void RPC_LinesClear(NetworkRunner runner)
    {
        DrawLinesOnPlane drawLinesOnPlane = GameObject.Find("Plane").GetComponent<DrawLinesOnPlane>();
        drawLinesOnPlane.FunctionClear();
    }

}
