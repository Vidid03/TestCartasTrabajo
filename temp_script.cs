csharp
using UnityEngine;
using UnityEngine.UI;

var canvasGO = GameObject.Find("Canvas");
var canvasRT = canvasGO.GetComponent<RectTransform>();
var screenWidth = canvasRT.sizeDelta.x;
var screenHeight = canvasRT.sizeDelta.y;

var btnGO = GameObject.Find("TopLeftButton");
var btnRT = btnGO.GetComponent<RectTransform>();

// Posición esquina superior izquierda (anchor upper-left)
// Con pivot en (0, 1), anchoredPosition debe ser (0, 0) desde la esquina
btnRT.anchorMin = new Vector2(0, 1);
btnRT.anchorMax = new Vector2(0, 1);
btnRT.pivot = new Vector2(0, 1);
btnRT.anchoredPosition = new Vector2(0, 0);

// Tamaño: 1/3 ancho, 1/10 alto
btnRT.sizeDelta = new Vector2(screenWidth / 3f, screenHeight / 10f);

// Agregar componentes
var image = btnGO.AddComponent<Image>();
image.color = Color.red;

var button = btnGO.AddComponent<Button>();
var colors = button.colors;
colors.normalColor = Color.red;
colors.highlightedColor = Color.green;
colors.pressedColor = new Color(0.5f, 0, 0);
colors.disabledColor = Color.gray;
colors.colorMultiplier = 1f;
colors.fadeDuration = 0.1f;
button.colors = colors;

Debug.Log($"Botón creado: tamaño {btnRT.sizeDelta}");