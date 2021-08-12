﻿using Moq;
using System;
using MuffiNet.Backend.Services;

namespace MuffiNet.Test.Shared.Mocks
{
    public static class CurrentDateTimeServiceMock
    {
        public static ICurrentDateTimeService MockCurrentDateTimeService()
        {
            var currentDateTimeMock = new Mock<ICurrentDateTimeService>();
            currentDateTimeMock.Setup(p => p.CurrentDateTime()).Returns(MockedDateTime);

            return currentDateTimeMock.Object;
        }

        public static DateTime MockedDateTime()
        {
            return new DateTime(2021, 06, 17, 12, 05, 10);
        }
    }
}