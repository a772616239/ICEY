using System;
using UnityEngine;

namespace I2.Loc
{
	public class Example_LocalizedString : MonoBehaviour
	{
		private void Start()
		{
			this._MyLocalizedString = "Term1";
			UnityEngine.Debug.Log(this._MyLocalizedString);
			this._MyLocalizedString = "Term2";
			string message = this._MyLocalizedString;
			UnityEngine.Debug.Log(message);
		}

		public LocalizedString _MyLocalizedString;

		public string _NormalString;

		[TermsPopup]
		public string _StringWithTermPopup;
	}
}
