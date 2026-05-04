using UnityEngine;
using UnityEngine.UI;

public class SortHandOnSuitButton : MonoBehaviour
{
    [SerializeField] Button suitButton;
    [SerializeField] PokerHandDeckManager deckManager;

    void OnEnable()
    {
        if (suitButton == null)
        {
            Debug.LogError("SortHandOnSuitButton: 'suitButton' must be assigned in Inspector.", this);
            return;
        }

        if (deckManager == null)
        {
            Debug.LogError("SortHandOnSuitButton: 'deckManager' must be assigned in Inspector.", this);
            return;
        }

        suitButton.onClick.AddListener(SortHand);
    }

    void OnDisable()
    {
        if (suitButton != null)
            suitButton.onClick.RemoveListener(SortHand);
    }

    void SortHand()
    {
        if (deckManager == null) return;
        deckManager.SortHandDescendingByRankThenSuit();
    }
}
