using HuntBot.Domain.HuntBotGames;
using HuntBot.Domain.SeedWork;
using Moq;
using System;
using Xunit;

namespace HuntBot.Tests.UnitTests.Domain
{
    /// <summary>
    /// Unit tests related to <see cref="HuntBotGame"/>.
    /// </summary>
    public class HuntBotGameTests
    {
        /// <summary>
        /// Default start date, which can be used when creating a new <see cref="HuntBotGame"/>, if no uniqueness is needed for the <see cref="DateTime"/> value.
        /// </summary>
        private readonly DateTime _defaultStartDate;

        /// <summary>
        /// Default end date, which can be used when creating a new <see cref="HuntBotGame"/>, if no uniqueness is needed for the <see cref="DateTime"/> value.
        /// </summary>
        private readonly DateTime _defaultEndDate;

        /// <summary>
        /// Initializes default values for <see cref="HuntBotGame"/> unit tests.
        /// </summary>
        public HuntBotGameTests()
        {
            _defaultStartDate = DateTime.UtcNow.AddHours(1);
            _defaultEndDate = _defaultStartDate.AddDays(1);
        }

        /// <summary>
        /// Test that ensures that when invoking <see cref="HuntBotGame.CreateNewHuntBotGame"/> with valid values, the new instance's properties match what is expected.
        /// </summary>
        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithValidParams_CreatesInstanceWithCorrectValues()
        {
            HuntBotGame huntBotGame;

            var id = Guid.NewGuid();
            var title = "MyGameTitle";
            var gameUniquenessCheckerMock = new Mock<IGameUniquenessChecker>();

            gameUniquenessCheckerMock.Setup(uc => uc.IsUnique(title)).Returns(true);
            huntBotGame = HuntBotGame.CreateNewHuntBotGame(id, title, _defaultStartDate, _defaultEndDate, gameUniquenessCheckerMock.Object);

            Assert.NotNull(huntBotGame);
            Assert.Equal(id, huntBotGame.Id);
            Assert.Equal(title, huntBotGame.Title);
            Assert.Equal(_defaultStartDate, huntBotGame.StartDate);
            Assert.Equal(_defaultEndDate, huntBotGame.EndDate);
        }

        /// <summary>
        /// Test that ensures that when invoking <see cref="HuntBotGame.AddParticipant"/> with an invalid title length, <see cref="BusinessRuleValidationException"/> is thrown.
        /// </summary>
        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithInvalidTitleLength_ThrowsBusinessRuleValidationException()
        {
            var id = Guid.NewGuid();
            var title = "ThisTitleIsSeveralCharactersTooLong";
            var gameUniquenessCheckerMock = new Mock<IGameUniquenessChecker>();

            Assert.Throws<BusinessRuleValidationException>(() => HuntBotGame.CreateNewHuntBotGame(
              id,
              title,
              _defaultStartDate,
              _defaultEndDate,
              gameUniquenessCheckerMock.Object
            ));
        }

        //public void HuntBotGame_AddParticipant
    }
}
