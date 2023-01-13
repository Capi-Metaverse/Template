using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{

    public Light light;
    
    public void activate(){
        light.enabled = !light.enabled;

    }

    
}
