using UnityEngine;

namespace TaoistFlip
{
    [CreateAssetMenu(fileName = "DefendCard", menuName = "Cards/DefendCard")]
    public class DefendCard : BaseCard
    {
        public override void OnCardActive(ActorController actor, ActorController target)
        {
            if (actor != null)
            {
                actor.OnRaisedShield(this.primaryValue);
                Debug.Log("[DefendCard] Active");
            }
        }
    }
}