using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPS
{
    public class BorderAnimation : MonoBehaviour
    {
        [SerializeField] private List<Image> horizontalEdges;
        [SerializeField] private List<Image> verticalEdges;
        [SerializeField] private Slider countdownSlider;

        private float timer, timerConfig;
        
        private void Start()
        {
            GameManager.OnRoundStart += RoundStart; 
            GameManager.OnResetRound += () => timer = timerConfig;
            GameManager.OnGameOver += () => countdownSlider.gameObject.SetActive(false);
        }

        private void RoundStart()
        {
            countdownSlider.gameObject.SetActive(true);
            timerConfig = timer = GameManager.Instance.GetGameConfig().GetTimer();
        }

        private Color lerpedColor = Color.white;
        private float lerpValue = 0;
        private void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                lerpValue = timer / timerConfig;
                countdownSlider.value = lerpValue;
                lerpedColor = Color.Lerp(Color.red, Color.white, lerpValue);
                countdownSlider.fillRect.GetComponent<Image>().color = lerpedColor;
                if (timer <= 0)
                    GameManager.OnTimeOver?.Invoke();
            }
        }
    }
}
