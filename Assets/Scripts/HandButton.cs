using UnityEngine;
using UnityEngine.UI;

namespace RPS.UI
{
    public class HandButton : MonoBehaviour
    {
        [SerializeField] private Hand thisHand;

        private void Awake()
        {
            this.gameObject.GetComponent<Toggle>().onValueChanged.AddListener(ToggleStateChange);
        }

        private void ToggleStateChange(bool arg0)
        {
            GameManager.OnClickedHand(thisHand, arg0);
        }
    }
}
