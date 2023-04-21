using Fusion;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class FriendList : MonoBehaviour {

    public GameObject FriendItemRequestPrefabSettings;
    public GameObject FriendRequestListSettings;
    public GameObject FriendItemPrefabSettings;
    public GameObject FriendListSettings;
    FriendManager FriendManager;
    FriendItem FriendItem;
    FriendRequest playerRequest;
    List<Friend> friends;
    [SerializeField] private bool displayMode;

    public bool DisplayMode { get => displayMode; set => displayMode = value; }

    // Start is called before the first frame update
    //Function to get the List of players and Kick them

    void Start()
    {
        FriendManager = gameObject.GetComponent<FriendManager>();
        friends = FriendManager.Friends;
    }
    public async void ListPlayers()
    {
        foreach (Transform child in FriendListSettings.transform)
        {
            Destroy(child.gameObject);
        }

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

        switch (DisplayMode)
        {
            case true:  
                {
                    //Iterate players to get Nickname && ActorNumber
                    for (int i = 0; i < friends.Count; i++)
                    {
                        if (friends[i].Tags == "confirmed")
                        {

                            //We create the userItem object
                            GameObject userItem = (GameObject)Instantiate(FriendItemPrefabSettings);

                            userItem.transform.SetParent(FriendListSettings.transform);
                            userItem.transform.localScale = Vector3.one;

                            //We configure the Nickname
                            TMP_Text PlayerNameText = userItem.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
                            FriendItem = userItem.GetComponent<FriendItem>();

                            PlayerNameText.text = friends[i].Username;
                            FriendItem.Username = friends[i].Username;
                            FriendItem.ID = friends[i].Id;
                        }
                    }
                    break;
                };
            case false:
                {

                    for (int i = 0; i < friends.Count; i++)
                    {
                        if (friends[i].Tags == "requester")
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
                    break;
                };
        }

    }
}
