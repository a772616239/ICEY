using System;
using GameWorld;

public class BattleZoneTrigger : BaseBehaviour
{
	private void Start()
	{
		EventManager.RegisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.EnterBattleModeByEnemyHurt), EventManager.ListenerQueue.Game);
		EventManager.RegisterEvent<PlayerHurtAtkEventArgs>("PlayerHurtAtk", new EventManager.FBEventHandler<PlayerHurtAtkEventArgs>(this.EnterBattleModeByPlayerHurt), EventManager.ListenerQueue.Game);
	}

	private void Update()
	{
	}

	private bool EnterBattleModeByPlayerHurt(string eventName, object sender, PlayerHurtAtkEventArgs msg)
	{
		if (R.Player.Transform.position.x < GameArea.MapRange.xMin + 3f || R.Player.Transform.position.x > GameArea.MapRange.xMax - 3f)
		{
			return true;
		}
		bool flag = false;
		if (msg.sender != null)
		{
			EnemyBullet component = msg.sender.GetComponent<EnemyBullet>();
			if (component != null && component.EnemyTypeOfShooter == EnemyType.空雷)
			{
				flag = true;
			}
		}
		if (!R.Mode.IsInBattleMode() && !flag)
		{
			R.Mode.EnterMode(Mode.AllMode.Battle);
		}
		return true;
	}

	private bool EnterBattleModeByEnemyHurt(string eventName, object sender, EnemyHurtAtkEventArgs msg)
	{
		if (!R.Mode.IsInBattleMode() && msg.hurted.GetComponent<SupplyBoxAction>() == null && msg.hurted.GetComponent<MusicPlayerAction>() == null)
		{
			R.Mode.EnterMode(Mode.AllMode.Battle);
		}
		return true;
	}
}
