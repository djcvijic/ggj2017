using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorExtensions
{

	[MenuItem("GGJ2017/New game")]
	private static void NewGame()
	{
		if (!Application.isPlaying)
			return;
		StandsController.I.StartNewGame();
	}

	[MenuItem("GGJ2017/Add simple wave")]
	private static void AddSimpleWave()
	{
		if (!Application.isPlaying)
			return;
		WaveController.I.AddSimpleWave();
	}

	[MenuItem("GGJ2017/Clear all waves")]
	private static void ClearAllWaves()
	{
		if (!Application.isPlaying)
			return;
		WaveController.I.ClearWaves();
	}
}
