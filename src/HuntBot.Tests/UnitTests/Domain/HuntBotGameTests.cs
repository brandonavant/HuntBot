using HuntBot.Domain.HuntBotGames;
using HuntBot.Domain.HuntBotGames.GameObjects;
using HuntBot.Domain.HuntBotGames.Participants;
using HuntBot.Domain.SeedWork;
using System;
using System.Collections.Generic;
using Xunit;

namespace HuntBot.Tests.UnitTests.Domain
{
    public class HuntBotGameTests
    {
        private readonly DateTime _defaultGameStartDate;
        private readonly DateTime _defaultGameEndDate;

        public HuntBotGameTests()
        {
            _defaultGameStartDate = DateTime.UtcNow.AddHours(1);
            _defaultGameEndDate = _defaultGameStartDate.AddDays(1);
        }

        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithValidParams_CreatesInstanceWithCorrectValues()
        {
            HuntBotGame huntBotGame;

            var id = Guid.NewGuid();
            huntBotGame = HuntBotGame.CreateNewHuntBotGame(id, HuntBotGameTestData.GameTitle, _defaultGameStartDate, _defaultGameEndDate, new List<string>());

            Assert.NotNull(huntBotGame);
            Assert.Equal(id, huntBotGame.Id);
            Assert.Equal(HuntBotGameTestData.GameTitle, huntBotGame.Title);
            Assert.Equal(_defaultGameStartDate, huntBotGame.StartDate);
            Assert.Equal(_defaultGameEndDate, huntBotGame.EndDate);
        }

        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithNonUniqueTitle_ThrowsBusinessRuleValidationException()
        {
            var id = Guid.NewGuid();

            Assert.Throws<BusinessRuleValidationException>(() => HuntBotGame.CreateNewHuntBotGame(
                id,
                HuntBotGameTestData.GameTitle,
                _defaultGameStartDate,
                _defaultGameEndDate,
                new List<string> { HuntBotGameTestData.GameTitle }
            ));
        }

        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithInvalidTitleLength_ThrowsBusinessRuleValidationException()
        {
            var id = Guid.NewGuid();
            var title = "ThisTitleIsSeveralCharactersTooLong";

            Assert.Throws<BusinessRuleValidationException>(() => HuntBotGame.CreateNewHuntBotGame(
              id,
              title,
              _defaultGameStartDate,
              _defaultGameEndDate,
              new List<string>()
            ));
        }

        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithStartDateInThePast_ThrowsBusinessRuleValidationException()
        {
            var id = Guid.NewGuid();
            var yesterday = DateTime.UtcNow.AddDays(-1);

            Assert.Throws<BusinessRuleValidationException>(() => HuntBotGame.CreateNewHuntBotGame(
                id,
                HuntBotGameTestData.GameTitle,
                yesterday,
                _defaultGameEndDate,
                new List<string>()
            ));
        }

        [Fact]
        public void HuntBotGame_CreateNewHuntBotGameWithEndDateMatchingStartDate_ThrowsBusinessRuleValidationException()
        {
            var id = Guid.NewGuid();

            Assert.Throws<BusinessRuleValidationException>(() => HuntBotGame.CreateNewHuntBotGame(
                id,
                HuntBotGameTestData.GameTitle,
                _defaultGameStartDate,
                _defaultGameStartDate,
                new List<string>()
            ));
        }

        [Fact]
        public void HuntBotGame_AddParticipantWithValidParams_CreatesInstanceWithCorrectValues()
        {
            GameParticipant gameParticipant;
            HuntBotGame huntBotGame;

            var huntBotGameId = Guid.NewGuid();

            huntBotGame = HuntBotGame.CreateNewHuntBotGame(huntBotGameId, HuntBotGameTestData.GameTitle, _defaultGameStartDate, _defaultGameEndDate, new List<string>());
            huntBotGame.AddParticipant(HuntBotGameTestData.ParticipantCitizenNumber, HuntBotGameTestData.ParticipantCitizenName, HuntBotGameTestData.FoundGameObjectId, 5);
            gameParticipant = huntBotGame.GameParticipants[0];

            Assert.Single(huntBotGame.GameParticipants);

            Assert.Equal(HuntBotGameTestData.ParticipantCitizenNumber, gameParticipant.Id);
            Assert.Equal(HuntBotGameTestData.ParticipantCitizenName, gameParticipant.CitizenName);
            Assert.Equal(5, gameParticipant.GamePoints);
        }

