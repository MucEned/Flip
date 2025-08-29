using UnityEngine;

namespace TaoistFlip
{
    public abstract class BalancingConfig : ScriptableObject
    {
        [SerializeField] private int balanceTurn = 3;

        public int BalanceTurn => balanceTurn;
    }
}