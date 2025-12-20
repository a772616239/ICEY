using System;
using ExtensionMethods;
using UnityEngine;

public class ShadowControl : BaseBehaviour
{
	private void Update()
	{
		Vector3 position = this._target.position;
		if (!this._targetGameObject.activeSelf || this.EnemyDie())
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.GetGroundHeight(position);
		this.SetShadowTransform(position);
		this.SetShadowColor(position);
	}

	public void SetTarget(Transform target)
	{
		this._target = target;
		this._targetGameObject = target.gameObject;
		BoxCollider2D component = target.GetComponent<BoxCollider2D>();
		this._eAttribute = target.GetComponent<EnemyAttribute>();
		this._isEnemy = (this._eAttribute != null);
		this._offset = ((!(component == null)) ? component.offset : Vector2.zero);
		this._size = ((!(component == null)) ? component.size : Vector2.one);
		this._shadowColor = this.sprite.color;
		this._layerValue = this.layer.value;
	}

	private void GetGroundHeight(Vector3 targetPos)
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(new Vector2(targetPos.x + this._offset.x + this._size.x / 3f, targetPos.y), Vector2.down, 100f, this._layerValue);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast(new Vector2(targetPos.x + this._offset.x - this._size.x / 3f, targetPos.y), Vector2.down, 100f, this._layerValue);
		this._noGround = (raycastHit2D.collider == null || raycastHit2D2.collider == null);
		this._groundY = Mathf.Max(raycastHit2D2.point.y, raycastHit2D.point.y);
	}

	private void SetShadowTransform(Vector3 targetPosition)
	{
		base.transform.position = new Vector3(targetPosition.x, this._groundY, targetPosition.z);
		if (this._noGround)
		{
			base.transform.localScale = new Vector3(0f, 0f, 1f);
			return;
		}
		float num = Mathf.Lerp(1f, 0f, (targetPosition.y - this._groundY) / 6f);
		base.transform.localScale = new Vector3(num, num, 1f);
	}

	private void SetShadowColor(Vector3 targetPosition)
	{
		Color color = this._shadowColor.SetAlpha(Mathf.Lerp(0.5f, 0f, (targetPosition.y - this._groundY) / 8f));
		if (Math.Abs(this._shadowColor.a - color.a) > 1.401298E-45f)
		{
			this._shadowColor = color;
			this.sprite.color = color;
		}
	}

	private bool EnemyDie()
	{
		return this._isEnemy && this._eAttribute.currentHp == 0;
	}

	private float _groundY;

	[SerializeField]
	private SpriteRenderer sprite;

	private Transform _target;

	private GameObject _targetGameObject;

	private EnemyAttribute _eAttribute;

	private bool _isEnemy;

	public LayerMask layer;

	private bool _noGround;

	private Vector2 _offset;

	private Vector2 _size;

	private Color _shadowColor;

	private int _layerValue;
}
