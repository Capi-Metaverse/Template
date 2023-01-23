using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Presentation : MonoBehaviour
{
    
    public int current = -1;
    public List<Sprite> sprites;
    public SpriteRenderer renderer;
    
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
   
}
