using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

namespace TaoistFlip
{
    public class FieldGridController : MonoBehaviour
    {
        [SerializeField] private BattleController controller;
        [SerializeField] private int gridSize = 4;
        [SerializeField] private Transform fieldContainer;
        [SerializeField] private AssetReference cardSample;

        private CardComponent[,] cardList = new CardComponent[0, 0];
        public void GenerateField(PlayerController playerController)
        {
            List<BaseCard> playerDeck = playerController.GetDeck();
            cardList = new CardComponent[this.gridSize, this.gridSize];
            foreach (Transform child in fieldContainer)
            {
                Destroy(child.gameObject);
            }
            SpawnAsync(playerDeck, this.gridSize).Forget();
        }

        public async UniTask SpawnAsync(List<BaseCard> playerDeck, int gridSize)
        {
            List<BaseCard> cards = GetFieldCards(playerDeck, gridSize);
            int index = 0;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    var handle = Addressables.InstantiateAsync(cardSample, fieldContainer);
                    GameObject obj = await handle.Task;
                    CardComponent cardComponent = obj.GetComponent<CardComponent>();
                    if (cardComponent != null)
                    {
                        cardComponent.Init(cards[index], (i, j), OnCardFlip);
                        cardList[i,j] = cardComponent;
                    }
                    index++;
                    Debug.Log("Spawn card at: " + i + "," + j);
                }
            }
        }

        private List<BaseCard> GetFieldCards(List<BaseCard> playerDeck, int gridSize)
        {
            List<BaseCard> result = new();
            //Fake cal
            foreach (var card in playerDeck)
            {
                result.Add(card);
                result.Add(card);
            }

            result.Shuffle();
            return result;
        }

        private void OnCardFlip(CardComponent card)
        {
            controller.OnCardFlip(card);
        }
    }
}
