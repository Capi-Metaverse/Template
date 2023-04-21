using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FriendListRequest : MonoBehaviour
{

    public GameObject FriendItemRequestPrefabSettings;
    public GameObject FriendRequestListSettings;
    FriendManager FriendManager;
    FriendRequest playerRequest;
    List<Friend> friends;

    // Start is called before the first frame update
    //Function to get the List of players and Kick them

    void Start()
    {
        FriendManager = gameObject.GetComponent<FriendManager>();
        friends = FriendManager.Friends;
    }
public async void ListPlayersRequest()
{

    foreach (Transform child in FriendRequestListSettings.transform)
    {
        Destroy(child.gameObject);
    }

    while (friends.Count == 0)
    {
        await Task.Delay(100);
    }

    StartCoroutine(Waitunesecon());
}
IEnumerator Waitunesecon()
{
    yield return new WaitForSeconds(1);

    //Iterate players to get Nickname && ActorNumber
    for (int i = 0; i < friends.Count; i++)
    {
        //We create the userItem object
        GameObject userItem = (GameObject)Instantiate(FriendItemRequestPrefabSettings);

        userItem.transform.SetParent(FriendRequestListSettings.transform);
        userItem.transform.localScale = Vector3.one;

        //We configure the Nickname
        TMP_Text PlayerNameText = userItem.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            playerRequest = userItem.GetComponent<FriendRequest>();

        PlayerNameText.text = friends[i].Username;
            playerRequest.ID = friends[i].Id;
    }
}
}
