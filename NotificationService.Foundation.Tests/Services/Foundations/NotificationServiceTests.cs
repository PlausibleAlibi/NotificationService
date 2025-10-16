// ---------------------------------------------------------------
// Copyright (c) PlausibleAlibi. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using NotificationService.Foundation.Brokers.DateTimes;
using NotificationService.Foundation.Models;
using Xunit;

namespace NotificationService.Foundation.Services.Foundations.Tests
{
    public class NotificationServiceTests
    {
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly INotificationService notificationService;

        public NotificationServiceTests()
        {
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.notificationService = new NotificationService(this.dateTimeBrokerMock.Object);
        }

        [Fact]
        public async Task ShouldAddNotificationAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            DateTimeOffset dateTimeOffset = randomDate;
            Notification randomNotification = CreateRandomNotification();
            Notification inputNotification = randomNotification;
            inputNotification.Id = default;
            inputNotification.CreatedDate = default;
            inputNotification.UpdatedDate = default;

            Notification expectedNotification = inputNotification.DeepClone();
            expectedNotification.CreatedDate = dateTimeOffset;
            expectedNotification.UpdatedDate = dateTimeOffset;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(dateTimeOffset);

            // when
            Notification actualNotification =
                await this.notificationService.AddNotificationAsync(inputNotification);

            // then
            Assert.NotEqual(Guid.Empty, actualNotification.Id);
            Assert.Equal(expectedNotification.Message, actualNotification.Message);
            Assert.Equal(expectedNotification.Title, actualNotification.Title);
            Assert.Equal(expectedNotification.CreatedDate, actualNotification.CreatedDate);
            Assert.Equal(expectedNotification.UpdatedDate, actualNotification.UpdatedDate);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionOnAddWhenNotificationIsNull()
        {
            // given
            Notification nullNotification = null;

            // when & then
            Assert.Throws<ArgumentNullException>(() =>
                this.notificationService.AddNotificationAsync(nullNotification));

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowArgumentExceptionOnAddWhenMessageIsInvalid(string invalidMessage)
        {
            // given
            Notification invalidNotification = CreateRandomNotification();
            invalidNotification.Message = invalidMessage;

            // when & then
            Assert.Throws<ArgumentException>(() =>
                this.notificationService.AddNotificationAsync(invalidNotification));

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeOffset(DateTime.UtcNow);

        private static Notification CreateRandomNotification() =>
            new Notification
            {
                Id = Guid.NewGuid(),
                Message = Guid.NewGuid().ToString(),
                Title = Guid.NewGuid().ToString(),
                CreatedDate = GetRandomDateTimeOffset(),
                UpdatedDate = GetRandomDateTimeOffset()
            };
    }

    public static class NotificationExtensions
    {
        public static Notification DeepClone(this Notification notification) =>
            new Notification
            {
                Id = notification.Id,
                Message = notification.Message,
                Title = notification.Title,
                CreatedDate = notification.CreatedDate,
                UpdatedDate = notification.UpdatedDate
            };
    }
}
