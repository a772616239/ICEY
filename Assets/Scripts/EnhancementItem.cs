using System;
using DatabaseModel;
using I2.Loc;

public class EnhancementItem
{
	public EnhancementItem(string name) : this(name, R.Player.Enhancement[name])
	{
	}

	public EnhancementItem(string name, int level)
	{
		Enhancement enhancement = Enhancement.FindByName(name);
		this.Name = name;
		this.Level = level;
		this.Cost = enhancement.GetNextLevelPrice(level);
		this.LocalizeName = ScriptLocalization.Get("ui/enhancement/" + enhancement.Name);
		this.Desc = enhancement.GetDesc(level);
		this.NextLevelDesc = enhancement.GetNextLevelDesc(level);
		this.Controlkeys = enhancement.GetControlKeys(level);
	}

	public bool Unlockable
	{
		get
		{
			bool flag = false;
			return this.Level != 3 && !flag;
		}
	}

	public bool CoinEnough
	{
		get
		{
			return R.Equipment.CoinNum >= this.Cost;
		}
	}

	public override bool Equals(object obj)
	{
		EnhancementItem enhancementItem = obj as EnhancementItem;
		return enhancementItem != null && (this.Cost == enhancementItem.Cost && this.Level == enhancementItem.Level && this.LocalizeName == enhancementItem.LocalizeName) && this.Name == enhancementItem.Name;
	}

	public override int GetHashCode()
	{
		return this.Cost.GetHashCode() + this.Level.GetHashCode() + this.LocalizeName.GetHashCode() + this.Name.GetHashCode();
	}

	public readonly int Level;

	public readonly string Name;

	public readonly int Cost;

	public readonly string LocalizeName;

	public readonly string Desc;

	public readonly string NextLevelDesc;

	public readonly string Controlkeys;
}
