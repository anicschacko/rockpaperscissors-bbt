using System.Collections.Generic;
using UnityEngine;

namespace RPS.Config
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private float gameTimer = 5f;
        [SerializeField] private float breakTimer = 2f;
        [SerializeField] private List<HandData> HandDatas;
        [SerializeField] private List<Sprite> handSprites;

        public float GetTimer() => gameTimer;

        public float GetBreakTimer() => breakTimer;
        
        public Sprite GetHandSprite(int index)
        {
            return handSprites[index];
        }

        public Sprite GetHandSprite(Hand hand)
        {
            return handSprites[(int)hand];
        }

        public List<Hand> GetWeakness(Hand hand)
        {
            var handData = HandDatas.Find(x => x.hand == hand);
            return handData.weakness;
        }
    }

    [System.Serializable]
    public class HandData
    {
        
        public Hand hand;
        public List<Hand> weakness;
    }
}