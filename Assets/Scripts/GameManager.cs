using System;
using System.Collections;
using RPS.Config;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RPS
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Config.GameConfig gameConfig;
        [SerializeField] private GameObject GamePlayPanel;
        [SerializeField] private GameObject GameoverPanel;
        
        [SerializeField] private GameObject MainMenuPanel;
        [SerializeField] private Button startGameButton;
        [SerializeField] private TextMeshProUGUI highScore;
        
        public static Action OnRoundStart;
        public static Action OnTimeOver;
        public static Action OnResetRound;
        public static Action OnGameOver;
        public static Action<Hand, bool> OnClickedHand;
        public static GameManager Instance;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            startGameButton.onClick.AddListener(OnClickStartButton);
            OnGameOver += () => StartCoroutine(nameof(GameOver));
            
            highScore.text = $"HIGH SCORE : {PlayerPrefs.GetInt("highscore")}";
        }

        public Config.GameConfig GetGameConfig() => gameConfig;
        
        private IEnumerator GameOver()
        {
            yield return null;
            GamePlayPanel.SetActive(false);
            GameoverPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            GameoverPanel.SetActive(false);
            MainMenuPanel.SetActive(true);
            highScore.text = $"HIGH SCORE : {PlayerPrefs.GetInt("highscore")}";
        }
        
        private void OnClickStartButton()
        {
            MainMenuPanel.SetActive(false);
            GamePlayPanel.SetActive(true);
        }
        
    }

}