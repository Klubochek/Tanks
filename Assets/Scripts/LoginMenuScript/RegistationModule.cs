using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class RegistationModule : MonoBehaviour
{
    private TextMeshProUGUI regStatusText;


    public void RegisterNewUser(string username, string email, string password, string spassword,TextMeshProUGUI regStatus)
    {
        regStatusText = regStatus;
        if (password != spassword) 
        {
            regStatusText.text="Passwords do not match ";
            regStatusText.color = Color.red;
            return; 
        }
        else
        {
            var request = new RegisterPlayFabUserRequest
            {
                Email = email,
                Username = username,
                Password = password,
                DisplayName = username
            };
            PlayFabClientAPI.RegisterPlayFabUser(request, ShowRegResult, ShowRegError);
            
            return;
        }
    }
    private void ShowRegError(PlayFabError obj)
    {
        regStatusText.text = obj.ErrorMessage;
        regStatusText.color = Color.red;
        return;
    }

    private void ShowRegResult(RegisterPlayFabUserResult obj)
    {
        regStatusText.color = Color.green;
        regStatusText.text = obj.ToString();
        return;
    }
}
