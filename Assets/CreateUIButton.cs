using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateUIButton : MonoBehaviour
{
    [ContextMenu("Create Button")]
    public static void CreateButton()
    {
        // Buscar el Canvas en la escena
        Canvas canvas = FindObjectOfType<Canvas>();
        
        if (canvas == null)
        {
            Debug.LogError("No se encontró un Canvas en la escena. Por favor, crea uno primero.");
            return;
        }
        
        // Crear el GameObject del botón
        GameObject buttonObj = new GameObject("Button");
        buttonObj.transform.SetParent(canvas.transform, false);
        
        // Agregar RectTransform y configurar
        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(160, 60);
        rectTransform.anchoredPosition = new Vector2(0, 0);
        
        // Agregar Image component (para el fondo del botón)
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = Color.white;
        
        // Agregar Button component
        Button button = buttonObj.AddComponent<Button>();
        
        // Configurar colores del Button (ColorBlock)
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(220f/255f, 220f/255f, 220f/255f, 1f); // Gris claro
        colors.pressedColor = new Color(200f/255f, 200f/255f, 200f/255f, 1f);
        colors.selectedColor = Color.white;
        colors.disabledColor = new Color(200f/255f, 200f/255f, 200f/255f, 0.5f);
        button.colors = colors;
        
        // Crear el hijo para el texto
        GameObject textObj = new GameObject("Text (TMP)");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        // Agregar TextMeshProUGUI
        TextMeshProUGUI textMeshPro = textObj.AddComponent<TextMeshProUGUI>();
        textMeshPro.text = "Boton IA";
        textMeshPro.fontSize = 24;
        textMeshPro.color = Color.black;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        
        // Configurar RectTransform del texto
        RectTransform textRectTransform = textObj.GetComponent<RectTransform>();
        textRectTransform.anchorMin = Vector2.zero;
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.offsetMin = Vector2.zero;
        textRectTransform.offsetMax = Vector2.zero;
        
        Debug.Log("Botón creado exitosamente en el Canvas!");
    }
}
