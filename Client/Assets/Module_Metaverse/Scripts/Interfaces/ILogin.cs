using PlayFab.PfEditor.EditorModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILogin 
{
    public void Register(string usernameInput, string emailInput, string passwordInput);
    public void OnReset(string emailInput);
    public void Login(string emailInput, string passwordInput);
   

}
