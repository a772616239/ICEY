using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	public class CompositeXffect : MonoBehaviour
	{
		private void Awake()
		{
			this.Initialize();
		}

		public void Initialize()
		{
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					XffectComponent component = transform.GetComponent<XffectComponent>();
					if (component == null)
					{
						XffectCache component2 = transform.GetComponent<XffectCache>();
						if (component2 != null)
						{
							component2.Init();
						}
					}
					else
					{
						component.Initialize();
						this.XffectList.Add(component);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		public void Active()
		{
			base.gameObject.SetActive(true);
			foreach (XffectComponent xffectComponent in this.XffectList)
			{
				xffectComponent.Active();
			}
		}

		public void DeActive()
		{
			base.gameObject.SetActive(false);
			foreach (XffectComponent xffectComponent in this.XffectList)
			{
				xffectComponent.DeActive();
			}
		}

		private List<XffectComponent> XffectList = new List<XffectComponent>();
	}
}
