using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadPPTX : MonoBehaviour, IMetaEvent
{
    public GameObject ChooseFile;
    public FileExplorer fileExplorer;
    bool activao=false;

    [System.Obsolete]
    public void activate(){
            ChooseFile.SetActive(!activao); 
            fileExplorer.loco();
            
        }
      
    
}
