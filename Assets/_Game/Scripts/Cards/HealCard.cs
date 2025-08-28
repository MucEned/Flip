using UnityEngine;

namespace TaoistFlip
{
    [CreateAssetMenu(fileName = "HealCard", menuName = "Cards/HealCard")]
    public class HealCard : BaseCard
    {
        public override void OnCardActive(ActorController actor, ActorController target)
        {
            if (actor != null)
            {
                actor.OnHealed(this.primaryValue);
                Debug.Log("[HealCard] Active");
            }
        }
    }
}