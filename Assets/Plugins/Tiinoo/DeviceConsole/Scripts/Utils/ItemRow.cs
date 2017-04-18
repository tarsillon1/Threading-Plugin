using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Tiinoo.DeviceConsole
{
	public class ItemRow
	{
		public object item;
		public int itemIndex;
		
		public RectTransform rect;
		public ItemDrawer drawer;
		
		public static ItemRow Create(Transform itemParent, GameObject itemPrefab)
		{
			ItemRow row = new ItemRow();
			
			GameObject go = GameObject.Instantiate(itemPrefab) as GameObject;
			row.rect = go.GetComponent(typeof(RectTransform)) as RectTransform;
			row.drawer = go.GetComponent(typeof(ItemDrawer)) as ItemDrawer;
			go.transform.SetParent(itemParent, false);
			
			return row;
		}

		public static float GetItemHeight(GameObject item)
		{
			float itemHeight = 0;
			ILayoutElement element = item.GetComponent(typeof(ILayoutElement)) as ILayoutElement;
			if (element == null)
			{
				RectTransform rectTrans = item.GetComponent(typeof(RectTransform)) as RectTransform;
				itemHeight = rectTrans.rect.height;
			}
			else
			{
				itemHeight = element.preferredHeight;
			}
			return itemHeight;
		}
	}
}
