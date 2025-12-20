using System;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderLayerAutoSet : MonoBehaviour
{
	private bool _open = true;

	[SerializeField]
	private Transform _groundGroup;

	[SerializeField]
	private Transform _ceilingGroup;

	[SerializeField]
	private Transform _wallGroup;

	[SerializeField]
	private Transform _obstacleGroup;

	[SerializeField]
	private Transform _oneWayPlatformGroup;

	[SerializeField]
	private Transform _fxGroundGroup;
}
