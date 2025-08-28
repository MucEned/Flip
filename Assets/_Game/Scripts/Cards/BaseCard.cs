using UnityEngine;

namespace TaoistFlip
{
    public abstract class BaseCard : ScriptableObject
    {
        [SerializeField] protected string cardID;
        [SerializeField] protected Sprite cardIcon;
        [SerializeField] protected eCardType type;
        [SerializeField] protected int primaryValue;

        [HideInInspector] public string CardID => cardID;
        [HideInInspector] public Sprite CardIcon => cardIcon;
        [HideInInspector] public eCardType Type => type;
        [HideInInspector] public int PrimaryValue => primaryValue;

        public abstract void OnCardActive(ActorController actor, ActorController target);
    }
}