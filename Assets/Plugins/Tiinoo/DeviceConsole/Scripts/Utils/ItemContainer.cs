using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Tiinoo.DeviceConsole
{
	public interface ItemContainer
	{
		void AddItem(object item);

		object GetItem(int index);

		void RemoveItem(object item);

		void ClearItems();

		object GetCurSelectedItem();

		void SetCurSelectedItem(object item);
	}
}

