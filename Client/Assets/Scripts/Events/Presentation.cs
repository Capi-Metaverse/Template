using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Presentation : MonoBehaviour
{
   
    public int current = 0;
    public List<Sprite> sprites;
    public SpriteRenderer renderer;

    
     public static bool IsEmpty<T>(List<T> list)
    {
        if (list == null) {
            return true;
        }
 
        return !list.Any();
    }

    
    public void OnAdvance(){
    
    if (current < (sprites.Count - 1))
    current++;
    renderer.sprite = sprites[current];


    }

    public void OnReturn(){

        if(current > 0){
        current--;
        renderer.sprite = sprites[current];
        }
        
    }
    public void OnDirect(){
        bool isEmpty = IsEmpty(sprites);
 
        if (isEmpty) {
            Debug.Log("List is Empty");
           
            renderer.sprite = sprites[0];
           
        }
        else {
            Debug.Log("List contains elements");
            renderer.sprite = sprites[0];
        }
    
    

    }
  
}