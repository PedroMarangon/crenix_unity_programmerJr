// Maded by Pedro M Marangon
using TMPro;
using UnityEngine;

namespace CrenixTeste
{
	public class GearRotationManager : MonoBehaviour
	{
		[TextArea, SerializeField] private string baseText, finishText;
		[SerializeField] private TMP_Text text;
		[SerializeField] private Inventory inventory;
		[SerializeField] private WorldGear pinkGear, blueGear, yellowGear, greenGear, purpleGear;

		//Set the base text on the speech bubble at the start
		private void Awake() => SetBaseText();

		public void SetBaseText() => text.text = baseText;

		/// <summary>
		/// Call gear.MatchColor() for every single gear, and if all are true, then change the text and starts rotating
		/// </summary>
		public void VerifyIfMatchesColors()
		{
			bool matchPink = pinkGear.MatchColor(inventory.Pink);
			bool matchBlue = blueGear.MatchColor(inventory.Blue);
			bool matchYellow = yellowGear.MatchColor(inventory.Yellow);
			bool matchGreen = greenGear.MatchColor(inventory.Green);
			bool matchPurple = purpleGear.MatchColor(inventory.Purple);


			if (matchPink && matchBlue && matchYellow && matchGreen && matchPurple)
			{
				text.text = finishText;

				pinkGear.StartRotating();
				blueGear.StartRotating();
				yellowGear.StartRotating();
				greenGear.StartRotating();
				purpleGear.StartRotating();
			}
			else
			{
				text.text = baseText;

				pinkGear.StopRotating();
				blueGear.StopRotating();
				yellowGear.StopRotating();
				greenGear.StopRotating();
				purpleGear.StopRotating();
			}
		}
	}

}