using Microsoft.AspNetCore.Http;

namespace VHM_APi_.Dtos.Chat
{
    public class LatestUserWithUnreadCountDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public int UnreadCount { get; set; } = 0;
        public string ImageUrl { get; set; }
    }
}
