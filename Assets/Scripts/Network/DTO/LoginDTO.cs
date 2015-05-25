public class LoginDTO : CommonDTO {
    public string account;
    public string password;
    public string playerName;
    public string UUID;

    public string internalVersion;//内部版本号
    public string officialVersion;//正式版本号

    public LoginDTO()
        :base()
    {
        this.internalVersion = Global.Instance.internalVersion;
        this.officialVersion = Global.Instance.officialVersion;
    }

    public LoginDTO(string username, string password)
        :this()
    {
        this.account = username;
        this.password = password;
    }
}
