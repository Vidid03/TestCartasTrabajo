using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThemeItem : MonoBehaviour
{
	[SerializeField] protected string _assetID;
	public string AssetID => _assetID;

	public abstract void ApplyResource(object resource);

	public abstract void OnValidate();

	public abstract void ClearForBuild();
}
