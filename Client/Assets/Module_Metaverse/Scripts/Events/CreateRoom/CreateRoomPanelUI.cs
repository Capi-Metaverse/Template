using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
using Player;
namespace CreateRoom
{
    public class CreateRoomPanelUI : MonoBehaviour
    {

        //Panels
        [SerializeField] private GameObject choosePanel;
        [SerializeField] private GameObject joinPanel;
        [SerializeField] private GameObject createPanel;

        GameManager gameManager;

        public void OpenUI()
        {
            this.gameObject.SetActive(true);
            choosePanel.SetActive(true);
            createPanel.SetActive(false);
            joinPanel.SetActive(false);
            gameManager = GameManager.FindInstance();
            GameObject player = PhotonManager.FindInstance().CurrentPlayer;
            player.GetComponent<CharacterInputHandler>().changeRoomPanel = this.gameObject;

            UIManager uiManager = UIManager.FindInstance();
            PauseManager pauseManager = PauseManager.FindInstance();
            pauseManager.Pause();
            uiManager.SetUIOff();

            //player.GetComponent<CharacterInputHandler>().DeactivateALL();
        }

        public void CloseUI()
        {
            this.gameObject.SetActive(false);
            GameObject player = PhotonManager.FindInstance().CurrentPlayer;
            //player.GetComponent<CharacterInputHandler>().ActiveALL();
        }

        public void OpenJoinUI()
        {
            choosePanel.SetActive(false);
            joinPanel.SetActive(true);
        }

        public void OpenCreateUI()
        {
            choosePanel.SetActive(false);
            createPanel.SetActive(true);
        }
    }
}
