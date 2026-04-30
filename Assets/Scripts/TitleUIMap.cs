using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

	public class TitleUIMap : MonoBehaviour
	{
		public ThemeGroup themeGroup;
		public void UIChangeTheme(string theme)
		{

			ThemeManager.Instance.SetTheme(theme);

			ThemeManager.Instance.LoadGroup(themeGroup);
		}


		private void Start()
		{
			ThemeManager.Instance.LoadGroup(themeGroup);
	}
}
