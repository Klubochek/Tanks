using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginModule : MonoBehaviour
{
    public LoginModule(PlayerData playerData)
    {
        this.playerData = playerData;
    }

    private PlayerData playerData;
    private TextMeshProUGUI logResultText;


    public void LogIn(string email, string password, TextMeshProUGUI logResult)
    {
        logResultText = logResult;
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, ShowLoginResult, ShowLoginError);
    }
    private void ShowLoginError(PlayFabError obj)
    {
        logResultText.text = obj.ErrorMessage;
        return;
    }

    private void ShowLoginResult(LoginResult obj)
    {
        logResultText.color = Color.green;
        logResultText.text = obj.ToString();
        Debug.Log(obj.PlayFabId);
        var request = new GetPlayerProfileRequest { PlayFabId = obj.PlayFabId };
        PlayFabClientAPI.GetPlayerProfile(request, SetCurrentUser, ShowLoginError);

    }

    private void SetCurrentUser(GetPlayerProfileResult obj)
    {
        Debug.Log(obj.PlayerProfile.DisplayName);
        playerData.PlayerName = obj.PlayerProfile.DisplayName;
        SceneManager.LoadScene(1);
        return;
    }
}