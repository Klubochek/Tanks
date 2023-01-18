using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class RegistationModule 
{
    private TextMeshProUGUI _regStatusText;


    public void RegisterNewUser(RegistrationParams regParams,TextMeshProUGUI regStatus)
    {
        _regStatusText = regStatus;
        if ( regParams.Password!= regParams.ConfrimPassword) 
        {
            _regStatusText.text="Passwords do not match ";
            _regStatusText.color = Color.red;
            return; 
        }
        else
        {
            var request = new RegisterPlayFabUserRequest
            {
                Email = regParams.Email,
                Username = regParams.Username,
                Password = regParams.Password,
                DisplayName = regParams.Username
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
