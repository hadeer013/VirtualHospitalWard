namespace VHM_APi_.Dtos
{
    public class UserDto:BaseUserDto
    {
        public int StatusCode { get; } = 200;
        public string Token { get; set; }
    }
}
