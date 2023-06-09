using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace RPS
{
    public class GameplayManager : MonoBehaviour
    {
        [Header("Game Manager Fields")] 
        [SerializeField] private Image aiHand;
        [SerializeField] private Image playerHand;
        [SerializeField] private TextMeshProUGUI playerScoreText;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private GameObject gameOver;

        private Hand currentAiHand = Hand.None;
        private Hand currentPlayerHand = Hand.None;
        private int playerScore = 0;
        private bool timerOver = false;
        private Config.GameConfig gameConfig;

        private void Awake()
        {
            GameManager.OnTimeOver += TimeOver;
            GameManager.OnClickedHand += OnClickedHand;
            gameConfig = GameManager.Instance.GetGameConfig();
        }

        private void OnEnable()
        {
            playerScoreText.text = $"SCORE : {playerScore}";
            StartRound();
        }

        void StartRound()
        {
            StartCoroutine(nameof(JumbleAIHand), GetRandomHand());
            GameManager.OnRoundStart?.Invoke();
        }

        private Hand GetRandomHand()
        {
            return (Hand)Random.Range(0, 5);
        }

        private void OnClickedHand(Hand hand, bool isActive)
        {
            playerHand.gameObject.SetActive(isActive);
            currentPlayerHand = hand;
            playerHand.sprite = gameConfig.GetHandSprite(hand);
        }


        private IEnumerator JumbleAIHand(Hand hand)
        {
            yield return null;
            currentAiHand = hand;
            int i = -1;
            while (true)
            {
                i++;
                i = i >= 5 ? 0 : i;

                aiHand.sprite = gameConfig.GetHandSprite(i);
                yield return new WaitForSeconds(0.2f);
                if (timerOver)
                    break;
            }
            aiHand.sprite = gameConfig.GetHandSprite(hand);
        }

        private void TimeOver()
        {
            timerOver = true;
            StartCoroutine(GetWinner(currentAiHand, currentPlayerHand));
        }

        private IEnumerator GetWinner(Hand ai, Hand player)
        {
            if (ai == player)
            {
                yield return new WaitForSeconds(gameConfig.GetBreakTimer());
                RestartGame();
                yield break;
            }
            if (gameConfig.GetWeakness(ai).Contains(player))
            {
                playerScoreText.text = $"SCORE : {++playerScore}";
                yield return new WaitForSeconds(gameConfig.GetBreakTimer());
                RestartGame();
            }
            else
            {
                if(PlayerPrefs.GetInt("highscore") < playerScore)
                    PlayerPrefs.SetInt("highscore", playerScore);
                yield return new WaitForSeconds(gameConfig.GetBreakTimer());
                Reset();
                playerScore = 0;
                GameManager.OnGameOver?.Invoke();
            }
        }

        private void Reset()
        {
            timerOver = false;
            toggleGroup.SetAllTogglesOff();
            playerHand.gameObject.SetActive(false);
            currentPlayerHand = Hand.None;
        }

        private void RestartGame()
        {
            Reset();
            GameManager.OnResetRound?.Invoke();
            StartRound();
        }

        private void OnDisable()
        {
            Reset();
        }

        private void OnDestroy()
        {
            GameManager.OnTimeOver -= TimeOver;
            GameManager.OnClickedHand -= OnClickedHand;
        }
    }
}
