// Maded by Pedro M Marangon
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CrenixTeste
{

	public class DraggingGearController : MonoBehaviour
    {

		[Header("Gear (World -> HUD) Visual")]
        [SerializeField] private Transform worldGear;
        [SerializeField] private RectTransform uiGear;
		[Header("HUD")]
		[SerializeField] private Canvas canvas;
		[SerializeField] private GraphicRaycaster raycaster;
		[SerializeField] private TMP_Text speechText;
		[SerializeField] private string speech_notCompleted;
		[SerializeField] private string speech_completed;
		[Header("Gears")]
		[SerializeField] private Gear pinkGear;
		[SerializeField] private Gear yellowGear;
		[SerializeField] private Gear blueGear;
		[SerializeField] private Gear greenGear;
		[SerializeField] private Gear purpleGear;
		[SerializeField] private List<HUDGear> hudGears;

		private GearPlace gearSelected;
		private bool isDraggingPlaceWithGear;
		private GameObject gearPlaced;
		private int totalGearsPlacedCorrectly = 0;

		public void ResetAllGears()
		{
			pinkGear.Reset();
			yellowGear.Reset();
			blueGear.Reset();
			greenGear.Reset();
			purpleGear.Reset();

			foreach (HUDGear gear in hudGears)
			{
				switch (gear.MyGearColor)
				{
					case GearColor.None:
						gear.SetColor(Color.white);
						break;
					case GearColor.Pink:
						gear.SetColor(pinkGear.color);
						break;
					case GearColor.Yellow:
						gear.SetColor(yellowGear.color);
						break;
					case GearColor.Purple:
						gear.SetColor(purpleGear.color);
						break;
					case GearColor.Blue:
						gear.SetColor(blueGear.color);
						break;
					case GearColor.Green:
						gear.SetColor(greenGear.color);
						break;
				}

				gear.SetGroup(1);
				gear.enabled = true;
			}

		}
		
		//Verification
		public void VerifyIfPlacedOnCorrectColor()
		{
			totalGearsPlacedCorrectly = 0;

			if (pinkGear.IsCorrect())
				totalGearsPlacedCorrectly ++;

			if (yellowGear.IsCorrect())
				totalGearsPlacedCorrectly++;

			if (purpleGear.IsCorrect())
				totalGearsPlacedCorrectly++;

			if (blueGear.IsCorrect())
				totalGearsPlacedCorrectly++;

			if (greenGear.IsCorrect())
				totalGearsPlacedCorrectly++;

			VerifyRotator();
		}
		private void VerifyRotator()
		{
			totalGearsPlacedCorrectly = Mathf.Clamp(totalGearsPlacedCorrectly, 0, 5);
			if (totalGearsPlacedCorrectly == 5) AllGearsState(speech_completed,true);
			else AllGearsState(speech_notCompleted,false);
		}
		private void AllGearsState(string text, bool enabled)
		{
			speechText.text = text;
			pinkGear.rotator.enabled = enabled;
			yellowGear.rotator.enabled = enabled;
			blueGear.rotator.enabled = enabled;
			greenGear.rotator.enabled = enabled;
			purpleGear.rotator.enabled = enabled;
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

				VerifyIfPlacedOnCorrectColor();

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

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
				SelectGear();

			if (Input.GetMouseButton(0) && isDraggingPlaceWithGear)
				DragGear();

			else if (Input.GetMouseButtonUp(0) && isDraggingPlaceWithGear)
				DropGear();
		}
	}

	[Serializable]
	public class Gear
	{
		public GearRotator rotator;
		public HUDGear hudGear;
		public GearPlace place;
		[Header("Color")]
		public GearColor gearColor;
		public Color color;

		public void Reset()
		{
			rotator.enabled = false;
			place.RemoveGear();
		}

		public bool MatchGearColor(GearColor requestedGearColor) => requestedGearColor == gearColor;

		public bool IsCorrect()
		{
			SpriteRenderer rot = rotator.transform.GetChild(0).GetComponent<SpriteRenderer>();

			bool hasGear = place.HasGear;
			bool correctColor = rot.color == color;

			return hasGear && correctColor;
		}
	}

}