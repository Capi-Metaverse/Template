

public interface ILogin 
{
    public void Register(string usernameInput, string emailInput, string passwordInput);
    public void OnReset(string emailInput);
    public void Login(string emailInput, string passwordInput);
   

}
