using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

namespace TaoistFlip
{
    public class ActorController : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected TMP_Text HPtxt;
        [SerializeField] protected TMP_Text Shieldtxt;
        private ActorData data;

        protected int hp = 5;
        protected int shield = 0;
        protected int speed = 1;
        public int Speed => speed;

        public void Setup(ActorData actorData)
        {
            this.data = actorData;

            SetHP(this.data.MaxHP);
            SetShield(0);
            this.speed = this.data.Speed;
        }

        public void OnAttacked(int dmg)
        {
            int currentHP = hp - Mathf.Max(dmg - shield, 0);
            SetHP(currentHP);
            SetShield(shield - dmg);
            animator.Play("GetHit");
        }

        public void OnAttack(int dmg)
        {
            animator.Play("Attack");
        }

        public void OnHealed(int amount)
        {
            int currentHP = hp + amount;
            SetHP(currentHP);
            animator.Play("Attack");
        }

        public virtual void OnStartTurn()
        {
            SetShield(0);
        }
        public virtual void OnEndTurn() {}
        public void OnRaisedShield(int amount)
        {
            int currentShield = shield + amount;
            SetShield(currentShield);
            animator.Play("Attack");
        }
        private void SetHP(int amount)
        {
            hp = Mathf.Max(amount, 0);

            HPtxt.text = hp.ToString();
            HPtxt.transform.DOKill();
            HPtxt.transform.localScale = Vector3.one;
            HPtxt.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
        }
        private void SetShield(int amount)
        {
            shield = Mathf.Max(amount, 0);

            Shieldtxt.text = shield.ToString();
            Shieldtxt.transform.DOKill();
            Shieldtxt.transform.localScale = Vector3.one;
            Shieldtxt.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
        }

        public List<BaseCard> GetDeck()
        {
            return data.Deck;
        }
    }
}