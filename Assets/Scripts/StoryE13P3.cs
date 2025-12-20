using System;
using System.Collections;
using ExtensionMethods;
using UnityEngine;

public class StoryE13P3 : BaseBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.P3Coroutine());
	}

	private IEnumerator P3Coroutine()
	{
		this.ucey = UnityEngine.Object.Instantiate<GameObject>(this.uceyPrefab, this.uceyPos.position.SetZ(LayerManager.ZNum.MMiddleE(EnemyAttribute.RankType.Normal)), Quaternion.identity);
		AirRobotAction action = this.ucey.GetComponent<AirRobotAction>();
		action.ChangeFace(1);
		R.Player.ActionController.ChangeState(PlayerAction.StateEnum.Disappear);
		yield return R.Audio.PlayVoiceOver("e13t13", null, false);
		YieldInstruction voiceOverT14 = R.Audio.PlayVoiceOver("e13t14", null, false);
		R.Camera.Controller.CameraZoom(this.uceyPos.position + Vector3.up * 4f, 5f, 6f);
		yield return voiceOverT14;
		yield return R.Audio.PlayVoiceOver("e13t15", null, false);
		yield return R.Ui.BlackScene.FadeBlack(0.3f, false);
		R.Camera.Controller.CameraZoomFinished();
		R.Camera.Controller.Pivot = this.ucey.transform;
		yield return new WaitForSeconds(1f);
		yield return R.Ui.BlackScene.FadeTransparent(0.3f, false);
		YieldInstruction voiceOverT15 = R.Audio.PlayVoiceOver("e13t16", null, false);
		action.AutoMove();
		while (this.ucey.transform.position.x - this.gate.position.x < 0f)
		{
			action.AutoMove();
			yield return null;
		}
		yield return voiceOverT15;
		UnityEngine.Object.DontDestroyOnLoad(this.ucey);
		this.gateToS12.Enter(false);
		yield break;
	}

	private GameObject ucey;

	[SerializeField]
	private GameObject uceyPrefab;

	[SerializeField]
	private Transform _sceneSprites;

	[SerializeField]
	private Transform gate;

	[SerializeField]
	private SceneGate gateToS12;

	[SerializeField]
	private Transform uceyPos;
}
