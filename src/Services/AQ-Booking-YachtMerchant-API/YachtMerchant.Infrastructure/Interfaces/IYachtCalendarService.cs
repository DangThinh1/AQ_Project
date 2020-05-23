using APIHelpers.Response;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using YachtMerchant.Core.Models.Calendar;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtCalendarService
    {
        BaseResponse<List<YachtCalendar>> Calendar(int merchantId, DateTime firstDay, DateTime lastDay);
    }
}
