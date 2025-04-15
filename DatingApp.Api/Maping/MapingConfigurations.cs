using DatingApp.Api.Contracts.Authentication;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Entities;
using DatingApp.Api.Extensions;
using Mapster;

namespace DatingApp.Api.Maping
{
    public class MapingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ApplicationUser, UserResponse>()
                .Map(dest => dest.Age, src => src.DateOfBirth.CalculateAge())
                .Map(dest => dest.PhotoUrl, src => src.Photos.SingleOrDefault(p => p.IsMain)!.Url);
        }
    }
}
