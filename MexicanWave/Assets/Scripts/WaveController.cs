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

	public float defaultSpeed;
	public float defaultRange;
	public int defaultBounces;
	public bool defaultGoingRight = true;
	public float defaultTimeToStart;

	public float positionMin;
	public float positionMax;

	public float defaultOffset;

	public AnimationCurve gaussCurve;

	public void ClearWaves()
	{
		allWaves.Clear();
	}

	public void AddSimpleWave()
	{
		AddWave(defaultSpeed, defaultRange, defaultBounces, defaultGoingRight, defaultTimeToStart, true);
	}

	public void AddWave(float speed, float range, int bounces = 0, bool goingRight = true, float timeToStart = 0f, bool testWave = false)
	{
		var wave = new Wave(speed, range, bounces, goingRight, timeToStart, testWave);
		allWaves.Add(wave);
	}

	public void AddGaussWave()
	{
		var wave = new Wave(gaussCurve, defaultRange, defaultBounces, defaultGoingRight, defaultTimeToStart, true);
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

	public void StartFirstWave()
	{
		allWaves.AddRange(WaveGenerator.I.GetNextLevel());
	}

	public void WaveEnded(Wave wave)
	{
		allWaves.Remove(wave);
		if (!wave.testWave && allWaves.Count == 0 && GameController.I.IsPlaying)
		{
			allWaves.AddRange(WaveGenerator.I.GetNextLevel());
		}
	}

}