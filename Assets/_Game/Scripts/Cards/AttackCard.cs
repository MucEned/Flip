using FeedbackUtil;
using UnityEngine;

namespace TaoistFlip
{
    [CreateAssetMenu(fileName = "AttackCard", menuName = "Cards/AttackCard")]
    public class AttackCard : BaseCard
    {
        public override void OnCardActive(ActorController actor, ActorController target)
        {
            if (target != null)
            {
                target.OnAttacked(this.primaryValue);
                actor.OnAttack(this.primaryValue);
                Debug.Log("[AttackCard] Active");

                ShakeUtil.OnScreenShake?.Invoke(1f, 1f, 0.5f);
            }
        }
    }
}