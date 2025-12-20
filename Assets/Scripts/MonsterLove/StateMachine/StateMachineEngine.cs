using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CIS;
using UnityEngine;

namespace MonsterLove.StateMachine
{
	public class StateMachineEngine : MonoBehaviour
	{
		public void Initialize<T>(StateMachineBehaviour entity)
		{
			Array values = Enum.GetValues(typeof(T));
			this.stateLookup = new Dictionary<Enum, StateMapping>();
			for (int i = 0; i < values.Length; i++)
			{
				StateMapping stateMapping = new StateMapping((Enum)values.GetValue(i));
				this.stateLookup.Add(stateMapping.state, stateMapping);
			}
			MethodInfo[] methods = entity.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			char[] separator = "_".ToCharArray();
			for (int j = 0; j < methods.Length; j++)
			{
				string[] array = methods[j].Name.Split(separator);
				if (array.Length > 1)
				{
					Enum key;
					try
					{
						key = (Enum)Enum.Parse(typeof(T), array[0]);
					}
					catch (ArgumentException)
					{
						for (int k = 0; k < this.ignoredNames.Length; k++)
						{
							if (array[0] == this.ignoredNames[k])
							{
								break;
							}
						}
						goto IL_279;
					}
					StateMapping stateMapping2 = this.stateLookup[key];
					string text = array[1];
					if (text != null)
					{
						if (!(text == "Enter"))
						{
							if (!(text == "Exit"))
							{
								if (!(text == "Update"))
								{
									if (!(text == "LateUpdate"))
									{
										if (text == "FixedUpdate")
										{
											stateMapping2.FixedUpdate = this.CreateDelegate<Action>(methods[j], entity);
										}
									}
									else
									{
										stateMapping2.LateUpdate = this.CreateDelegate<Action>(methods[j], entity);
									}
								}
								else
								{
									stateMapping2.Update = this.CreateDelegate<Action>(methods[j], entity);
								}
							}
							else if (methods[j].ReturnType == typeof(IEnumerator))
							{
								stateMapping2.Exit = this.CreateDelegate<StateMapping.DoNothingCoroutine>(methods[j], entity);
							}
							else
							{
								StateMapping.DoNothing action = this.CreateDelegate<StateMapping.DoNothing>(methods[j], entity);
								stateMapping2.Exit = delegate(object userData)
								{
									action(userData);
									return null;
								};
							}
						}
						else if (methods[j].ReturnType == typeof(IEnumerator))
						{
							stateMapping2.Enter = this.CreateDelegate<StateMapping.DoNothingCoroutine>(methods[j], entity);
						}
						else
						{
							StateMapping.DoNothing action = this.CreateDelegate<StateMapping.DoNothing>(methods[j], entity);
							stateMapping2.Enter = delegate(object userData)
							{
								action(userData);
								return null;
							};
						}
					}
				}
				IL_279:;
			}
		}

		private V CreateDelegate<V>(MethodInfo method, object target) where V : class
		{
			V v = Delegate.CreateDelegate(typeof(V), target, method) as V;
			if (v == null)
			{
				throw new ArgumentException("Unabled to create delegate for method called " + method.Name);
			}
			return v;
		}

		public void ChangeState(Enum newState, object userData = null)
		{
			if (this.stateLookup == null)
			{
				throw new Exception("States have not been configured, please call initialized before trying to set state");
			}
			if (!this.stateLookup.ContainsKey(newState))
			{
				throw new Exception("No state with the name " + newState.ToString() + " can be found. Please make sure you are called the correct type the statemachine was initialized with");
			}
			if (this.debug && this.currentState != null)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"currState: ",
					this.currentState.state,
					" nextState: ",
					newState
				}));
			}
			StateMapping stateMapping = this.stateLookup[newState];
			if (this.currentState == stateMapping)
			{
				return;
			}
			this._ChangeToNewStateRoutine(stateMapping, userData);
		}

		private void _ChangeToNewStateRoutine(StateMapping newState, object userData = null)
		{
			if (this.currentState != null)
			{
				IEnumerator enumerator = this.currentState.Exit(null);
				if (enumerator != null)
				{
					base.StartCoroutine(enumerator);
				}
			}
			this.currentState = newState;
			if (this.currentState != null)
			{
				IEnumerator enumerator2 = this.currentState.Enter(userData);
				if (enumerator2 != null)
				{
					base.StartCoroutine(enumerator2);
				}
			}
		}

		private IEnumerator ChangeToNewStateRoutine(StateMapping newState)
		{
			if (this.currentState != null)
			{
				IEnumerator exitRoutine = this.currentState.Exit(null);
				if (exitRoutine != null)
				{
					yield return base.StartCoroutine(exitRoutine);
				}
			}
			this.currentState = newState;
			if (this.currentState != null)
			{
				IEnumerator enterRoutine = this.currentState.Enter(null);
				if (enterRoutine != null)
				{
					yield return base.StartCoroutine(enterRoutine);
				}
			}
			yield break;
		}

		private void FixedUpdate()
		{
			if (!this.isStart)
			{
				return;
			}
			if (SingletonMonoBehaviourClass<GameLogicMgr>.instance.IsPause())
			{
				return;
			}
			if (this.currentState != null)
			{
				this.currentState.FixedUpdate();
			}
		}

		private void Update()
		{
			if (!this.isStart)
			{
				return;
			}
			if (SingletonMonoBehaviourClass<GameLogicMgr>.instance.IsPause())
			{
				return;
			}
			if (this.currentState != null)
			{
				this.currentState.Update();
			}
		}

		private void LateUpdate()
		{
			if (!this.isStart)
			{
				return;
			}
			if (SingletonMonoBehaviourClass<GameLogicMgr>.instance.IsPause())
			{
				return;
			}
			if (this.currentState != null)
			{
				this.currentState.LateUpdate();
			}
		}

		public void PauseStateMachine(bool isPause)
		{
			this.isStart = !isPause;
		}

		public static void DoNothing()
		{
		}

		public static void DoNothingCollider(Collider other)
		{
		}

		public static void DoNothingCollision(Collision other)
		{
		}

		public static IEnumerator DoNothingCoroutine(object userData = null)
		{
			yield break;
		}

		public Enum GetState()
		{
			if (this.currentState != null)
			{
				return this.currentState.state;
			}
			return null;
		}

		public bool debug;

		private StateMapping currentState;

		private Dictionary<Enum, StateMapping> stateLookup;

		private Dictionary<string, Delegate> methodLookup;

		private readonly string[] ignoredNames = new string[]
		{
			"add",
			"remove",
			"get",
			"set"
		};

		private bool isStart = true;
	}
}
