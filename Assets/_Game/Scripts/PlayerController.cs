using System.Collections.Generic;
using UnityEngine;

namespace TaoistFlip
{
    public class PlayerController : ActorController
    {
        private PlayerCardData playerCardData;
        public PlayerCardData PlayerCardData => playerCardData;

        public List<BaseCard> GetDeck()
        {
            return playerCardData.Deck;
        }
    }
}