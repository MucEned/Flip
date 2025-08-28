using System.Collections.Generic;
using UnityEngine;

namespace TaoistFlip
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Player Data")]
    public class PlayerCardData : ScriptableObject
    {
        public List<BaseCard> Deck;
    }
}