using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadPPTX : MonoBehaviour, IMetaEvent
{
    public GameObject ChooseFile;
    public FileExplorer fileExplorer;
    bool activao=false;
     public void activate(bool host){
            ChooseFile.SetActive(!activao); 
            fileExplorer.loco();
            
        }
      
    
}
