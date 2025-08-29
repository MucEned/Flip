using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using System.Text.RegularExpressions;

namespace TaoistFlip
{
    public class BattleController
    {
        [SerializeField] private BalancingConfig balancingConfig;
        [SerializeField] private FieldGridController field;
        private PlayerController player;
        private OpponentController opponent;
        private eGameBattleState state;
        private MatchData matchData = new();
        public void Setup(PlayerController player, OpponentController opponent)
        {
            this.player = player;
            this.opponent = opponent;
            CalculateSpeed();
            field.GenerateField(this.player);
        }
        private void CalculateSpeed()
        {
            matchData.TimeForEachPlayerAction = 1f / (balancingConfig.BalanceTurn * ((float)player.Speed / (float)opponent.Speed));
        }

        public void StartGame()
        {
            opponent.DrawCard();
            ChangeMicroState(eGameBattleState.PlayerStartTurn);
        }

        public void ResetField()
        {
            if (!IsState(eGameBattleState.PlayerActionTurn))
                return;

            field.GenerateField(this.player);
            ChangeMicroState(eGameBattleState.PlayerEndTurn);
        }

        public bool IsState(eGameBattleState checker)
        {
            return this.state == checker;
        }


        public void ChangeMicroState(eGameBattleState newState)
        {
            if (this.state == newState)
                return;

            this.state = newState;
            switch (this.state)
            {
                case eGameBattleState.PlayerStartTurn:
                OnPlayerStartTurn();
                break;
                case eGameBattleState.PlayerActionTurn:
                OnPlayerActionTurn();
                break;
                case eGameBattleState.PlayerEndTurn:
                OnPlayerEndTurn();
                break;
                case eGameBattleState.OpponentStartTurn:
                OnOpponentStartTurn();
                break;
                case eGameBattleState.OpponentActionTurn:
                OnOpponentActionTurn();
                break;
                case eGameBattleState.OpponentEndTurn:
                OnOpponentEndTurn();
                break;
            }
        }

        private void OnPlayerStartTurn()
        {
            this.player.OnStartTurn();
            ChangeMicroState(eGameBattleState.PlayerActionTurn);
        }

        private void OnPlayerActionTurn()
        {
            //OnChangeMicroState(eGameMicroState.PlayerActionPhase);
        }

        private void OnPlayerEndTurn()
        {
            this.player.OnEndTurn();
            ChangeMicroState(eGameBattleState.OpponentStartTurn);
        }

        private void OnOpponentStartTurn()
        {
            this.opponent.OnStartTurn();
            this.opponent.UpdateActionPoint(matchData.TimeForEachPlayerAction);
            ChangeMicroState(eGameBattleState.OpponentActionTurn);
        }

        private void OnOpponentActionTurn()
        {
            while (opponent.CurrentActionPoint > 1)
            {
                opponent.DoOpponentAction(this.player);
            }
            opponent.UpdateActionPoint(matchData.TimeForEachPlayerAction - 1);
        }

        private void OnOpponentEndTurn()
        {
            this.opponent.OnEndTurn();
            ChangeMicroState(eGameBattleState.PlayerStartTurn);
        }

//-----------------------------

        public bool OnCardFlip(CardComponent card)
        {
            if (!IsState(eGameBattleState.PlayerActionTurn))
                return false;

            foreach (var flippingCard in matchData.CurrentFlippingCards)
            {
                if (flippingCard.CardData.CardID == card.CardData.CardID)
                {
                    OnScore(flippingCard, card).Forget();
                    return true;
                }
            }
            matchData.CurrentFlippingCards.Add(card);
            if (matchData.CurrentFlippingCards.Count >= matchData.MaxFlipCard)
            {
                FlipDown().Forget();
            }
            return true;
        }

        private async UniTask FlipDown()
        {
            await UniTask.WaitForSeconds(1);
            foreach (var flippingCard in matchData.CurrentFlippingCards)
            {
                flippingCard.FlipDown();
            }
            OnPlayerEndTurn();
            matchData.CurrentFlippingCards.Clear();
        }

        private async UniTask OnScore(CardComponent card1, CardComponent card2)
        {
            card1.CardData.OnCardActive(player, opponent);

            await UniTask.WaitForSeconds(1);
            card1.ShowDown();
            card2.ShowDown();
            matchData.CurrentFlippingCards.Clear();
            SetPlayerEndTurn();
        }






        private void SetPlayerEndTurn() //Update
        {
            ChangeMicroState(eGameBattleState.PlayerEndTurn);
        }
    }

    public enum eGameBattleState
    {
        PlayerStartTurn,
        PlayerActionTurn,
        PlayerEndTurn,
        OpponentStartTurn,
        OpponentActionTurn,
        OpponentEndTurn,
    }
}