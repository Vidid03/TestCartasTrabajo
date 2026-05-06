using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(AdvancedLensDistortionRenderer), PostProcessEvent.AfterStack, "Custom/Advanced Lens Distortion")]
public sealed class AdvancedLensDistortion : PostProcessEffectSettings
{
	[Header("Deformación Principal")]
	[Range(-8f, 8f)] public FloatParameter intensity = new FloatParameter { value = 0f };
	[Range(0.1f, 8f)] public FloatParameter zoom = new FloatParameter { value = 1f };
	public Vector2Parameter center = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };

	[Header("Control de Ejes")]
	[Range(0f, 1f), Tooltip("Cuánto afecta la distorsión al eje X")]
	public FloatParameter factorX = new FloatParameter { value = 1f };

	[Range(0f, 1f), Tooltip("Cuánto afecta la distorsión al eje Y")]
	public FloatParameter factorY = new FloatParameter { value = 1f };
}

internal sealed class AdvancedLensDistortionRenderer : PostProcessEffectRenderer<AdvancedLensDistortion>
{
	// Dentro del método Render de AdvancedLensDistortionRenderer
	public override void Render(PostProcessRenderContext context)
	{
		if (context.camera.cameraType == CameraType.SceneView)
		{
			context.command.Blit(context.source, context.destination);
			return;
		}

		var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/AdvancedLensDistortion"));

		// CALCULAMOS EL ASPECT RATIO
		// Ejemplo: 1920 / 1080 = 1.77
		float aspect = (float)context.width / (float)context.height;
		sheet.properties.SetFloat("_Aspect", aspect);

		sheet.properties.SetFloat("_Intensity", settings.intensity);
		sheet.properties.SetFloat("_Zoom", 1f / settings.zoom);
		sheet.properties.SetVector("_Center", settings.center);
		sheet.properties.SetVector("_AxisFactors", new Vector2(settings.factorX, settings.factorY));

		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
	}
}