// ---------------------------------------------------------------
// Copyright (c) PlausibleAlibi. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace NotificationService.Foundation.Exceptions
{
    public class NotificationValidationException : Exception
    {
        public NotificationValidationException(Exception innerException)
            : base(message: "Notification validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
