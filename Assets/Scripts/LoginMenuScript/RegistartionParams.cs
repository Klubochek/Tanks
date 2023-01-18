public class RegistrationParams
{
    public string Email {get; private set;}
    public string Username { get; private set;}
    public string Password { get; private set;}
    public string ConfrimPassword { get; private set;}

    public RegistrationParams(string email, string username,string password,string confirmPassword)
    {
        Email = email;
        Username = username;
        Password = password;
        ConfrimPassword= confirmPassword;
    }
}
