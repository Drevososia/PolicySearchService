namespace PolicySearchService.Models;

public class AuthRequest
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string OrgCode { get; set; } = string.Empty;
}
