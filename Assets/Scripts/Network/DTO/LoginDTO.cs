public class LoginDTO : CommonDTO {
    public string account;
    public string password;
    public string playerName;
    public string UUID;

    public LoginDTO()
        :base()
    {
        
    }

    public LoginDTO(string username, string password)
        :base()
    {
        this.account = username;
        this.password = password;
    }
}
