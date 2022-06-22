﻿using LinkedOut.Common.Domain;
using LinkedOut.Common.Domain.Enum;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Helper;
using LinkedOut.DB;
using LinkedOut.User.Domain.Vo;
using LinkedOut.User.Manager;
using Microsoft.EntityFrameworkCore;

namespace LinkedOut.User.Service.Impl;

public class UserInfoService : IUserInfoService
{
    private readonly UserInfoManager _userInfoManager;

    private readonly UserManager _userManager;

    private readonly SubscribedManager _subscribedManager;

    private readonly LinkedOutContext _context;

    public UserInfoService(UserInfoManager userInfoManager, UserManager userManager, LinkedOutContext context,
        SubscribedManager subscribedManager)
    {
        _userInfoManager = userInfoManager;
        _userManager = userManager;
        _context = context;
        _subscribedManager = subscribedManager;
    }

    public async Task<UserInfoVo<string>> GetUserInfo(int firstUserId, int secondUserId)
    {
        var userInfoById = _userInfoManager.GetUserInfoById(secondUserId);
        var userById = _userManager.GetUserById(secondUserId);

        if (userInfoById == null || userById == null)
        {
            throw new ApiException($"{firstUserId}或{secondUserId}为ID的某个用户不存在");
        }

        var fansNum = await _context.Subscribeds.CountAsync(o => o.SecondUserId == secondUserId);

        var followNum = await _context.Subscribeds.CountAsync(o => o.FirstUserId == secondUserId);

        var (state, _) = _subscribedManager.GetRelation(firstUserId, secondUserId);
        return _userInfoManager.CombineUserAndUserInfo((int) state, fansNum,
            followNum, userById, userInfoById);
    }

    public async Task UpdateUserInfo(UserInfoVo<IFormFile> userVo)
    {
        var unifiedId = (int) userVo.UnifiedId;

        var userById = _userManager.GetUserById(unifiedId);
        var userInfoById = _userInfoManager.GetUserInfoById(unifiedId);
        if (userById == null || userInfoById == null)
        {
            throw new ApiException($"id为{unifiedId}用户不存在");
        }

        var password = userVo.Password;
        if (!string.IsNullOrWhiteSpace(password))
        {
            userById.Password = password;
        }

        var idCard = userVo.IdCard;
        if (!string.IsNullOrWhiteSpace(idCard))
        {
            userInfoById.IdCard = idCard;
        }

        var email = userVo.Email;
        if (!string.IsNullOrWhiteSpace(email))
        {
            userById.Email = email;
        }

        var trueName = userVo.TrueName;
        if (!string.IsNullOrWhiteSpace(trueName))
        {
            userById.TrueName = trueName;
        }

        //下面这些属性都可以为空串
        var age = userVo.Age;
        if (age != null)
        {
            userInfoById.Age = age;
        }

        var gender = userVo.Gender;
        if (gender!=null)
        {
            userInfoById.Gender = gender;
        }

        var briefInfo = userVo.BriefInfo;
        if (briefInfo!=null)
        {
            userById.BriefInfo = briefInfo;
        }

        var livePlace = userVo.LivePlace;
        if (livePlace!=null)
        {
            userInfoById.LivePlace = livePlace;
        }

        var phoneNum = userVo.PhoneNum;
        if (phoneNum!=null)
        {
            userInfoById.PhoneNum = phoneNum;
        }

        var prePosition = userVo.PrePosition;
        if (prePosition!=null)
        {
            userInfoById.PrePosition = prePosition;
        }

        var avatar = Task.Run(() =>
        {
            var avatar = userVo.Avatar;
            if (avatar == null) return;
            var fileElement = new FileElement
            {
                File = avatar,
                BucketType = BucketType.Avatar,
                AssociateId = unifiedId
            };
            var url = OssHelper.UploadFile(fileElement);
            userById.Avatar = url;
        });


        var back = Task.Run(() =>
        {
            var background = userVo.Back;
            if (background == null) return;
            var fileElement = new FileElement
            {
                File = background,
                BucketType = BucketType.Back,
                AssociateId = unifiedId
            };
            var url = OssHelper.UploadFile(fileElement);
            userById.Background = url;
        });

        await Task.WhenAll(avatar, back);
        await _context.SaveChangesAsync();

    }
}