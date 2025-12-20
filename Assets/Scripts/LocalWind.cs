using System;
using UnityEngine;

public class LocalWind : MonoBehaviour, IBlowee, ILevelItem
{
	protected virtual void Start()
	{
	}

	public virtual Vector2 GetDir(IBlowable blowable, Vector2 pos)
	{
		return Vector2.zero;
	}

	public virtual float GetPower(IBlowable blowable, Vector2 pos)
	{
		return this.power;
	}

	public virtual float GetMaxAccelSqr()
	{
		return 0f;
	}

	public virtual void ParserIntegrator(Integrator integrator)
	{
	}

	public virtual GameObject GetGO()
	{
		return base.gameObject;
	}

	public virtual bool SelfFilter()
	{
		return false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (this.SelfFilter())
		{
			return;
		}
		if (this.filteredTags.Length > 0)
		{
			foreach (string tag in this.filteredTags)
			{
				if (other.CompareTag(tag))
				{
					return;
				}
			}
		}
		Component component = other.GetComponent(typeof(IBlowable));
		if (component != null)
		{
			IBlowable blowable = (IBlowable)component;
			blowable._OnWindEnter(this);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (this.SelfFilter())
		{
			return;
		}
		if (this.filteredTags.Length > 0)
		{
			foreach (string tag in this.filteredTags)
			{
				if (other.CompareTag(tag))
				{
					return;
				}
			}
		}
		Component component = other.GetComponent(typeof(IBlowable));
		if (component != null)
		{
			IBlowable blowable = (IBlowable)component;
			blowable._OnWindExit(this);
		}
	}

	public virtual void OnReset()
	{
	}

	public virtual void OnPause(bool isPause)
	{
	}

	public virtual void OnEntertBlock(ILevelBlock block)
	{
	}

	public virtual void OnLeaveBlock(ILevelBlock block)
	{
	}

	public virtual void OnPickingTime(float time)
	{
	}

	public float power;

	public string[] filteredTags;

	public float maxHeroYSpeed;

	public float minHeroYSpeed;
}
