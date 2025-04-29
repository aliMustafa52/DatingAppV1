using DatingApp.Api.Contracts.Authentication;
using DatingApp.Api.Contracts.Messages;
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
                .Map(dest => dest.PhotoUrl, src => src.Photos.SingleOrDefault(p => p.IsMain)!.Url,
                    srcCond => srcCond.Photos.SingleOrDefault(p => p.IsMain) != null);


            config.NewConfig<Message, MessageResponse>()
                .Map(dest => dest.SenderPhotoUrl, src => src.Sender
                            .Photos.SingleOrDefault(p => p.IsMain)!.Url)
                .Map(dest => dest.RecipientPhotoUrl, src => src.Recipient
                            .Photos.SingleOrDefault(p => p.IsMain)!.Url);
        }
    }
}
