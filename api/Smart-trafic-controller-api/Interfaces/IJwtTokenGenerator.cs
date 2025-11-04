
namespace Smart_trafic_controller_api.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId);
    }
}