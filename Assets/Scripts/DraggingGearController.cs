// Maded by Pedro M Marangon
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
		private bool isDraggingPlaceWithGear;
		private GameObject gearPlaced;

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if (hit && hit.collider.TryGetComponent<GearPlace>(out var place) && place.HasGear)
				{
					gearPlaced = place.transform.GetChild(1).GetChild(0).gameObject;
					gearPlaced.SetActive(false);

					isDraggingPlaceWithGear = true;
					worldGear.GetComponent<SpriteRenderer>().color = uiGear.GetComponent<Image>().color = place.CurrentColor;
				}
				else isDraggingPlaceWithGear = false;
			}

			if (Input.GetMouseButton(0) && isDraggingPlaceWithGear)
			{
				//World gear
				Vector2 wgPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				worldGear.position = wgPos;

				//UI Gear
				uiGear.position = Input.mousePosition;
			}

			else if (Input.GetMouseButtonUp(0) && isDraggingPlaceWithGear)
			{
				DropGear();

				//If DROPPED ON UI
				PointerEventData pointerData = new PointerEventData(EventSystem.current);
				pointerData.position = Input.mousePosition;
				List<RaycastResult> results = new List<RaycastResult>();
				raycaster.Raycast(pointerData, results);

				if (results.Count > 0)
				{
					if(results[0].gameObject.TryGetComponent<HUDGear>(out var gear)) gear.GetComponentInParent<HUDSlot>().DropGearFromWorld();
					else if (results[0].gameObject.TryGetComponent<HUDSlot>(out var slot)) slot.DropGearFromWorld();
				}
				//ELSE
				else
				{
					gearPlaced.SetActive(true);
					gearPlaced.transform.localPosition = Vector3.zero;
				}

				gearPlaced = null;

			}
		}

		private void DropGear()
		{
			worldGear.position = new Vector2(0, 10);
			uiGear.anchoredPosition = Vector2.one * 400;
			isDraggingPlaceWithGear = false;
		}
	}

}