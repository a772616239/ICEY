using System;

namespace SRF
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
	public sealed class RequiredFieldAttribute : Attribute
	{
		public RequiredFieldAttribute(bool autoSearch)
		{
			this.AutoSearch = autoSearch;
		}

		public RequiredFieldAttribute()
		{
		}

		public bool AutoSearch
		{
			get
			{
				return this._autoSearch;
			}
			set
			{
				this._autoSearch = value;
			}
		}

		public bool AutoCreate
		{
			get
			{
				return this._autoCreate;
			}
			set
			{
				this._autoCreate = value;
			}
		}

		[Obsolete]
		public bool EditorOnly
		{
			get
			{
				return this._editorOnly;
			}
			set
			{
				this._editorOnly = value;
			}
		}

		private bool _autoCreate;

		private bool _autoSearch;

		private bool _editorOnly = true;
	}
}
