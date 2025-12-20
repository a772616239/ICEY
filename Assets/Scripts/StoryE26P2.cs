using System;
using System.Collections;
using DG.Tweening;
using I2.Loc;
using UnityEngine;

public class StoryE26P2 : BaseBehaviour
{
	private bool e26finish
	{
		set
		{
			SaveStorage.Set("E26_Finish", value);
		}
	}

	private void Awake()
	{
		R.Ui.HideUI(false);
		SingletonMono<MobileInputPlayer>.Instance.Visible = false;
	}

	private void Start()
	{
		R.Player.ActionController.ChangeState(PlayerAction.StateEnum.Disappear);
		R.Camera.Controller.IsFollowPivot = false;
		R.Camera.Controller.MovableCamera.position = new Vector3(17f, 5f, -16f);
		base.StartCoroutine(this.P2Coroutine());
	}

	private void OnDestroy()
	{
		R.Player.ActionController.ChangeState(PlayerAction.StateEnum.Idle);
		R.Camera.Controller.IsFollowPivot = true;
		SingletonMono<MobileInputPlayer>.Instance.Visible = true;
		CameraFilterUtils.Remove<CameraFilterPack_FX_Glitch1>(null);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Vcr>(null);
		CameraFilterUtils.Remove<CameraFilterPack_Color_Chromatic_Aberration>(null);
		CameraFilterUtils.Remove<CameraFilterPack_TV_ARCADE_2>(null);
		WorldTime.Resume();
	}

	private IEnumerator P2Coroutine()
	{
		R.Audio.PlayBGM(487, true);
		CameraFilterUtils.Create<CameraFilterPack_FX_Glitch1>(null);
		CameraFilterUtils.Create<CameraFilterPack_TV_Vcr>(null);
		CameraFilterUtils.Create<CameraFilterPack_TV_ARCADE_2>(null);
		yield return DOTween.To(() => this._blackPoint.color, delegate(Color color)
		{
			this._blackPoint.color = color;
		}, Color.clear, 7f).WaitForCompletion();
		R.Camera.Controller.CameraZoom(new Vector3(17f, 3f, -10f), 5f, 0f);
		yield return new WaitForSeconds(6f);
		yield return R.Camera.Controller.CameraMoveTo(new Vector3(21f, 3f, -10f), 1.5f, Ease.Linear);
		yield return new WaitForSeconds(1f);
		R.Camera.Controller.CameraMoveTo(new Vector3(15f, 3f, -10f), 1.5f, Ease.Linear);
		yield return new WaitForSeconds(1.2f);
		this._faust.state.SetAnimation(0, "IdleToCall", false);
		this._faust.state.AddAnimation(0, "Call", true, 0.33f);
		yield return new WaitForSeconds(0.33f);
		YieldInstruction e26t4 = R.Audio.PlayVoiceOver("e26t4", null, true);
		this._playerNormal.gameObject.SetActive(false);
		this._playerHurt.gameObject.SetActive(true);
		this._playerHurt.state.SetAnimation(0, "CallByFaust", false);
		this._playerChange.state.SetAnimation(0, "Play", false);
		yield return new WaitForSeconds(4f);
		yield return e26t4;
		WorldTime.Pause();
		CameraFilterUtils.Create<CameraFilterPack_Color_Chromatic_Aberration>(null);
		yield return WorldTime.WaitForSecondsIgnoreTimeScale(1.5f);
		CameraFilterUtils.Remove<CameraFilterPack_Color_Chromatic_Aberration>(null);
		WorldTime.Resume();
		SingletonMono<EffectController>.Instance.Generate(215, null, R.Camera.Transform.position, default(Vector3), default(Vector3), true);
		yield return new WaitForSeconds(0.15f);
		CameraFilterUtils.Remove<CameraFilterPack_FX_Glitch1>(null);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Vcr>(null);
		CameraFilterUtils.Remove<CameraFilterPack_TV_ARCADE_2>(null);
		yield return new WaitForSeconds(1f);
		R.Audio.StopBGM(false);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.PrintlnInstantly(ScriptLocalization.Story.e26s9);
		yield return new WaitForSeconds(1f);
		YieldInstruction e26t5 = R.Audio.PlayVoiceOver("e26t5", null, false);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e26s10, 0.1f);
		yield return e26t5;
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.PrintlnInstantly(ScriptLocalization.Story.e26s11);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.CloseWithAnim();
		this.e26finish = true;
		yield return SaveManager.ModifySaveData(delegate(GameData gameData)
		{
			if (gameData.ThisSaveValidStorage.ContainsKey("E26_Finish"))
			{
				gameData.ThisSaveValidStorage["E26_Finish"] = 1;
			}
			else
			{
				gameData.ThisSaveValidStorage.Add("E26_Finish", 1);
			}
		});
		yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	[SerializeField]
	private SpriteRenderer _blackPoint;

	[SerializeField]
	private SkeletonAnimation _faust;

	[SerializeField]
	private SkeletonAnimation _playerNormal;

	[SerializeField]
	private SkeletonAnimation _playerHurt;

	[SerializeField]
	private SkeletonAnimation _playerChange;
}
