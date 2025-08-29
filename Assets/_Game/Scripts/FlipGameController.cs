using UnityEngine;
using System.Collections.Generic;

namespace TaoistFlip
{
    public class FlipGameController : MonoBehaviour
    {
        public static FlipGameController Instance { get; private set;}
        [SerializeField] private PlayerController player;
        [SerializeField] private OpponentController opponent;
        [SerializeField] private ActorData playerData;
        [SerializeField] private ActorData opponentData;
        [SerializeField] private BattleController battleController;

        public bool ActionPhase {get; private set;}

        private eGameState mainState = eGameState.None;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //testing
            ChangeMainState(eGameState.Starting);
        }

        private void ChangeMainState(eGameState newState)
        {
            if (this.mainState == newState)
                return;

            this.mainState = newState;
            switch (this.mainState)
            {
                case eGameState.Starting:
                OnInitBattle();
                break;
                case eGameState.BattlePhase:
                OnStartBattle();
                break;
                case eGameState.Ending:
                OnEndBattle();
                break;
            }
        }

        private void OnInitBattle()
        {
            LoadPlayerData();
            LoadOpponent();
            this.battleController.Setup(player, opponent);

            //On data loaded
            ChangeMainState(eGameState.BattlePhase);
        }

        private void OnStartBattle()
        {
            battleController.StartGame();
        }

        private void OnEndBattle()
        {
            
        }
        
//-------------------------------------------------
        private void LoadPlayerData()
        {
            this.player.Setup(playerData, OnPlayerDead);
        }
        private void LoadOpponent()
        {
            this.opponent.Setup(opponentData, OnOpponentDead);
        }
        private void OnPlayerDead()
        {
            //Endgame
        }
        private void OnOpponentDead()
        {
            //Load another opponent
        }
//-------------------------------------------------
    }

    public enum eGameState
    {
        None,
        Starting,
        BattlePhase,
        Ending,
    }

    public class MatchData
    {
        public int Turn;
        public int MaxFlipCard = 2;
        public float TimeForEachPlayerAction = 0f;
        public List<CardComponent> CurrentFlippingCards = new();
        public MatchData()
        {
            Turn = 0;
            MaxFlipCard = 2;
            TimeForEachPlayerAction = 0f;
            CurrentFlippingCards = new();
        }
    }
}