using UnityEngine;
using System.Collections;

public class Wave
{

	public float centerPosition;
	public float range;
	public float speed;

	public float LeftBorder { get { return centerPosition - range; } }
	public float RightBorder { get { return centerPosition + range; } }

	public Wave(float centerPosition, float range, float speed)
	{
		this.centerPosition = centerPosition;
		this.range = range;
		this.speed = speed;
	}

	public void UpdateWave()
	{
		centerPosition += speed * Time.deltaTime;
		Debug.Log(centerPosition);

		// check for end
		if (LeftBorder >= WaveController.I.positionMax)
		{
			WaveController.I.WaveEnded(this);
		}
	}

	// 0 0 0 LEFT 0 - 1 Center 1 - 0 Right 0 0 0
	public float CalculateInfluence(float val)
	{
		if (val < LeftBorder || val > RightBorder)
			return 0f;

		if (val < centerPosition)
		{
			// 0 in left border, 1 in center position
			return (val - LeftBorder) / (centerPosition - LeftBorder);
		}
		else
		{
			// 1 in center position, 0 in right border
			return 1f - (val - centerPosition) / (RightBorder - centerPosition);
		}
	}

}
