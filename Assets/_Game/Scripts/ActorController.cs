using TMPro;
using UnityEngine;

namespace TaoistFlip
{
    public class ActorController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [SerializeField] private TMP_Text HPtxt;
        [SerializeField] private TMP_Text Shieldtxt;

        private int hp = 5;
        private int shield = 0;
        private int speed;

        public void Setup(int hp, int shield, int speed)
        {
            SetHP(hp);
            SetShield(shield);
            this.speed = speed;
        }

        public void OnAttacked(int dmg)
        {
            int currentHP = hp - dmg;
            SetHP(currentHP);
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

        public void OnRaisedShield(int amount)
        {
            int currentShield = shield + amount;
            SetShield(currentShield);
            animator.Play("Attack");
        }


        private void SetHP(int amount)
        {
            hp = amount;
            HPtxt.text = hp.ToString();
        }
        private void SetShield(int amount)
        {
            shield = amount;
            Shieldtxt.text = shield.ToString();
        }
    }
}