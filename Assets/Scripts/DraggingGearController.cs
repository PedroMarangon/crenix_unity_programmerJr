// Maded by Pedro M Marangon
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CrenixTeste
{

	public class DraggingGearController : MonoBehaviour
    {

        [SerializeField] private Transform worldGear;
        [SerializeField] private RectTransform uiGear;
		[SerializeField] private Canvas canvas;
		[SerializeField] private GraphicRaycaster raycaster;
		[Header("Gears")]
		[SerializeField] private Gear pinkGear;
		[SerializeField] private Gear yellowGear;
		[SerializeField] private Gear blueGear;
		[SerializeField] private Gear greenGear;
		[SerializeField] private Gear purpleGear;
		private GearPlace gearSelected;
		private bool isDraggingPlaceWithGear;
		private GameObject gearPlaced;


		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
				SelectGear();

			if (Input.GetMouseButton(0) && isDraggingPlaceWithGear)
				DragGear();

			else if (Input.GetMouseButtonUp(0) && isDraggingPlaceWithGear)
				DropGear();
		}

		//Manipulate gears
		private void SelectGear()
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if (hit && hit.collider.TryGetComponent<GearPlace>(out var place) && place.HasGear)
			{
				gearPlaced = place.transform.GetChild(1).GetChild(0).gameObject;
				gearPlaced.SetActive(false);
				gearSelected = place;

				isDraggingPlaceWithGear = true;
				worldGear.GetComponent<SpriteRenderer>().color = uiGear.GetComponent<Image>().color = place.CurrentColor;
			}
			else isDraggingPlaceWithGear = false;
		}
		private void DragGear()
		{
			//World gear
			Vector2 wgPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldGear.position = wgPos;

			//UI Gear
			uiGear.position = Input.mousePosition;
		}
		private void DropGear()
		{
			ReleaseGearVisual();

			List<RaycastResult> results = GetUIOnMousePos();

			if (results.Count > 0)
			{
				gearSelected.RemoveGear();
				if (results[0].gameObject.TryGetComponent<HUDGear>(out var gear))
				{
					HUDSlot slot = gear.GetComponentInParent<HUDSlot>();
					PlaceInInventory(slot);
				}
				else if (results[0].gameObject.TryGetComponent<HUDSlot>(out var slot))
					PlaceInInventory(slot);
				else ReturnGearToWorld();

				gearPlaced.SetActive(true);
			}
			else
			{
				gearPlaced.SetActive(true);
				gearPlaced.transform.localPosition = Vector3.zero;
			}

			gearPlaced = null;
			gearSelected = null;
		}

		//Acessory methods for gear manipulation
		private void PlaceInInventory(HUDSlot slot)
		{
			if (!slot.DropGearFromWorld())
			{
				GearColor color = gearSelected.OldGearColor;

				if (pinkGear.MatchGearColor(color)) pinkGear.Reset();
				if (blueGear.MatchGearColor(color)) blueGear.Reset();
				if (yellowGear.MatchGearColor(color)) yellowGear.Reset();
				if (greenGear.MatchGearColor(color)) greenGear.Reset();
				if (purpleGear.MatchGearColor(color)) purpleGear.Reset();
			}
		}
		private void ReturnGearToWorld()
		{
			gearPlaced.SetActive(true);

			if (gearPlaced.TryGetComponent<SpriteRenderer>(out var rend))
			{
				rend.color = gearSelected.OldColor;
				rend.enabled = true;
			}

			gearSelected.PlaceGear(gearSelected.OldGearColor, gearSelected.OldColor);
		}
		private void ReleaseGearVisual()
		{
			worldGear.position = new Vector2(0, 10);
			uiGear.anchoredPosition = Vector2.one * 400;
			isDraggingPlaceWithGear = false;
		}
		private List<RaycastResult> GetUIOnMousePos()
		{
			PointerEventData pointerData = new PointerEventData(EventSystem.current);
			pointerData.position = Input.mousePosition;
			List<RaycastResult> results = new List<RaycastResult>();
			raycaster.Raycast(pointerData, results);
			return results;
		}

		public void ResetAllGears()
		{
			pinkGear.Reset();
			yellowGear.Reset();
			blueGear.Reset();
			greenGear.Reset();
			purpleGear.Reset();
		}
	}

	[Serializable]
	public class Gear
	{
		[Header("HUD")]
		public HUDSlot slot;
		public HUDGear gear;
		[Header("World")]
		public GearRotator rotator;
		public GearPlace place;
		[Header("Color")]
		public GearColor gearColor;
		public Color color;

		public void Reset()
		{
			rotator.enabled = false;

			place.RemoveGear();

			slot.DropGearFromWorld();

		}

		public bool Matches(HUDSlot requestedSlot, GearColor requestedGearColor)
		{
			bool slotMatch = requestedSlot == slot;
			bool gcMatch = requestedGearColor == gearColor;

			return gcMatch && slotMatch;

		}

		public bool MatchGearColor(GearColor requestedGearColor) => requestedGearColor == gearColor;
	}

}