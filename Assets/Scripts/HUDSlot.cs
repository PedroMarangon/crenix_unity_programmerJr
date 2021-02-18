// Maded by Pedro M Marangon
using UnityEngine;
using UnityEngine.EventSystems;

namespace CrenixTeste
{
	public class HUDSlot : MonoBehaviour, IDropHandler
	{
		public void OnDrop(PointerEventData eventData)
		{
			GameObject draggedObj = eventData.pointerDrag;
			if (draggedObj && draggedObj.TryGetComponent<HUDGear>(out var gear))
			{
				draggedObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			}
			else if(draggedObj)
			{
				GetComponentInChildren<HUDGear>().Activate();
			}

			UnlockDrag();
		}

		public void DropGearFromWorld()
		{
			GetComponentInChildren<HUDGear>().Activate();
			UnlockDrag();
		}

		public void BlockDrag()
		{
			var hudGear = GetComponentInChildren<HUDGear>();

			if (hudGear) hudGear.enabled = false;
		}

		public void UnlockDrag()
		{
			var hudGear = GetComponentInChildren<HUDGear>();
			if(hudGear) hudGear.enabled = true;
		}
	}

}