namespace StudentSkillManagement.Presentation.Contracts.Authentication
{
    public sealed record AuthenticationResponse(
        Guid UserId,
        string Email,
        string AccessToken
    );
}
