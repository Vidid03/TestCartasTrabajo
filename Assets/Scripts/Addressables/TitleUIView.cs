using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class TitleUIView : MonoBehaviour
{
	public Transform parent;

	private GameObject current;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UIChangePrefab(string name)
    {
		Addressables.InstantiateAsync(name, parent).Completed += (handle) =>
		{
			if (handle.Status == AsyncOperationStatus.Succeeded)
			{
				Destroy(current);
				current = handle.Result;
			}

		};
	}

}
