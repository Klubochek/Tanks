using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginMenuUI : MonoBehaviour
{
    //Login fields and text
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passField;
    [SerializeField] private TextMeshProUGUI logStatusText;

    //Registation fields and text
    [SerializeField] private TMP_InputField regEmailField;
    [SerializeField] private TMP_InputField regNickField;
    [SerializeField] private TMP_InputField regPass;
    [SerializeField] private TMP_InputField regConfirmPass;
    [SerializeField] private TextMeshProUGUI regStatusText;

    //Menu objects
    [SerializeField] private GameObject regMenu;
    [SerializeField] private GameObject logMenu;

    
    // ScriptableObject for PlayerData;
    [SerializeField] private PlayerData playerData;

    private RegistationModule registationModule;
    private LoginModule loginModule;

    private void Awake()
    {
        loginModule = new LoginModule(playerData);
    }

    public void OnRegTextClick()
    {
        logMenu.SetActive(false);
        regMenu.SetActive(true);
        registationModule = new RegistationModule();
    }
    public void OnSignUpButtonClick()
    {
        registationModule.RegisterNewUser(regNickField.text, regEmailField.text, regPass.text, regConfirmPass.text,regStatusText);
    }
    public void OnBackButtonClick()
    {
        regMenu.SetActive(false);
        logMenu.SetActive(true);
    }
    public void OnSignInButtonClick()
    {
        loginModule.LogIn(emailField.text, passField.text, logStatusText);
    }
   

   
}
