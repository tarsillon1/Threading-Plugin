using UnityEngine;
using System.Collections;

namespace Tiinoo.DeviceConsole
{
	public interface ItemDrawer
	{
		void Draw(object item, bool isOddRow, bool isSelected);
	}
}

