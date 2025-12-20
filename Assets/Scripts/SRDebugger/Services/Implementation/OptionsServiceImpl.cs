using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using SRDebugger.Internal;
using SRF.Service;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(IOptionsService))]
	public class OptionsServiceImpl : IOptionsService
	{
		public OptionsServiceImpl()
		{
			this._optionsReadonly = new ReadOnlyCollection<OptionDefinition>(this._options);
			this.Scan(SROptions.Current);
			SROptions.Current.PropertyChanged += this.OnSROptionsPropertyChanged;
		}

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler OptionsUpdated;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<PropertyChangedEventArgs> OptionsValueUpdated;

		public ICollection<OptionDefinition> Options
		{
			get
			{
				return this._optionsReadonly;
			}
		}

		public void Scan(object obj)
		{
			this.AddContainer(obj);
		}

		public void AddContainer(object obj)
		{
			if (this._optionContainerLookup.ContainsKey(obj))
			{
				throw new Exception("An object should only be added once.");
			}
			ICollection<OptionDefinition> collection = SRDebuggerUtil.ScanForOptions(obj);
			this._optionContainerLookup.Add(obj, collection);
			if (collection.Count > 0)
			{
				this._options.AddRange(collection);
				this.OnOptionsUpdated();
				INotifyPropertyChanged notifyPropertyChanged = obj as INotifyPropertyChanged;
				if (notifyPropertyChanged != null)
				{
					notifyPropertyChanged.PropertyChanged += this.OnPropertyChanged;
				}
			}
		}

		public void RemoveContainer(object obj)
		{
			if (!this._optionContainerLookup.ContainsKey(obj))
			{
				return;
			}
			ICollection<OptionDefinition> collection = this._optionContainerLookup[obj];
			this._optionContainerLookup.Remove(obj);
			foreach (OptionDefinition item in collection)
			{
				this._options.Remove(item);
			}
			INotifyPropertyChanged notifyPropertyChanged = obj as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged -= this.OnPropertyChanged;
			}
			this.OnOptionsUpdated();
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (this.OptionsValueUpdated != null)
			{
				this.OptionsValueUpdated(this, propertyChangedEventArgs);
			}
		}

		private void OnSROptionsPropertyChanged(object sender, string propertyName)
		{
			this.OnPropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
		}

		private void OnOptionsUpdated()
		{
			if (this.OptionsUpdated != null)
			{
				this.OptionsUpdated(this, EventArgs.Empty);
			}
		}

		private readonly Dictionary<object, ICollection<OptionDefinition>> _optionContainerLookup = new Dictionary<object, ICollection<OptionDefinition>>();

		private readonly List<OptionDefinition> _options = new List<OptionDefinition>();

		private readonly IList<OptionDefinition> _optionsReadonly;
	}
}
