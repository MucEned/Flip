using UnityEngine;

namespace TaoistFlip
{
    public class FieldGridController : MonoBehaviour
    {
        [SerializeField] private int gridSize = 4;
        private CardComponent[,] cardList = new CardComponent[0, 0];
    }
}
