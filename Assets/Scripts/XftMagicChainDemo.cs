using System;
using UnityEngine;
using Xft;

[ExecuteInEditMode]
public class XftMagicChainDemo : MonoBehaviour
{
	private void Update()
	{
		float magnitude = (this.Target.transform.position - this.MagicChain.transform.position).magnitude;
		Vector2 scale = new Vector2(1f, magnitude);
		this.MagicChain.SetScale(scale, "line");
		Vector3 forward = this.Target.transform.position - this.MagicChain.transform.position;
		this.MagicChain.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(150f, 0f, 400f, 25f), "move around Sphere2 in the editor scene to see how it works!");
	}

	public XffectComponent MagicChain;

	public Transform Target;
}
