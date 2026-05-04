using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using System.Net;

public class ThemeManager : MonoBehaviour
{
	public static ThemeManager Instance { get; private set; }

	[SerializeField] private string _currentTheme = "Default";


	private Dictionary<string, object> _cachedResources = new Dictionary<string, object>();
	private Dictionary<string, AsyncOperationHandle<Object>> _handles = new Dictionary<string, AsyncOperationHandle<Object>>();

	private HashSet<string> _activeKeys = new HashSet<string>();

	private void Awake()
	{
		if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
		else { Destroy(gameObject); }
	}

	public async Task LoadGroup(ThemeGroup group)
	{
		if (group == null || group.Items.Count == 0) return;

		_activeKeys.Clear();
		List<Task> loadingTasks = new List<Task>();

		foreach (var item in group.Items)
		{
			if (item == null || string.IsNullOrEmpty(item.AssetID)) continue;

			string address = $"{_currentTheme}/{item.AssetID}";
			_activeKeys.Add(address);
			loadingTasks.Add(ProcessItem(item, address));
		}

		await Task.WhenAll(loadingTasks);
		CleanupUnusedAssets();
	}

	private async Task ProcessItem(ThemeItem item, string address)
	{
		if (_cachedResources.TryGetValue(address, out object fastResource))
		{
			item.ApplyResource(fastResource);
			return;
		}

		if (_handles.TryGetValue(address, out AsyncOperationHandle<Object> existingHandle))
		{
			await existingHandle.Task;

			if (_cachedResources.TryGetValue(address, out object res)) item.ApplyResource(res);
			return;
		}

		var handle = Addressables.LoadAssetAsync<Object>(address);
		_handles.Add(address, handle);

		await handle.Task;

		if (handle.Status == AsyncOperationStatus.Succeeded)
		{
			object finalResource = handle.Result;

			if (finalResource is Texture2D tex)
			{
				finalResource = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
			}

			_cachedResources[address] = finalResource;
			item.ApplyResource(finalResource);
		}
		else
		{
			_handles.Remove(address);
		}
	}

	private void CleanupUnusedAssets()
	{
		List<string> keysToRemove = new List<string>();

		foreach (var key in _handles.Keys)
		{
			if (!_activeKeys.Contains(key))
			{

				if (_cachedResources.TryGetValue(key, out object res) && res is Sprite s)
				{
					if (res != _handles[key].Result) Destroy(s);
				}

				Addressables.Release(_handles[key]);
				keysToRemove.Add(key);
			}
		}

		foreach (var key in keysToRemove)
		{
			_handles.Remove(key);
			_cachedResources.Remove(key);
		}
	}

	public void SetTheme(string theme) { _currentTheme = theme; }

	// Añadir esto al ThemeManager
#if UNITY_EDITOR

[ContextMenu("Prepare for Build")]
public void PrepareForBuild()
{
	ThemeItem[] allItems = Object.FindObjectsByType<ThemeItem>(FindObjectsSortMode.None);

	foreach (var item in allItems)
	{
		item.ClearForBuild();
		EditorUtility.SetDirty(item.gameObject);
	}
}

	[ContextMenu("Restore for design")]
	public void RestoreForDesign()
	{
		var allItems = Object.FindObjectsByType<ThemeItem>(FindObjectsSortMode.None);

		foreach (var item in allItems)
		{
			if (string.IsNullOrEmpty(item.AssetID)) continue;
			string address = $"{_currentTheme}/{item.AssetID}";

			var handle = Addressables.LoadAsset<Object>(address);

			object finalResource = handle.Result;

			if (finalResource is Texture2D tex)
			{
				finalResource = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
			}

			item.ApplyResource(finalResource);

		}
	}
#endif
}