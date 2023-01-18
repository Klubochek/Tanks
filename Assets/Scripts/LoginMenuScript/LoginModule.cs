using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginModule
{
    public LoginModule(PlayerData playerData)
    {
        _playerData = playerData;
    }

    private PlayerData _playerData;
    private TextMeshProUGUI _logResultText;


    public void LogIn(string email, string password, TextMeshProUGUI logResult)
    {
        _logResultText = logResult;
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, ShowLoginResult, ShowLoginError);
    }
    private void ShowLoginError(PlayFabError obj)
    {
        _logResultText.text = obj.ErrorMessage;
        return;
    }

    private void ShowLoginResult(LoginResult obj)
    {
        _logResultText.color = Color.green;
        _logResultText.text = obj.ToString();
        Debug.Log(obj.PlayFabId);
        var request = new GetPlayerProfileRequest { PlayFabId = obj.PlayFabId };
        PlayFabClientAPI.GetPlayerProfile(request, SetCurrentUser, ShowLoginError);

    }

    private void SetCurrentUser(GetPlayerProfileResult obj)
    {
        Debug.Log(obj.PlayerProfile.DisplayName);
        _playerData.PlayerName = obj.PlayerProfile.DisplayName;
        SceneManager.LoadScene(1);
        return;
    }
}