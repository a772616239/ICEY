using System;
using UnityEngine;

[ExecuteInEditMode]
public class ZClamp : BaseBehaviour
{
	private float ZNum
	{
		get
		{
			switch (this.ZLayer)
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
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	private void LateUpdate()
	{
		Vector3 position = base.transform.position;
		position.z = this.ZNum - (float)this.SortNum * 0.0001f;
		if (base.transform.position != position)
		{
			base.transform.position = position;
		}
		if (this.FixedScale)
		{
			Vector3 localScale = base.transform.localScale;
			float z = 0.0001f / base.transform.lossyScale.z;
			localScale.z = z;
			base.transform.localScale = localScale;
		}
	}

	public LayerManager.ZNumEnum ZLayer;

	public int SortNum;

	private const float Scale = 0.0001f;

	private const float FixedScaleValue = 0.0001f;

	public bool FixedScale;
}
