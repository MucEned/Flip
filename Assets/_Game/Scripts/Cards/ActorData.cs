using System.Collections.Generic;
using UnityEngine;

namespace TaoistFlip
{
    [CreateAssetMenu(fileName = "Actor Data", menuName = "Actor Data")]
    public class ActorData : ScriptableObject
    {
        public List<BaseCard> Deck;
        [SerializeField] private int maxHP;
        [SerializeField] private int speed;
        public int MaxHP => maxHP;
        public int Speed => speed;
    }
}