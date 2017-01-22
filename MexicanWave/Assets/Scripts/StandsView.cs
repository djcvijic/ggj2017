using UnityEngine;
using System.Collections.Generic;

public class StandsView : MonoBehaviour
{
	public List<ColorPick> TeamColors;

	[System.Serializable]
	public class ColorPick
	{
		public Color mainColor;
		public List<Color> secondaryColors;
	}


	#region Singleton

	private static StandsView instance;
	public static StandsView I
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<StandsView>();
			}
			return instance;
		}
	}

	#endregion

	public GameObject singleSeatPrefab;
	public Transform parentContainer;

	public float humanJitterHorizontal;
	public float humanJitterVertical;

	public float accessoryProbability;
	public float accessorySittingOffset;

	private List<SingleSeat> allSeats = new List<SingleSeat>();

	public void Reinitialize()
	{
		var firstId = Mathf.FloorToInt(Random.value * TeamColors.Count);
		var first = TeamColors[firstId];
		var firstColor = first.mainColor;
		var secondColor = firstColor;
		while (secondColor == firstColor)
		{
			secondColor = TeamColors[firstId].secondaryColors[Mathf.FloorToInt(Random.value * TeamColors[firstId].secondaryColors.Count)];
		}

		// remove existing seats
		foreach (var seat in allSeats)
			seat.Reinitialize();
		
		// create new ones
		for (int y = 0; y < StandsController.I.height; y++)
		{
			for (int x = 0; x < StandsController.I.width; x++)
			{
				var index = y * StandsController.I.width + x;
				SingleSeat singleSeat;
				if (index < allSeats.Count)
				{
					singleSeat = allSeats[index];
					singleSeat.gameObject.SetActive(true);
				}
				else
				{
					singleSeat = (Instantiate(singleSeatPrefab)).GetComponent<SingleSeat>();
					allSeats.Add(singleSeat);
				}

				var randomBlend = Random.value;
				var color = firstColor * randomBlend + secondColor * (1 - randomBlend);

				singleSeat.Init(this, x, y, color, firstColor);
				singleSeat.transform.parent = parentContainer;
				singleSeat.transform.localPosition = new Vector2(x, y);
			}
		}
	}

	public SingleSeat At(int x, int y)
	{
		return allSeats[y * StandsController.I.width + x];
	}
}
