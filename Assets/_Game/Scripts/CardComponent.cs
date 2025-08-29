
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TaoistFlip
{
    public class CardComponent : MonoBehaviour
    {
        public (int, int) Position;
        [SerializeField] private Image mainIcon;
        [SerializeField] private Sprite backSprite;
        private BaseCard cardData;
        private eState state = eState.FaceDown;
        private Func<CardComponent, bool> OnCardFlip;
        public BaseCard CardData => this.cardData;
        public void Init(BaseCard cardData, (int, int) position, Func<CardComponent, bool> OnCardFlip)
        {
            this.cardData = cardData;
            this.Position = position;
            mainIcon.color = Color.white;
            this.OnCardFlip = OnCardFlip;

            this.state = eState.FaceDown;
        }

        public void Flip(eState newState)
        {
            switch (newState)
            {
                case eState.FaceDown:
                    FlipDown();
                break;
                case eState.FaceUp:
                    FlipUp();
                break;
            }
        }

        private void FlipDown()
        {   
            this.mainIcon.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
            this.mainIcon.transform.DOShakeRotation(0.2f, 45, 2, 1);
            this.mainIcon.sprite = backSprite;
            this.state = eState.FaceDown;
        }

        private void FlipUp()
        {
            if (this.state != eState.FaceDown)
                return;
                
            bool isSuccess = this.OnCardFlip.Invoke(this);
            if (isSuccess)
            {
                this.mainIcon.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
                this.mainIcon.transform.DOShakeRotation(0.2f, 45, 2, 1);
                this.mainIcon.sprite = cardData.CardIcon;
                this.state = eState.FaceUp;
            }
        }

        public void ShowDown()
        {
            this.state = eState.ShowDown;
            mainIcon.color = Color.black;
        }

        public void OnCardTap()
        {
            Flip(eState.FaceUp);
        }

        public void OnCardHold()
        {

        }
    }

    public enum eState
    {
        FaceDown,
        FaceUp,
        ShowDown,
    }

    public enum eCardType
    {
        Attack,
        Defend,
        Heal,
    }
}
