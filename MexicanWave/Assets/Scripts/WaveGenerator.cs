using UnityEngine;
using System.Collections.Generic;

public class WaveGenerator : MonoBehaviour
{
	#region Singleton

	private static WaveGenerator instance;
	public static WaveGenerator I
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<WaveGenerator>();
			}
			return instance;
		}
	}

	#endregion

	[System.Serializable]
	public class WaveData
	{
		public float range;
		public float speed;
		public int remainingBounces;
		public bool goingRight;
		public bool goingRandom;
		public float timeToStart;
	}

	[System.Serializable]
	public class WaveLevel
	{
		public int level;
		public List<WaveData> waves;
	}

	public List<WaveLevel> levels;

	private int currentLevel = 0;

	public void Reset()
	{
		currentLevel = 0;
	}

	public List<Wave> GetNextLevel()
	{
		// move level up
		if (++currentLevel == levels.Count)
		{
			// end of game, put infinite wave
			Debug.Log("END");
			return new List<Wave>() { new Wave(15f, 2f, -1, Util.RandomBool()) };
		}
		var ret = new List<Wave>();
		for (int i = 0; i < levels[currentLevel].waves.Count; i++)
		{
			var data = levels[currentLevel].waves[i];
			ret.Add(new Wave(data.speed, data.range, data.remainingBounces, 
				data.goingRandom ? data.goingRight : Util.RandomBool(), data.timeToStart));
		}
		return ret;
	}
}
