// ---------------------------------------------------------------
// Copyright (c) PlausibleAlibi. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace NotificationService.Foundation.Exceptions
{
    public class NotificationServiceException : Exception
    {
        public NotificationServiceException(Exception innerException)
            : base(message: "Notification service error occurred, contact support.",
                  innerException)
        { }
    }
}
