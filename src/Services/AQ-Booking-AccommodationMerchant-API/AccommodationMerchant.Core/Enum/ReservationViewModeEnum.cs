using System;
using System.Collections.Generic;
using System.Text;

namespace AccommodationMerchant.Core.Enum
{
    public enum ReservationViewModeEnum
    {
        ViewAllStatusReservation = 0,
        ViewReservationWaitingPayment = 1,
        ViewReservationPaid = 2,
        ViewReservationPending = 3,
        ViewReservationAccepted = 4,
        ViewReservationRejected = 5,
        ViewReservationCancelled = 6,
        ViewReservationCompleted = 7
    }
}
