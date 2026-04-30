using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardHoverFocus : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RectTransform reference;
    public float hoverEnterDuration = 0.15f;
    public float hoverExitDuration = 0.1f;
    public float scaleMultiplier = 1.08f;
    public float liftAmount = 22f;

    RectTransform _rt;
    Vector2 _basePos;
    Vector3 _baseScale;
    int _restSibling;

    Vector2 _startPos;
    Vector2 _targetPos;
    Vector3 _startScale;
    Vector3 _targetScale;
    int _targetSibling;

    float _t;
    float _animDuration;
    bool _animating;

    static readonly List<RectTransform> _cache = new List<RectTransform>(32);
    static readonly Dictionary<Transform, int> _hoverCounts = new Dictionary<Transform, int>(8);
    static readonly Dictionary<Transform, int> _selectedCounts = new Dictionary<Transform, int>(8);
    static Transform _activeSelectionGroup;

    bool _isHovered;
    bool _isSelected;
    bool _pointerInside;
    Outline _outline;

    [SerializeField] Color defaultOutlineColor = Color.black;
    [SerializeField] Color selectedOutlineColor = Color.red;

    public bool IsSelected => _isSelected;

    void Awake()
    {
        _rt = transform as RectTransform;
        _basePos = _rt.anchoredPosition;
        _baseScale = _rt.localScale;
        _restSibling = _rt.GetSiblingIndex();
        _outline = GetComponent<Outline>();
        SetOutlineColor(defaultOutlineColor);
    }

    void OnEnable()
    {
        if (_rt == null) _rt = transform as RectTransform;
        if (_outline == null) _outline = GetComponent<Outline>();
        _basePos = _rt.anchoredPosition;
        _baseScale = _rt.localScale;
        _restSibling = _rt.GetSiblingIndex();
        SetOutlineColor(_isSelected ? selectedOutlineColor : defaultOutlineColor);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_rt == null) return;
        _pointerInside = true;
        if (_isSelected) return;

        if (!_isHovered)
        {
            _isHovered = true;
            SetHoverCount(_rt.parent, +1);
        }

        AnimateToHoverState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_rt == null) return;
        _pointerInside = false;
        if (_isSelected) return;

        if (_isHovered)
        {
            _isHovered = false;
            SetHoverCount(_rt.parent, -1);
        }

        BeginAnim(
            _rt.anchoredPosition,
            _basePos,
            _rt.localScale,
            _baseScale,
            _restSibling,
            hoverExitDuration
        );
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_rt == null) return;

        if (_isSelected)
        {
            Deselect();
            return;
        }

        if (!_isHovered) return;
        if (!CanSelectInCurrentGroup()) return;

        Select();
    }

    static void SetHoverCount(Transform parent, int delta)
    {
        if (parent == null) return;
        _hoverCounts.TryGetValue(parent, out var count);
        count += delta;
        if (count <= 0) _hoverCounts.Remove(parent);
        else _hoverCounts[parent] = count;
    }

    static int GetHoverCount(Transform parent)
    {
        if (parent == null) return 0;
        return _hoverCounts.TryGetValue(parent, out var count) ? count : 0;
    }

    bool CanSelectInCurrentGroup()
    {
        var group = _rt.parent;
        if (group == null) return false;
        return _activeSelectionGroup == null || _activeSelectionGroup == group;
    }

    static void SetSelectedCount(Transform parent, int delta)
    {
        if (parent == null) return;
        _selectedCounts.TryGetValue(parent, out var count);
        count += delta;
        if (count <= 0) _selectedCounts.Remove(parent);
        else _selectedCounts[parent] = count;
    }

    static int GetSelectedCount(Transform parent)
    {
        if (parent == null) return 0;
        return _selectedCounts.TryGetValue(parent, out var count) ? count : 0;
    }

    void Select()
    {
        if (_isSelected) return;

        _isSelected = true;
        _activeSelectionGroup = _rt.parent;
        SetSelectedCount(_rt.parent, +1);
        SetOutlineColor(selectedOutlineColor);

        if (!_isHovered)
        {
            _isHovered = true;
            SetHoverCount(_rt.parent, +1);
        }

        AnimateToHoverState();
        if (_rt.parent != null) ApplyBaseOrdering(_rt.parent);
    }

    void Deselect()
    {
        if (!_isSelected) return;

        _isSelected = false;
        SetSelectedCount(_rt.parent, -1);
        if (GetSelectedCount(_rt.parent) == 0 && _activeSelectionGroup == _rt.parent)
            _activeSelectionGroup = null;

        SetOutlineColor(defaultOutlineColor);

        if (!_pointerInside)
        {
            if (_isHovered)
            {
                _isHovered = false;
                SetHoverCount(_rt.parent, -1);
            }

            BeginAnim(
                _rt.anchoredPosition,
                _basePos,
                _rt.localScale,
                _baseScale,
                _restSibling,
                hoverExitDuration
            );
        }

        if (_rt.parent != null) ApplyBaseOrdering(_rt.parent);
    }

    void AnimateToHoverState()
    {
        var dir = Vector2.up;
        if (reference != null)
        {
            var parentRt = _rt.parent as RectTransform;
            var cardWorld = _rt.TransformPoint(_rt.rect.center);
            var refWorld = reference.TransformPoint(reference.rect.center);
            var cardInParent = parentRt.InverseTransformPoint(cardWorld);
            var refInParent = parentRt.InverseTransformPoint(refWorld);
            var d = (Vector2)(refInParent - cardInParent);
            if (d.sqrMagnitude > 0.0001f) dir = d.normalized;
        }

        BeginAnim(
            _rt.anchoredPosition,
            _basePos + dir * liftAmount,
            _rt.localScale,
            _baseScale * scaleMultiplier,
            _rt.parent.childCount - 1,
            hoverEnterDuration
        );
    }

    public void ClearSelectionForTransfer()
    {
        if (!_isSelected || _rt == null) return;

        var sourceParent = _rt.parent;

        _isSelected = false;
        _pointerInside = false;

        if (_isHovered)
        {
            _isHovered = false;
            SetHoverCount(sourceParent, -1);
        }

        SetSelectedCount(sourceParent, -1);
        if (GetSelectedCount(sourceParent) == 0 && _activeSelectionGroup == sourceParent)
            _activeSelectionGroup = null;

        _animating = false;
        SetOutlineColor(defaultOutlineColor);

        if (sourceParent != null)
            ApplyBaseOrdering(sourceParent);
    }

    public void RefreshBaseSnapshot()
    {
        if (_rt == null) _rt = transform as RectTransform;
        _basePos = _rt.anchoredPosition;
        _baseScale = _rt.localScale;
        _restSibling = _rt.GetSiblingIndex();
    }

    public static void ReorderGroup(Transform parent)
    {
        if (parent == null) return;
        ApplyBaseOrdering(parent);
    }

    void SetOutlineColor(Color color)
    {
        if (_outline == null) return;
        _outline.effectColor = color;
    }

    void BeginAnim(Vector2 fromPos, Vector2 toPos, Vector3 fromScale, Vector3 toScale, int sibling, float duration)
    {
        _startPos = fromPos;
        _targetPos = toPos;
        _startScale = fromScale;
        _targetScale = toScale;
        _targetSibling = sibling;
        _animDuration = Mathf.Max(0.0001f, duration);
        _t = 0f;
        _animating = true;

        if (_targetSibling == _rt.parent.childCount - 1)
            _rt.SetSiblingIndex(_targetSibling);
    }

    static void ApplyBaseOrdering(Transform parent)
    {
        _cache.Clear();
        var hovered = new List<RectTransform>(8);
        var selected = new List<RectTransform>(8);
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i) is RectTransform rt)
            {
                var hover = rt.GetComponent<CardHoverFocus>();
                if (hover != null && hover._isSelected)
                    selected.Add(rt);
                else if (hover != null && hover._isHovered)
                    hovered.Add(rt);
                else
                    _cache.Add(rt);
            }
        }

        // Left cards over right cards:
        // higher sibling index renders on top, so assign ascending indices
        // from right to left (x descending first).
        _cache.Sort((a, b) => b.anchoredPosition.x.CompareTo(a.anchoredPosition.x));
        int idx = 0;
        for (int i = 0; i < _cache.Count; i++)
        {
            _cache[i].SetSiblingIndex(idx++);
        }

        // Keep hovered cards above base cards preserving current relative order.
        hovered.Sort((a, b) => a.GetSiblingIndex().CompareTo(b.GetSiblingIndex()));
        for (int i = 0; i < hovered.Count; i++)
        {
            hovered[i].SetSiblingIndex(idx++);
        }

        // Selected cards stay above all others, ordered left over right.
        selected.Sort((a, b) => b.anchoredPosition.x.CompareTo(a.anchoredPosition.x));
        for (int i = 0; i < selected.Count; i++)
        {
            selected[i].SetSiblingIndex(idx++);
        }
    }

    void Update()
    {
        if (!_animating) return;
        _t += Time.unscaledDeltaTime / _animDuration;
        var k = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(_t));

        _rt.anchoredPosition = Vector2.LerpUnclamped(_startPos, _targetPos, k);
        _rt.localScale = Vector3.LerpUnclamped(_startScale, _targetScale, k);

        if (_t >= 1f)
        {
            _animating = false;
            _rt.anchoredPosition = _targetPos;
            _rt.localScale = _targetScale;
            _rt.SetSiblingIndex(_targetSibling);

            if (_isSelected && _rt.parent != null)
            {
                ApplyBaseOrdering(_rt.parent);
                return;
            }

            // After hover-out, restore deterministic group order so left cards
            // always remain above right cards.
            // Hovered cards are excluded from this reorder and remain on top.
            if (_targetSibling == _restSibling && _rt.parent != null)
            {
                ApplyBaseOrdering(_rt.parent);
                _restSibling = _rt.GetSiblingIndex();
            }

            // Ensure base snapshot remains exact canonical rest state.
            if (!_isHovered && _targetSibling == _restSibling)
            {
                _basePos = _targetPos;
                _baseScale = _targetScale;
            }
        }
    }
}
