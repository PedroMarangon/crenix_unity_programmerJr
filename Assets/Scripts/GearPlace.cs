// Maded by Pedro M Marangon
using CrenixTeste;
using UnityEngine;

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

		
		public void PlaceGear(GearColor color, Color rendColor)
		{
			rend.enabled = true;
			thisColor = color;

			rend.color = rendColor;
		}

		public void RemoveGear()
		{
			rend.enabled = false;
			thisColor = GearColor.None;

			rend.color = Color.white;
		}

	}

}