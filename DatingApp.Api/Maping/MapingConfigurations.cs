using DatingApp.Api.Contracts.Authentication;
using DatingApp.Api.Entities;
using Mapster;

namespace DatingApp.Api.Maping
{
    public class MapingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<ApplicationUser, RegisterResponse>()
            //    .Map(dest => dest.Username, src => src.UserName);
        }
    }
}
