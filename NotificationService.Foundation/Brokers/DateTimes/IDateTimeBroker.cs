// ---------------------------------------------------------------
// Copyright (c) PlausibleAlibi. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace NotificationService.Foundation.Brokers.DateTimes
{
    public interface IDateTimeBroker
    {
        DateTimeOffset GetCurrentDateTimeOffset();
    }
}
