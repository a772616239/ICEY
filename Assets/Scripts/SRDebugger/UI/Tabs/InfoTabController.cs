using System;
using System.Collections.Generic;
using System.Text;
using SRDebugger.Services;
using SRDebugger.UI.Controls;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.UI.Tabs
{
	public class InfoTabController : SRMonoBehaviourEx
	{
		protected override void OnEnable()
		{
			base.OnEnable();
			this.Refresh();
		}

		public void Refresh()
		{
			ISystemInformationService service = SRServiceManager.GetService<ISystemInformationService>();
			foreach (string text in service.GetCategories())
			{
				if (!this._infoBlocks.ContainsKey(text))
				{
					InfoBlock value = this.CreateBlock(text);
					this._infoBlocks.Add(text, value);
				}
			}
			foreach (KeyValuePair<string, InfoBlock> keyValuePair in this._infoBlocks)
			{
				this.FillInfoBlock(keyValuePair.Value, service.GetInfo(keyValuePair.Key));
			}
		}

		private void FillInfoBlock(InfoBlock block, IList<InfoEntry> info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (InfoEntry infoEntry in info)
			{
				if (infoEntry.Title.Length > num)
				{
					num = infoEntry.Title.Length;
				}
			}
			num += 2;
			bool flag = true;
			foreach (InfoEntry infoEntry2 in info)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("<color=");
				stringBuilder.Append("#BCBCBC");
				stringBuilder.Append(">");
				stringBuilder.Append(infoEntry2.Title);
				stringBuilder.Append(": ");
				stringBuilder.Append("</color>");
				for (int i = infoEntry2.Title.Length; i <= num; i++)
				{
					stringBuilder.Append(' ');
				}
				if (infoEntry2.Value is bool)
				{
					stringBuilder.Append((!(bool)infoEntry2.Value) ? '×' : '✓');
				}
				else
				{
					stringBuilder.Append(infoEntry2.Value);
				}
			}
			block.Content.text = stringBuilder.ToString();
		}

		private InfoBlock CreateBlock(string title)
		{
			InfoBlock infoBlock = SRInstantiate.Instantiate<InfoBlock>(this.InfoBlockPrefab);
			infoBlock.Title.text = title;
			infoBlock.CachedTransform.SetParent(this.LayoutContainer, false);
			return infoBlock;
		}

		public const char Tick = '✓';

		public const char Cross = '×';

		public const string NameColor = "#BCBCBC";

		private Dictionary<string, InfoBlock> _infoBlocks = new Dictionary<string, InfoBlock>();

		[RequiredField]
		public InfoBlock InfoBlockPrefab;

		[RequiredField]
		public RectTransform LayoutContainer;
	}
}
