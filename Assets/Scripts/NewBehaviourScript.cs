using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : BaseBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.TestCoroutine());
	}

	private IEnumerator TestCoroutine()
	{
		yield return new WaitForSeconds(1f);
		string text = "自从习近平在2012年11月的中共十八届一中全会上当选总书记和军委主席，成为最高领导人后，他以反腐败为手段，在许多观察家眼中以惊人的速度巩固权力。自邓小平以来，退休的领导人在挑选新任领导人方面发挥重大影响。第四代领导人胡锦涛是1990年代邓小平首肯的。在胡锦涛掌权十年，习近平同样得到包括前总书记江泽民在内的高层领导人的支持，并在2007年与属团派的李克强一起被推荐成为政治局常委。";
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.Println(text, 0.1f);
		yield break;
	}

	private Queue<float> _queue = new Queue<float>();
}
