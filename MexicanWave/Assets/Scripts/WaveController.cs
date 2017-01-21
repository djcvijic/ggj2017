using UnityEngine;
using System.Collections.Generic;

public class WaveController : MonoBehaviour
{
	#region Singleton

	private static WaveController instance;
	public static WaveController I
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<WaveController>();
			}
			return instance;
		}
	}

	#endregion

	private List<Wave> allWaves = new List<Wave>();

	public float DefaultSpeed;
	public float DefaultRange;

	public float positionMin;
	public float positionMax;

	public void ClearWaves()
	{
		allWaves.Clear();
	}

	public void AddSimpleWave()
	{
		var wave = new Wave(positionMin - DefaultRange, DefaultRange, DefaultSpeed);
		allWaves.Add(wave);
	}

	void Update()
	{
		for (int i = 0; i < allWaves.Count; i++)
		{
			allWaves[i].UpdateWave();
		}
	}

	public float CalculateInfluence(float val)
	{
		// stands are offseted
		val += positionMin;
		var influence = 0f;
		for (int i = 0; i < allWaves.Count; i++)
		{
			var currInfluence = allWaves[i].CalculateInfluence(val);
			if (currInfluence > influence)
			{
				influence = currInfluence;
			}
		}
		return influence;
	}

	public void WaveEnded(Wave wave)
	{
		Debug.Log("Wave ended");
		allWaves.Remove(wave);
	}

}