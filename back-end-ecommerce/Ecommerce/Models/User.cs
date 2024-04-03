public class User : BaseModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? UserType { get; set; }
}
