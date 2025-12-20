using System;
using System.Collections;
using DG.Tweening;
using GameWorld;
using I2.Loc;
using UnityEngine;

public class StoryE15P3 : BaseBehaviour
{
	private bool e15Finish
	{
		set
		{
			SaveStorage.Set("E15_Finish", value);
		}
	}

	private void Start()
	{
		R.Audio.PlayBGM(371, true);
		this.gate.openType = SceneGate.OpenType.None;
		R.Ui.HideUI(true);
	}

	private void Update()
	{
		if (this.spwanPoint.position.x - R.Player.Transform.position.x < 14f && !this.show)
		{
			this.show = true;
			base.StartCoroutine(this.P3Coroutine());
		}
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.EnemyDie), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.EnemyDie), EventManager.ListenerQueue.Game);
	}

	private bool EnemyDie(string eventName, object sender, EventArgs msg)
	{
		this.enemyDie = true;
		return true;
	}

	private IEnumerator P3Coroutine()
	{
		InputSetting.Stop(false);
		R.Player.ActionController.StartMove();
		yield return new WaitForSeconds(1f);
		R.Player.ActionController.StopMove();
		yield return new WaitForSeconds(3f);
		R.Camera.Controller.IsFollowPivot = false;
		R.Camera.Controller.CameraMoveTo(new Vector2(this.spwanPoint.position.x, R.Camera.Controller.MovableCamera.position.y), 1.5f, Ease.Linear);
		yield return new WaitForSeconds(3f);
		this.effect.SetActive(true);
		this.effect.GetComponent<Animation>().Play("Appear", PlayMode.StopAll);
		yield return new WaitForSeconds(0.2f);
		Singleton<EnemyGenerator>.Instance.GenerateEnemy(EnemyType.黄衣老者, new Vector2?(this.spwanPoint.position), false, true);
		R.Audio.PlayEffect(370, new Vector3?(this.spwanPoint.position));
		yield return new WaitForSeconds(2f);
		R.Camera.Controller.IsFollowPivot = true;
		InputSetting.Resume(false);
		while (!this.enemyDie)
		{
			yield return null;
		}
		yield return R.Audio.PlayEffect(478, null).clip.length;
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.yellow));
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e15s1, 0.1f);
		this.e15Finish = true;
		R.Trophy.AwardTrophy(18);
		yield return SaveManager.ModifySaveData(delegate(GameData gameData)
		{
			if (gameData.ThisSaveValidStorage.ContainsKey("E15_Finish"))
			{
				gameData.ThisSaveValidStorage["E15_Finish"] = 1;
			}
			else
			{
				gameData.ThisSaveValidStorage.Add("E15_Finish", 1);
			}
		});
		yield return new WaitForSeconds(3f);
		CameraFilterUtils.Remove<CameraFilterPack_EyesVision_2>(null);
		R.Ui.BlackScene.Alpha = 1f;
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return new WaitForSeconds(1f);
		R.Ui.BlackScene.Alpha = 0f;
		R.Ui.ShowUI(true);
		yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	private bool enemyDie;

	[SerializeField]
	private Transform spwanPoint;

	[SerializeField]
	private GameObject effect;

	[SerializeField]
	private SceneGate gate;

	private bool show;
}
