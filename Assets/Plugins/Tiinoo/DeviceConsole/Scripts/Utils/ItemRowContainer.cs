using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Tiinoo.DeviceConsole
{
	public class ItemRowContainer
	{
		private List<ItemRow> m_poolRows = new List<ItemRow>();	
		private List<ItemRow> m_visibleRows = new List<ItemRow>();

		private ItemContainer m_itemContainer;
		private System.Func<ItemRow> m_funcCreateRow;

		public ItemRowContainer()
		{

		}

		public void Init(ItemContainer itemContainer, System.Func<ItemRow> funcCreateRow)
		{
			m_itemContainer = itemContainer;
			m_funcCreateRow = funcCreateRow;
		}

		public void InvalidateVisibleRows()
		{
			for (int i = m_visibleRows.Count-1; i >= 0; i--)
			{
				InvalidateRow(m_visibleRows[i].itemIndex);
			}
		}

		public void InvalidateRow(int itemIndex)
		{
			for (int i = 0; i < m_visibleRows.Count; i++)
			{
				if (itemIndex == m_visibleRows[i].itemIndex)
				{
					m_poolRows.Add(m_visibleRows[i]);
					m_visibleRows.RemoveAt(i);
					break;
				}
			}
		}

		// Visible items which are not in range should be invisible.
		public bool ProcessVisibleItemsNotInRange(int beginItemIndex, int endItemIndex)
		{
			bool hasOccurred = false;
			for (int i = m_visibleRows.Count-1; i >= 0; i--)
			{
				ItemRow row = m_visibleRows[i];
				if ((row.itemIndex < beginItemIndex) || (row.itemIndex >= endItemIndex))
				{
					m_poolRows.Add(row);
					m_visibleRows.RemoveAt(i);
					hasOccurred |= true;
				}
			}
			return hasOccurred;
		}

		// Invisible items which are in range should be visible.
		public bool ProcessInvisibleItemsInRange(int beginItemIndex, int endItemIndex)
		{
			bool hasOccurred = false;
			for (int i = beginItemIndex; i < endItemIndex; i++)
			{
				if (!IsVisibleItem(i))
				{
					ItemRow row = RequestRow(i);
					m_visibleRows.Add(row);
					hasOccurred |= true;
				}
			}
			return hasOccurred;
		}

		private bool IsVisibleItem(int itemIndex)
		{
			for (int i = 0; i < m_visibleRows.Count; i++)
			{
				if (m_visibleRows[i].itemIndex == itemIndex)
				{
					return true;
				}
			}
			return false;
		}

		private ItemRow RequestRow(int itemIndex)
		{
			ItemRow row = null;

			if (m_poolRows.Count > 0)
			{
				object item = m_itemContainer.GetItem(itemIndex);
				
				for (int i = 0; i < m_poolRows.Count; i++)
				{
					if (item == m_poolRows[i].item)
					{
						row = m_poolRows[i];
						m_poolRows.RemoveAt(i);
						break;
					}
				}
				
				if (row == null)
				{
					row = PopEndRow(m_poolRows);
				}
			}
			else
			{
				row = m_funcCreateRow();
			}

			if (row != null)
			{
				SetRow(row, itemIndex);
			}
			
			return row;
		}
		
		private void SetRow(ItemRow row, int itemIndex)
		{
			row.itemIndex = itemIndex;
			row.item = m_itemContainer.GetItem(itemIndex);
			
			bool isOddRow = (itemIndex % 2 != 0) ? true : false;
			
			bool isSelected = false;
			object curSelectedItem = m_itemContainer.GetCurSelectedItem();
			if ((curSelectedItem != null) && (row.item == curSelectedItem))
			{
				isSelected = true;
			}
			
			row.drawer.Draw(row.item, isOddRow, isSelected);
		}
		
		private static ItemRow PopEndRow(List<ItemRow> rows)
		{
			if (rows == null || rows.Count == 0)
			{
				return null;
			}
			
			int index = rows.Count - 1;
			ItemRow row = rows[index];
			rows.RemoveAt(index);
			return row;
		}

		public void UpdateVisibleRowsItemIndex(List<object> items)
		{
			int count = VisibleRowCount;
			for (int i = 0; i < count; i++)
			{
				int itemIndex = items.IndexOf(m_visibleRows[i].item);
				m_visibleRows[i].itemIndex = itemIndex;
			}
		}

		public int PoolRowCount
		{
			get {return m_poolRows.Count;}
		}

		public int VisibleRowCount
		{
			get {return m_visibleRows.Count;}
		}

		public ItemRow GetPoolRow(int index)
		{
			return m_poolRows[index];
		}

		public ItemRow GetVisibleRow(int index)
		{
			return m_visibleRows[index];
		}
	}
}

