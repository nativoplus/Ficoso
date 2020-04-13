namespace NativoPlusStudio.FCSOAuth
{
    public interface IBaseResponse
    {
        string Code { get; set; }
        string Message { get; set; }
        bool Status { get; set; }
    }
}