using PolicySearchService.Models.Authorization;

namespace PolicySearchService.Services.AuthService;

public interface IAuthService
{
    AuthResponse? Authenticate(AuthRequest request);
}
