using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

public class StoryE14P2 : BaseBehaviour
{
	private bool first
	{
		get
		{
			return RoundStorage.Get("E14_P2", true);
		}
		set
		{
			RoundStorage.Set("E14_P2", value);
		}
	}

	private bool e14p1
	{
		get
		{
			return RoundStorage.Get("E14_P1", false);
		}
	}

	private bool e14Finish
	{
		set
		{
			SaveStorage.Set("E14_Finish", value);
		}
	}

	private bool e14ShowArrow
	{
		get
		{
			return RoundStorage.Get("E14_ShowArrow", false);
		}
		set
		{
			RoundStorage.Set("E14_ShowArrow", value);
		}
	}

	private void Start()
	{
		if (this.e14p1 && this.first)
		{
			this.first = false;
			base.StartCoroutine(this.P2Coroutine());
		}
		this._arrow.SetActive(this.e14ShowArrow);
	}

	private void Update()
	{
	}

	private IEnumerator P2Coroutine()
	{
		this.gate1.openType = SceneGate.OpenType.None;
		this.gate2.openType = SceneGate.OpenType.None;
		yield return R.Audio.PlayVoiceOver("e14t9", null, false);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.red));
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e14s1, 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 3f);
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithAnim();
		this.e14Finish = true;
		R.Trophy.AwardTrophy(17);
		yield return SaveManager.ModifySaveData(delegate(GameData gameData)
		{
			if (gameData.ThisSaveValidStorage.ContainsKey("E14_Finish"))
			{
				gameData.ThisSaveValidStorage["E14_Finish"] = 1;
			}
			else
			{
				gameData.ThisSaveValidStorage.Add("E14_Finish", 1);
			}
		});
		yield return R.Audio.PlayVoiceOver("e14t10", null, false);
		this.e14ShowArrow = true;
		this._arrow.SetActive(true);
		this.gate1.openType = SceneGate.OpenType.Right;
		this.gate2.openType = SceneGate.OpenType.Left;
		yield break;
	}

	[SerializeField]
	private SceneGate gate1;

	[SerializeField]
	private SceneGate gate2;

	[SerializeField]
	private GameObject _arrow;
}
