namespace Common.AuthViewModels;

public class ResetPasswordResponse
{
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; }
}
 
