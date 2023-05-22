using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class AchivementList : MonoBehaviour
{
    [SerializeField] private GameObject AchivementsUIItem;
    [SerializeField] private GameObject AchivementsUIList;
    List<Achievement> list;


    public AchievementsManager achievementsManager;
 
   
    public void InstanceAchivementItem()
    {
        list = achievementsManager.currentAchievements;

        //Iterate players to get Nickname && ActorNumber
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].activate == true)
            {

                //We create the userItem object
                GameObject userItem = (GameObject)Instantiate(AchivementsUIItem);

                userItem.transform.SetParent(AchivementsUIList.transform);
                userItem.transform.localScale = Vector3.one;

                //We configure the Nickname
                TMP_Text PlayerNameText = userItem.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();

                PlayerNameText.text = list[i].Name;
            }
        }
        }
}
