using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasicTools
{
	public class SwitchObject : MonoBehaviour
	{
		private void Start()
		{
		}

		private void Update()
		{
		}

		private void OnGUI()
		{
			for (int i = 0; i < this.objs.Count; i++)
			{
				if (GUI.Button(new Rect(700f, (float)(100 * i + 100), 200f, 50f), (!this.objs[i].activeSelf) ? ("打开" + this.objs[i].name) : "关闭"))
				{
					this.objs[i].SetActive(this.objs[i].activeSelf);
				}
			}
		}

		public List<GameObject> objs;
	}
}
