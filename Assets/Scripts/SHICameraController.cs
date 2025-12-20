using System;
using CIS;
using DG.Tweening;
using UnityEngine;

public class SHICameraController : SingletonMonoBehaviourClass<SHICameraController>, ICameraController
{
	private Transform HeroTrans
	{
		get
		{
			return SingletonMonoBehaviourClass<GameLogicMgr>.instance.playerController.Transform;
		}
	}

	private IPlayerController playerController
	{
		get
		{
			return SingletonMonoBehaviourClass<GameLogicMgr>.instance.playerController;
		}
	}

	private void Awake()
	{
		this.ScreenWidth = (float)Screen.width;
		this.ScreenHeight = (float)Screen.height;
		this.currentX = base.transform.localPosition.x;
	}

	public GameObject GetGO()
	{
		return base.gameObject;
	}

	public void FadeInMask(float time, float endValue, Action doneCallback)
	{
		this.mask.enabled = true;
		this.mask.color = new Color(this.mask.color.r, this.mask.color.g, this.mask.color.b, 0f);
		this.mask.DOFade(endValue, time).OnComplete(delegate
		{
			if (doneCallback != null)
			{
				doneCallback();
			}
		});
	}

	public void FadeOutMask(float time, float endValue, Action doneCallback)
	{
		this.mask.DOFade(endValue, time).OnComplete(delegate
		{
			if (doneCallback != null)
			{
				doneCallback();
			}
			this.mask.enabled = false;
		});
	}

	public void ZoomTo(Vector2 destPos, float size, float duration)
	{
		float orthographicSize = this.mainCam.orthographicSize;
		float z = base.transform.position.z;
		DOTween.To(delegate(float x)
		{
			this.mainCam.orthographicSize = x;
		}, orthographicSize, size, duration);
		base.transform.DOMove(new Vector3(destPos.x, destPos.y, z), duration, false);
	}

	public void SetSize(float size)
	{
		this.mainCam.orthographicSize = size;
	}

	public void EnableFollowPlayer(bool enable)
	{
		this.follow = enable;
	}

	public void SetTargetTrans(Transform trans)
	{
		this.targetTrans = trans;
	}

	public void ShakeCamera(float duration, float strenth, float vibration)
	{
		this.shakeCamera = true;
		Transform parent = base.transform.parent;
		parent.DOShakePosition(duration, strenth, (int)vibration, 90f, false, true).OnComplete(delegate
		{
			this.shakeCamera = false;
		});
	}

	public void ReloadLimitBorder(Transform up, Transform down, Transform left, Transform right)
	{
		this.leftLimitTrans = left;
		this.rightLimitTrans = right;
		this.DownLimitTrans = down;
		this.UpLimitTrans = up;
		this.leftLimit = this.leftLimitTrans.position.x;
		this.rightLimit = this.rightLimitTrans.position.x;
		this.downLimit = this.DownLimitTrans.position.y + this.ScreenHeight / 2f;
		this.upLimit = this.UpLimitTrans.position.y - this.ScreenHeight / 2f;
	}

	public void SetCurrentPos(float x, float y)
	{
		this.currentX = x;
		this.currentY = y;
	}

	private void Update()
	{
		if (this.targetTrans != null)
		{
			base.transform.position = new Vector3(this.targetTrans.position.x, this.targetTrans.position.y, base.transform.position.z);
			return;
		}
		if (!this.follow)
		{
			return;
		}
		Vector3 position = this.HeroTrans.position;
		float x = position.x;
		float y = position.y;
		float num = (this.playerController == null) ? 0f : this.playerController.Velocity.x;
		float num2 = num * this.xOffsetSeconds + this.offset.x;
		float y2 = this.offset.y;
		if (Mathf.Abs(this.lastVelocityX - num) > 1f)
		{
			this.cameraMultiplier = 1.5f;
		}
		this.lastVelocityX = num;
		this.currentX += (x + num2 - this.currentX) * SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime * this.cameraMultiplier;
		this.currentY = y + y2;
		this.currentX = Mathf.Clamp(this.currentX, this.leftLimit + this.ScreenWidth / 2f, this.rightLimit - this.ScreenWidth / 2f);
		this.currentY = Mathf.Clamp(this.currentY, this.downLimit, this.upLimit);
		base.transform.localPosition = new Vector3(this.currentX, this.currentY, base.transform.localPosition.z);
		if (Mathf.Abs(x + num2 - this.currentX) > 1f)
		{
			this.cameraMultiplier += SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		}
	}

	public Transform leftLimitTrans;

	public Transform rightLimitTrans;

	public Transform DownLimitTrans;

	public Transform UpLimitTrans;

	public Camera mainCam;

	public Vector2 offset;

	public SpriteRenderer mask;

	public SpriteRenderer overlayMask;

	public float leftLimit;

	public float rightLimit;

	public float xOffsetSeconds;

	private float cameraMultiplier;

	private float ScreenWidth;

	private float ScreenHeight;

	public float downLimit;

	public float upLimit;

	private Transform targetTrans;

	private bool follow = true;

	private bool shakeCamera;

	private float currentX;

	private float currentY;

	private float lastVelocityX;

	private float lastX;

	private float lastY;
}
