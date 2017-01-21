using UnityEngine;
using System.Collections;

public static class Util
{

	public static float RandomOffset(float offset)
	{
		return (Random.value > 0.5f ? -1 : 1) * offset * Random.value;
	}

	public static bool RandomBool()
	{
		return Random.value > 0.5f;
	}

	public static bool Approximately(this float val, float other)
	{
		return Mathf.Abs(val - other) < 0.01f;
	}

}
