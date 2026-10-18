using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using ExtensionMethods;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Camera))]
public class CameraController : SingletonMono<CameraController>
{
	private void Awake()
	{
		this._camera = base.GetComponent<Camera>();
		this._blur = base.GetComponent<CameraMotionBlur>();
		this._globalBloom = base.GetComponent<Bloom>();
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	private void LateUpdate()
	{
		if (this.IsFollowPivot && this.Pivot != null)
		{
			this.UpdateCamera(Time.deltaTime);
		}
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		this.ManualOffsetY = 0f;
		this._fieldOfView = 52f;
		this.MovableCamera.position = this.MovableCamera.position.SetZ(-10f);
	}

	public Transform Pivot
	{
		get
		{
			if (this._pivot == null)
			{
				return R.Player.Transform;
			}
			return this._pivot;
		}
		set
		{
			this._pivot = value;
		}
	}

	public Transform MovableCamera
	{
		get
		{
			return base.transform.parent;
		}
	}

	public float FieldOfView
	{
		get
		{
			return this._fieldOfView;
		}
		set
		{
			this._fieldOfView = Mathf.Clamp(value, 0f, 179f);
		}
	}

	private float Fv2DeltaZ()
	{
		float num = Mathf.Abs(-10f);
		return num * Mathf.Tan(0.4537856f) / Mathf.Tan(this.FieldOfView / 2f * 0.0174532924f) - num;
	}

	private float MaxVisibleZ
	{
		get
		{
			return -12f + this.Fv2DeltaZ();
		}
	}

	private float MaxDetectZ
	{
		get
		{
			return this.MaxVisibleZ - -2f + this.Fv2DeltaZ();
		}
	}

	private float MinVisibleZ
	{
		get
		{
			return -8f + this.Fv2DeltaZ();
		}
	}

	private float MinDetectZ
	{
		get
		{
			return this.MinVisibleZ - -2f + this.Fv2DeltaZ();
		}
	}

	private Rect MaxVisibleRect
	{
		get
		{
			return this.CameraRect(this.MaxVisibleZ);
		}
	}

	private Rect MaxDetectRect
	{
		get
		{
			return this.CameraRect(this.MaxDetectZ);
		}
	}

	private Rect MinVisibleRect
	{
		get
		{
			return this.CameraRect(this.MinVisibleZ);
		}
	}

	private Rect MinDetectRect
	{
		get
		{
			return this.CameraRect(this.MinDetectZ);
		}
	}

	public YieldInstruction CameraMoveTo(Vector3 pos, float second, Ease ease = Ease.Linear)
	{
		if (!this._isLock)
		{
			this.IsFollowPivot = false;
			this._isLock = true;
			pos.z = this.MovableCamera.position.z;
			this.KillTweening();
			return this.MovableCamera.DOMove(this.CameraPositionClamp(pos), second, false).SetEase(ease).OnComplete(delegate
			{
				this.CamereMoveFinished(false);
			}).WaitForCompletion();
		}
		return null;
	}

	public void CameraMoveToBySpeed(Vector3 pos, float speed, bool canReturn = false, Ease type = Ease.Linear)
	{
		if (!this._isLock)
		{
			this.IsFollowPivot = false;
			this._isLock = true;
			pos.z = this.MovableCamera.position.z;
			if (canReturn)
			{
				Vector3[] path = new Vector3[]
				{
					this.CameraPositionClamp(pos),
					this.MovableCamera.position
				};
				this.MovableCamera.DOPath(path, speed, PathType.Linear, PathMode.Full3D, 10, null).SetSpeedBased(true).SetEase(type).OnComplete(delegate
				{
					this.CamereMoveFinished(true);
				});
			}
			else
			{
				this.MovableCamera.DOMove(this.CameraPositionClamp(pos), speed, false).SetSpeedBased(true).SetEase(type).OnComplete(delegate
				{
					this.CamereMoveFinished(false);
				});
			}
		}
	}

	private void CamereMoveFinished(bool follow)
	{
		this._isLock = false;
		this.IsFollowPivot = follow;
	}

	public void CameraZoom(Vector3 pos, float second, float deltaZ = 3f)
	{
		if (!this._isLock)
		{
			this._isLock = true;
			this.IsFollowPivot = false;
			float num = (1f - Mathf.Tan(0.4537856f) / Mathf.Tan(0.0174532924f * this.FieldOfView / 2f)) * 7f;
			pos.z = -10f + deltaZ + num;
			this.MovableCamera.DOMove(this.CameraPositionClamp(pos), second, false).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.ZoomFinished));
		}
	}

	public void ZoomFinished()
	{
		this._isLock = false;
	}

	public void CameraZoomFinished()
	{
		this._pivot = R.Player.Transform;
		this.IsFollowPivot = true;
	}

	public void CameraShake(float second, float strength = 0.2f, CameraController.ShakeTypeEnum type = CameraController.ShakeTypeEnum.Rect, bool isLoop = false)
	{
		if (Math.Abs(second) < 0.01f)
		{
			return;
		}
		Vector3 strength2 = new Vector3(1f, 1f, 0f) * strength;
		if (type == CameraController.ShakeTypeEnum.Vertical)
		{
			strength2.x = 0f;
		}
		if (type == CameraController.ShakeTypeEnum.Horizon)
		{
			strength2.y = 0f;
		}
		this.KillTweening();
		base.transform.DOShakePosition(second, strength2, 100, 90f, false, true).SetLoops((!isLoop) ? 1 : -1).OnKill(new TweenCallback(this.OnShakeFinished)).OnComplete(new TweenCallback(this.OnShakeFinished));
	}

	private void OnShakeFinished()
	{
		base.transform.localPosition = Vector3.zero;
	}

	public void KillTweening()
	{
		base.transform.DOKill(false);
	}

	public void OpenMotionBlur(float second, float scale, Vector3 pos)
	{
	}

	private IEnumerator CameraMotionBlur(float second, float scale, Vector3 pos)
	{
		float startTime = Time.time;
		float calTime = 0f;
		this._blur.preview = true;
		do
		{
			this._blur.velocityScale = scale * calTime / second;
			Vector2 camPos = this._camera.WorldToViewportPoint(pos);
			Vector2 blurPos = camPos * -2f + new Vector2(1f, 1f / this._camera.aspect);
			Vector3 realBlurScale = blurPos;
			realBlurScale.z = 1f;
			realBlurScale *= 13f;
			this._blur.previewScale = realBlurScale;
			calTime += Time.deltaTime;
			yield return null;
		}
		while (Time.time - startTime < second);
		this._blur.enabled = false;
		this._isMotionBlur = false;
		yield break;
	}

	public void CloseMotionBlur()
	{
	}

	public void EnableGlobalBloom()
	{
		this._globalBloom.enabled = true;
	}

	public void DisableGlobalBloom()
	{
		this._globalBloom.enabled = false;
	}

	public void CameraBloom(float recoveryTime, float waitingTime)
	{
		if (this._bloom == null)
		{
			base.StartCoroutine(this.BloomCoroutine(recoveryTime, waitingTime));
		}
	}

	private IEnumerator BloomCoroutine(float recoveryTime, float waitingTime)
	{
		this._bloom = R.Camera.AddComponent<BloomOptimized>();
		this._bloom.fastBloomShader = (this._bloom.fastBloomShader ?? Shader.Find("Hidden/FastBloom"));
		if (!this._bloom.enabled)
		{
			this._bloom.enabled = true;
		}
		this._bloom.intensity = 2.5f;
		this._bloom.threshold = 0.3f;
		yield return new WaitForSeconds(waitingTime);
		float calTime = 0f;
		float startTime = Time.time;
		while (Time.time - startTime < recoveryTime)
		{
			this._bloom.intensity = Mathf.Lerp(2.5f, 0.38f, Mathf.Clamp(calTime, 0f, recoveryTime) / recoveryTime);
			this._bloom.threshold = Mathf.Lerp(0.3f, 0.4f, Mathf.Clamp(calTime, 0f, recoveryTime) / recoveryTime);
			yield return null;
			calTime += Time.deltaTime;
		}
		this._bloom.enabled = false;
		UnityEngine.Object.Destroy(this._bloom);
		this._bloom = null;
		yield break;
	}

	private Vector3 CameraPositionClamp(Vector3 pos)
	{
		float num = -pos.z * Mathf.Tan(0.4537856f);
		Vector2 vector = new Vector2(num * this._camera.aspect, num);
		Rect cameraRange = GameArea.CameraRange;
		float num2 = cameraRange.width / (-2f * Mathf.Tan(this.FieldOfView / 2f) * this._camera.aspect);
		cameraRange.xMin += vector.x;
		cameraRange.xMax -= vector.x;
		cameraRange.yMin += vector.y;
		cameraRange.yMax -= vector.y;
		Vector3 result;
		result.x = ((cameraRange.xMin >= cameraRange.xMax) ? cameraRange.center.x : Mathf.Clamp(pos.x, cameraRange.xMin, cameraRange.xMax));
		result.y = ((cameraRange.yMin >= cameraRange.yMax) ? cameraRange.center.y : Mathf.Clamp(pos.y, cameraRange.yMin, cameraRange.yMax));
		result.z = pos.z;
		return result;
	}

	private Vector3 CalculateFollowCameraPos()
	{
		if (this.Pivot == null)
		{
			return Vector3.zero;
		}
		Vector3 position = this.Pivot.position;
		Vector3? farestEnemyPosition = R.Enemy.GetFarestEnemyPosition(position, new Rect?(this.MaxDetectRect));
		Vector3 vector;
		if (farestEnemyPosition == null)
		{
			vector = position;
			vector.z = -10f;
		}
		else
		{
			vector = (position + farestEnemyPosition.Value) / 2f;
			float num;
			if (Mathf.Abs(((Vector2)position - (Vector2)farestEnemyPosition.Value).Slope()) > new Vector2(16f, 9f).Slope())
			{
				num = Mathf.Abs(position.y - farestEnemyPosition.Value.y);
			}
			else
			{
				num = Mathf.Abs(position.x - farestEnemyPosition.Value.x) * 9f / 16f;
			}
			vector.z = -(num / 2f) / Mathf.Tan(0.4537856f);
			vector.z += -6f;
			vector.z = Mathf.Clamp(vector.z, this.MaxVisibleZ, this.MinVisibleZ);
		}
		float distance = Physics2D.Raycast(vector, Vector2.down, 10f, LayerManager.GroundMask).distance;
		float num2 = (this.ManualOffsetY + 2.4f) * (this.MovableCamera.position.z / -10f);
		vector += Vector3.up * ((distance <= num2) ? (num2 - distance) : 0f);
		vector.z += this.Fv2DeltaZ();
		return vector;
	}

	private Rect CameraRect(float z)
	{
		Vector2 vector;
		vector.y = -z * Mathf.Tan(0.4537856f);
		vector.x = vector.y * this._camera.aspect;
		return new Rect
		{
			min = new Vector2(this.MovableCamera.position.x - vector.x, this.MovableCamera.position.y - vector.y),
			max = new Vector2(this.MovableCamera.position.x + vector.x, this.MovableCamera.position.y + vector.y)
		};
	}

	public void CameraResetPostionAfterSwitchScene()
	{
		this.IsFollowPivot = true;
		Vector3 vector = this.CameraPositionClamp(this.Pivot.position);
		this.MovableCamera.position = new Vector3(vector.x, vector.y, this.MovableCamera.position.z);
		this.UpdateCamera(1000f);
	}

	private void UpdateCamera(float deltaTime)
	{
		Vector3 pos = Vector3.SmoothDamp(this.MovableCamera.position, this.CalculateFollowCameraPos(), ref this._currentSpeed, this._smoothTime, float.PositiveInfinity, deltaTime);
		if (!this._isLock)
		{
			this.MovableCamera.position = this.CameraPositionClamp(pos);
		}
	}

	private const float NormalPreviewScale = 13f;

	public const float CameraDefaultZ = -10f;

	private const float DeltaZ = -2f;

	private const float DefaultFv = 52f;

	private Transform _pivot;

	public bool IsFollowPivot = true;

	private Camera _camera;

	private bool _isLock;

	private bool _isMotionBlur;

	private CameraMotionBlur _blur;

	[SerializeField]
	private readonly float _smoothTime = 0.4f;

	public float ManualOffsetY;

	private float _fieldOfView = 52f;

	private Bloom _globalBloom;

	private BloomOptimized _bloom;

	private Vector3 _currentSpeed = Vector3.zero;

	public enum ShakeTypeEnum
	{
		Vertical,
		Horizon,
		Rect
	}
}
