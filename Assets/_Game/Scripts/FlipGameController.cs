using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace TaoistFlip
{
    public class FlipGameController : MonoBehaviour
    {
        public static FlipGameController Instance { get; private set;}
        [SerializeField] private PlayerController player;
        [SerializeField] private OpponentController opponent;
        [SerializeField] private ActorData playerData;
        [SerializeField] private List<ActorData> opponentData;
        [SerializeField] private BattleController battleController;

        public bool ActionPhase {get; private set;}
        private int currentOpponentindex = 0;

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
            battleController.EndGame();
            ChangeMainState(eGameState.Ending);
        }
        
//-------------------------------------------------
        private bool wasPlayerLoaded = false;
        private void LoadPlayerData()
        {
            if (wasPlayerLoaded)
                return;
            
            wasPlayerLoaded = true;
            this.player.Setup(playerData, OnPlayerDead);
        }
        private void LoadOpponent()
        {
            this.opponent.Setup(opponentData[currentOpponentindex], OnOpponentDead);
        }
        private void OnPlayerDead()
        {
            OnEndBattle();
            ReloadScene().Forget();
        }
        private void OnOpponentDead()
        {
            OnEndBattle();
            currentOpponentindex = Mathf.Clamp(currentOpponentindex + 1, 0, opponentData.Count - 1);
            ReSpawnOpponent().Forget();
        }
//------TEMP-------------------------------------------
        private async UniTask ReloadScene()
        {
            await UniTask.WaitForSeconds(1f);
            SceneManager.LoadScene("SampleScene");
        }
        private async UniTask ReSpawnOpponent()
        {
            await UniTask.WaitForSeconds(1f);
            ChangeMainState(eGameState.Starting);
        }
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