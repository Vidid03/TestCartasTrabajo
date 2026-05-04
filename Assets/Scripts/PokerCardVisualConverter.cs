using UnityEngine;
using UnityEngine.UI;
using uVegas.Core.Cards;
using uVegas.UI;

[ExecuteAlways]
public class PokerCardVisualConverter : MonoBehaviour
{
    public enum PokerCardStyle
    {
        Classic,
        Modern,
        Neo
    }

    [Header("Card Data")]
    [SerializeField] PokerCardStyle style = PokerCardStyle.Classic;
    [SerializeField] Suit suit = Suit.Hearts;
    [SerializeField] Rank rank = Rank.Ace;

    [Header("Outline")]
    [SerializeField] bool useRedOutline;
    [SerializeField] Color defaultOutlineColor = Color.black;
    [SerializeField] Color selectedOutlineColor = Color.red;

    [Header("References")]
    [SerializeField] Image baseImage;
    [SerializeField] Image rankImage;
    [SerializeField] Image suitImage;
    [SerializeField] Outline outline;
    [SerializeField] UICard uiCard;

    [Header("Themes")]
    [SerializeField] CardTheme classicTheme;
    [SerializeField] CardTheme modernTheme;
    [SerializeField] CardTheme neoTheme;

    public PokerCardStyle CurrentStyle => style;
    public Suit CurrentSuit => suit;
    public Rank CurrentRank => rank;

    public void Apply()
    {
        CacheMissingReferences();
        ApplyCardSprites();
        ApplyOutline();
    }

    public void SetCard(PokerCardStyle cardStyle, Suit cardSuit, Rank cardRank, bool redOutline)
    {
        style = cardStyle;
        suit = cardSuit;
        rank = cardRank;
        useRedOutline = redOutline;
        Apply();
    }

    public void SetRedOutline(bool redOutline)
    {
        useRedOutline = redOutline;
        ApplyOutline();
    }

    void Awake()
    {
        Apply();
    }

    void OnEnable()
    {
        Apply();
    }

    void OnValidate()
    {
        Apply();
    }

    void CacheMissingReferences()
    {
        if (baseImage == null)
            baseImage = GetComponent<Image>();

        if (outline == null)
            outline = GetComponent<Outline>();

        if (uiCard == null)
            uiCard = GetComponent<UICard>();

        if (rankImage == null)
        {
            var rankTransform = transform.Find("Rank");
            if (rankTransform != null)
                rankImage = rankTransform.GetComponent<Image>();
        }

        if (suitImage == null)
        {
            var suitTransform = transform.Find("Suit");
            if (suitTransform != null)
                suitImage = suitTransform.GetComponent<Image>();
        }
    }

    void ApplyCardSprites()
    {
        if (uiCard == null)
            return;

        var theme = GetSelectedTheme();
        if (theme == null)
            return;

        uiCard.Init(new Card(suit, rank), theme);
    }

    void ApplyOutline()
    {
        if (outline == null)
            return;

        outline.enabled = true;
        outline.effectColor = useRedOutline ? selectedOutlineColor : defaultOutlineColor;
    }

    CardTheme GetSelectedTheme()
    {
        switch (style)
        {
            case PokerCardStyle.Modern:
                return modernTheme;
            case PokerCardStyle.Neo:
                return neoTheme;
            default:
                return classicTheme;
        }
    }
}
