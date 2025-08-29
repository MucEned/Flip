using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using DG.Tweening;
using System;

namespace TaoistFlip
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField] private BalancingConfig balancingConfig;
        [SerializeField] private FieldGridController field;
        [SerializeField] private GameObject resetButton;
        [SerializeField] private CanvasGroup UICanvas;
        [SerializeField] private TMP_Text phaseDebug;
        private PlayerController player;
        private OpponentController opponent;
        private eGameBattleState state = eGameBattleState.None;
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
            matchData.TimeForEachPlayerAction = (float)Math.Ceiling(100f / (balancingConfig.BalanceTurn * ((float)player.Speed / (float)opponent.Speed))) / 100f;
        }

        public void StartGame()
        {
            opponent.DrawCard();
            ChangeMicroState(eGameBattleState.PlayerStartTurn).Forget();
        }

        public void EndGame()
        {
            ChangeMicroState(eGameBattleState.None).Forget();
        }

        public void ResetField()
        {
            if (!IsState(eGameBattleState.PlayerActionTurn))
                return;

            field.GenerateField(this.player);
            ChangeMicroState(eGameBattleState.PlayerEndTurn).Forget();
        }

        public void Wait()
        {
            if (!IsState(eGameBattleState.PlayerActionTurn))
                return;

            ClearHand();
        }

        public bool IsState(eGameBattleState checker)
        {
            return this.state == checker;
        }

        public async UniTask ChangeMicroState(eGameBattleState newState)
        {
            if (this.state == newState)
                return;

            this.state = newState;
            UpdatePhaseText();
            await UniTask.WaitForSeconds(0.5f);
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
            ChangeMicroState(eGameBattleState.PlayerActionTurn).Forget();
        }

        private void OnPlayerActionTurn()
        {
            UICanvas.alpha = 1;
            isBocking = false;
            //OnChangeMicroState(eGameMicroState.PlayerActionPhase).Forget();
        }

        private void OnPlayerEndTurn()
        {
            UICanvas.alpha = 0.2f;
            this.player.OnEndTurn();
            isBocking = false;
            ChangeMicroState(eGameBattleState.OpponentStartTurn).Forget();
        }

        private void OnOpponentStartTurn()
        {
            this.opponent.OnStartTurn();
            this.opponent.AddActionPoint(matchData.TimeForEachPlayerAction);
            ChangeMicroState(eGameBattleState.OpponentActionTurn).Forget();
        }

        private void OnOpponentActionTurn()
        {
            while (this.opponent.CurrentActionPoint > 1) //update later
            {
                this.opponent.DoOpponentAction(this.player);
            }
            ChangeMicroState(eGameBattleState.OpponentEndTurn).Forget();
        }

        private void OnOpponentEndTurn()
        {
            this.opponent.OnEndTurn();
            ChangeMicroState(eGameBattleState.PlayerStartTurn).Forget();
        }

//-----------------------------
        private bool isBocking = false;
        public bool OnCardFlip(CardComponent card)
        {
            if (!IsState(eGameBattleState.PlayerActionTurn))
                return false;

            if (isBocking)
                return false;

            foreach (var flippingCard in matchData.CurrentFlippingCards)
            {
                if (flippingCard.CardData.CardID == card.CardData.CardID)
                {
                    isBocking = true;
                    OnScore(flippingCard, card).Forget();
                    return true;
                }
            }
            matchData.CurrentFlippingCards.Add(card);
            if (matchData.CurrentFlippingCards.Count >= matchData.MaxFlipCard)
            {
                isBocking = true;
                FlipDown().Forget();
            }
            return true;
        }

        private async UniTask FlipDown()
        {
            await UniTask.WaitForSeconds(1);
            ClearHand();
        }

        private void ClearHand()
        {
            foreach (var flippingCard in matchData.CurrentFlippingCards)
            {
                flippingCard.Flip(eState.FaceDown);
            }
            ChangeMicroState(eGameBattleState.PlayerEndTurn).Forget();
            matchData.CurrentFlippingCards.Clear();
        }

        private async UniTask OnScore(CardComponent card1, CardComponent card2)
        {
            card1.CardData.OnCardActive(player, opponent);

            await UniTask.WaitForSeconds(1);
            card1.ShowDown();
            card2.ShowDown();
            matchData.CurrentFlippingCards.Clear();
            ChangeMicroState(eGameBattleState.PlayerEndTurn).Forget();
        }
//-----DEBUG
        private void UpdatePhaseText()
        {
            this.phaseDebug.transform.DOKill();
            this.phaseDebug.transform.localScale = Vector3.one;
            this.phaseDebug.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2, 1);
            this.phaseDebug.text = this.state.ToString();
        }
    }
//-------------------

    public enum eGameBattleState
    {
        None,
        PlayerStartTurn,
        PlayerActionTurn,
        PlayerEndTurn,
        OpponentStartTurn,
        OpponentActionTurn,
        OpponentEndTurn,
    }
}