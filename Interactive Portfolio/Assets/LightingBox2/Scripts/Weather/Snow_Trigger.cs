using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LightingBox.Effects
{
	[ExecuteInEditMode]
	public class Snow_Trigger : MonoBehaviour
	{

		public float onEnterIntensity = 3f;
		public float onExitIntensity = 0f;
		public float triggerRadius = 100f;
		public float fadeSpeed = 1f;

		void Update()
		{
			MeshRenderer[] mR = GameObject.FindObjectsOfType<MeshRenderer>();
			foreach (MeshRenderer m in mR)
			{
				if (Vector3.Distance(transform.position, m.transform.position) <= triggerRadius)
				{
					Shader.SetGlobalFloat("_SnowIntensity", Mathf.Lerp(Shader.GetGlobalFloat("_SnowIntensity"), onEnterIntensity, Time.deltaTime * fadeSpeed));
					Shader.SetGlobalFloat("SnowIntensity", Mathf.Lerp(Shader.GetGlobalFloat("_SnowIntensity"), onEnterIntensity, Time.deltaTime * fadeSpeed));
				}
				else
				{
					Shader.SetGlobalFloat("_SnowIntensity", Mathf.Lerp(Shader.GetGlobalFloat("_SnowIntensity"), onExitIntensity, Time.deltaTime * fadeSpeed));
					Shader.SetGlobalFloat("SnowIntensity", Mathf.Lerp(Shader.GetGlobalFloat("_SnowIntensity"), onExitIntensity, Time.deltaTime * fadeSpeed));
				}
			}
		}

		void OnDrawGizmos()
		{
			//triggerRadius = transform.localScale.x + transform.localScale.y + transform.localScale.z;
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, triggerRadius);
		}

	}

}