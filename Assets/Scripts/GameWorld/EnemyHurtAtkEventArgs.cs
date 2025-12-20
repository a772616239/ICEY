using System;
using LitJson;
using UnityEngine;

namespace GameWorld
{
	public class EnemyHurtAtkEventArgs : EventArgs
	{
		public EnemyHurtAtkEventArgs(GameObject _hurted, GameObject _sender, int _attackId, Vector3 _hurtPos, HurtCheck.BodyType _body, EnemyHurtAtkEventArgs.PlayerNormalAtkData _attackData, bool _forceHurt = false)
		{
			this.hurted = _hurted;
			this.sender = _sender;
			this.attackId = _attackId;
			this.hurtPos = _hurtPos;
			this.body = _body;
			this.attackData = _attackData;
			this.forceHurt = _forceHurt;
			this.hurtType = EnemyHurtAtkEventArgs.HurtTypeEnum.Normal;
		}

		public EnemyHurtAtkEventArgs(GameObject _hurted, EnemyHurtAtkEventArgs.HurtTypeEnum type)
		{
			this.hurted = _hurted;
			this.hurtType = type;
		}

		public EnemyHurtAtkEventArgs(GameObject _hurted, EnemyHurtAtkEventArgs.HurtTypeEnum type, string playerState)
		{
			this.hurted = _hurted;
			this.hurtType = type;
			this.attackData = new EnemyHurtAtkEventArgs.PlayerNormalAtkData(playerState);
		}

		public EnemyHurtAtkEventArgs.PlayerNormalAtkData attackData;

		public int attackId;

		public HurtCheck.BodyType body;

		public bool forceHurt;

		public GameObject hurted;

		public Vector3 hurtPos;

		public EnemyHurtAtkEventArgs.HurtTypeEnum hurtType;

		public GameObject sender;

		public enum HurtTypeEnum
		{
			Normal,
			ExecuteFollow,
			Execute,
			QTEHurt,
			Flash
		}

		public class PlayerNormalAtkData : EventArgs
		{
			public PlayerNormalAtkData(JsonData atkData, bool _firstHurt)
			{
				this.damagePercent = atkData.Get<float>("damagePercent", 1f);
				this.atkName = atkData.Get<string>("atkName", "Atk1");
				this.camShakeFrame = atkData.Get<int>("shakeClip", 0);
				this.shakeStrength = atkData.Get<float>("shakeOffset", 0f);
				this.shakeType = atkData.Get<int>("shakeType", 0);
				this.frozenFrame = atkData.Get<int>("frozenClip", 0);
				this.shakeFrame = atkData.Get<int>("frameShakeClip", 0);
				this.joystickShakeNum = atkData.Get<int>("joystickShakeNum", -1);
				this.firstHurt = _firstHurt;
			}

			public PlayerNormalAtkData(string _atkName)
			{
				this.atkName = _atkName;
			}

			public float damagePercent = 1f;

			public string atkName;

			public bool firstHurt;

			public int camShakeFrame;

			public float shakeStrength = 1f;

			public int shakeType;

			public int frozenFrame;

			public int shakeFrame;

			public int joystickShakeNum;
		}
	}
}
