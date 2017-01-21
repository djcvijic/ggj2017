﻿using UnityEngine;
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
		AddWave(defaultRange, defaultSpeed, defaultBounces, defaultGoingRight, defaultTimeToStart);
	}

	public void AddWave(float range, float speed, int bounces = 0, bool goingRight = true, float timeToStart = 0f)
	{
		var wave = new Wave(range, speed, bounces, goingRight, timeToStart);
		allWaves.Add(wave);
	}

	public void AddGaussWave()
	{
		var wave = new Wave(gaussCurve, defaultRange, defaultBounces, defaultGoingRight, defaultTimeToStart);
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