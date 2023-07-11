using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace PresentationModule
{
    public class Presentation : MonoBehaviour
    {

        public int current = 0;
        public List<Sprite> sprites = new List<Sprite>();
        public new SpriteRenderer renderer;
        public TMP_Text loadingPressCanvas;

        /// <summary>
        /// Determines if the list is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">True, if the list is empty.</param>
        /// <returns></returns>
        public static bool IsEmpty<T>(List<T> list)
        {
            if (list == null)
            {
                return true;
            }
            return !list.Any();
        }

        /// <summary>
        /// Renders the next sprite.
        /// </summary>
        public void OnAdvance()
        {
            if (current < (sprites.Count - 1))
                current++;
            renderer.sprite = sprites[current];
        }

        /// <summary>
        /// Renders the previous sprite.
        /// </summary>
        public void OnReturn()
        {

            if (current > 0)
            {
                current--;
                renderer.sprite = sprites[current];
            }
        }

        /// <summary>
        /// Checks if the list has content and displays the first sprite.
        /// </summary>
        public void OnDirect()
        {
            bool isEmpty = IsEmpty(sprites);

            if (isEmpty)
            {
                Debug.Log("List is Empty");
                renderer.sprite = sprites[0];
                loadingPressCanvas.SetText("There´s nothing in file");
            }
            else
            {
                Debug.Log("List contains elements");
                loadingPressCanvas.enabled = false;
                renderer.sprite = sprites[0];
            }
        }
    }
}