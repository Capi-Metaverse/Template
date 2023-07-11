using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;

namespace Settings
{
    public class ControlSensitivity : MonoBehaviour
    {
        public Slider slider;
        public float sliderValue;
        public ControlSensitivityPlayFab controlSensitivityPlayFab;
        GameManager gameManager;

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameManager.FindInstance();
            slider.value = gameManager.Sensitivity;

        }
        /// <summary>
        /// Change the sensitivity
        /// </summary>
        /// <param name="value"></param>
        public void ChangeSlider(float value)
        {
            sliderValue = value;
            gameManager.Sensitivity = slider.value;
            controlSensitivityPlayFab.ChangeSensitivity(gameManager.Sensitivity);
            //Debug.Log(sensi);
        }
    }
}
