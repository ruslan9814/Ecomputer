using Infrasctructure.BlobStorage;
using Infrasctructure.Repositories.Interfaces;
using Infrasctructure.UnitOfWork;
using Infrasctructure.CurrentUser;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands;

public sealed record UploadUserImageCommand(IFormFile ImageFile) : IRequest<Result<string>>;

internal sealed class UploadUserImageCommandHandler(
    IBlobService blobStorageService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService) : IRequestHandler<UploadUserImageCommand, Result<string>>
{
    private readonly IBlobService _blobStorageService = blobStorageService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<Result<string>> Handle(UploadUserImageCommand request, 
        CancellationToken cancellationToken)
    {
 
        if (!_currentUserService.IsAuthenticated)
            return Result.Failure<string>("Пользователь не авторизован.");
 
        int userId;
        try
        {
            userId = _currentUserService.UserId;  
        }
        catch (UnauthorizedAccessException)
        {
            return Result.Failure<string>("Не удалось определить идентификатор пользователя.");
        }
 
        if (request.ImageFile == null || request.ImageFile.Length == 0)
            return Result.Failure<string>("Файл не предоставлен.");

 
        var imageUrl = await _blobStorageService.UploadFileAsync(request.ImageFile);
        if (string.IsNullOrEmpty(imageUrl))
            return Result.Failure<string>("Не удалось загрузить изображение.");

 
        var user = await _userRepository.GetAsync(userId, includeRelated: false);
        if (user == null)
            return Result.Failure<string>("Пользователь не найден.");
 
        await _userRepository.UpdateImageUrlAsync(userId, imageUrl, cancellationToken);
        await _unitOfWork.Commit();

        return Result.Success(imageUrl);
    }
}