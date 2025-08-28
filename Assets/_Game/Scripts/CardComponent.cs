
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
        private Action<CardComponent> OnCardFlip;
        public BaseCard CardData => this.cardData;
        public void Init(BaseCard cardData, (int, int) position, Action<CardComponent> OnCardFlip)
        {
            this.cardData = cardData;
            this.Position = position;
            mainIcon.color = Color.white;
            this.OnCardFlip = OnCardFlip;

            this.state = eState.FaceDown;
        }

        public void Flip()
        {
            switch (state)
            {
                case eState.FaceDown:
                    FlipUp();
                break;
                case eState.FaceUp:
                    FlipDown();
                break;
            }

            this.OnCardFlip?.Invoke(this);
        }

        public void FlipDown()
        {   
            this.mainIcon.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
            this.mainIcon.transform.DOShakeRotation(0.2f, 45, 2, 1);
            this.mainIcon.sprite = backSprite;
            this.state = eState.FaceDown;
        }

        public void FlipUp()
        {
            this.mainIcon.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
            this.mainIcon.transform.DOShakeRotation(0.2f, 45, 2, 1);
            this.mainIcon.sprite = cardData.CardIcon;
            this.state = eState.FaceUp;
            this.OnCardFlip?.Invoke(this);
        }

        public void ShowDown()
        {
            this.state = eState.ShowDown;
            mainIcon.color = Color.black;
        }

        public void OnCardTap()
        {
            if (FlipGameController.Instance.ActionPhase)
            {
                FlipUp(); //test
            }
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
