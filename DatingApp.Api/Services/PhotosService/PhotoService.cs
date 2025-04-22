using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Api.Helpers;
using Microsoft.Extensions.Options;

namespace DatingApp.Api.Services.PhotosService
{
    public class PhotoService : IPhotoService
    {
        private readonly CloudinarySettings _cloudinarySettings;
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            _cloudinarySettings = cloudinarySettings.Value;

            var acc = new Account(_cloudinarySettings.CloudName, _cloudinarySettings.ApiKey
            , _cloudinarySettings.ApiSecret);


            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500)
                        .Width(500)
                        .Crop("fill")
                        .Gravity("face"),
                    Folder = "DateAppV1"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
