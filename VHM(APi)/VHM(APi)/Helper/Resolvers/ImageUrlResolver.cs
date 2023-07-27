using AutoMapper;
using Microsoft.Extensions.Configuration;
using VHM.DAL.Entities;
using VHM_APi_.Dtos;

namespace VHM_APi_.Helper.Resolvers
{
    public class ImageUrlResolver : IValueResolver<ApplicationUser, ApplicationUserDto, string>
    {
        private readonly IConfiguration configuration;

        public ImageUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(ApplicationUser source, ApplicationUserDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
            {
                return $"{configuration["BaseUrl"]}{source.ImageUrl}";
            }
            return null;
        }
    }
}
