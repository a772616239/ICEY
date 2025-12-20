using System;
using UnityEngine;

namespace SceneStory
{
	public class DahalAnimTransformProxy : BaseBehaviour
	{
		private void OnEnable()
		{
		}

		private void Update()
		{
			if (this.dac == null)
			{
				this.SetBoss();
				return;
			}
			if (this.refreshPos)
			{
				this.dac.transform.position = base.transform.position;
			}
			if (this.refreshOffset)
			{
				Vector3 position = this.dac.transform.position + (this.offset - this.lastOffset);
				this.lastOffset = this.offset;
				this.dac.transform.position = position;
			}
			if (this.rigid == null)
			{
				return;
			}
			if (this.isKinematic != this.rigid.isKinematic)
			{
				this.rigid.isKinematic = this.isKinematic;
			}
		}

		public void SetBoss()
		{
			GameObject boss = R.Enemy.Boss;
			if (boss == null)
			{
				return;
			}
			this.dac = boss.GetComponent<DahalAction>();
			this.rigid = boss.GetComponent<Rigidbody2D>();
		}

		private DahalAction dac;

		private Rigidbody2D rigid;

		public bool refreshPos;

		public Vector3 offset = Vector3.zero;

		public bool refreshOffset;

		[Header("当有重力的时候要打开")]
		public bool isKinematic;

		private Vector3 lastOffset = Vector3.zero;
	}
}
