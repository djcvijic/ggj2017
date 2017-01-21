using UnityEngine;
using System.Collections.Generic;

public class StandsView : MonoBehaviour
{


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

	private List<SingleSeat> allSeats = new List<SingleSeat>();

	public void Reinitialize()
	{
		// remove existing seats
		foreach(var seats in allSeats)
			seats.gameObject.SetActive(false);
		
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

				singleSeat.Init(x, y);
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
