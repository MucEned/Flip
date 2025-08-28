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
        [SerializeField] private PlayerCardData opponentData;
        [SerializeField] private CardDictionary cardDictionary;
        [SerializeField] private FieldGridController field;
        [SerializeField] private GameObject resetButton;

        private List<CardComponent> currentFlippingCards = new();
        private int maxFlipCard = 2;
        public bool ActionPhase {get; private set;}


        [SerializeField] private int balanceTurn = 3;
        private float currentOpponentActionTime = 0;
        private float timeForEachPlayerAction = 0.34f;

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
            CalculateSpeed();
            OpponentDrawCard();
        }

        public void ResetField()
        {
            ActionPhase = true;
            field.GenerateField(playerData.Deck);
            CalculateSpeed();
            OnPlayerEndTurn();
        }

        private void CalculateSpeed()
        {
            timeForEachPlayerAction = 1f / (balanceTurn * ((float)player.Speed / (float)opponent.Speed));
        }

        public void OnCardFlip(CardComponent card)
        {
            foreach (var flippingCard in currentFlippingCards)
            {
                if (flippingCard.CardData.CardID == card.CardData.CardID)
                {
                    OnScore(flippingCard, card).Forget();
                    return;
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
            OnPlayerStartTurn();
            foreach (var flippingCard in currentFlippingCards)
            {
                flippingCard.FlipDown();
            }
            OnPlayerEndTurn();
            ActionPhase = true;
            currentFlippingCards.Clear();
        }

        private async UniTask OnScore(CardComponent card1, CardComponent card2)
        {
            OnPlayerStartTurn();
            card1.CardData.OnCardActive(player, opponent);

            await UniTask.WaitForSeconds(1);
            card1.ShowDown();
            card2.ShowDown();
            currentFlippingCards.Clear();
            OnPlayerEndTurn();
        }

        private void OnPlayerStartTurn()
        {
            player.OnStartTurn();
            opponent.OnStartTurn();
        }

        private void OnPlayerEndTurn()
        {
            currentOpponentActionTime += timeForEachPlayerAction;

            while (currentOpponentActionTime > 1)
            {
                DoOpponentAction();
                currentOpponentActionTime -= 1;
            }

            opponent.UpdateActionBar(currentOpponentActionTime);

            player.OnEndTurn();
            opponent.OnEndTurn();
        }

        private void DoOpponentAction()
        {
            opponent.AutoDoAction(this.player);
            OpponentDrawCard();
        }

        private void OpponentDrawCard()
        {
            int randomCard = UnityEngine.Random.Range(0, this.opponentData.Deck.Count);
            BaseCard currentCard = this.opponentData.Deck[randomCard];
            opponent.SetCurrentCard(currentCard);
        }
    }
}