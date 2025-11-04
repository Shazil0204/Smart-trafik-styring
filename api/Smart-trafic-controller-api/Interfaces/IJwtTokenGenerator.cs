namespace Smart_traffic_controller_api.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId);
    }
}
