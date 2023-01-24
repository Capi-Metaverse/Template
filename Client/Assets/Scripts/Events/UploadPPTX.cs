using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadPPTX : MonoBehaviour, IMetaEvent
{
    public GameObject ChooseFile;
    bool activao=false;
     public void activate(){
            ChooseFile.SetActive(!activao); 
            
        }
      
    
}
