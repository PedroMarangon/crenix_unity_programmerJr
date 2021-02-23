// Maded by Pedro M Marangon
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CrenixTeste
{

	public class Inventory : MonoBehaviour
    {

        [SerializeField] private List<InventorySlot> slots;
		[SerializeField] private RectTransform visualDrag;
		[Header("Colors")]
		[SerializeField] private Color pink;
		[SerializeField] private Color blue;
		[SerializeField] private Color yellow;
		[SerializeField] private Color green;
		[SerializeField] private Color purple;
		private GraphicRaycaster raycaster;

		public Color Pink => pink;
		public Color Blue => blue;
		public Color Yellow => yellow;
		public Color Green => green;
		public Color Purple => purple;

		//Basic setup of the inventory
		private void Awake()
		{
			ResetGears();
			DeactivateVisualDrag();
			foreach (InventorySlot slot in slots) slot.SetInventory(this);
			raycaster = FindObjectOfType<GraphicRaycaster>();
		}

		/// <summary>
		/// Activate the visual gear for dragging and set it's color
		/// </summary>
		/// <param name="color">The color to set the visual gear drag</param>
		public void ActivateVisualDrag(Color color)
		{
			visualDrag.gameObject.SetActive(true);
			visualDrag.GetComponent<Image>().color = color;
		}

		/// <summary>
		/// Deactivates the visual dragging gear
		/// </summary>
		public void DeactivateVisualDrag() => visualDrag.gameObject.SetActive(false);

		/// <summary>
		/// Reset all gears - i.e. bring back every gear to inventory in it's original position
		/// </summary>
		public void ResetGears()
		{
			//Bring back to inventory
			SetColorOrder(pink, blue, yellow, green, purple);
			//For each world gear in the scene, deactivate it
			foreach (WorldGear gear in FindObjectsOfType<WorldGear>())
				gear.Deactivate();

			//Reset the text in the speech bubble
			FindObjectOfType<GearRotationManager>()?.SetBaseText();
		}

		/// <summary>
		/// Set the color of a slot based on it's index on the array (i.e. it's sibling index)
		/// </summary>
		/// <param name="index">Index of the array / which child it is</param>
		/// <param name="color">The color to set the gear</param>
		/// <param name="visible">Will it be visible? (By default, it will)</param>
		public void SetColor(int index, Color color, bool visible = true) => slots[index].SetColor(color, visible);

		/// <summary>
		/// Set the position for the visual dragging gear
		/// </summary>
		/// <param name="position">the position to put the visual dragging gear</param>
		public void SetVisualDragPos(Vector3 position)
		{
			if (visualDrag) visualDrag.position = position;
		}
		
		/// <summary>
		/// Tries to place a gear in the world (Called from InventorySlot)
		/// </summary>
		/// <param name="draggedColor">The color of the dragging gear</param>
		/// <param name="index">The index of the gear</param>
		public void TryToPlaceInWorld(Color draggedColor, int index)
		{
			//If hitted a UI element
			if (GetUIOnMousePos().Count > 0)
			{
				RaycastResult result = GetUIOnMousePos()[0];

				//If it's an InventorySlot, then change colors in the case of the slot having a gear or just set the color
				if (result.gameObject.TryGetComponent<InventorySlot>(out var iSlot))
				{
					if (iSlot.HasGear)
					{
						Color droppedColor = iSlot.GetColor;
						SetColor(index, droppedColor);
					}
					iSlot.SetColor(draggedColor, true);
				}
				else SetColor(index, draggedColor);
			}
			//Not hitted UI
			else
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				//If hitted something in the world
				if (hit.collider)
				{
					//If it's an world gear, then apply the same logic (if has gear, then switch colors, else set the color)
					if (hit.collider.TryGetComponent<WorldGear>(out var gear))
					{
						if (gear.HasGear)
						{
							Color droppedColor = gear.GetColor;
							gear.SetColor(draggedColor);
							SetColor(index, droppedColor);
						}
						else
						{
							gear.SetColor(draggedColor);
							gear.Activate();
						}
					}
				}
				//else go back to inventory
				else SetColor(index, draggedColor);
			}
			//Verify it all colors match
			FindObjectOfType<GearRotationManager>().VerifyIfMatchesColors();
		}

		/// <summary>
		/// Tries to place a gear in the UI (Called from WorlgGear)
		/// </summary>
		/// <param name="draggedColor">The color of the dragging gear</param>
		/// <param name="gear">The world gear that called the method</param>
		public void TryToPlaceInUI(Color draggedColor, WorldGear gear)
		{
			//If hitted a UI element
			if (GetUIOnMousePos().Count > 0)
			{
				RaycastResult result = GetUIOnMousePos()[0];

				//If it's an InventorySlot, then change colors in the case of the slot having a gear or just set the color
				if (result.gameObject.TryGetComponent<InventorySlot>(out var iSlot))
				{
					if (iSlot.HasGear)
					{
						Color droppedColor = iSlot.GetColor;
						gear.SetColor(droppedColor);
						gear.Activate();
					}					
					iSlot.SetColor(draggedColor, true);
				}
				else gear.Activate();
			}
			//Not hitted UI
			else
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				//If hitted something in the world
				if (hit.collider)
				{
					//If it's an world gear, then apply the same logic (if has gear, then switch colors, else set the color)
					if (hit.collider.TryGetComponent<WorldGear>(out var otherGear))
					{

						if (otherGear.HasGear)
						{
							Color droppedColor = otherGear.GetColor;

							otherGear.SetColor(draggedColor);
							gear.SetColor(droppedColor);
							gear.Activate();
						}
						else
						{
							otherGear.SetColor(draggedColor);
							otherGear.Activate();
						}
					}
				}
				//else go back to original place
				else gear.Activate();
			}
			//Verify it all colors match
			FindObjectOfType<GearRotationManager>().VerifyIfMatchesColors();
		}

		/// <summary>
		/// Get a list of UI items beneath of the mouse position
		/// </summary>
		/// <returns>A list of UI items that's placed beneath of the mouse position</returns>
		private List<RaycastResult> GetUIOnMousePos()
		{
			//Create a pointerData with the current Event System and set the position to be Input.mousePosition
			PointerEventData pointerData = new PointerEventData(EventSystem.current);
			pointerData.position = Input.mousePosition;
			//Create a list of RaycastResults and return it
			List<RaycastResult> results = new List<RaycastResult>();
			raycaster.Raycast(pointerData, results);
			return results;
		}

		/// <summary>
		/// Set the color order of the inventory slots, while also activating all the slots
		/// </summary>
		/// <param name="c1">The color to assign to the first slot</param>
		/// <param name="c2">The color to assign to the second slot</param>
		/// <param name="c3">The color to assign to the third slot</param>
		/// <param name="c4">The color to assign to the fourth slot</param>
		/// <param name="c5">The color to assign to the fifth slot</param>
		private void SetColorOrder(Color c1, Color c2, Color c3, Color c4, Color c5)
		{
			SetColor(0, c1);
			SetColor(1, c2);
			SetColor(2, c3);
			SetColor(3, c4);
			SetColor(4, c5);
		}
    }

}