using System;
using UnityEngine;
using Xft;

public class SphericalBillboardDemo : MonoBehaviour
{
	private void Start()
	{
		this.QuadExplode.Initialize();
		this.SphericalExplode.Initialize();
		this.QuadFogs.Initialize();
		this.SphericalFogs.Initialize();
	}

	private void ActiveEffect(XffectComponent xft)
	{
		this.QuadExplode.DeActive();
		this.SphericalExplode.DeActive();
		this.QuadFogs.DeActive();
		this.SphericalFogs.DeActive();
		xft.Active();
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(0f, 0f, 200f, 30f), "Spherical Explode"))
		{
			this.ActiveEffect(this.SphericalExplode);
		}
		if (GUI.Button(new Rect(0f, 30f, 200f, 30f), "QuadExplode"))
		{
			this.ActiveEffect(this.QuadExplode);
		}
		if (GUI.Button(new Rect(0f, 60f, 200f, 30f), "Spherical Fogs"))
		{
			this.ActiveEffect(this.SphericalFogs);
		}
		if (GUI.Button(new Rect(0f, 90f, 200f, 30f), "Quad Fogs"))
		{
			this.ActiveEffect(this.QuadFogs);
		}
	}

	public XffectComponent QuadExplode;

	public XffectComponent SphericalExplode;

	public XffectComponent QuadFogs;

	public XffectComponent SphericalFogs;
}
