using HuntBot.Domain.HuntBotGames;
using HuntBot.Domain.HuntBotGames.GameObjects;
using HuntBot.Domain.HuntBotGames.Participants;
using HuntBot.Domain.SeedWork;
using Moq;
using System;
using Xunit;

namespace HuntBot.Tests.UnitTests.Domain
{
    public class HuntBotGameTests
    { 
        private readonly DateTime _defaultGameStartDate;
        private readonly DateTime _defaultGameEndDate;
        private readonly string _defaultGameTitle;
        private readonly int _defaultParticipantCitizenNumber;
        private readonly string _defaultParticipantCitizenName;
        private readonly int _defaultFoundGameObjectId;

        public HuntBotGameTests()
        {
            _defaultGameStartDate = DateTime.UtcNow.AddHours(1);
            _defaultGameEndDate = _defaultGameStartDate.AddDays(1);
            _defaultGameTitle = "DefaultGameTitle";
            _defaultParticipantCitizenNumber = 339566;
            _defaultParticipantCitizenName = "Droog";
            _defaultFoundGameObjectId = 1000;
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
            GameParticipant gameParticipant;
            HuntBotGame huntBotGame;

            var huntBotGameId = Guid.NewGuid();
            var gameUniquenessCheckerMock = new Mock<IGameUniquenessChecker>();
            var participantUniquenessCheckerMock = new Mock<IParticipantUniquenessChecker>();

            gameUniquenessCheckerMock.Setup(uc => uc.IsUnique(_defaultGameTitle)).Returns(true);
            participantUniquenessCheckerMock.Setup(uc => uc.IsUnique(huntBotGameId, _defaultParticipantCitizenNumber)).Returns(true);

            huntBotGame = HuntBotGame.CreateNewHuntBotGame(huntBotGameId, _defaultGameTitle, _defaultGameStartDate, _defaultGameEndDate, gameUniquenessCheckerMock.Object);
            huntBotGame.AddParticipant(_defaultParticipantCitizenNumber, _defaultParticipantCitizenName, _defaultFoundGameObjectId, 5, participantUniquenessCheckerMock.Object);
            gameParticipant = huntBotGame.Participants[0];

            Assert.Single(huntBotGame.Participants);

            Assert.Equal(_defaultParticipantCitizenNumber, gameParticipant.Id);
            Assert.Equal(_defaultParticipantCitizenName, gameParticipant.CitizenName);
            Assert.Equal(5, gameParticipant.GamePoints);
        }

        [Fact]
        public void HuntBotGame_AddParticipantWhoHasAlreadyBeenAdded_ThrowsBusinessRuleValidationException()
        {
            HuntBotGame huntBotGame;

            var huntBotGameId = Guid.NewGuid();
            var gameUniquenessCheckerMock = new Mock<IGameUniquenessChecker>();
            var participantUniquenessCheckerMock = new Mock<IParticipantUniquenessChecker>();

            gameUniquenessCheckerMock.Setup(uc => uc.IsUnique(_defaultGameTitle)).Returns(true);
            participantUniquenessCheckerMock.Setup(uc => uc.IsUnique(huntBotGameId, _defaultParticipantCitizenNumber)).Returns(false);

            huntBotGame = HuntBotGame.CreateNewHuntBotGame(huntBotGameId, _defaultGameTitle, _defaultGameStartDate, _defaultGameEndDate, gameUniquenessCheckerMock.Object);
            
            Assert.Throws<BusinessRuleValidationException>(() => huntBotGame.AddParticipant(
                _defaultParticipantCitizenNumber,
                _defaultParticipantCitizenName,
                _defaultFoundGameObjectId,
                5,
                participantUniquenessCheckerMock.Object
            ));
        }

        [Fact]
        public void HuntBotGame_ParticipantFoundSingleObject_AddsFindAndPointsProperly()
        {
            HuntBotGame huntBotGame;
            GameParticipant gameParticipant;
            GameObjectFind gameObjectFind;

            var pointsAwarded = 5000;
            var huntBotGameId = Guid.NewGuid();
            var gameUniquenessCheckerMock = new Mock<IGameUniquenessChecker>();
            var participantUniquenessCheckerMock = new Mock<IParticipantUniquenessChecker>();
            var pointsAwardedForRegistrationObjectFind = 5;

            gameUniquenessCheckerMock.Setup(uc => uc.IsUnique(_defaultGameTitle)).Returns(true);
            participantUniquenessCheckerMock.Setup(uc => uc.IsUnique(huntBotGameId, _defaultParticipantCitizenNumber)).Returns(true);

            huntBotGame = HuntBotGame.CreateNewHuntBotGame(huntBotGameId, _defaultGameTitle, _defaultGameStartDate, _defaultGameEndDate, gameUniquenessCheckerMock.Object);
            huntBotGame.AddParticipant(_defaultParticipantCitizenNumber, _defaultParticipantCitizenName, _defaultFoundGameObjectId, pointsAwardedForRegistrationObjectFind, participantUniquenessCheckerMock.Object);
            gameParticipant = huntBotGame.Participants[0];

            gameParticipant.ParticipantFoundObject(_defaultFoundGameObjectId, pointsAwarded);
            gameObjectFind = gameParticipant.ObjectFinds[1]; // 0 is the object found upon participant creation.

            Assert.Equal(2, gameParticipant.ObjectFinds.Count);
            Assert.NotNull(gameObjectFind);
            Assert.Equal(_defaultFoundGameObjectId, gameObjectFind.ObjectId);
            Assert.Equal(pointsAwarded, gameObjectFind.Points);
            Assert.Equal(DateTime.UtcNow.ToString("s"), gameObjectFind.FoundDate.ToString("s"));
            Assert.Equal(gameParticipant.GamePoints, pointsAwarded + pointsAwardedForRegistrationObjectFind);
        }
    }
}
