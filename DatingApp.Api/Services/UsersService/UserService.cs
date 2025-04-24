using DatingApp.Api.Abstractions;
using DatingApp.Api.Contracts.Common;
using DatingApp.Api.Contracts.Users;
using DatingApp.Api.Entities;
using DatingApp.Api.Errors;
using DatingApp.Api.Interfaces;
using DatingApp.Api.Services.PhotosService;
using Mapster;

namespace DatingApp.Api.Services.UsersService
{
    public class UserService(IUserRepository userRepository,IPhotoService photoService) 
        : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPhotoService _photoService = photoService;

        public async Task<PaginatedList<UserResponse>> GetAllAsync(RequestFilters requestFilters, CancellationToken cancellationToken = default)
        {
            var users = _userRepository.GetAll();

            users = users
                    .Where(u => u.UserName != requestFilters.CurrentUsername);

            if (!string.IsNullOrEmpty(requestFilters.Gender))
            {
                users = users
                .Where(u =>u.Gender.ToLower() == requestFilters.Gender.ToLower());
            }

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-requestFilters.MaxAge - 1 ));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-requestFilters.MinAge));

            users = users.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            //users = requestFilters.OrderBy switch
            //{
            //    "created" => users.OrderByDescending(x => x.Created),
            //    _ => users.OrderByDescending(x => x.LastActive)
            //};
            if (requestFilters.OrderBy == "created")
                users = users.OrderByDescending(x => x.Created);
            else
                users = users.OrderByDescending(x => x.LastActive);


            var usersResponse = users.ProjectToType<UserResponse>();

            var response = await PaginatedList<UserResponse>.CreateAsync(usersResponse,
                requestFilters.PageNumber, requestFilters.PageSize, cancellationToken);

            return response;
        } 

        public async Task<Result<UserResponse>> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            var userResponse = user.Adapt<UserResponse>();

            return Result.Success(userResponse);
        }

        public async Task<Result<UserResponse>> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            var userResponse = user.Adapt<UserResponse>();

            return Result.Success(userResponse);
        }

        public async Task<Result> UpdateUserByUsernameAsync(string username, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            request.Adapt(user);

            _userRepository.Update(user);
            var updated = await _userRepository.SaveAllAsync();
            if(updated == 0)
                return Result.Failure<UserResponse>(UserErrors.FailedToSaveChanges);

            return Result.Success();
        }

        public async Task<Result<PhotoResponse>> AddPhotoToUserAsync(string username, IFormFile file)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user is null)
                return Result.Failure<PhotoResponse>(UserErrors.UserNotFound);

            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error is not null)
                return Result.Failure<PhotoResponse>(new Error("Cant Add Photo"
                        ,result.Error.Message,StatusCodes.Status400BadRequest));

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            if(user.Photos.Count == 0)
                photo.IsMain = true;

            user.Photos.Add(photo);

            var updated = await _userRepository.SaveAllAsync();
            if (updated == 0)
                return Result.Failure<PhotoResponse>(UserErrors.FailedToSaveChanges);

            return Result.Success(photo.Adapt<PhotoResponse>());
        }

        public async Task<Result> SetMainPhotoToUserAsync(string username, int photoId)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user is null)
                return Result.Failure<PhotoResponse>(UserErrors.UserNotFound);

            var photo = user.Photos
                .SingleOrDefault(p => p.Id == photoId);
            if (photo is null)
                return Result.Failure<PhotoResponse>(UserErrors.PhotoIsNotFound);

            if (photo.IsMain)
                return Result.Failure<PhotoResponse>(UserErrors.PhotoAlreadyMain);

            var oldMainPhoto = user.Photos
                .SingleOrDefault(p => p.IsMain);
            if (oldMainPhoto is null)
                return Result.Failure<PhotoResponse>(UserErrors.MainPhotoNotFound);

            oldMainPhoto.IsMain = false;
            photo.IsMain = true;

            var updated = await _userRepository.SaveAllAsync();
            if (updated == 0)
                return Result.Failure<PhotoResponse>(UserErrors.FailedToSaveChanges);

            return Result.Success();
        }

        public async Task<Result> DeletePhotoFromUserAsync(string username, int photoId)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user is null)
                return Result.Failure<PhotoResponse>(UserErrors.UserNotFound);

            var photo = user.Photos
                .SingleOrDefault(p => p.Id == photoId);
            if (photo is null)
                return Result.Failure<PhotoResponse>(UserErrors.PhotoIsNotFound);

            if (photo.IsMain)
                return Result.Failure<PhotoResponse>(UserErrors.CanootDeleteMainPhoto);

            if(photo.PublicId is not null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error is not null)
                    return Result.Failure(new Error("Canoot Delete Photo"
                        , result.Error.Message, StatusCodes.Status400BadRequest));
            }

            user.Photos.Remove(photo);

            var updated = await _userRepository.SaveAllAsync();
            if (updated == 0)
                return Result.Failure(UserErrors.FailedToSaveChanges);

            return Result.Success();
        }
    }
}
