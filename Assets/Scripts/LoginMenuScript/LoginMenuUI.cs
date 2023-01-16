using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginMenuUI : MonoBehaviour
{
    //Login fields and text
    [SerializeField] private TMP_InputField _emailField;
    [SerializeField] private TMP_InputField _passField;
    [SerializeField] private TextMeshProUGUI _logStatusText;

    //Registation fields and text
    [SerializeField] private TMP_InputField _regEmailField;
    [SerializeField] private TMP_InputField _regNickField;
    [SerializeField] private TMP_InputField _regPass;
    [SerializeField] private TMP_InputField _regConfirmPass;
    [SerializeField] private TextMeshProUGUI _regStatusText;

    //Menu objects
    [SerializeField] private GameObject _regMenu;
    [SerializeField] private GameObject _logMenu;

    
    // ScriptableObject for PlayerData;
    [SerializeField] private PlayerData _playerData;

    private RegistationModule _registationModule;
    private LoginModule _loginModule;

    private void Awake()
    {
        _loginModule = new LoginModule(_playerData);
    }

    public void OnRegTextClick()
    {
        _logMenu.SetActive(false);
        _regMenu.SetActive(true);
        _registationModule = new RegistationModule();
    }
    public void OnSignUpButtonClick()
    {
        _registationModule.RegisterNewUser(_regNickField.text, _regEmailField.text, _regPass.text, _regConfirmPass.text,_regStatusText);
    }
    public void OnBackButtonClick()
    {
        _regMenu.SetActive(false);
        _logMenu.SetActive(true);
    }
    public void OnSignInButtonClick()
    {
        _loginModule.LogIn(_emailField.text, _passField.text, _logStatusText);
    }
   

   
}
