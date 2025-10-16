// ---------------------------------------------------------------
// Copyright (c) PlausibleAlibi. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using NotificationService.Foundation.Brokers.DateTimes;
using NotificationService.Foundation.Models;

namespace NotificationService.Foundation.Services.Foundations
{
    public class NotificationService : INotificationService
    {
        private readonly IDateTimeBroker dateTimeBroker;

        public NotificationService(IDateTimeBroker dateTimeBroker)
        {
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<Notification> AddNotificationAsync(Notification notification) =>
        TryCatch(() =>
        {
            ValidateNotificationOnAdd(notification);
            notification.Id = Guid.NewGuid();
            notification.CreatedDate = this.dateTimeBroker.GetCurrentDateTimeOffset();
            notification.UpdatedDate = notification.CreatedDate;

            return ValueTask.FromResult(notification);
        });

        private void ValidateNotificationOnAdd(Notification notification)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (string.IsNullOrWhiteSpace(notification.Message))
            {
                throw new ArgumentException("Message is required.", nameof(notification));
            }
        }

        private ValueTask<Notification> TryCatch(Func<ValueTask<Notification>> returningNotificationFunction)
        {
            try
            {
                return returningNotificationFunction();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
