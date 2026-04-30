using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UploadSelectedCardsToPlay : MonoBehaviour
{
    [SerializeField] RectTransform cardsLowerDeck;
    [SerializeField] RectTransform cardsInPlay;
    [SerializeField] RectTransform referencePoint;
    [SerializeField] Button uploadButton;

    [SerializeField] float fallbackArcWidth = 420f;
    [SerializeField] float fallbackArcHeight = 52f;
    [SerializeField] float fallbackLeftRotationZ = -11f;
    [SerializeField] float fallbackRightRotationZ = 11f;

    void Awake()
    {
        if (cardsLowerDeck == null)
        {
            var lower = GameObject.Find("Canvas/BlackjackTemplateUI/CardsLowerDeck");
            if (lower != null) cardsLowerDeck = lower.transform as RectTransform;
        }

        if (cardsInPlay == null)
        {
            var play = GameObject.Find("Canvas/BlackjackTemplateUI/CardsInPlay");
            if (play != null) cardsInPlay = play.transform as RectTransform;
        }

        if (referencePoint == null)
        {
            var reference = GameObject.Find("Canvas/BlackjackTemplateUI/Reference");
            if (reference != null) referencePoint = reference.transform as RectTransform;
        }

        if (uploadButton == null)
            uploadButton = GetComponent<Button>();
    }

    void OnEnable()
    {
        if (uploadButton != null)
            uploadButton.onClick.AddListener(UploadSelected);
    }

    void OnDisable()
    {
        if (uploadButton != null)
            uploadButton.onClick.RemoveListener(UploadSelected);
    }

    public void UploadSelected()
    {
        if (cardsLowerDeck == null || cardsInPlay == null) return;

        var lowerArc = CaptureArcTemplate(cardsLowerDeck);
        var playArc = CaptureArcTemplate(cardsInPlay);

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

        LayoutArc(cardsLowerDeck, lowerArc);
        LayoutArc(cardsInPlay, playArc);
        CardHoverFocus.ReorderGroup(cardsLowerDeck);
        CardHoverFocus.ReorderGroup(cardsInPlay);
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
            if (group.GetChild(i) is RectTransform rt)
                cards.Add(rt);
        }

        Vector2 referenceLocal = GetReferenceLocalPosition(group);

        if (cards.Count < 2)
        {
            float fallbackRadius = Mathf.Max(1f, fallbackArcHeight * 4f);
            float halfAngle = Mathf.Asin(Mathf.Clamp((fallbackArcWidth * 0.5f) / fallbackRadius, -0.95f, 0.95f));
            float fallbackAngleStep = halfAngle * 0.5f;
            return new ArcTemplate
            {
                referenceLocal = referenceLocal,
                radius = fallbackRadius,
                centerAngle = -Mathf.PI * 0.5f,
                angleStep = fallbackAngleStep,
                rotationCenter = 0f,
                rotationPerRadian = (fallbackRightRotationZ - fallbackLeftRotationZ) / Mathf.Max(0.0001f, halfAngle * 2f)
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
        if (referencePoint == null)
        {
            var reference = GameObject.Find("Canvas/BlackjackTemplateUI/Reference");
            if (reference != null) referencePoint = reference.transform as RectTransform;
        }
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
        int count = group.childCount;
        if (count <= 0) return;

        float halfSpan = arc.angleStep * (count - 1) * 0.5f;
        float startAngle = arc.centerAngle - halfSpan;

        for (int i = 0; i < count; i++)
        {
            if (!(group.GetChild(i) is RectTransform card)) continue;

            float angle = startAngle + arc.angleStep * i;
            float x = arc.referenceLocal.x + Mathf.Cos(angle) * arc.radius;
            float y = arc.referenceLocal.y + Mathf.Sin(angle) * arc.radius;
            float rot = arc.rotationCenter + (angle - arc.centerAngle) * arc.rotationPerRadian;

            card.anchoredPosition = new Vector2(x, y);
            card.localScale = Vector3.one;
            card.localEulerAngles = new Vector3(0f, 0f, rot);
        }

        for (int i = 0; i < count; i++)
        {
            var hover = group.GetChild(i).GetComponent<CardHoverFocus>();
            if (hover != null)
                hover.RefreshBaseSnapshot();
        }
    }
}
