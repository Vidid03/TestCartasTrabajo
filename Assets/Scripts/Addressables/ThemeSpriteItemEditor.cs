using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(ThemeSpriteItem))]
public class ThemeSpriteItemEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		ThemeSpriteItem item = (ThemeSpriteItem)target;
		Image img = item.GetComponent<Image>();

		if (img != null && img.sprite != null)
		{
			// Si el nombre del sprite es distinto al ID guardado, actualizamos
			if (item.AssetID != img.sprite.name)
			{
				// Usamos SerializedObject para que Unity registre el cambio correctamente
				SerializedProperty prop = serializedObject.FindProperty("_assetID");
				prop.stringValue = img.sprite.name;
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}