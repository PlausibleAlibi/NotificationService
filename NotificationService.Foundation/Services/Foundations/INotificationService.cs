// ---------------------------------------------------------------
// Copyright (c) PlausibleAlibi. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NotificationService.Foundation.Models;

namespace NotificationService.Foundation.Services.Foundations
{
    public interface INotificationService
    {
        ValueTask<Notification> AddNotificationAsync(Notification notification);
    }
}
