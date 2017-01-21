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

	public float[,] Seats { get; private set; }

	public float[,] RandomOffset { get; private set; }

	public void Start()
	{
		StartNewGame();
	}

	void Update()
	{
		// update stands depending on waves
		for (int i = 0; i < width; i++)
		{
			var columnValue = WaveController.I.CalculateInfluence(i);
			for (int j = 0; j < height; j++)
			{
				if (columnValue > 0 && RandomOffset [i, j] == 0f)
				{
					RandomOffset[i, j] = Random.value * WaveController.I.defaultOffset;
				}
				else if (columnValue == 0 && RandomOffset [i, j] != 0f)
				{
					RandomOffset[i, j] = 0f;
				}
				Seats[i, j] = columnValue + RandomOffset[i, j];
			}
		}
		PlayerController.I.CheckPlayers();
	}

	public void StartNewGame(int w, int h)
	{
		RandomOffset = new float[w,h];
		Seats = new float[w,h];
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				//Seats[i, j] = i *1f / w;
				Seats[i, j] = 0f;
			}
		}
		StandsView.I.Reinitialize();
		WaveController.I.ClearWaves();
		PlayerController.I.ActivatePlayers(2);
	}

	public void StartNewGame()
	{
		StartNewGame(width, height);
	}

	public float Value(int x, int y)
	{
		return Seats[x, y];
	}

}
