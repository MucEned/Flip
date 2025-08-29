using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace TaoistFlip
{
    public class ActorController : MonoBehaviour
    {
        public Action OnActorDead;
        [SerializeField] protected Animator animator;
        [SerializeField] protected TMP_Text HPtxt;
        [SerializeField] protected TMP_Text Shieldtxt;
        [SerializeField] protected GameObject ShieldIcon;

        //Temp
        [SerializeField] protected ParticleSystem hitVFX;
        [SerializeField] protected ParticleSystem shieldVFX;
        [SerializeField] protected ParticleSystem healVFX;
        //----
        private ActorData data;

        protected int maxHP;
        protected int hp = 5;
        protected int shield = 0;
        protected int speed = 1;
        public int Speed => speed;

        public void Setup(ActorData actorData, Action OnActorDead)
        {
            this.data = actorData;
            this.maxHP = this.data.MaxHP;
            this.OnActorDead = OnActorDead;

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
            hitVFX.Play();
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
            healVFX.Play();
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
            shieldVFX.Play();
        }
        private void SetHP(int amount)
        {
            hp = Mathf.Clamp(amount, 0, maxHP);

            HPtxt.text = hp.ToString();
            HPtxt.transform.DOKill();
            HPtxt.transform.localScale = Vector3.one;
            HPtxt.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);

            if (hp <= 0)
            {
                Dead();
            }
        }
        private void SetShield(int amount)
        {
            shield = Mathf.Max(amount, 0);
            ShieldIcon.SetActive(shield > 0);

            Shieldtxt.text = shield.ToString();
            Shieldtxt.transform.DOKill();
            Shieldtxt.transform.localScale = Vector3.one;
            Shieldtxt.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
        }

        public List<BaseCard> GetDeck()
        {
            return data.Deck;
        }

        private void Dead()
        {
            OnActorDead?.Invoke();
        }
    }
}