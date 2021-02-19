// Maded by Pedro M Marangon
using CrenixTeste;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CrenixTeste
{
	public enum GearColor { None, Pink, Yellow, Purple, Blue, Green }

	public class GearPlace : MonoBehaviour
	{

		[SerializeField] private GearColor thisCorrectColor;
		[SerializeField] private SpriteRenderer rend;
		[SerializeField] private bool hasGear;
		private GearColor thisColor;

		public bool HasGear => hasGear;
		public bool CorrectColor => thisColor == thisCorrectColor;

		public Color CurrentColor => rend.color;
		public Color OldColor { get; private set; }
		public GearColor OldGearColor { get; private set; }
		
		public void PlaceGear(GearColor color, Color rendColor)
		{
			rend.enabled = true;
			thisColor = color;

			rend.color = rendColor;
			hasGear = true;
		}

		public void RemoveGear()
		{
			rend.enabled = false;

			OldGearColor = thisColor;
			thisColor = GearColor.None;

			OldColor = rend.color;

			rend.color = Color.white;
			hasGear = false;
		}

	}

}