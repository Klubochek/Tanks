using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class RegistationModule : MonoBehaviour
{
    private TextMeshProUGUI _regStatusText;


    public void RegisterNewUser(string username, string email, string password, string spassword,TextMeshProUGUI regStatus)
    {
        _regStatusText = regStatus;
        if (password != spassword) 
        {
            _regStatusText.text="Passwords do not match ";
            _regStatusText.color = Color.red;
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
        _regStatusText.text = obj.ErrorMessage;
        _regStatusText.color = Color.red;
        return;
    }

    private void ShowRegResult(RegisterPlayFabUserResult obj)
    {
        _regStatusText.color = Color.green;
        _regStatusText.text = obj.ToString();
        return;
    }
}
