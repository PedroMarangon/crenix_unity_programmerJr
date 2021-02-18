// Maded by Pedro M Marangon
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CrenixTeste
{
	[RequireComponent(typeof(CanvasGroup))]
	public class HUDGear : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		[SerializeField] private Canvas canvas;
		[SerializeField] private Transform parentTransform;
		[SerializeField] private GearColor myGearColor;
		[Range(0,1), SerializeField] private float alphaWhenDragging;

		private RectTransform rectTransform;
		private CanvasGroup canvasGroup;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			canvasGroup = GetComponent<CanvasGroup>();
		}

		public void OnBeginDrag(PointerEventData eventData) => SetGroup(alphaWhenDragging, false);
		public void OnDrag(PointerEventData eventData) => rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
		public void OnEndDrag(PointerEventData eventData)
		{
			SetGroup(1, true);

			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if (hit.collider)
			{
				//Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);

				rectTransform.anchoredPosition = Vector2.zero;

				if (hit.collider.TryGetComponent<GearPlace>(out var gear) && !gear.HasGear)
				{
					gear.PlaceGear(myGearColor,GetComponent<Image>().color);
					SetGroup(0);
					if(transform.parent) transform.parent.GetComponent<HUDSlot>()?.BlockDrag();
				}
			}
		}

		public void Activate() => SetGroup(1f);
		private void SetGroup(float alpha, bool blockRaycasts = true)
		{
			canvasGroup.alpha = alpha;
			canvasGroup.blocksRaycasts = blockRaycasts;
			//transform.parent = blockRaycasts ? parentTransform : null;
			transform.SetParent(blockRaycasts ? parentTransform : canvas.transform);

		}
	}

}