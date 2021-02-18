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

			UnlockDrag();
		}

		public void BlockDrag() => GetComponentInChildren<HUDGear>().enabled = false;
		public void UnlockDrag() => GetComponentInChildren<HUDGear>().enabled = false;
	}

}