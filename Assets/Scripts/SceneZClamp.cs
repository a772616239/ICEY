using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("FantaBlade/Scene/场景层级编辑限制脚本")]
public class SceneZClamp : MonoBehaviour
{
	private float znum
	{
		get
		{
			switch (this.zLayer)
			{
			case LayerManager.ZNumEnum.NNear:
				return LayerManager.ZNum.NNear;
			case LayerManager.ZNumEnum.MNear:
				return LayerManager.ZNum.MNear;
			case LayerManager.ZNumEnum.NMiddle:
				return LayerManager.ZNum.NMiddle;
			case LayerManager.ZNumEnum.MMiddle_P:
				return LayerManager.ZNum.MMiddle_P;
			case LayerManager.ZNumEnum.MMiddle_E:
				return LayerManager.ZNum.MMiddleE(EnemyAttribute.RankType.Normal);
			case LayerManager.ZNumEnum.FMiddle:
				return LayerManager.ZNum.FMiddle;
			case LayerManager.ZNumEnum.NFar:
				return LayerManager.ZNum.NFar;
			case LayerManager.ZNumEnum.MFar:
				return LayerManager.ZNum.MFar;
			case LayerManager.ZNumEnum.FFar:
				return LayerManager.ZNum.FFar;
			case LayerManager.ZNumEnum.BgFar:
				return LayerManager.ZNum.BgFar;
			case LayerManager.ZNumEnum.Fx:
				return LayerManager.ZNum.Fx;
			default:
				Log.Error("ZClamp发生严重错误，选择了尚未实现代码的Layer,请仔细检查源代码");
				return LayerManager.ZNum.Fx;
			}
		}
	}

	[SerializeField]
	private LayerManager.ZNumEnum zLayer;

	[SerializeField]
	private int sortNum;

	[SerializeField]
	private bool fixedScale;

	private const float scale = 0.0001f;
}
