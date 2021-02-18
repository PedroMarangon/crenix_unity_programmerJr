// Maded by Pedro M Marangon
using UnityEngine;

namespace CrenixTeste
{

	public class GearRotator : MonoBehaviour
	{

		private enum RotationWay { Clockwise, Anticlockwise}

		[SerializeField] private float rotateSpeed = 20f;
		[SerializeField] private RotationWay rotationWay;
		private float clockDir;

		private void Update()
		{
			clockDir = (rotationWay == RotationWay.Clockwise) ? -1 : 1;
			transform.Rotate(Vector3.forward * rotateSpeed * clockDir);
		}

	}

}