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

	[MenuItem("GGJ2017/Add double wave")]
	private static void AddDoubleWave()
	{
		if (!Application.isPlaying)
			return;
		WaveController.I.AddWave(3f, 12f, 1, true);
		WaveController.I.AddWave(3f, 12f, 1, false);
	}

	[MenuItem("GGJ2017/Add gauss wave")]
	private static void AddGaussWave()
	{
		if (!Application.isPlaying)
			return;
		WaveController.I.AddGaussWave();
	}

	[MenuItem("GGJ2017/Clear all waves")]
	private static void ClearAllWaves()
	{
		if (!Application.isPlaying)
			return;
		WaveController.I.ClearWaves();
	}
}
