﻿using LinkedOut.DB;
using LinkedOut.DB.Entity;
using LinkedOut.User.Domain.Vo;

namespace LinkedOut.User.Manager;

public class EnterpriseInfoManager
{

    private readonly LinkedOutContext _context;

    public EnterpriseInfoManager(LinkedOutContext context)
    {
        _context = context;
    }

    public EnterpriseInfo? GetEnterpriseInfoById(int? unifiedId)
    {
        return _context.EnterpriseInfos
            .SingleOrDefault(o => o.UnifiedId == unifiedId);
    }

    public EnterpriseInfoVo<string> CombineEnterpriseAndInfo(int isSubscribed, 
        int fansNum,
        int followNum,
        DB.Entity.User userById, 
        EnterpriseInfo enterpriseInfo)
    {
        return new EnterpriseInfoVo<string>
        {
            IsSubscribed = isSubscribed,
            UnifiedId = userById.UnifiedId,
            Avatar = userById.Avatar,
            Back = userById.Background,
            BriefInfo = userById.BriefInfo,
            Email = userById.Email,
            TrueName = userById.TrueName,
            ContactWay = enterpriseInfo.ContactWay,
            Description = enterpriseInfo.Description,
            FansNum = fansNum,
            FollowNum = followNum
        };
    }

}