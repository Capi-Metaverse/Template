
using System.Collections.Generic;

using TMPro;
using Manager;
using UnityEngine;

public class FriendList : MonoBehaviour
{

    [SerializeField] private GameObject FriendItemRequestPrefabSettings;
    [SerializeField] private GameObject FriendRequestListSettings;
    [SerializeField] private GameObject FriendItemPrefabSettings;
    [SerializeField] private GameObject FriendListSettings;

    private GameManager gameManager;
    private FriendManager FriendManager;
    private FriendItem FriendItem;
    private FriendRequest playerRequest;
    private List<Friend> friends;

    [SerializeField] private bool displayMode;

    public bool DisplayMode { get => displayMode; set => displayMode = value; }

    // Start is called before the first frame update
    //Function to get the List of players and Kick them
    /// <summary>
    /// Destroy the children and clean up the lists
    /// </summary>
    void Start()
    {
        gameManager = GameManager.FindInstance();
        FriendManager = gameObject.GetComponent<FriendManager>();
        friends = FriendManager.Friends;
    }
    public void CleanFriendsPanel()
    {
        foreach (Transform child in FriendListSettings.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in FriendRequestListSettings.transform)
        {
            Destroy(child.gameObject);
        }
    }
    /// <summary>
    /// Iterate players to get Nickname && ActorNumber, each time this function is called, the lists are cleaned and the children are destroyed.
    /// </summary>
    public void InstanceFriendItem()
    {
        CleanFriendsPanel();
        
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
                FriendItem.Id = friends[i].Id;
            }

            else if (friends[i].Tags == "requester")
            {
                //We create the userItem object
                GameObject userItem = (GameObject)Instantiate(FriendItemRequestPrefabSettings);

                userItem.transform.SetParent(FriendRequestListSettings.transform);
                userItem.transform.localScale = Vector3.one;

                //We configure the Nickname
                TMP_Text PlayerNameText = userItem.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
                playerRequest = userItem.GetComponent<FriendRequest>();

                PlayerNameText.text = friends[i].Username;
                playerRequest.Id = friends[i].Id;
            }
        }
    }
}
