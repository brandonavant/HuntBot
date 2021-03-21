using HuntBot.Domain.HuntBotGames;
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
        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithValidParams_CreatesInstanceWithCorrectValues()
        {
            HuntBotGame huntBotGame;

            var id = Guid.NewGuid();
            var title = "MyGameTitle";
            var startDate = DateTime.UtcNow.AddHours(1);
            var endDate = startDate.AddDays(1);
            var gameUniquenessChecker = new Mock<IGameUniquenessChecker>();

            gameUniquenessChecker.Setup(uc => uc.IsUnique(title)).Returns(true);
            huntBotGame = HuntBotGame.CreateNewHuntBotGame(id, title, startDate, endDate, gameUniquenessChecker.Object);

            Assert.NotNull(huntBotGame);
            Assert.Equal(id, huntBotGame.Id);
            Assert.Equal(title, huntBotGame.Title);
            Assert.Equal(startDate, huntBotGame.StartDate);
            Assert.Equal(endDate, huntBotGame.EndDate);
        }
    }
}
