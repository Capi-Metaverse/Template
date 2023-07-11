using UnityEngine;
using System.Collections.Generic;


namespace Friends
{
    //Class that represents a friend of the current user in the application
    public class Friend
    {
        private string username;
        private string id;
        private string tags;

        public string Username { get => username; set => username = value; }
        public string Id { get => id; set => id = value; }
        public string Tags { get => tags; set => tags = value; }
    }

    public class FriendManager : MonoBehaviour
    {


        private List<Friend> friends = new List<Friend>();

        public List<Friend> Friends { get => friends; set => friends = value; }

    }
}