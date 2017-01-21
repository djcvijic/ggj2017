using UnityEngine;
using System.Collections.Generic;

public class StandsController : MonoBehaviour
{
	#region Singleton

	private static StandsController instance;
	public static StandsController I
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<StandsController>();
			}
			return instance;
		}
	}

	#endregion

	public int width;
	public int height;

	private float[,] seats;

	public void Start()
	{
		StartNewGame(width, height);
		StandsView.I.Reinitialize();
	}

	public void StartNewGame(int w, int h)
	{
		seats = new float[w,h];
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				seats[i, j] = 0f;
			}
		}
	}

	public float Value(int x, int y)
	{
		return seats[x, y];
	}
}
