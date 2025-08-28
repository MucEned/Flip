using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TaoistFlip
{
    public class ActorController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Slider actionBar;

        [SerializeField] private TMP_Text HPtxt;
        [SerializeField] private TMP_Text Shieldtxt;

        [SerializeField] private int hp = 5;
        [SerializeField] private int shield = 0;
        [SerializeField] private int speed = 1;

        public int Speed => speed;

        //will move
        private float currentActionTimePoint = 0f;
        private BaseCard currentCard;
        [SerializeField] private Image currentCardIcon;

        public void Setup(int hp, int shield, int speed)
        {
            SetHP(hp);
            SetShield(shield);
            this.speed = speed;
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

        public void OnStartTurn()
        {
            SetShield(0);
        }

        public void OnEndTurn()
        {
            // shield = 0;
        }

        public void SetCurrentCard(BaseCard card)
        {
            this.currentCard = card;
            this.currentCardIcon.sprite = card.CardIcon;
            this.currentCardIcon.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
        }

        public void AutoDoAction(ActorController target)
        {
            if (currentCard != null)
            {
                currentCard.OnCardActive(this, target);
            }
        }

        public void UpdateActionBar(float amount)
        {
            currentActionTimePoint = amount;
            this.actionBar.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
            this.actionBar.DOValue(currentActionTimePoint, 0.25f);
        }

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
            HPtxt.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
        }
        private void SetShield(int amount)
        {
            shield = Mathf.Max(amount, 0);
            Shieldtxt.text = shield.ToString();
            HPtxt.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
        }
    }
}