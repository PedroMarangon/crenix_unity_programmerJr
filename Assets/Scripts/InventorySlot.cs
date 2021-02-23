// Maded by Pedro M Marangon
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CrenixTeste
{
	public class InventorySlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
	{
		[SerializeField] private Image img;
		private Inventory inventory;

		//Get the image color
		public Color GetColor => img.color;
		//Verify if it has a gear
		public bool HasGear { get; private set; }
		
		/// <summary>
		/// Activates the visual drag, set it to Input.mousePosition and deactivates the image
		/// </summary>
		public void OnBeginDrag(PointerEventData eventData)
		{
			inventory.ActivateVisualDrag(img.color);
			inventory.SetVisualDragPos(Input.mousePosition);
			SetColor(img.color, false);
		}

		/// <summary>
		/// Just sets the visual drag position to Input.mousePosition
		/// </summary>
		public void OnDrag(PointerEventData eventData) => inventory.SetVisualDragPos(Input.mousePosition);

		/// <summary>
		/// Set the visual drag position to Input.mousePosition, deactivates it and try to place the gear in the world
		/// </summary>
		public void OnEndDrag(PointerEventData eventData)
		{
			inventory.SetVisualDragPos(Input.mousePosition);
			inventory.DeactivateVisualDrag();
			inventory.TryToPlaceInWorld(img.color, transform.GetSiblingIndex());
		}

		/// <summary>
		/// Set the color of the image and set it's visibility/having a gear
		/// </summary>
		/// <param name="color">The color to set</param>
		/// <param name="visible">will it be visible?</param>
		public void SetColor(Color color, bool visible)
		{
			img.color = color;
			img.enabled = HasGear = visible;
		}

		//Assigns the inventory externaly
		public void SetInventory(Inventory inv) => inventory = inv;

	}

}