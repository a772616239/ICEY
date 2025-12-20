using System;
using UnityEngine;

public class LevelEditorPoint
{
	public string prefabName;

	public bool useLine;

	public LevelEditorPoint.PointType pointType;

	public Color color = new Color(0.7f, 0.3f, 0.15f);

	public LevelEditorPoint.LevelEditorLine levelLine = new LevelEditorPoint.LevelEditorLine();

	public enum PointType
	{
		Parent,
		Left,
		Right
	}

	public class LevelEditorLine
	{
		public int length = 1;

		public float angle = 90f;

		public Color color = new Color(0.7f, 0.3f, 0.15f);
	}
}
