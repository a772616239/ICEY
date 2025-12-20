using System;
using UnityEngine;

public class CameraFilterDebugGUI : DebugGuiBase
{
	private static GameObject Cam
	{
		get
		{
			return Camera.main.gameObject;
		}
	}

	private void Change_Filters()
	{
		if (this._filterCount > this._filterTypes.Length - 1)
		{
			this._filterCount = 1;
		}
		if (this._filterCount < 0)
		{
			this._filterCount = this._filterTypes.Length - 1;
		}
		if (this._filterType != null && CameraFilterDebugGUI.Cam.GetComponent(this._filterType))
		{
			UnityEngine.Object.Destroy(CameraFilterDebugGUI.Cam.GetComponent(this._filterType));
		}
		this._filterType = this._filterTypes[this._filterCount];
		CameraFilterDebugGUI.Cam.AddComponent(this._filterType);
		this._text = this._filterCount + ". " + this._filterType;
	}

	private void Update()
	{
		if (!this.InVisible)
		{
			return;
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this._filterCount--;
			this.Change_Filters();
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
		{
			this._filterCount++;
			this.Change_Filters();
		}
	}

	protected override string UIDebugButtonName
	{
		get
		{
			return "滤镜";
		}
	}

	public override void OnDebugGUI()
	{
		Vector2 vector = new Vector2((float)Screen.width / 1280f, (float)Screen.height / 720f);
		Vector4 vector2 = new Vector4(280f, 0f, 720f, 60f);
		GUI.Button(new Rect(vector2.x * vector.x, vector2.y * vector.y, vector2.z * vector.x, vector2.w * vector.y), this._text);
		vector2 = new Vector4(480f, 660f, 320f, 60f);
		if (this._filterType != null && CameraFilterDebugGUI.Cam.GetComponent(this._filterType))
		{
			if (GUI.Button(new Rect(vector2.x * vector.x, vector2.y * vector.y, vector2.z * vector.x, vector2.w * vector.y), " Stop Filter"))
			{
				UnityEngine.Object.Destroy(CameraFilterDebugGUI.Cam.GetComponent(this._filterType));
				this._text = "<---- NO FILTER : PRESS BUTTON HERE --->";
			}
		}
		else if (GUI.Button(new Rect(vector2.x * vector.x, vector2.y * vector.y, vector2.z * vector.x, vector2.w * vector.y), " Add Filter"))
		{
			this.Change_Filters();
		}
		vector2 = new Vector4(0f, 0f, 280f, 60f);
		if (GUI.Button(new Rect(vector2.x * vector.x, vector2.y * vector.y, vector2.z * vector.x, vector2.w * vector.y), " Preview "))
		{
			this._filterCount--;
			this.Change_Filters();
		}
		vector2 = new Vector4(1000f, 0f, 280f, 60f);
		if (GUI.Button(new Rect(vector2.x * vector.x, vector2.y * vector.y, vector2.z * vector.x, vector2.w * vector.y), " Next "))
		{
			this._filterCount++;
			this.Change_Filters();
		}
	}

	protected override void GUIStart()
	{
		this._text = "<---- NO FILTER : PRESS BUTTON HERE --->";
	}

	private string _text;

	private int _filterCount = -1;

	private Type _filterType;

	private readonly Type[] _filterTypes = CameraFilterUtils.FilterTypes;
}
