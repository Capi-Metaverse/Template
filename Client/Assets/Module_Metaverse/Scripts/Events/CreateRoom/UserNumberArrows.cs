using System;
using TMPro;
using UnityEngine;

namespace CreateRoom
{
    public class UserNumberArrows : MonoBehaviour, ICreateRoomArrows
    {
        [SerializeField] private TMP_InputField inputPlayers;

        /// <summary>
        /// Decrease the number of users that can enter in the room
        /// </summary>
        public void OnLeftClick()
        {
            int userCount = Int32.Parse(inputPlayers.text);

            if (userCount > 0)
            {
                inputPlayers.text = (userCount - 1).ToString();
            }
        }

        /// <summary>
        /// Increase the number of users that can enter in the room
        /// </summary>
        public void OnRightClick()
        {
            int userCount = Int32.Parse(inputPlayers.text);

            if (userCount < 8)
            {
                inputPlayers.text = (userCount + 1).ToString();
            }
        }
    }
}

