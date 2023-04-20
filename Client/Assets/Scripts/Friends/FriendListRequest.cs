using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FriendListRequest : MonoBehaviour
{

public GameObject FriendItemRequestPrefabSettings;
public GameObject FriendRequestListSettings;
AddFriendManager Addfriendmanager;
PlayerRequest playerrequest;
List<string> NamePlayer = new List<string>();
List<string> IDS = new List<string>();

// Start is called before the first frame update
//Function to get the List of players and Kick them

void Start()
{

    Addfriendmanager = gameObject.GetComponent<AddFriendManager>();
}
public async void ListPlayersRequest()
{

    foreach (Transform child in FriendRequestListSettings.transform)
    {
        Destroy(child.gameObject);
    }

    while (Addfriendmanager.listFriendsConfirmed.Count == 0)
    {
        await Task.Delay(100);
    }

    StartCoroutine(Waitunesecon());
}
IEnumerator Waitunesecon()
{
    yield return new WaitForSeconds(1);
    NamePlayer = Addfriendmanager.listFriendsConfirmed;
    IDS = Addfriendmanager.listFriendsIdsConfirmed;

    //Iterate players to get Nickname && ActorNumber
    for (int i = 0; i < NamePlayer.Count; i++)
    {
        //We create the userItem object
        GameObject userItem = (GameObject)Instantiate(FriendItemRequestPrefabSettings);

        userItem.transform.SetParent(FriendRequestListSettings.transform);
        userItem.transform.localScale = Vector3.one;

        //We configure the Nickname
        TMP_Text PlayerNameText = userItem.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            playerrequest = userItem.GetComponent<PlayerRequest>();

        PlayerNameText.text = NamePlayer[i];
            playerrequest.ID = IDS[i];


    }
}
}
