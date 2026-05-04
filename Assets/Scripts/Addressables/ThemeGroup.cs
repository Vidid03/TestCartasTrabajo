using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeGroup : MonoBehaviour
{
	[SerializeField] private List<ThemeItem> _items = new List<ThemeItem>();
	public List<ThemeItem> Items => _items;

	private void OnEnable()
	{
		if (ThemeManager.Instance != null)
		{
			//_ = ThemeManager.Instance.LoadGroup(this);
		}
	}

	// Botón en el inspector para no hacerlo a mano
	[ContextMenu("Recoger Theme Items")]
	private void GetItems()
	{
		_items.Clear();
		_items.AddRange(GetComponentsInChildren<ThemeItem>(true));
#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(this);
#endif
	}
}
