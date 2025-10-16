// ---------------------------------------------------------------
// Copyright (c) PlausibleAlibi. All rights reserved.
// ---------------------------------------------------------------

using System;

namespace NotificationService.Foundation.Exceptions
{
    public class NotificationDependencyException : Exception
    {
        public NotificationDependencyException(Exception innerException)
            : base(message: "Notification dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
