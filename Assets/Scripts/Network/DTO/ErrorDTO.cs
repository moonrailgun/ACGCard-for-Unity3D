public class ErrorDTO : CommonDTO
{
    public int errorCode;

    public ErrorDTO()
        : base()
    {

    }

    public ErrorDTO(int errorCode)
        : base()
    {
        this.errorCode = errorCode;
    }
}