using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegistationAndLogin : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passField;
    [SerializeField] private TextMeshProUGUI logStatusText;


    [SerializeField] private TMP_InputField regEmailField;
    [SerializeField] private TMP_InputField regNickField;
    [SerializeField] private TMP_InputField regPass;
    [SerializeField] private TMP_InputField regConfirmPass;
    [SerializeField] private TextMeshProUGUI regStatusText;

    [SerializeField] private GameObject regMenu;
    [SerializeField] private GameObject logMenu;

    [SerializeField] private PlayerData playerData;

    public void OnRegTextClick()
    {
        logMenu.SetActive(false);
        regMenu.SetActive(true);
    }
    public void OnSignUpButtonClick()
    {
        if (regPass.text != regConfirmPass.text) {regStatusText.text = "Passwords do not match "; }
        else
        {
            var request = new RegisterPlayFabUserRequest
            {
                Email = regEmailField.text,
                Username = regNickField.text,
                Password = regPass.text,
                DisplayName = regNickField.text
            };
            PlayFabClientAPI.RegisterPlayFabUser(request, ShowRegResult, ShowRegError);
        }
    }

    

    public void OnBackButtonClick()
    {
       
        regMenu.SetActive(false);
        logMenu.SetActive(true);   
    }
    public void OnSignInButtonClick()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailField.text,
            Password = passField.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, ShowLoginResult, ShowLoginError);
    }
    private void ShowLoginError(PlayFabError obj)
    {
        logStatusText.text = obj.ErrorMessage;
        return;
    }

    private void ShowLoginResult(LoginResult obj)
    {
        logStatusText.color = Color.green;
        logStatusText.text=obj.ToString();
        Debug.Log(obj.PlayFabId);
        var request = new GetPlayerProfileRequest { PlayFabId=obj.PlayFabId};
        PlayFabClientAPI.GetPlayerProfile(request, SetCurrentUser, ShowLoginError);  
        
    }

    private void SetCurrentUser(GetPlayerProfileResult obj)
    {
        Debug.Log(obj.PlayerProfile.DisplayName);
        playerData.PlayerName = obj.PlayerProfile.DisplayName;
        SceneManager.LoadScene(1);
        return;
    }

    private void ShowRegError(PlayFabError obj)
    {
        regStatusText.text = obj.ErrorMessage;
        return;
    }

    private void ShowRegResult(RegisterPlayFabUserResult obj)
    {
        regStatusText.color = Color.green;
        regStatusText.text=obj.ToString();
        return;
    }
}
