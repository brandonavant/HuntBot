using HuntBot.Domain.Shared;
using System;
using Xunit;

namespace HuntBot.Tests.UnitTests.Domain
{
    public class VrtTests
    {
        [Fact]
        public void Vrt_InstantiatingVrtWithUtc_YieldsConvertedValue()
        {
            var utcNow = DateTime.UtcNow;
            var vrtNow = new Vrt(utcNow);

            Assert.Equal(utcNow.AddHours(-2), vrtNow.Value);
        }

        [Fact]
        public void Vrt_AssingingDateTimeToVrt_YieldsConvertedValue()
        {
            var utcNow = DateTime.UtcNow;
            Vrt vrtNow = utcNow;

            Assert.Equal(utcNow.AddHours(-2), vrtNow.Value);
        }

        [Fact]
        public void Vrt_AssigningVrtToDateTime_RetainsVrtValue()
        {
            var vrtNow = new Vrt(DateTime.UtcNow);
            DateTime utcNow = vrtNow;

            Assert.Equal(vrtNow.Value, utcNow);
        }
    }
}
