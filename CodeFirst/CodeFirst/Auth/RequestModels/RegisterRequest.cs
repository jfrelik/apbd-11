namespace CodeFirst.Auth.RequestModels;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}