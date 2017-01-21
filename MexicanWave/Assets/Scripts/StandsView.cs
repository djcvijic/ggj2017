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
		for (int i = 0; i < StandsController.I.width; i++)
		{
			for (int j = 0; j < StandsController.I.height; j++)
			{
				var index = i * StandsController.I.width + j;
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

				singleSeat.Init(i, j);
				singleSeat.transform.parent = parentContainer;
				singleSeat.transform.localPosition = new Vector2(i, j);
			}
		}
	}
}
