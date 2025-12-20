using System;
using UnityEngine;
using Xft;

public class demo : MonoBehaviour
{
	private void Start()
	{
		this.explode_energy.Initialize();
		this.ice_impact.Initialize();
		this.suckblood.Initialize();
		this.firestorm.Initialize();
		this.phantomsword.Initialize();
		this.cyclonestorm.Initialize();
		this.radial_energy.Initialize();
		this.lightning_storm.Initialize();
		this.glow_per_obj.Initialize();
	}

	private void Reset()
	{
		this.explode_energy.DeActive();
		this.ice_impact.DeActive();
		this.suckblood.DeActive();
		this.firestorm.DeActive();
		this.phantomsword.DeActive();
		this.cyclonestorm.DeActive();
		this.radial_energy.DeActive();
		this.lightning_storm.DeActive();
		this.glow_per_obj.DeActive();
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(60f, 0f, 500f, 30f), "Requires unity pro to get the best result.");
		if (GUI.Button(new Rect(0f, 0f, 50f, 30f), "1"))
		{
			this.explode_energy.Active();
		}
		if (GUI.Button(new Rect(0f, 30f, 50f, 30f), "2"))
		{
			this.ice_impact.Active();
		}
		if (GUI.Button(new Rect(0f, 60f, 50f, 30f), "3"))
		{
			this.suckblood.Active();
		}
		if (GUI.Button(new Rect(0f, 90f, 50f, 30f), "4"))
		{
			this.firestorm.Active();
		}
		if (GUI.Button(new Rect(0f, 120f, 50f, 30f), "5"))
		{
			this.phantomsword.Active();
		}
		if (GUI.Button(new Rect(0f, 150f, 50f, 30f), "6"))
		{
			this.cyclonestorm.Active();
		}
		if (GUI.Button(new Rect(0f, 180f, 50f, 30f), "7"))
		{
			this.radial_energy.Active();
		}
		if (GUI.Button(new Rect(0f, 210f, 50f, 30f), "8"))
		{
			this.lightning_storm.Active();
		}
		if (GUI.Button(new Rect(0f, 240f, 50f, 30f), "9"))
		{
			this.glow_per_obj.Active();
		}
		if (GUI.Button(new Rect(0f, 270f, 50f, 30f), "Reset"))
		{
			this.Reset();
		}
	}

	public XffectComponent explode_energy;

	public XffectComponent ice_impact;

	public CompositeXffect suckblood;

	public CompositeXffect firestorm;

	public XffectComponent phantomsword;

	public CompositeXffect cyclonestorm;

	public XffectComponent radial_energy;

	public CompositeXffect lightning_storm;

	public XffectComponent glow_per_obj;
}
