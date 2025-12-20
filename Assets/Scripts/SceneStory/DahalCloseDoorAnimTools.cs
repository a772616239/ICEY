using System;

namespace SceneStory
{
	public class DahalCloseDoorAnimTools : BaseBehaviour
	{
		public void SetSelfActiveFalse()
		{
			base.gameObject.SetActive(false);
		}

		public void CloseDoor()
		{
			this.gateL.Appear();
			this.gateR.Appear();
			this.CloseArea();
		}

		private void CloseArea()
		{
			GameArea.EnemyRange.xMin = this.gateL.gameObject.transform.Find("PlayerLimitPosLX").transform.position.x;
			GameArea.EnemyRange.xMax = this.gateR.gameObject.transform.Find("PlayerLimitPosRX").transform.position.x;
			GameArea.PlayerRange.xMin = this.gateL.gameObject.transform.Find("PlayerLimitPosLX").transform.position.x;
			GameArea.PlayerRange.xMax = this.gateR.gameObject.transform.Find("PlayerLimitPosRX").transform.position.x;
			GameArea.CameraRange.xMin = this.gateL.gameObject.transform.Find("CameraLimitPosLX").transform.position.x;
			GameArea.CameraRange.xMax = this.gateR.gameObject.transform.Find("CameraLimitPosRX").transform.position.x;
		}

		public BattleZoneGate gateL;

		public BattleZoneGate gateR;
	}
}
