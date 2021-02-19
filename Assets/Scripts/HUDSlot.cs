// Maded by Pedro M Marangon
using UnityEngine;
using UnityEngine.EventSystems;

namespace CrenixTeste
{
	public class HUDSlot : MonoBehaviour, IDropHandler
	{

		private HUDGear gear;

		private void Awake() => gear = GetComponentInChildren<HUDGear>();


		public void OnDrop(PointerEventData eventData)
		{
			GameObject draggedObj = eventData.pointerDrag;
			if (draggedObj && draggedObj.TryGetComponent<HUDGear>(out var hudG))
			{
				draggedObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				hudG.PutInInventory();
			}
			else if(draggedObj)
			{
				gear.Activate();
				gear.PutInInventory();
			}


			UnlockDrag();
		}

		public bool DropGearFromWorld()
		{
			if (gear && gear.IsInInventory) return false;
						

			gear.Activate();
			gear.PutInInventory();
			UnlockDrag();

			return true;
		}

		public void BlockDrag()
		{
			if (gear) gear.enabled = false;
		}

		public void UnlockDrag()
		{
			if(gear) gear.enabled = true;
		}
	}

}