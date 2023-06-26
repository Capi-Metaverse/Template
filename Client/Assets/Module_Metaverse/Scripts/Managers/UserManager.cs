using UnityEngine;

public enum UserRole
{
    Admin,
    Moderator,
    Client,
    Employee
}
public class UserManager : MonoBehaviour
{

    [SerializeField] private string _username = "None";
    public string Username
    {
        get { return _username; }
        set { _username = value; }
    }

    [SerializeField] private string _email = "None";
    public string Email
    {
        get { return _email; }
        set { _email = value; }
    }

    [SerializeField] private string _userID = "None";
    public string UserID
    {
        get { return _userID; }
        set { _userID = value; }
    }

    [SerializeField] private UserRole _userRole;
    public UserRole UserRole { get => _userRole; set => _userRole = value; }


    //Static function to get the singleton
    public static UserManager FindInstance()
    {
        return FindObjectOfType<UserManager>();
    }


    //Initialization
    private void Awake()
    {
        //When this component awake, it get the others game managers
        UserManager[] managers = FindObjectsOfType<UserManager>();

        //Check if there is more managers
        if (managers != null && managers.Length > 1)
        {
            // There should never be more than a single App container in the context of this sample.
            Destroy(gameObject);
            return;

        }
    }





    }
