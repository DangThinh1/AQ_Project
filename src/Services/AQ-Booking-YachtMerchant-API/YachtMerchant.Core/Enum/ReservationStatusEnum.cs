using System;
using System.Collections.Generic;
using System.Text;

namespace YachtMerchant.Core.Enum
{
    public enum ReservationStatusEnum
    {
        WaitingPayment=1,
        Paid =2,
        Pending=3,
        Accepted=4,
        Rejected=5,
        Cancelled=6,
        Completed=7

    }
}
