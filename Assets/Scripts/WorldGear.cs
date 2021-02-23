// Maded by Pedro M Marangon
using UnityEngine;

namespace CrenixTeste
{
	public class WorldGear : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer rend;
		[SerializeField] private float rotationSpeed = 5;
		[SerializeField] private bool rotateClockwise = true;
		private bool isDragging;
		private Inventory inventory;
		private bool isRotating;

		//Get the image color
		public Color GetColor => rend.color;
		//Verify if it has a gear
		public bool HasGear { get; private set; }

		//Basic setup
		private void Awake()
		{
			inventory = FindObjectOfType<Inventory>();
			Deactivate();
		}

		/// <summary>
		/// Set the color of the Sprite Renderer
		/// </summary>
		/// <param name="draggedColor">the color to set</param>
		public void SetColor(Color draggedColor) => rend.color = draggedColor;

		/// <summary>
		/// Activates the renderer and define that it has a gear
		/// </summary>
		public void Activate() => rend.enabled = HasGear = true;

		/// <summary>
		/// Deactivates the renderer and define that it has a gear
		/// </summary>
		public void Deactivate() => rend.enabled = HasGear = false;

		/// <summary>
		/// Verifies if the requested color and the renderer's color matches, while seeing if it has a gear
		/// </summary>
		/// <param name="requestedColor">The color to compare with the renderer's color</param>
		/// <returns>if the requested color matches with the renderer's color and if it has a gear</returns>
		public bool MatchColor(Color requestedColor) => (GetColor == requestedColor) && HasGear;

		/// <summary>
		/// Start rotating the gear
		/// </summary>
		public void StartRotating() => isRotating = true;

		/// <summary>
		/// Stop rotating the gear
		/// </summary>
		public void StopRotating() => isRotating = false;

		/// <summary>
		/// Rotate the gear if it's rotating
		/// </summary>
		private void Update()
		{
			if (isRotating) rend.transform.Rotate(Vector3.forward * rotationSpeed * (rotateClockwise ? -1 : 1));
		}

		/// <summary>
		/// Activates the visual dragging, set it to Input.mousePosition, and deactivate this world gear
		/// </summary>
		private void OnMouseDown()
		{
			if (!HasGear) return;

			inventory.ActivateVisualDrag(GetColor);
			inventory.SetVisualDragPos(Input.mousePosition);
			Deactivate();
			isDragging = true;
		}
		//Update the visual position
		private void OnMouseDrag()
		{
			if (!isDragging || HasGear) return;

			inventory.SetVisualDragPos(Input.mousePosition);
		}
		//Deactivate the visual dragging and try to place the gear in the UI
		private void OnMouseUp()
		{
			if (!isDragging) return;
			inventory.SetVisualDragPos(Input.mousePosition);
			inventory.DeactivateVisualDrag();

			inventory.TryToPlaceInUI(GetColor, this);
			isDragging = false;
		}
	}

}