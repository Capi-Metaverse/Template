
using TMPro;
using UnityEditor;
using UnityEngine;

namespace AchievementModule
{
    public class AchievementReached : MonoBehaviour
    {
        public static void ShowAchievement(string achName, Transform achZone)
        {
            /*
            string path = "Assets/Module_Metaverse/Prefabs/Achievement/Achievement.prefab";
            GameObject achPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            Transform child = achPrefab.transform.GetChild(0);
            string text = child.GetComponent<TMP_Text>().text;
            text = achName;

            Instantiate(achPrefab, achZone);
            */
        }
    }
}

