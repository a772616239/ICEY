using System;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("")]
public abstract class MotionBlurGhost : MonoBehaviour
{
	public abstract void SetOpacity(float a_fOpacity);

	public abstract void Hide();

	public abstract void Show();

	public void SetTransformToGhost(Transform a_rTransformToGhost)
	{
		base.transform.localScale = Vector3.one;
	}

	protected static SpriteRenderer CopyComponent(SpriteRenderer original, GameObject destination)
	{
		Type type = original.GetType();
		Component component = destination.GetComponent<SpriteRenderer>();
		if (component == null)
		{
			component = destination.AddComponent(type);
		}
		FieldInfo[] fields = type.GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			fieldInfo.SetValue(component, fieldInfo.GetValue(original));
		}
		PropertyInfo[] properties = type.GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (!(propertyInfo.Name == "material") && !(propertyInfo.Name == "materials"))
			{
				if (propertyInfo.CanWrite)
				{
					propertyInfo.SetValue(component, propertyInfo.GetValue(original, null), null);
				}
			}
		}
		return component as SpriteRenderer;
	}
}
