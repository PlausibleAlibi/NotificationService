// ---------------------------------------------------------------
// Copyright (c) PlausibleAlibi. All rights reserved.
// ---------------------------------------------------------------

namespace NotificationService.Foundation.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
