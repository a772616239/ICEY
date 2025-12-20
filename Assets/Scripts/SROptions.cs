using System;
using System.ComponentModel;
using System.Diagnostics;
using FBInput;

public class SROptions
{
	public static SROptions Current
	{
		get
		{
			return SROptions._current;
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event SROptionsPropertyChanged PropertyChanged;

	public void OnPropertyChanged(string propertyName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, propertyName);
		}
	}

	[SROptions.NumberRangeAttribute(0.0, 1.0)]
	public int InputSolution
	{
		get
		{
			return InputDriver.Solution;
		}
		set
		{
			InputDriver.Solution = value;
		}
	}

	[SROptions.NumberRangeAttribute(0.0, 99.0)]
	[Category("Go Level")]
	public int C { get; set; }

	[SROptions.NumberRangeAttribute(1.0, 99.0)]
	[Category("Go Level")]
	public int L { get; set; }

	[Category("Go Level")]
	[SROptions.NumberRangeAttribute(1.0, 99.0)]
	public int S { get; set; }

	[Category("Go Level")]
	public bool t { get; set; }

	[Category("Go Level")]
	public void GoLevel()
	{
		LevelManager.LoadScene(string.Format("C{0}L{1}S{2}{3}", new object[]
		{
			this.C,
			this.L,
			this.S,
			(!this.t) ? string.Empty : "_t"
		}));
	}

	[Category("Go Level")]
	[SROptions.NumberRangeAttribute(0.0, 7.0)]
	public int palaceNum { get; set; }

	[Category("Go Level")]
	public void GoPalace()
	{
		LevelManager.LoadScene(string.Format("palace{0}", this.palaceNum));
	}

	[Category("降低渲染分辨率")]
	public bool ReduceResolution
	{
		get
		{
			return !(ReduceResolutionAndBlend.Instance == null) && ReduceResolutionAndBlend.Instance.enabled;
		}
		set
		{
			if (ReduceResolutionAndBlend.Instance == null)
			{
				ReduceResolutionAndBlend.Init();
			}
			if (ReduceResolutionAndBlendUI.Instance == null)
			{
				ReduceResolutionAndBlendUI.Init();
			}
			ReduceResolutionAndBlend.Instance.enabled = value;
			ReduceResolutionAndBlendUI.Instance.enabled = value;
		}
	}

	public bool DebugUI
	{
		get
		{
			return UIDebug.IsVisible;
		}
		set
		{
			if (UIDebug.IsVisible)
			{
				SingletonMono<UIDebug>.Instance.Close();
			}
			else
			{
				SingletonMono<UIDebug>.Instance.Open();
			}
		}
	}

	private static readonly SROptions _current = new SROptions();

	[AttributeUsage(AttributeTargets.Property)]
	public sealed class NumberRangeAttribute : Attribute
	{
		public NumberRangeAttribute(double min, double max)
		{
			this.Min = min;
			this.Max = max;
		}

		public readonly double Max;

		public readonly double Min;
	}

	[AttributeUsage(AttributeTargets.Property)]
	public sealed class IncrementAttribute : Attribute
	{
		public IncrementAttribute(double increment)
		{
			this.Increment = increment;
		}

		public readonly double Increment;
	}

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	public sealed class SortAttribute : Attribute
	{
		public SortAttribute(int priority)
		{
			this.SortPriority = priority;
		}

		public readonly int SortPriority;
	}

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	public sealed class DisplayNameAttribute : Attribute
	{
		public DisplayNameAttribute(string name)
		{
			this.Name = name;
		}

		public readonly string Name;
	}
}
