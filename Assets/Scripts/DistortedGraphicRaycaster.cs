using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class DistortedGraphicRaycaster : GraphicRaycaster
{
	private AdvancedLensDistortion settings;
	private PostProcessVolume volume;
	private RectTransform rectTransform;

	[Header("General Precision")]
	[Range(0.001f, 0.5f)]
	public float correctionSmoothness = 0.05f;
	[Range(5, 100)]
	public int iterations = 20;

	[Header("Exponential Curve")]
	[Tooltip("Menos de 1.0 = Más fuerte al inicio, más leve al final. Más de 1.0 = Al revés.")]
	[Range(0.1f, 2.0f)]
	public float exponentialFactor = 0.5f;

	[Header("Axis Sensitivity")]
	public float intensityMultiplierX = 1.0f;
	public float intensityMultiplierY = 1.0f;

	private Vector2 lastOriginalMousePos;
	private Vector2 lastCorrectedMousePos;
	private bool hasClicked = false;

	protected override void Start()
	{
		base.Start();
		rectTransform = GetComponent<RectTransform>();
		volume = FindFirstObjectByType<PostProcessVolume>();
		if (volume != null) volume.profile.TryGetSettings(out settings);
	}

	public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
	{
		if (settings == null || !settings.active || eventCamera == null)
		{
			base.Raycast(eventData, resultAppendList);
			return;
		}

		lastOriginalMousePos = eventData.position;
		hasClicked = true;

		Vector2 mouseUV = eventCamera.ScreenToViewportPoint(eventData.position);
		Vector2 correctedUV = GetExactInverseUV(mouseUV);

		lastCorrectedMousePos = new Vector2(
			(correctedUV.x - 0.5f) * rectTransform.rect.width,
			(correctedUV.y - 0.5f) * rectTransform.rect.height
		);

		Vector2 finalScreenPos = eventCamera.ViewportToScreenPoint(correctedUV);

		Vector2 originalPos = eventData.position;
		eventData.position = finalScreenPos;
		base.Raycast(eventData, resultAppendList);
		eventData.position = originalPos;
	}

	private void OnDrawGizmos()
	{
		if (!hasClicked || rectTransform == null) return;

		Gizmos.color = Color.yellow;
		Vector2 localOriginal;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, lastOriginalMousePos, eventCamera, out localOriginal);
		Gizmos.DrawSphere(rectTransform.TransformPoint(localOriginal), 15f);

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(rectTransform.TransformPoint(lastCorrectedMousePos), 18f);

		Gizmos.DrawLine(rectTransform.TransformPoint(localOriginal), rectTransform.TransformPoint(lastCorrectedMousePos));
	}

	Vector2 GetExactInverseUV(Vector2 targetUV)
	{
		float intensity = settings.intensity.value;
		float zoom = 1f / settings.zoom.value;
		Vector2 center = settings.center.value;
		float aspect = (float)eventCamera.pixelWidth / (float)eventCamera.pixelHeight;

		Vector2 currentGuess = targetUV;

		for (int i = 0; i < iterations; i++)
		{
			Vector2 dir = currentGuess - center;
			Vector2 correctedDir = dir;
			correctedDir.x *= aspect;

			float distSq = Vector2.Dot(correctedDir, correctedDir);
			float distCalc = intensity * distSq;

			Vector2 axisOffset = new Vector2(
				dir.x * (1.0f + distCalc * intensityMultiplierX),
				dir.y * (1.0f + distCalc * intensityMultiplierY)
			);

			Vector2 currentDistortedPos = center + axisOffset * zoom;
			Vector2 error = currentDistortedPos - targetUV;

			float errorMag = error.magnitude;
			if (errorMag < 0.0000001f) break;

			// --- APLICACIÓN DE LA CURVA EXPONENCIAL ---
			// Elevamos la magnitud del error al factor exponencial.
			// Esto hace que los errores pequeños se amplifiquen y los grandes se atenúen
			// si el factor es menor a 1.0.
			float curvedMag = Mathf.Pow(errorMag, exponentialFactor);
			Vector2 curvedError = error.normalized * curvedMag;

			currentGuess += curvedError * correctionSmoothness;
		}

		return currentGuess;
	}
}