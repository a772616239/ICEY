using System;
using System.Collections;
using DG.Tweening;
using GameWorld;
using I2.Loc;
using UnityEngine;

public class StoryE10P4 : BaseBehaviour
{
	private bool e10Finish
	{
		set
		{
			SaveStorage.Set("E10_Finish", value);
		}
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleOver), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleOver), EventManager.ListenerQueue.Game);
	}

	private void Update()
	{
		if (this.blackHole.gameObject.activeSelf)
		{
			Vector3 position = this.blackHole.transform.position;
			position.x = R.Camera.Transform.position.x;
			position.y = R.Camera.Transform.position.y;
			this.blackHole.transform.position = position;
		}
	}

	private bool BattleOver(string eventName, object sender, BattleEventArgs args)
	{
		if (args.Status == BattleEventArgs.BattleStatus.End)
		{
			base.StartCoroutine(this.MainCoroutine());
		}
		return true;
	}

	private IEnumerator MainCoroutine()
	{
		InputSetting.Stop(false);
		yield return new WaitForSeconds(2f);
		yield return R.Audio.PlayVoiceOver("e10t26", null, false);
		R.Camera.Controller.CameraZoom(R.Player.Transform.position + new Vector3(0f, 1f, 0f), 9f, 5f);
		YieldInstruction t27 = R.Audio.PlayVoiceOver("e10t27", null, false);
		yield return new WaitForSeconds(9f);
		yield return t27;
		R.Camera.Controller.ZoomFinished();
		yield return new WaitForSeconds(1f);
		foreach (Transform transform in this._night.GetComponentsInChildren<Transform>())
		{
			this.Fade(transform, 0f, 5f);
		}
		foreach (Transform transform2 in this._day.GetComponentsInChildren<Transform>())
		{
			this.Fade(transform2, 1f, 5f);
		}
		R.Camera.Controller.CameraZoom(base.transform.position + new Vector3(0f, 18f, 0f), 10f, -28f);
		YieldInstruction t28 = R.Audio.PlayVoiceOver("e10t28", null, false);
		this.blackHole.gameObject.SetActive(true);
		yield return new WaitForSeconds(3f);
		this.blackHole.StartDistort(8f);
		this.blackHole.StartAbsorb(10f);
		yield return new WaitForSeconds(7f);
		yield return t28;
		CameraFilterUtils.Create<CameraFilterPack_Real_VHS>(null);
		R.Audio.PlayEffect(369, new Vector3?(R.Player.Transform.position));
		R.Audio.PlayEffect(356, new Vector3?(R.Player.Transform.position));
		yield return new WaitForSeconds(0.5f);
		CameraFilterUtils.Remove<CameraFilterPack_Real_VHS>(null);
		R.Audio.StopBGM(true);
		yield return R.Ui.EndTitle.Show(false);
		R.Ui.BlackScene.Alpha = 1f;
		R.Audio.PlayEffect(376, null);
		yield return new WaitForSeconds(0.85f);
		R.Audio.PlayEffect(375, null);
		yield return new WaitForSeconds(1.6f);
		R.Audio.PlayEffect(385, null);
		yield return new WaitForSeconds(1.2f);
		R.Audio.PlayEffect(386, null);
		yield return new WaitForSeconds(2.8f);
		R.Audio.PlayEffect(384, null);
		yield return new WaitForSeconds(2.4f);
		R.Ui.BlackScene.Alpha = 0f;
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.yellow));
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e10s14, 0.1f);
		yield return new WaitForSeconds(3f);
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Ui.BlackScene.Alpha = 1f;
		this.e10Finish = true;
		InputSetting.Resume(false);
		R.Ui.BlackScene.Alpha = 0f;
		R.Trophy.AwardTrophy(12);
		yield return SaveManager.ModifySaveData(delegate(GameData gameData)
		{
			if (gameData.ThisSaveValidStorage.ContainsKey("E10_Finish"))
			{
				gameData.ThisSaveValidStorage["E10_Finish"] = 1;
			}
			else
			{
				gameData.ThisSaveValidStorage.Add("E10_Finish", 1);
			}
		});
		yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	private void Fade(Transform transform, float endValue, float duration)
	{
		SpriteRenderer component = transform.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			component.DOFade(endValue, duration);
		}
	}

	[SerializeField]
	private Transform _day;

	[SerializeField]
	private Transform _night;

	[SerializeField]
	private BlackHole blackHole;
}
