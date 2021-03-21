using HuntBot.Domain.SeedWork;

namespace HuntBot.Domain.HuntBotGames.GameObjects
{
    /// <summary>
    /// Objects that are hidden and search for as part of a HuntBot game.
    /// </summary>
    public class GameObject : Entity<int>    
    {
        /// <summary>
        /// Points to be awarded to a <see cref="EventParticipants.EventParticipant"/> when this object is found.
        /// </summary>
        public int Points { get; set; }
    }
}