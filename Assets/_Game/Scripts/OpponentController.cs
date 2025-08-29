using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace TaoistFlip
{
    public class OpponentController : ActorController
    {
        private float currentActionPoint = 0f;
        private BaseCard currentCard;
        [SerializeField] protected Slider actionBar;
        [SerializeField] private Image currentCardIcon;

        public float CurrentActionPoint => this.currentActionPoint;
        public void SetCurrentCard(BaseCard card)
        {
            this.currentCard = card;
            this.currentCardIcon.sprite = card.CardIcon;
            this.currentCardIcon.DOKill();
            this.currentCardIcon.transform.localScale = Vector3.one;
            this.currentCardIcon.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
        }

        private void AutoDoAction(ActorController target)
        {
            if (currentCard != null)
            {
                currentCard.OnCardActive(this, target);
            }
        }

        public void DoOpponentAction(ActorController player)
        {
            AutoDoAction(player);
            DrawCard();
        }

        public void UpdateActionPoint(float amount)
        {
            currentActionPoint += amount;
            this.actionBar.DOKill();
            this.actionBar.transform.localScale = Vector3.one;
            this.actionBar.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
            this.actionBar.DOValue(currentActionPoint, 0.25f);
        }

        public void DrawCard()
        {
            int randomCard = UnityEngine.Random.Range(0, GetDeck().Count);
            BaseCard currentCard = GetDeck()[randomCard];
            SetCurrentCard(currentCard);
        }
    }
}