using UnityEngine;
using System.Collections;

public static class Util
{

	public static float RandomOffset(float offset)
	{
		return (Random.value > 0.5f ? -1 : 1) * offset * Random.value;
	}

}
