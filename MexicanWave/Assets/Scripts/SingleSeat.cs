using UnityEngine;
using System.Collections;

public class SingleSeat : MonoBehaviour
{
	public StandsView view;
	public int x { get; private set; }
	public int y { get; private set; }

	public Transform myHuman;
	public Sprite[] humanSprites;

	public int playedId;

	private SpriteRenderer myRenderer;
	private Vector3 initialHumanPosition;
	private float animationVal;
	public float animationSpeed;

	public SpriteRenderer seat;
	public GameObject[] accessories;
	private Transform myAccessory;
	private Vector3 initialAccessoryPosition;

	private Color oldColor;

	void Awake()
	{
		myRenderer = myHuman.GetComponent<SpriteRenderer>();
		humanSprites = Resources.LoadAll<Sprite>("Sprites/ljudi");
		initialHumanPosition = myHuman.transform.localPosition;
	}

	public void Init(StandsView view, int x, int y, Color color, Color accessoryColor)
	{
		this.view = view;
		this.x = x;
		this.y = y;

		myRenderer.color = color;

		myHuman.transform.localPosition = initialHumanPosition;
		myHuman.transform.Translate(
			(float)((Random.value - 0.5) * view.humanJitterHorizontal),
			(float)((Random.value - 0.5) * view.humanJitterVertical),
			0);

		playedId = -1;
		myRenderer.enabled = true;

		DeactivateAccessories();
		if (Random.value < view.accessoryProbability)
		{
			var randomAccessory = accessories[Mathf.FloorToInt(Random.value * accessories.Length)];
			randomAccessory.SetActive(true);
			myAccessory = randomAccessory.transform;
			initialAccessoryPosition = myAccessory.localPosition;
			if (myAccessory.Equals(accessories[0].transform) && !initialAccessoryPosition.y.Equals(2.85f))
			{
				Debug.Log("inital position in init is " + initialAccessoryPosition.y);
			}
			randomAccessory.GetComponent<SpriteRenderer>().color = accessoryColor;
		}

		// fix sprite ordering
		seat.sortingOrder = -y * 3;
		myRenderer.sortingOrder = -y * 3 + 1;
		for (int i = 0; i < accessories.Length; i++)
		{
			accessories[i].GetComponent<SpriteRenderer>().sortingOrder = -y * 3 + 2;
		}
	}

	public void InvertColor()
	{
		var myColor = oldColor = myRenderer.color;
		var currentBrightness = (myColor.r + myColor.g + myColor.b) / 3;

		var inverseColor = Color.white - myColor;
		var newBrightness = (inverseColor.r + inverseColor.g + inverseColor.b) / 3;
		var brightnessQuotient = currentBrightness / newBrightness;
		inverseColor *= brightnessQuotient;
		inverseColor.a = 1.0f;

		myRenderer.color = inverseColor;
	}

	public void ResetColor()
	{
		myRenderer.color = oldColor;
	}

	public void DeactivateAccessories()
	{
		foreach (GameObject accessory in accessories)
		{
			accessory.SetActive(false);
		}
	}

	public void Reinitialize()
	{
		seat.gameObject.SetActive(false);
		if (myAccessory != null)
		{
			myAccessory.localPosition = initialAccessoryPosition;
		}
	}

	void Update()
	{
		var val = playedId != -1 ? PlayerController.I.StandingValue(playedId) : StandsController.I.Value(x, y);
		int index = Mathf.FloorToInt(humanSprites.Length * val);
		animationVal = Mathf.Lerp(animationVal, val, animationSpeed * Time.deltaTime);

		if (!animationVal.Approximately(val))
		{
			index = Mathf.FloorToInt(humanSprites.Length * animationVal);
		}

		if (index >= humanSprites.Length) index = humanSprites.Length - 1;
		//Debug.Log(index);
		myRenderer.sprite = humanSprites[index];
		if (playedId > -1)
		{
			myRenderer.enabled = !PlayerController.I.Players [playedId].isDead;
		}

		if (myAccessory != null)
		{
			var accessoryPositionQuotient = 1 - ((float)(1 + index) / humanSprites.Length);
			var accessoryPositionOffset = accessoryPositionQuotient * new Vector3(0, view.accessorySittingOffset, 0);
			if (myAccessory.Equals(accessories[0].transform) && !initialAccessoryPosition.y.Equals(2.85f))
			{
				Debug.Log("initial position in update is " + initialAccessoryPosition.y);
			}
			myAccessory.localPosition = initialAccessoryPosition + accessoryPositionOffset;
		}
	}

}
