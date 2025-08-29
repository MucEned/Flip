using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using System.Text.RegularExpressions;

namespace TaoistFlip
{
    public class FlipGameController : MonoBehaviour
    {
        public static FlipGameController Instance { get; private set;}
        [SerializeField] private PlayerController player;
        [SerializeField] private OpponentController opponent;
        [SerializeField] private PlayerCardData playerData;
        [SerializeField] private PlayerCardData opponentData;
        [SerializeField] private CardDictionary cardDictionary;
        [SerializeField] private GameObject resetButton;

        private BattleController battleController;
        public bool ActionPhase {get; private set;}

        private eGameState mainState;
        private eGameBattleState microState;

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
            
        }

        private void LoadOpponent()
        {
            
        }
//-------------------------------------------------
    }

    public enum eGameState
    {
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
    }
}