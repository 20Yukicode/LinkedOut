﻿using LinkedOut.Common.Api;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Feign.User;
using LinkedOut.Common.Feign.User.Dto;
using LinkedOut.User.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.User.Client;

[Route("feign")]
[ApiController]
public class UserClient :  IUserFeignClient
{
    private readonly IUserService _userService;

    public UserClient(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("demo")]
    public async Task<MessageModel<string>> Demo(string a)
    {
        throw new ApiException(("error"));
        return MessageModel<string>.Success(a+"233");
    }

    [HttpGet("userInfo")]
    public async Task<MessageModel<UserDto>> GetUserInfo(int unifiedId)
    {
        var user = await _userService.GetUserOeEnterpriseInfo(unifiedId);

        var userDto = new UserDto
        {
            UnifiedId = user.UnifiedId,
            UserBriefInfo = user.BriefInfo,
            UserIconUrl = user.Avatar,
            UserName = user.UserName,
            UserType = user.UserType
        };
        return MessageModel<UserDto>.Success(userDto);
    }
}

