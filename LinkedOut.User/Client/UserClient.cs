﻿using System.ComponentModel.DataAnnotations;
using LinkedOut.Common.Api;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Feign.User;
using LinkedOut.Common.Feign.User.Dto;
using LinkedOut.User.Manager;
using LinkedOut.User.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.User.Client;

[Route("feign")]
[ApiController]
public class UserClient :  IUserFeignClient
{
    private readonly IUserService _userService;

    private readonly SubscribedManager _subscribedManager;

    public UserClient(IUserService userService, SubscribedManager subscribedManager)
    {
        _userService = userService;
        _subscribedManager = subscribedManager;
    }

    [HttpGet("subscribe")]
    public async Task<MessageModel<List<UserDto>>> GetSubscribeUserId([Required] int unifiedId)
    {
        var subscribeUserIds = await _userService.GetSubscribeUserIds(unifiedId);
        return MessageModel<List<UserDto>>.Success(subscribeUserIds);
    }

    [HttpGet("demo")]
    public async Task<MessageModel<string>> Demo(string a)
    {
        // throw new ApiException(("error"));
        return MessageModel<string>.Success(a+"233");
    }

    [HttpGet("userInfo")]
    public async Task<MessageModel<UserDto>> GetUserInfo([Required]int unifiedId)
    {
        var userDto = await _userService.GetUserOrEnterpriseInfo(unifiedId);

        return MessageModel<UserDto>.Success(userDto);
    }
}

