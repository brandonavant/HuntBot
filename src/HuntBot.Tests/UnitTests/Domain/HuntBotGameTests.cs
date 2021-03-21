using HuntBot.Domain.HuntBotGames;
using HuntBot.Domain.HuntBotGames.Participants;
using HuntBot.Domain.HuntBotGames.Rules;
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
        private readonly DateTime _defaultGameStartDate;

        /// <summary>
        /// Default end date, which can be used when creating a new <see cref="HuntBotGame"/>, if no uniqueness is needed for the <see cref="DateTime"/> value.
        /// </summary>
        private readonly DateTime _defaultGameEndDate;

        /// <summary>
        /// Default game title, which can be used when creating a <see cref="HuntBotGame"/> instance if no uniqueness is needed for the title.
        /// </summary>
        private readonly string _defaultGameTitle;

        /// <summary>
        /// Initializes default values for <see cref="HuntBotGame"/> unit tests.
        /// </summary>
        public HuntBotGameTests()
        {
            _defaultGameStartDate = DateTime.UtcNow.AddHours(1);
            _defaultGameEndDate = _defaultGameStartDate.AddDays(1);
            _defaultGameTitle = "DefaultGameTitle";
        }

        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithValidParams_CreatesInstanceWithCorrectValues()
        {
            HuntBotGame huntBotGame;

            var id = Guid.NewGuid();
            var gameUniquenessCheckerMock = new Mock<IGameUniquenessChecker>();

            gameUniquenessCheckerMock.Setup(uc => uc.IsUnique(_defaultGameTitle)).Returns(true);
            huntBotGame = HuntBotGame.CreateNewHuntBotGame(id, _defaultGameTitle, _defaultGameStartDate, _defaultGameEndDate, gameUniquenessCheckerMock.Object);

            Assert.NotNull(huntBotGame);
            Assert.Equal(id, huntBotGame.Id);
            Assert.Equal(_defaultGameTitle, huntBotGame.Title);
            Assert.Equal(_defaultGameStartDate, huntBotGame.StartDate);
            Assert.Equal(_defaultGameEndDate, huntBotGame.EndDate);
        }

        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithNonUniqueTitle_ThrowsBusinessRuleValidationException()
        {
            var id = Guid.NewGuid();
            var gameUniquenessCheckerMock = new Mock<IGameUniquenessChecker>();

            gameUniquenessCheckerMock.Setup(uc => uc.IsUnique(_defaultGameTitle)).Returns(false);

            Assert.Throws<BusinessRuleValidationException>(() => HuntBotGame.CreateNewHuntBotGame(
                id,
                _defaultGameTitle,
                _defaultGameStartDate,
                _defaultGameEndDate,
                gameUniquenessCheckerMock.Object
            ));
        }

        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithInvalidTitleLength_ThrowsBusinessRuleValidationException()
        {
            var id = Guid.NewGuid();
            var title = "ThisTitleIsSeveralCharactersTooLong";
            var gameUniquenessCheckerMock = new Mock<IGameUniquenessChecker>();

            Assert.Throws<BusinessRuleValidationException>(() => HuntBotGame.CreateNewHuntBotGame(
              id,
              title,
              _defaultGameStartDate,
              _defaultGameEndDate,
              gameUniquenessCheckerMock.Object
            ));
        }

        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithStartDateInThePast_ThrowsBusinessRuleValidationException()
        {
            var id = Guid.NewGuid();
            var yesterday = DateTime.UtcNow.AddDays(-1);
            var gameUniquenessCheckerMock = new Mock<IGameUniquenessChecker>();

            Assert.Throws<BusinessRuleValidationException>(() => HuntBotGame.CreateNewHuntBotGame(
                id,
                _defaultGameTitle,
                yesterday,
                _defaultGameEndDate,
                gameUniquenessCheckerMock.Object
            ));
        }

        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithEndDateMatchingStartDate_ThrowsBusinessRuleValidationException()
        {
            var id = Guid.NewGuid();
            var gameUniquenessCheckerMock = new Mock<IGameUniquenessChecker>();

            Assert.Throws<BusinessRuleValidationException>(() => HuntBotGame.CreateNewHuntBotGame(
                id,
                _defaultGameTitle,
                _defaultGameStartDate,
                _defaultGameStartDate,
                gameUniquenessCheckerMock.Object
            ));
        }

        [Fact]
        public void HuntBotGame_AddParticipantWithValidParams_CreatesInstanceWithCorrectValues()
        {
            HuntBotGameParticipant huntBotGameParticipant;
            HuntBotGameParticipant huntBotGameParticipantInList;
            HuntBotGame huntBotGame;

            var huntBotGameId = Guid.NewGuid();
            var gameUniquenessCheckerMock = new Mock<IGameUniquenessChecker>();
            var citizenNumber = 339566;
            var citizenName = "Droog";
            var participantUniquenessCheckerMock = new Mock<IParticipantUniquenessChecker>();

            gameUniquenessCheckerMock.Setup(uc => uc.IsUnique(_defaultGameTitle)).Returns(true);
            participantUniquenessCheckerMock.Setup(uc => uc.IsUnique(huntBotGameId, citizenNumber)).Returns(true);

            huntBotGame = HuntBotGame.CreateNewHuntBotGame(huntBotGameId, _defaultGameTitle, _defaultGameStartDate, _defaultGameEndDate, gameUniquenessCheckerMock.Object);
            huntBotGameParticipant = huntBotGame.AddParticipant(citizenNumber, citizenName, participantUniquenessCheckerMock.Object);
            huntBotGameParticipantInList = huntBotGame.Participants[0];

            Assert.Single(huntBotGame.Participants);
            Assert.Equal(huntBotGameParticipant, huntBotGameParticipantInList);
            Assert.Equal(citizenNumber, huntBotGameParticipantInList.Id);
            Assert.Equal(citizenName, huntBotGameParticipantInList.CitizenName);
        }

        //public void HuntBotGame_AddParticipant
    }
}
