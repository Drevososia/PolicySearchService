using PolicySearchService.Models;

namespace PolicySearchService.Services;

public interface IAuthService
{
    AuthResponse? Authenticate(AuthRequest request);
}
