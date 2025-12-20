using System;
using System.Linq;
using SaveDataModel;
using UnityEngine;

namespace GameWorld
{
	public class PlayerManager
	{
		public GameObject GameObject
		{
			get
			{
				GameObject result;
				if ((result = this._gameObject) == null)
				{
					result = (this._gameObject = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault((GameObject p) => p.GetComponent<PlayerAction>() != null));
				}
				return result;
			}
			private set
			{
				this._gameObject = value;
			}
		}

		public Transform Transform
		{
			get
			{
				Transform result;
				if ((result = this._transform) == null)
				{
					result = ((!(this.GameObject != null)) ? null : (this._transform = this.GameObject.transform));
				}
				return result;
			}
		}

		public string UserName
		{
			get
			{
				return R.GameData.UserName;
			}
		}

		public string RoleName
		{
			get
			{
				return R.GameData.RoleName;
			}
			set
			{
				R.GameData.RoleName = value;
			}
		}

		public PlayerAction Action
		{
			get
			{
				PlayerAction result;
				if ((result = this._action) == null)
				{
					result = (this._action = this.GetComponent<PlayerAction>());
				}
				return result;
			}
		}

		public PlayerAbilities Abilities
		{
			get
			{
				PlayerAbilities result;
				if ((result = this._abilities) == null)
				{
					result = (this._abilities = this.GetComponent<PlayerAbilities>());
				}
				return result;
			}
		}

		public PlayerTimeController TimeController
		{
			get
			{
				PlayerTimeController result;
				if ((result = this._timeController) == null)
				{
					result = (this._timeController = this.GetComponent<PlayerTimeController>());
				}
				return result;
			}
		}

		public StateMachine StateMachine
		{
			get
			{
				StateMachine result;
				if ((result = this._stateMachine) == null)
				{
					result = (this._stateMachine = this.GetComponent<StateMachine>());
				}
				return result;
			}
		}

		public Rigidbody2D Rigidbody2D
		{
			get
			{
				Rigidbody2D result;
				if ((result = this._rigidbody2D) == null)
				{
					result = (this._rigidbody2D = this.GetComponent<Rigidbody2D>());
				}
				return result;
			}
		}

		public Enhancement Enhancement
		{
			get
			{
				return R.GameData.Enhancement;
			}
		}

		public PlayerActionController ActionController
		{
			get
			{
				PlayerActionController result;
				if ((result = this._actionController) == null)
				{
					result = (this._actionController = this.GetComponent<PlayerActionController>());
				}
				return result;
			}
		}

		public T GetComponent<T>()
		{
			return this.GameObject.GetComponent<T>();
		}

		public void SetPosition(Vector2 position)
		{
			this.Transform.position = new Vector3(position.x, position.y, this.Transform.position.z);
		}

		public void Kill()
		{
			this.Attribute.currentHP = 0;
		}

		public bool IsNearSceneGate()
		{
			for (int i = 0; i < R.SceneGate.GatesInCurrentScene.Count; i++)
			{
				SceneGate sceneGate = R.SceneGate.GatesInCurrentScene[i];
				if (MathfX.isInMiddleRange(this.Transform.position.x, sceneGate.transform.position.x, sceneGate.data.TriggerSize.x))
				{
					return true;
				}
			}
			return false;
		}

		public bool CanExecute()
		{
			return this.Action.pab.execute.CanExecute;
		}

		private GameObject _gameObject;

		private Transform _transform;

		private PlayerAction _action;

		private PlayerAbilities _abilities;

		private PlayerTimeController _timeController;

		private StateMachine _stateMachine;

		private Rigidbody2D _rigidbody2D;

		public readonly PlayerAttribute Attribute = new PlayerAttribute();

		private PlayerActionController _actionController;
	}
}