        [Fact]
        public void HuntBotGame_AddParticipantWhoHasAlreadyBeenAdded_ThrowsBusinessRuleValidationException()
        {
            HuntBotGame huntBotGame;

            var huntBotGameId = Guid.NewGuid();            
            huntBotGame = HuntBotGame.CreateNewHuntBotGame(huntBotGameId, HuntBotGameTestData.GameTitle, _defaultGameStartDate, _defaultGameEndDate, new List<string>());
            huntBotGame.AddParticipant(HuntBotGameTestData.ParticipantCitizenNumber, HuntBotGameTestData.ParticipantCitizenName, HuntBotGameTestData.FoundGameObjectId, 5);
            
            Assert.Throws<BusinessRuleValidationException>(() => huntBotGame.AddParticipant
            (
                HuntBotGameTestData.ParticipantCitizenNumber,
                HuntBotGameTestData.ParticipantCitizenName,
                HuntBotGameTestData.FoundGameObjectId,
                5
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
            var pointsAwardedForRegistrationObjectFind = 5;

            huntBotGame = HuntBotGame.CreateNewHuntBotGame(huntBotGameId, HuntBotGameTestData.GameTitle, _defaultGameStartDate, _defaultGameEndDate, new List<string>());
            huntBotGame.AddParticipant(HuntBotGameTestData.ParticipantCitizenNumber, HuntBotGameTestData.ParticipantCitizenName, HuntBotGameTestData.FoundGameObjectId, pointsAwardedForRegistrationObjectFind);
            gameParticipant = huntBotGame.GameParticipants[0];

            gameParticipant.ParticipantFoundObject(HuntBotGameTestData.FoundGameObjectId, pointsAwarded);
            gameObjectFind = gameParticipant.ObjectFinds[1]; // 0 is the object found upon participant creation.

            Assert.Equal(2, gameParticipant.ObjectFinds.Count);
            Assert.NotNull(gameObjectFind);
            Assert.Equal(HuntBotGameTestData.FoundGameObjectId, gameObjectFind.ObjectId);
            Assert.Equal(pointsAwarded, gameObjectFind.Points);
            Assert.Equal(DateTime.UtcNow.ToString("s"), gameObjectFind.FoundDate.ToString("s"));
            Assert.Equal(gameParticipant.GamePoints, pointsAwarded + pointsAwardedForRegistrationObjectFind);
        }

        [Fact]
        public void HuntBotGame_AddGameObjectWithValidParams_AddGameObjectsToHuntBotGame()
        {
            HuntBotGame huntBotGame;
            GameObject gameObject;

            var huntBotGameId = Guid.NewGuid();
            huntBotGame = HuntBotGame.CreateNewHuntBotGame(huntBotGameId, HuntBotGameTestData.GameTitle, _defaultGameStartDate, _defaultGameEndDate, new List<string>());
            huntBotGame.AddGameObject(HuntBotGameTestData.GameObjectId, HuntBotGameTestData.WorldName, HuntBotGameTestData.GamePoints);
            gameObject = huntBotGame.GameObjects[0];

            Assert.NotNull(gameObject);
            Assert.Equal(HuntBotGameTestData.GameObjectId, gameObject.Id);
            Assert.Equal(HuntBotGameTestData.WorldName, gameObject.WorldName);
            Assert.Equal(10, HuntBotGameTestData.GamePoints);
        }

        [Fact]
        public void HuntBotGame_AddGameObjectThatWasAlreadyAdded_ThrowsBusinessRuleValidationException()
        {
            HuntBotGame huntBotGame;

            var huntBotGameId = Guid.NewGuid();
            huntBotGame = HuntBotGame.CreateNewHuntBotGame(huntBotGameId, HuntBotGameTestData.GameTitle, _defaultGameStartDate, _defaultGameEndDate, new List<string>());
            huntBotGame.AddGameObject(HuntBotGameTestData.GameObjectId, HuntBotGameTestData.WorldName, HuntBotGameTestData.GamePoints);

            Assert.Throws<BusinessRuleValidationException>(() => huntBotGame.AddGameObject
            (
                HuntBotGameTestData.GameObjectId,
                HuntBotGameTestData.WorldName, 
                HuntBotGameTestData.GamePoints
            ));
        }
    }
}
