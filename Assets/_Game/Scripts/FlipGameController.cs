using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace TaoistFlip
{
    public class FlipGameController : MonoBehaviour
    {
        public static FlipGameController Instance { get; private set;}
        [SerializeField] private ActorController player;
        [SerializeField] private ActorController opponent;
        [SerializeField] private PlayerCardData playerData;
        [SerializeField] private CardDictionary cardDictionary;
        [SerializeField] private FieldGridController field;
        [SerializeField] private GameObject resetButton;

        private List<CardComponent> currentFlippingCards = new();
        private int maxFlipCard = 2;
        public bool ActionPhase {get; private set;}

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            OnGameStart();
        }

        private void OnGameStart()
        {
            ActionPhase = true;
            field.GenerateField(playerData.Deck);
        }

        public void ResetField()
        {
            ActionPhase = true;
            field.GenerateField(playerData.Deck);
        }

        public void OnCardFlip(CardComponent card)
        {
            foreach (var flippingCard in currentFlippingCards)
            {
                if (flippingCard.CardData.CardID == card.CardData.CardID)
                {
                    OnScore(flippingCard, card).Forget();
                    break;
                }
            }
            currentFlippingCards.Add(card);
            if (currentFlippingCards.Count >= maxFlipCard)
            {
                ActionPhase = false;
                FlipDown().Forget();
            }
        }

        private async UniTask FlipDown()
        {
            await UniTask.WaitForSeconds(1);
            foreach (var flippingCard in currentFlippingCards)
            {
                flippingCard.FlipDown();
            }
            ActionPhase = true;
            currentFlippingCards.Clear();
        }

        private async UniTask OnScore(CardComponent card1, CardComponent card2)
        {
            card1.CardData.OnCardActive(player, opponent);

            await UniTask.WaitForSeconds(1);
            card1.ShowDown();
            card2.ShowDown();
        }
    }
}