using System;
using UnityEngine;

namespace BasicTools
{
	public class AutoAwakeOpenOrClose : MonoBehaviour
	{
		private void Awake()
		{
			for (int i = 0; i < this.objs.Length; i++)
			{
				GameObject gameObject = this.objs[i];
				gameObject.SetActive(this.open);
			}
		}

		public bool open;

		public GameObject[] objs;
	}
}
