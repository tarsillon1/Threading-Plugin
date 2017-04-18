using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Tiinoo.DeviceConsole
{
	public class ItemVerticalLayoutGroup : LayoutGroup, ItemContainer, IPointerClickHandler
	{
		#region inspector
		public ScrollRect scrollRect;
		public GameObject itemPrefab;
		public int itemSpace = 0;
		public bool autoScrollToBottom = true;
		#endregion

		public event System.Action<object> onItemSelected;	// item

		private const float SCROLL_DISTANCE_TOLERANCE = 0.0001f;
		private const int EXTRA_ROW_COUNT = 3;
		
		private List<object> m_items = new List<object>();	// contains all items added by AddItem()
		private float m_itemHeight = 0;
		
		private int m_visibleRowCount;
		
		private object m_curSelectedItem;				// current selected item
		private int m_curSelectedItemIndex = -1;		// index of current selected item in m_items
		
		private bool m_isDirty = false;
		
		private bool m_shouldScrollToBottom = false;

		private ItemRowContainer m_rowContainer = new ItemRowContainer();
		
		protected override void Awake()
		{
			base.Awake();

			CheckChildAlignment();
			StartListenEvents();
			m_rowContainer.Init(this, CreateRow);
		}
		
		protected override void OnEnable()
		{
			base.OnEnable();
			MarkDirty();
		}

		private void Update()
		{
			CheckCurSelectedItem();
			
			ScrollToBottom();
			
			if (m_isDirty)
			{
				CheckShouldScrollToBottom();
				Refresh();
				m_isDirty = false;
			}
		}

		private void Refresh()
		{
			float beginY = rectTransform.anchoredPosition.y;
			float scrollRectHeight = UGUIUtil.GetRect(scrollRect.transform).height;
			float rowHeight = GetRowHeight();
			
			// Calculate which items should be visible. [beginItemIndex, endItemIndex)
			float f_beginItemIndex = beginY / rowHeight - EXTRA_ROW_COUNT;
			float f_endItemIndex = (beginY + scrollRectHeight) / rowHeight + EXTRA_ROW_COUNT;

			int beginItemIndex = Mathf.FloorToInt(f_beginItemIndex);
			int endItemIndex = Mathf.CeilToInt(f_endItemIndex);
			if (beginItemIndex < 0)
			{
				beginItemIndex = 0;
			}
			if (endItemIndex > m_items.Count)
			{
				endItemIndex = m_items.Count;
			}
			
			bool layout = false;

			if (m_rowContainer.ProcessVisibleItemsNotInRange(beginItemIndex, endItemIndex))
			{
				layout |= true;
			}
			
			if (m_rowContainer.ProcessInvisibleItemsInRange(beginItemIndex, endItemIndex))
			{
				layout |= true;
			}

			if (m_rowContainer.VisibleRowCount != m_visibleRowCount)
			{
				layout |= true;
			}

			if (layout)
			{
				LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
			}
			
			m_visibleRowCount = m_rowContainer.VisibleRowCount;
		}

		private void CheckChildAlignment()
		{
			if (childAlignment == TextAnchor.MiddleLeft 
			    || childAlignment == TextAnchor.MiddleRight 
			    || childAlignment == TextAnchor.MiddleCenter)
			{
				childAlignment = TextAnchor.UpperLeft;
			}
		}

		private void StartListenEvents()
		{
			scrollRect.onValueChanged.AddListener(HandleScrollRectOnValueChanged);
		}

		private void MarkDirty()
		{
			m_isDirty = true;
		}
		
		private float GetItemHeight()
		{
			if (m_itemHeight > 0)
			{
				return m_itemHeight;
			}

			m_itemHeight = ItemRow.GetItemHeight(itemPrefab);

			if (m_itemHeight < 1)
			{
				m_itemHeight = 20;
			}
			return m_itemHeight;
		}

		private float GetRowHeight()
		{
			float rowHeight = GetItemHeight() + itemSpace;
			return rowHeight;
		}
		
		public override float minHeight
		{
			get 
			{ 
				float height = GetRowHeight() * m_items.Count + (padding.top + padding.bottom);
				return height;
			}
		}
		
		public override void SetLayoutHorizontal()
		{
			float size = rectTransform.rect.width - (padding.left + padding.right);
			
			int poolRowCount = m_rowContainer.PoolRowCount;
			float pos1 = rectTransform.rect.width * 2;
			for (int i = 0; i < poolRowCount; i++)
			{
				RectTransform rect = m_rowContainer.GetPoolRow(i).rect;
				SetChildAlongAxis(rect, 0, pos1, size);
			}
			
			int visibleRowCount = m_rowContainer.VisibleRowCount;
			float pos2 = padding.left;
			for (int i = 0; i < visibleRowCount; i++)
			{
				RectTransform rect = m_rowContainer.GetVisibleRow(i).rect;
				SetChildAlongAxis(rect, 0, pos2, size);
			}
		}
		
		public override void SetLayoutVertical()
		{
			int visibleRowCount = m_rowContainer.VisibleRowCount;
			float rowHeight = GetRowHeight();
			float size = GetItemHeight();
			for (int i = 0; i < visibleRowCount; i++)
			{
				ItemRow row = m_rowContainer.GetVisibleRow(i);
				float pos = rowHeight * row.itemIndex + padding.top;
				SetChildAlongAxis(row.rect, 1, pos, size);
			}
		}
		
		public override void CalculateLayoutInputVertical()
		{
			SetLayoutInputForAxis(minHeight, minHeight, 0, 1);
		}

		#region items
		
		public void AddItem(object item)
		{
			m_items.Add(item);
			MarkDirty();
		}

		public object GetItem(int index)
		{
			if (index < 0 || index > m_items.Count-1)
			{
				return null;
			}
			return m_items[index];
		}
		
		public void RemoveItem(object item)
		{
			if (m_curSelectedItem == item)
			{
				SetCurSelectedItem(null);
			}
			
			int itemIndex = m_items.IndexOf(item);
			if (itemIndex < 0)
			{
				return;
			}
			
			m_rowContainer.InvalidateRow(itemIndex);
			m_items.RemoveAt(itemIndex);
			m_rowContainer.UpdateVisibleRowsItemIndex(m_items);
			
			MarkDirty();
		}
		
		public void ClearItems()
		{
			SetCurSelectedItem(null);
			m_rowContainer.InvalidateVisibleRows();
			m_items.Clear();

			MarkDirty();
		}

		public object GetCurSelectedItem()
		{
			return m_curSelectedItem;
		}
		
		public void SetCurSelectedItem(object item)
		{
			if (m_curSelectedItem == item)
			{
				return;
			}
			
//			object oldItem = m_curSelectedItem;
			
			if (m_curSelectedItem != null)
			{
				m_rowContainer.InvalidateRow(m_curSelectedItemIndex);
			}
			
			int newSelectedItemIndex = -1;
			if (item != null)
			{
				newSelectedItemIndex = m_items.IndexOf(item);
				m_rowContainer.InvalidateRow(newSelectedItemIndex);
			}
			
			m_curSelectedItem = item;
			m_curSelectedItemIndex = newSelectedItemIndex;
			
			if (onItemSelected != null)
			{
				onItemSelected(m_curSelectedItem);
			}
			
			MarkDirty();
			
//			Debug.Log(string.Format("select {0} -> {1}", oldItem, m_curSelectedItem));
		}
		
		private void CheckCurSelectedItem()
		{
			if (m_curSelectedItem != null)
			{
				if (!m_items.Contains(m_curSelectedItem))
				{
					SetCurSelectedItem(null);
				}
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			GameObject clickObj = eventData.pointerPressRaycast.gameObject;
			if (clickObj != null)
			{
				Vector3 localPos = rectTransform.InverseTransformPoint(clickObj.transform.position);
				
				float rowHeight = GetRowHeight();
				float f_index = (Mathf.Abs(localPos.y) - padding.top) / rowHeight;
				int index = (int)f_index;
				
				if (index < 0 || index > m_items.Count-1)
				{
					SetCurSelectedItem(null);
				}
				else
				{
					SetCurSelectedItem(m_items[index]);
				}
			}
		}

		private ItemRow CreateRow()
		{
			ItemRow row = ItemRow.Create(rectTransform, itemPrefab);
			return row;
		}

		#endregion
		
		#region scroll
		
		private void HandleScrollRectOnValueChanged(Vector2 size)
		{
			if (size.y < 0)
			{
				scrollRect.verticalNormalizedPosition = 0;
			}
			else if (size.y > 1)
			{
				scrollRect.verticalNormalizedPosition = 1;
			}
			
			MarkDirty();
		}
		
		private void CheckShouldScrollToBottom()
		{
			if (!autoScrollToBottom)
			{
				return;
			}
			
			if (scrollRect.verticalNormalizedPosition < SCROLL_DISTANCE_TOLERANCE)
			{
				m_shouldScrollToBottom = true;
			}
		}
		
		private void ScrollToBottom()
		{
			if (!autoScrollToBottom)
			{
				return;
			}
			
			if (m_shouldScrollToBottom)
			{
				UGUIUtil.ScrollToBottom(scrollRect);
				m_shouldScrollToBottom = false;
			}
		}

		#endregion
	}
}

