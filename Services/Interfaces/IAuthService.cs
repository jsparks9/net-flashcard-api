﻿using Quiz_API.Models.DTOs;
using Quiz_API.Models.DTOs.Internal;

namespace Quiz_API.Services
{
  public interface IAuthService
  {
    Task<string> LoginAsync(LoginModel loginModel);
    void CreateUser(CreateUserModel createUserModel);
    UserInfoDto GetUserInfoFromAuthHeader(string authHeader);
  }
}
