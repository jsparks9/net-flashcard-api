﻿using Quiz_API.Models.DTOs;
using Quiz_API.Models.DTOs.Internal;

namespace Quiz_API.Services
{
  public interface IAuthService
  {
    LoginResponseDto Login(LoginModel loginModel);
    LoginResponseDto CreateUser(CreateUserModel createUserModel);
    UserInfoDto GetUserInfoFromAuthHeader(string authHeader);
  }
}
