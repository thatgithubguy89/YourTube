﻿using YourTube.Api.Models;
using YourTube.Api.Models.Requests;

namespace YourTube.Api.Interfaces.Repositories
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task AddVideoAsync(AddVideoRequest addVideoRequest);
        Task<List<Video>> GetAllVideosAsync(string? searchPhrase);
        Task<List<Video>> GetFavoriteVideosByUsernameAsync(string username);
    }
}
