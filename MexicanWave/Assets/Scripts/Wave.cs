using UnityEngine;
using System.Collections;

public class Wave
{

	public float centerPosition;
	public float range;
	public float speed;
	public int remainingBounces;

	public float LeftBorder { get { return centerPosition - range; } }
	public float RightBorder { get { return centerPosition + range; } }

	private bool goingRight;
	public AnimationCurve speedCurve;

	public float Speed
	{
		get
		{
			if (speedCurve != null)
			{
				float min = WaveController.I.positionMin, max = WaveController.I.positionMax;
				var val = Mathf.Clamp(centerPosition, min, max);
				return speedCurve.Evaluate((val-min)/(max-min));
			}
			return speed;
		}
	}

	public Wave(float range, float speed, int remainingBounces, bool initialRight = true)
	{
		this.centerPosition = initialRight ? WaveController.I.positionMin - range : WaveController.I.positionMax + range;
		this.range = range;
		this.speed = speed;
		this.remainingBounces = remainingBounces;
		this.goingRight = initialRight;
		this.speedCurve = null;
	}

	public Wave(AnimationCurve speed, float range, int remainingBounces = 0, bool initialRight = true)
	{
		this.centerPosition = initialRight ? WaveController.I.positionMin - range : WaveController.I.positionMax + range;
		this.speedCurve = speed;
		this.range = range;
		this.remainingBounces = remainingBounces;
		this.goingRight = initialRight;
	}

	public void UpdateWave()
	{
		if (goingRight)
		{
			centerPosition += Speed * Time.deltaTime;

			// check for end
			if (LeftBorder >= WaveController.I.positionMax)
			{
				CheckForEnd();
			}
		}
		else
		{
			centerPosition -= Speed * Time.deltaTime;

			// check for end
			if (RightBorder <= WaveController.I.positionMin)
			{
				CheckForEnd();
			}
		}
	}

	private void CheckForEnd()
	{
		if (remainingBounces == 0)
		{
			WaveController.I.WaveEnded(this);
		}
		else
		{
			remainingBounces--;
			goingRight = !goingRight;
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
