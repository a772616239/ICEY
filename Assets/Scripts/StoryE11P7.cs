using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE11P7 : BaseBehaviour
{
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !this._hasEntered)
		{
			this._hasEntered = true;
			this._wall.SetActive(true);
			R.Ui.Toast.Show(ScriptLocalization.Story.e11s4, 5f, true);
			R.Audio.PlayEffect(387, null);
			base.StartCoroutine(this.Sequence0());
		}
	}

	private IEnumerator Sequence0()
	{
		yield return R.Audio.PlayVoiceOver("e11t22", null, false);
		yield return new WaitForSeconds(2f);
		R.Audio.StopBGM(true);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e11s5, 0.1f);
		yield return new WaitForSeconds(3f);
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Mode.ExitMode(Mode.AllMode.Shi);
		R.Ui.ShowUI(false);
		R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	[SerializeField]
	private GameObject _wall;

	private bool _hasEntered;
}
