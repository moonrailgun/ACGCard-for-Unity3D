public class LoginDTO : CommonDTO {
    public string username;
    public string password;
    public LoginDTO()
        :base()
    {
        
    }

    public LoginDTO(string username, string password)
        :base()
    {
        this.username = username;
        this.password = password;
    }
}
