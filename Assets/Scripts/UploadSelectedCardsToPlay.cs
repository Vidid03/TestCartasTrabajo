using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UploadSelectedCardsToPlay : MonoBehaviour
{
    const float FallbackArcWidth = 420f;
    const float FallbackArcHeight = 52f;
    const float FallbackLeftRotationZ = -11f;
    const float FallbackRightRotationZ = 11f;
    const float HandTargetCardSpacing = 70f;
    const float PlayTargetCardSpacing = 92f;
    const float HandHorizontalSpreadMultiplier = 1f;

    [Header("References")]
    [SerializeField] RectTransform cardsLowerDeck;
    [SerializeField] RectTransform cardsInPlay;
    [SerializeField] RectTransform referencePoint;
    [SerializeField] Button uploadButton;
    [SerializeField] PokerHandDeckManager deckManager;

    [Header("Layout")]
    [SerializeField] float cardScaleMultiplier = 0.85f;

    void OnEnable()
    {
        if (uploadButton == null)
        {
            Debug.LogError("UploadSelectedCardsToPlay: 'uploadButton' must be assigned in Inspector.", this);
            return;
        }

        uploadButton.onClick.AddListener(UploadSelected);
    }

    void OnDisable()
    {
        if (uploadButton != null)
            uploadButton.onClick.RemoveListener(UploadSelected);
    }

    public void UploadSelected()
    {
        if (!ValidateRequiredReferences()) return;
        if (PokerHandDeckManager.IsInteractionLocked) return;

        var selected = new List<RectTransform>(8);
        for (int i = 0; i < cardsLowerDeck.childCount; i++)
        {
            if (!(cardsLowerDeck.GetChild(i) is RectTransform child)) continue;
            var hover = child.GetComponent<CardHoverFocus>();
            if (hover != null && hover.IsSelected)
                selected.Add(child);
        }

        if (selected.Count == 0) return;

        for (int i = 0; i < selected.Count; i++)
        {
            var card = selected[i];
            var hover = card.GetComponent<CardHoverFocus>();
            if (hover != null)
                hover.ClearSelectionForTransfer();

            card.SetParent(cardsInPlay, false);
        }

        RelayoutGroups();

        if (deckManager != null)
            deckManager.TryStartResolvePhase();
    }

    public void RelayoutGroups()
    {
        if (!ValidateRequiredReferences()) return;

        var lowerArc = CaptureArcTemplate(cardsLowerDeck);
        var playArc = CaptureArcTemplate(cardsInPlay);

        // Keep hand arc anchored to a fixed center to avoid cumulative drift.
        lowerArc.centerAngle = -Mathf.PI * 0.5f;

        // Keep play arc centered horizontally in its own group.
        playArc.centerAngle = -Mathf.PI * 0.5f;
        playArc.referenceLocal = new Vector2(0f, playArc.referenceLocal.y);

        float targetSpacing = HandTargetCardSpacing;
        lowerArc.angleStep = ComputeStepFromSpacing(lowerArc.radius, targetSpacing, lowerArc.angleStep);

        float playSpacing = PlayTargetCardSpacing;
        playArc.angleStep = ComputeStepFromSpacing(playArc.radius, playSpacing, playArc.angleStep);

        LayoutArc(cardsLowerDeck, lowerArc);
        LayoutArc(cardsInPlay, playArc);
        CardHoverFocus.ReorderGroup(cardsLowerDeck);
        CardHoverFocus.ReorderGroup(cardsInPlay);
    }

    static float ComputeStepFromSpacing(float radius, float targetSpacing, float currentStep)
    {
        float r = Mathf.Max(1f, radius);
        float halfChord = Mathf.Clamp(targetSpacing * 0.5f, 0f, r * 0.999f);
        float step = 2f * Mathf.Asin(halfChord / r);

        if (Mathf.Abs(step) < 0.0001f)
            return currentStep;

        return currentStep < 0f ? -step : step;
    }

    struct ArcTemplate
    {
        public Vector2 referenceLocal;
        public float radius;
        public float centerAngle;
        public float angleStep;
        public float rotationCenter;
        public float rotationPerRadian;
    }

    ArcTemplate CaptureArcTemplate(RectTransform group)
    {
        var cards = new List<RectTransform>(group.childCount);
        for (int i = 0; i < group.childCount; i++)
        {
            if (!(group.GetChild(i) is RectTransform rt))
                continue;

            if (!rt.gameObject.activeSelf)
                continue;

            if (rt.gameObject.activeInHierarchy)
                cards.Add(rt);
        }

        Vector2 referenceLocal = GetReferenceLocalPosition(group);

        if (cards.Count < 2)
        {
            float fallbackRadius = Mathf.Max(1f, FallbackArcHeight * 4f);
            float halfAngle = Mathf.Asin(Mathf.Clamp((FallbackArcWidth * 0.5f) / fallbackRadius, -0.95f, 0.95f));
            float fallbackAngleStep = halfAngle * 0.5f;
            return new ArcTemplate
            {
                referenceLocal = referenceLocal,
                radius = fallbackRadius,
                centerAngle = -Mathf.PI * 0.5f,
                angleStep = fallbackAngleStep,
                rotationCenter = 0f,
                rotationPerRadian = (FallbackRightRotationZ - FallbackLeftRotationZ) / Mathf.Max(0.0001f, halfAngle * 2f)
            };
        }

        cards.Sort((a, b) => a.anchoredPosition.x.CompareTo(b.anchoredPosition.x));

        var angles = new List<float>(cards.Count);
        float radiusSum = 0f;
        for (int i = 0; i < cards.Count; i++)
        {
            Vector2 d = cards[i].anchoredPosition - referenceLocal;
            radiusSum += d.magnitude;
            angles.Add(Mathf.Atan2(d.y, d.x));
        }

        float radius = Mathf.Max(1f, radiusSum / cards.Count);
        float centerAngle = (angles[0] + angles[angles.Count - 1]) * 0.5f;
        float angleStep = (angles.Count > 1) ? (angles[angles.Count - 1] - angles[0]) / (angles.Count - 1) : 0.12f;
        if (Mathf.Abs(angleStep) < 0.0001f) angleStep = 0.12f;

        float leftRot = NormalizeAngle(cards[0].localEulerAngles.z);
        float rightRot = NormalizeAngle(cards[cards.Count - 1].localEulerAngles.z);
        float rotationCenter = (leftRot + rightRot) * 0.5f;
        float rotationPerRadian = (rightRot - leftRot) / Mathf.Max(0.0001f, (angles[angles.Count - 1] - angles[0]));

        return new ArcTemplate
        {
            referenceLocal = referenceLocal,
            radius = radius,
            centerAngle = centerAngle,
            angleStep = angleStep,
            rotationCenter = rotationCenter,
            rotationPerRadian = rotationPerRadian
        };
    }

    Vector2 GetReferenceLocalPosition(RectTransform targetGroup)
    {
        if (targetGroup == null) return Vector2.zero;
        if (referencePoint == null) return Vector2.zero;

        Vector3 world = referencePoint.TransformPoint(referencePoint.rect.center);
        return targetGroup.InverseTransformPoint(world);
    }

    static float NormalizeAngle(float z)
    {
        if (z > 180f) z -= 360f;
        return z;
    }

    void LayoutArc(RectTransform group, ArcTemplate arc)
    {
        var cards = new List<RectTransform>(group.childCount);
        for (int i = 0; i < group.childCount; i++)
        {
            if (!(group.GetChild(i) is RectTransform card))
                continue;

            if (!card.gameObject.activeSelf)
                continue;

            if (card.gameObject.activeInHierarchy)
                cards.Add(card);
        }

        int count = cards.Count;
        if (count <= 0) return;

        float halfSpan = arc.angleStep * (count - 1) * 0.5f;
        float startAngle = arc.centerAngle - halfSpan;

        for (int i = 0; i < count; i++)
        {
            var card = cards[i];

            float angle = startAngle + arc.angleStep * i;
            float x = arc.referenceLocal.x + Mathf.Cos(angle) * arc.radius;
            float y = arc.referenceLocal.y + Mathf.Sin(angle) * arc.radius;
            float rot = arc.rotationCenter + (angle - arc.centerAngle) * arc.rotationPerRadian;
            float scale = Mathf.Clamp(cardScaleMultiplier, 0.6f, 1.2f);

            if (group == cardsLowerDeck)
            {
                float spread = HandHorizontalSpreadMultiplier;
                x = arc.referenceLocal.x + (x - arc.referenceLocal.x) * spread;
            }

            card.anchoredPosition = new Vector2(x, y);
            card.localScale = Vector3.one * scale;
            card.localEulerAngles = new Vector3(0f, 0f, rot);
        }

        for (int i = 0; i < count; i++)
        {
            var hover = cards[i].GetComponent<CardHoverFocus>();
            if (hover != null)
                hover.RefreshBaseSnapshot();
        }
    }

    bool ValidateRequiredReferences()
    {
        if (cardsLowerDeck == null)
        {
            Debug.LogError("UploadSelectedCardsToPlay: 'cardsLowerDeck' must be assigned in Inspector.", this);
            return false;
        }

        if (cardsInPlay == null)
        {
            Debug.LogError("UploadSelectedCardsToPlay: 'cardsInPlay' must be assigned in Inspector.", this);
            return false;
        }

        if (referencePoint == null)
        {
            Debug.LogError("UploadSelectedCardsToPlay: 'referencePoint' must be assigned in Inspector.", this);
            return false;
        }

        if (deckManager == null)
        {
            Debug.LogError("UploadSelectedCardsToPlay: 'deckManager' must be assigned in Inspector.", this);
            return false;
        }

        return true;
    }
}
