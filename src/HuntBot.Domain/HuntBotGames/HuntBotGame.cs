using HuntBot.Domain.HuntBotGames.GameObjects;
using HuntBot.Domain.HuntBotGames.Participants;
using HuntBot.Domain.HuntBotGames.Rules;
using HuntBot.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuntBot.Domain.HuntBotGames
{
    /// <summary>
    /// Encapsulates HuntBot game information.
    /// </summary>
    public class HuntBotGame : AggregateRoot
    {
        /// <summary>
        /// The title of the HuntBot game.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The UTC in which the game starts.
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// The UTC in which the game ends.
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// List of game participants.
        /// </summary>
        public List<GameParticipant> Participants { get; private set; }

        /// <summary>
        /// List of game objects.
        /// </summary>
        public List<GameObject> GameObjects { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="HuntBotGame"/>.
        /// </summary>
        /// <param name="id">The id of the newly created game..</param>
        /// <param name="title">The title of the newly-created game.</param>
        /// <param name="startDate">The date and time in which the game begins.</param>
        /// <param name="endDate">The date and time in which the game ends.</param>
        private HuntBotGame(Guid id, string title, DateTime startDate, DateTime endDate)
        {
            Participants = new List<GameParticipant>();
            GameObjects = new List<GameObject>();

            ApplyChange(new Events.HuntBotGameCreated
            {
                Id = id,
                Title = title,
                StartDate = startDate,
                EndDate = endDate
            });
        }

        /// <summary>
        /// Creates a new instance of <see cref="HuntBotGame"/>.
        /// </summary>
        /// <param name="id">The unique identifier of the <see cref="HuntBotGame"/> instance.</param>
        /// <param name="title">The title of the game.</param>
        /// <param name="startDate">The date and time in which the game begins.</param>
        /// <param name="endDate">The date and time in which the game ends.</param>
        /// <param name="gameUniquenessChecker">Used to determine if the given game title is unique.</param>
        /// <returns>A newly-created instance of <see cref="HuntBotGame"/>.</returns>
        public static HuntBotGame CreateNewHuntBotGame(
            Guid id,
            string title, 
            DateTime startDate, 
            DateTime endDate,
            List<string> gameTitles
        )
        {
            CheckRule(new GameTitleLengthMustBeCorrectRule(title));
            CheckRule(new GameTitleMustBeUniqueRule(title, gameTitles));
            CheckRule(new GameStartDateMustNotBeInThePastRule(startDate));
            CheckRule(new GameEndDateMustBeAfterStartDateRule(startDate, endDate));

            return new HuntBotGame(id, title, startDate, endDate);
        }

        /// <summary>
        /// Adds a participant to the <see cref="Participants"/>.
        /// </summary>
        /// <param name="citizenNumber">The citizen number of the participant.</param>
        /// <param name="citizenName">The citizen name of the participant.</param>
        /// <param name="participantUniquenessChecker">Used to determine if the given citizen number has already been registered as a participant in this game.</param>
        /// <returns>A newly-created instance of <see cref="GameParticipant"/>.</returns>
        public void AddParticipant(int citizenNumber, string citizenName, int foundObjectId, int points, List<GameParticipant> gameParticipants)
        {
            CheckRule(new ParticipantIsNotRegisteredInGameRule(citizenNumber, Id, gameParticipants));

            ApplyChange(new Events.ParticipantAdded
            {
                CitizenNumber = citizenNumber,
                CitizenName = citizenName,
                FoundObjectId = foundObjectId,
                Points = points
            });            
        }

        /// <summary>
        /// Adds a game object to the <see cref="HuntBotGame"/> instance.
        /// </summary>
        /// <param name="objectId">The ObjectId of the object to add.</param>
        /// <param name="worldName"></param>
        /// <param name="points"></param>
        public void AddGameObject(int objectId, string worldName, int points, List<GameObject> gameObjects)
        {
            CheckRule(new GameObjectIsUniqueRule(objectId, gameObjects));

            ApplyChange(new Events.GameObjectAdded
            {
                ObjectId = objectId,
                WorldName = worldName,
                Points = points
            });
        }

        /// <summary>
        /// Marks that a participant has found a particular game object.
        /// </summary>
        /// <param name="citizenNumber">The CtizienNumber of the participant.</param>
        /// <param name="objectId">The ObjectId of the object found.</param>
        /// <param name="points">The number of points to be awarded by the find.</param>
        public void ParticipantFoundObject(int citizenNumber, int objectId, int points)
        {
            var participant = Participants.FirstOrDefault(p => p.Id == citizenNumber);

            CheckRule(new ParticipantIsRegisteredInGameRule(participant));

            participant.ParticipantFoundObject(objectId, points);
        }

        /// <summary>
        /// Matches an event to the event type and applies the corresponding changes to the <see cref="HuntBotGame"/> instance.
        /// </summary>
        /// <param name="event">The event to apply to the aggregate instance.</param>
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.HuntBotGameCreated e:
                    Id = e.Id;
                    Title = e.Title;
                    StartDate = e.StartDate;
                    EndDate = e.EndDate;
                    break;
                case Events.ParticipantAdded e:
                    var newParticipant = new GameParticipant(ApplyChange);

                    ApplyToEntity(newParticipant, e);
                    Participants.Add(newParticipant);
                    break;
                case Events.GameObjectAdded e:
                    var newObject = new GameObject(ApplyChange);

                    ApplyToEntity(newObject, e);
                    GameObjects.Add(newObject);
                    break;
            }
        }
    }
}