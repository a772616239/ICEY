using System;
using System.Collections;
using ExtensionMethods;
using I2.Loc;
using UnityEngine;

public class StoryE13P4 : BaseBehaviour
{
	private bool e13Finish
	{
		set
		{
			SaveStorage.Set("E13_Finish", value);
		}
	}

	private void Awake()
	{
		R.Camera.Controller.CameraResetPostionAfterSwitchScene();
		this.ucey = GameObject.FindGameObjectWithTag("Enemy");
		this.ucey.transform.position = this.uceyPos.position.SetZ(LayerManager.ZNum.MMiddleE(EnemyAttribute.RankType.Normal));
	}

	private void Start()
	{
		base.StartCoroutine(this.P4Coroutine());
	}

	private IEnumerator P4Coroutine()
	{
		this.ucey.GetComponent<AirRobotAction>().StopMoveToIdle();
		this.dao = Singleton<EnemyGenerator>.Instance.GenerateEnemy(EnemyType.斩轮式一型, new Vector2?(this.daoPos.position), false, true);
		this.pao = Singleton<EnemyGenerator>.Instance.GenerateEnemy(EnemyType.炮击式一型, new Vector2?(this.paoPos.position), false, true);
		this.dao.GetComponent<DaoAction>().ChangeFace(1);
		base.StartCoroutine(this.DaoMoe());
		base.StartCoroutine(this.PaoMoe());
		base.StartCoroutine(this.UceyLook());
		yield return R.Audio.PlayVoiceOver("e13t17", null, false);
		YieldInstruction voiceOver = R.Audio.PlayVoiceOver("e13t18", null, false);
		R.Camera.Controller.CameraZoom(R.Camera.Controller.MovableCamera.position, 5f, -5f);
		yield return new WaitForSeconds(5f);
		yield return voiceOver;
		this.dao.GetComponent<EnemyAttribute>().currentHp = 1;
		this.pao.GetComponent<EnemyAttribute>().currentHp = 1;
		this.ucey.GetComponent<EnemyAttribute>().currentHp = 1;
		R.Player.ActionController.ChangeState(PlayerAction.StateEnum.Idle);
		R.Player.ActionController.TurnRound(1);
		R.Player.ActionController.StartMove();
		while (this.playerStop.position.x - R.Player.Transform.position.x > 0f)
		{
			yield return null;
		}
		R.Player.ActionController.StopMove();
		R.Camera.Controller.Pivot = R.Player.Transform;
		yield return new WaitForSeconds(2f);
		R.Player.Transform.GetComponent<Claymore>().StartCharge(false);
		yield return new WaitForSeconds(4f);
		R.Player.Transform.GetComponent<Claymore>().ReleaseCharge(false);
		yield return new WaitForSeconds(3f);
		R.Ui.BlackScene.Alpha = 1f;
		R.Audio.StopBGM(true);
		R.Camera.Controller.CameraZoom(R.Camera.Controller.MovableCamera.position, 5f, 5f);
		yield return new WaitForSeconds(1f);
		yield return R.Audio.PlayVoiceOver("e13t19", null, false);
		yield return new WaitForSeconds(5f);
		R.Ui.BlackScene.Alpha = 0f;
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e13s2, 0.1f);
		this.e13Finish = true;
		R.Trophy.AwardTrophy(16);
		yield return SaveManager.ModifySaveData(delegate(GameData gameData)
		{
			if (gameData.ThisSaveValidStorage.ContainsKey("E13_Finish"))
			{
				gameData.ThisSaveValidStorage["E13_Finish"] = 1;
			}
			else
			{
				gameData.ThisSaveValidStorage.Add("E13_Finish", 1);
			}
		});
		yield return new WaitForSeconds(3f);
		yield return R.Ui.Terminal.CloseWithAnim();
		InputSetting.Resume(false);
		yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	private IEnumerator DaoMoe()
	{
		while (this.dao != null && !this.dao.GetComponent<EnemyAttribute>().isDead)
		{
			int random = UnityEngine.Random.Range(0, 3);
			if (random != 0)
			{
				if (random == 1)
				{
					this.dao.GetComponent<DaoAction>().Idle3();
				}
			}
			else
			{
				this.dao.GetComponent<DaoAction>().Idle2();
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 4f));
		}
		yield break;
	}

	private IEnumerator PaoMoe()
	{
		while (this.pao != null && !this.pao.GetComponent<EnemyAttribute>().isDead)
		{
			int random = UnityEngine.Random.Range(0, 3);
			if (random != 0)
			{
				if (random == 1)
				{
					this.pao.GetComponent<DaoAction>().Idle3();
				}
			}
			else
			{
				this.pao.GetComponent<DaoAction>().Idle2();
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 4f));
		}
		yield break;
	}

	private IEnumerator UceyLook()
	{
		while (this.ucey != null && !this.ucey.GetComponent<EnemyAttribute>().isDead)
		{
			int random = UnityEngine.Random.Range(0, 2);
			if (random != 0)
			{
				if (random == 1)
				{
					this.ucey.GetComponent<AirRobotAction>().TurnRound(1);
				}
			}
			else
			{
				this.ucey.GetComponent<AirRobotAction>().TurnRound(-1);
			}
			yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 4f));
		}
		yield break;
	}

	private GameObject ucey;

	private GameObject dao;

	private GameObject pao;

	[SerializeField]
	private Transform uceyPos;

	[SerializeField]
	private Transform daoPos;

	[SerializeField]
	private Transform paoPos;

	[SerializeField]
	private Transform playerStop;

	[SerializeField]
	private Transform _sceneSprites;
}
