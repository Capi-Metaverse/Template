using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Presentation : MonoBehaviour
{
   
    public int current = 0;
    public List<Sprite> sprites = new List<Sprite>();
    public new SpriteRenderer renderer;

    
    public static bool IsEmpty<T>(List<T> list)
    {
        if (list == null) 
        {
            return true;
        }
        return !list.Any();
    }

    //splice advance event
    public void OnAdvance()
    { 
        if (current < (sprites.Count - 1))
        current++;
        renderer.sprite = sprites[current];
    }
    //splice return event
    public void OnReturn(){

        if(current > 0)
        {
            current--;
            renderer.sprite = sprites[current];
        }
        
    }
    
    //event to check if the list has content and display the first splice
    public void OnDirect(){
        bool isEmpty = IsEmpty(sprites);
 
        if (isEmpty) 
        {
            Debug.Log("List is Empty");
            renderer.sprite = sprites[0];   
        }
        else 
        {
            Debug.Log("List contains elements");
            renderer.sprite = sprites[0];
        }
    }
}