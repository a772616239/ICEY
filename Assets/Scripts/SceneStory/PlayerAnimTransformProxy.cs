using System;
using UnityEngine;

namespace SceneStory
{
	public class PlayerAnimTransformProxy : BaseBehaviour
	{
		private void OnEnable()
		{
			GameObject gameObject = R.Player.GameObject;
			if (gameObject == null)
			{
				return;
			}
			this.pac = gameObject.GetComponent<PlayerAction>();
		}

		public void PlayerHasGravity(BoolEnum open)
		{
			if (open == BoolEnum.True)
			{
				this.pac.GetComponent<Rigidbody2D>().gravityScale = 1f;
			}
			if (open == BoolEnum.False)
			{
				this.pac.GetComponent<Rigidbody2D>().gravityScale = 0f;
			}
		}

		private void Update()
		{
			if (this.pac == null)
			{
				return;
			}
			if (this.refreshPos)
			{
				Vector3 position = base.transform.position;
				position.z = this.pac.transform.position.z;
				this.pac.transform.position = position;
			}
			if (this.refreshOffset)
			{
				Vector3 position2 = this.pac.transform.position + (this.lastOffset - this.offset);
				position2.z = this.pac.transform.position.z;
				this.lastOffset = this.offset;
				this.pac.transform.position = position2;
			}
		}

		private PlayerAction pac;

		[Header("玩家设置只会更新2D属性")]
		public bool refreshPos;

		public Vector3 offset = Vector3.zero;

		public bool refreshOffset;

		private Vector3 lastOffset = Vector3.zero;
	}
}
