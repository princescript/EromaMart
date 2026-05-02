using Server.DTOs.Auth;
using Server.Entities;
using Server.Helpers;
using Server.Repositories;



namespace Server.Services;

public interface IUserService
{
    Task<long?> RegisterUserAsync(RegisterRequest request, string ip, CancellationToken ct = default);
    Task<AuthResponse?> LoginUserAsync(LoginRequest request, string ip, CancellationToken ct = default);
}
public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IJwtHelper _jwtHelper;
    public UserService(IUserRepository repository, IJwtHelper jwtHelper)
    {
        _repository = repository;
        _jwtHelper = jwtHelper;
    }
    public async Task<long?> RegisterUserAsync(RegisterRequest request, string ip, CancellationToken ct = default)
    {
        if (request == null)
            return null;
        if (string.IsNullOrWhiteSpace(request.user_name) ||
            string.IsNullOrWhiteSpace(request.phone) ||
            string.IsNullOrWhiteSpace(request.password))
            return null;

        var userName = request.user_name.Trim().ToLower();
        var userEmail = request.email?.Trim().ToLower();
        var userPhone = request.phone.Trim();

        var foundUser = await _repository.FindUserAsync(x=>x.phone == userPhone,ct);

        if (foundUser != null) 
            return null;

        if (userName.Length < 3 || userName.Length > 100)
            return null;

        var user = new UserMaster
        {
            user_name = userName,
            email = userEmail,
            phone = userPhone,
            password_hash = BCrypt.Net.BCrypt.HashPassword(request.password),
            role = 1,
            isactive = true,
            create_date = DateTime.Now,
            create_by = 1,
            ip_address = ip
        };
        
        await _repository.RegisterUserAsync(user, ct);
        return user.user_id;
    }
    public async Task<AuthResponse?> LoginUserAsync(LoginRequest request, string ip, CancellationToken ct = default)
    {
        if (request == null)
            return null;

        if (string.IsNullOrWhiteSpace(request.phone) ||
            string.IsNullOrWhiteSpace(request.password))
            return null;

        var userPhone = request.phone.Trim();
        var foundUser = await _repository.FindUserAsync(x => x.phone == userPhone, ct);

        if (foundUser == null)
            return null;

        var verify = BCrypt.Net.BCrypt.Verify(request.password, foundUser.password_hash);


        var token = _jwtHelper.GenerateToken(foundUser);
        return new AuthResponse
        {
            AccessToken = token
        };
    }
}
