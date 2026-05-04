using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using uVegas.Core.Cards;

public class PokerHandDeckManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] RectTransform cardsLowerDeck;
    [SerializeField] RectTransform cardsInPlay;
    [SerializeField] UploadSelectedCardsToPlay uploadSelectedCardsToPlay;
    [SerializeField] EventSystem eventSystem;

    [Header("Gameplay")]
    [SerializeField] int initialHandSize = 10;
    [SerializeField] float resolveLockSeconds = 5f;
    [SerializeField] bool autoInitializeOnStart = true;
    [SerializeField] bool autoResolveWhenCardsInPlay = true;
    [SerializeField] bool clearCardsInPlayAtStart = true;

    public static bool IsInteractionLocked { get; private set; }

    readonly List<CardData> deck = new List<CardData>(52);
    readonly Dictionary<RectTransform, CardData> assignedCards = new Dictionary<RectTransform, CardData>(32);
    readonly List<RectTransform> cache = new List<RectTransform>(32);

    bool initialized;
    bool resolving;

    static readonly Suit[] StandardSuits =
    {
        Suit.Hearts,
        Suit.Diamonds,
        Suit.Clubs,
        Suit.Spades
    };

    static readonly Rank[] StandardRanks =
    {
        Rank.Two,
        Rank.Three,
        Rank.Four,
        Rank.Five,
        Rank.Six,
        Rank.Seven,
        Rank.Eight,
        Rank.Nine,
        Rank.Ten,
        Rank.Jack,
        Rank.Queen,
        Rank.King,
        Rank.Ace
    };

    struct CardData
    {
        public Suit suit;
        public Rank rank;
    }

    void Start()
    {
        if (!ValidateRequiredReferences())
            return;

        if (autoInitializeOnStart)
            InitializeRuntimeDeckAndHand();
    }

    void OnDisable()
    {
        if (IsInteractionLocked)
            SetInteractionLocked(false);

        resolving = false;
    }

    void Update()
    {
        if (!initialized || resolving || !autoResolveWhenCardsInPlay) return;
        if (!ValidateRequiredReferences()) return;

        if (CountActiveChildren(cardsInPlay) > 0)
            TryStartResolvePhase();
    }

    public void InitializeRuntimeDeckAndHand()
    {
        if (!ValidateRequiredReferences()) return;

        SetInteractionLocked(false);
        BuildFreshDeck();

        if (clearCardsInPlayAtStart)
            DeactivateCardsInPlayAtStart();

        PrepareInitialHand();

        initialized = true;
    }

    public bool TryStartResolvePhase()
    {
        if (!initialized || resolving) return false;
        if (cardsInPlay == null || CountActiveChildren(cardsInPlay) <= 0) return false;

        StartCoroutine(ResolvePlayedCardsRoutine());
        return true;
    }

    public bool SortHandDescendingByRankThenSuit()
    {
        if (!initialized) return false;
        if (IsInteractionLocked) return false;
        if (!ValidateRequiredReferences()) return false;

        cache.Clear();
        CollectActiveChildren(cardsLowerDeck, cache);
        if (cache.Count <= 1)
            return false;

        cache.Sort(CompareCardsForHandSort);

        for (int i = 0; i < cache.Count; i++)
            cache[i].SetSiblingIndex(i);

        uploadSelectedCardsToPlay.RelayoutGroups();
        return true;
    }

    IEnumerator ResolvePlayedCardsRoutine()
    {
        resolving = true;
        SetInteractionLocked(true);

        float delay = Mathf.Max(0f, resolveLockSeconds);
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        cache.Clear();
        CollectActiveChildren(cardsInPlay, cache);
        for (int i = 0; i < cache.Count; i++)
        {
            var card = cache[i];
            if (card == null) continue;

            var hover = card.GetComponent<CardHoverFocus>();
            if (hover != null)
                hover.ClearSelectionForTransfer();

            card.SetParent(cardsLowerDeck, false);
            card.gameObject.SetActive(true);
            AssignNewCard(card);
        }

        RelayoutGroups();
        SetInteractionLocked(false);
        resolving = false;
    }

    void PrepareInitialHand()
    {
        cache.Clear();
        CollectActiveChildren(cardsLowerDeck, cache);

        int activeCount = cache.Count;
        if (activeCount == 0)
            return;

        int handSize = initialHandSize <= 0
            ? activeCount
            : Mathf.Clamp(initialHandSize, 1, activeCount);

        for (int i = 0; i < activeCount; i++)
        {
            bool shouldBeActive = i < handSize;
            cache[i].gameObject.SetActive(shouldBeActive);

            if (shouldBeActive)
            {
                AssignNewCard(cache[i]);
            }
            else
            {
                assignedCards.Remove(cache[i]);
            }
        }

        RelayoutGroups();
    }

    void DeactivateCardsInPlayAtStart()
    {
        cache.Clear();
        CollectAllChildren(cardsInPlay, cache);
        for (int i = 0; i < cache.Count; i++)
        {
            var card = cache[i];
            if (card == null) continue;

            var hover = card.GetComponent<CardHoverFocus>();
            if (hover != null)
                hover.ClearSelectionForTransfer();

            card.gameObject.SetActive(false);
            assignedCards.Remove(card);
        }
    }

    void AssignNewCard(RectTransform cardTransform)
    {
        if (cardTransform == null) return;

        if (deck.Count == 0)
            RebuildDeckExcludingActiveAssignments();

        if (deck.Count == 0)
        {
            Debug.LogWarning("PokerHandDeckManager: No cards available in deck.");
            return;
        }

        int index = Random.Range(0, deck.Count);
        CardData drawn = deck[index];
        deck.RemoveAt(index);
        assignedCards[cardTransform] = drawn;

        var converter = cardTransform.GetComponent<PokerCardVisualConverter>();
        if (converter != null)
        {
            converter.SetCard(converter.CurrentStyle, drawn.suit, drawn.rank, false);
        }

        var hover = cardTransform.GetComponent<CardHoverFocus>();
        if (hover != null)
            hover.RefreshBaseSnapshot();
    }

    void BuildFreshDeck()
    {
        deck.Clear();
        for (int s = 0; s < StandardSuits.Length; s++)
        {
            for (int r = 0; r < StandardRanks.Length; r++)
            {
                deck.Add(new CardData
                {
                    suit = StandardSuits[s],
                    rank = StandardRanks[r]
                });
            }
        }
    }

    void RebuildDeckExcludingActiveAssignments()
    {
        BuildFreshDeck();

        cache.Clear();
        if (cardsLowerDeck != null) CollectActiveChildren(cardsLowerDeck, cache);
        if (cardsInPlay != null) CollectActiveChildren(cardsInPlay, cache);

        for (int i = 0; i < cache.Count; i++)
        {
            RectTransform card = cache[i];
            if (card == null) continue;
            if (!assignedCards.TryGetValue(card, out var used)) continue;

            for (int j = 0; j < deck.Count; j++)
            {
                if (deck[j].suit == used.suit && deck[j].rank == used.rank)
                {
                    deck.RemoveAt(j);
                    break;
                }
            }
        }
    }

    void RelayoutGroups()
    {
        if (uploadSelectedCardsToPlay != null)
        {
            uploadSelectedCardsToPlay.RelayoutGroups();
            return;
        }

        if (cardsLowerDeck != null)
            CardHoverFocus.ReorderGroup(cardsLowerDeck);

        if (cardsInPlay != null)
            CardHoverFocus.ReorderGroup(cardsInPlay);
    }

    static int CountActiveChildren(RectTransform parent)
    {
        if (parent == null) return 0;

        int count = 0;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject.activeSelf)
                count++;
        }

        return count;
    }

    static void CollectActiveChildren(RectTransform parent, List<RectTransform> output)
    {
        if (parent == null || output == null) return;

        for (int i = 0; i < parent.childCount; i++)
        {
            if (!(parent.GetChild(i) is RectTransform child)) continue;
            if (!child.gameObject.activeSelf) continue;
            output.Add(child);
        }
    }

    static void CollectAllChildren(RectTransform parent, List<RectTransform> output)
    {
        if (parent == null || output == null) return;

        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i) is RectTransform child)
                output.Add(child);
        }
    }

    void SetInteractionLocked(bool locked)
    {
        IsInteractionLocked = locked;

        if (eventSystem == null)
        {
            Debug.LogError("PokerHandDeckManager: 'eventSystem' must be assigned in Inspector.", this);
            return;
        }

        eventSystem.enabled = !locked;
    }

    bool ValidateRequiredReferences()
    {
        if (cardsLowerDeck == null)
        {
            Debug.LogError("PokerHandDeckManager: 'cardsLowerDeck' must be assigned in Inspector.", this);
            return false;
        }

        if (cardsInPlay == null)
        {
            Debug.LogError("PokerHandDeckManager: 'cardsInPlay' must be assigned in Inspector.", this);
            return false;
        }

        if (uploadSelectedCardsToPlay == null)
        {
            Debug.LogError("PokerHandDeckManager: 'uploadSelectedCardsToPlay' must be assigned in Inspector.", this);
            return false;
        }

        if (eventSystem == null)
        {
            Debug.LogError("PokerHandDeckManager: 'eventSystem' must be assigned in Inspector.", this);
            return false;
        }

        return true;
    }

    int CompareCardsForHandSort(RectTransform a, RectTransform b)
    {
        var aData = GetCardDataForSort(a);
        var bData = GetCardDataForSort(b);

        int aRank = GetRankSortValue(aData.rank);
        int bRank = GetRankSortValue(bData.rank);

        if (aRank != bRank)
            return bRank.CompareTo(aRank);

        int aSuit = GetSuitSortValue(aData.suit);
        int bSuit = GetSuitSortValue(bData.suit);

        if (aSuit != bSuit)
            return bSuit.CompareTo(aSuit);

        return 0;
    }

    CardData GetCardDataForSort(RectTransform card)
    {
        if (card != null && assignedCards.TryGetValue(card, out var assigned))
            return assigned;

        var converter = card != null ? card.GetComponent<PokerCardVisualConverter>() : null;
        if (converter != null)
        {
            return new CardData
            {
                suit = converter.CurrentSuit,
                rank = converter.CurrentRank
            };
        }

        return new CardData
        {
            suit = Suit.Hidden,
            rank = Rank.None
        };
    }

    static int GetRankSortValue(Rank rank)
    {
        switch (rank)
        {
            case Rank.Ace: return 14;
            case Rank.King: return 13;
            case Rank.Queen: return 12;
            case Rank.Jack: return 11;
            case Rank.Ten: return 10;
            case Rank.Nine: return 9;
            case Rank.Eight: return 8;
            case Rank.Seven: return 7;
            case Rank.Six: return 6;
            case Rank.Five: return 5;
            case Rank.Four: return 4;
            case Rank.Three: return 3;
            case Rank.Two: return 2;
            default: return 0;
        }
    }

    static int GetSuitSortValue(Suit suit)
    {
        switch (suit)
        {
            case Suit.Spades: return 4;
            case Suit.Hearts: return 3;
            case Suit.Diamonds: return 2;
            case Suit.Clubs: return 1;
            default: return 0;
        }
    }
}
