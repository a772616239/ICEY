using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiScroller : MonoBehaviour
{
	public void SetUpPointer()
	{
		foreach (ScrollGroup scrollGroup in this.groups)
		{
			for (int i = 0; i < scrollGroup.pages.Count; i++)
			{
				scrollGroup.pageSnake[i].index = i;
				scrollGroup.pages[i].index = i;
			}
		}
	}

	public void ResetPage()
	{
		foreach (ScrollGroup scrollGroup in this.groups)
		{
			int index = 0;
			foreach (ScrollPage scrollPage in scrollGroup.pages)
			{
				ScrollPage scrollPage2 = new ScrollPage();
				scrollPage2.initPos = scrollPage.initPos;
				scrollPage2.id = scrollPage.id;
				scrollPage2.index = scrollPage.index;
				scrollPage2.originalPos = scrollPage.originalPos;
				scrollGroup.pageSnake[index] = scrollPage2;
			}
		}
	}

	public List<ScrollGroup> groups;
}
