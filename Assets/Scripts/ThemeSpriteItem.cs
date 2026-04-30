using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ThemeSpriteItem : ThemeItem
{
	[SerializeField, HideInInspector] private Image _image;

	public override void OnValidate()
	{
		if (_image == null) _image = GetComponent<Image>();
		if (string.IsNullOrEmpty(_assetID) && _image.sprite != null)
			_assetID = _image.sprite.name;
	}

	public override void ApplyResource(object resource)
	{
		if (resource is Sprite sprite) _image.sprite = sprite;
	}

	public override void ClearForBuild()
	{
		if (_image == null) _image = GetComponent<Image>();
		_image.sprite = null; 
	}
}
