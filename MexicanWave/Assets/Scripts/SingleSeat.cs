using UnityEngine;
using System.Collections;

public class SingleSeat : MonoBehaviour
{
	public int x { get; private set; }
	public int y { get; private set; }
	public Sprite[] humanSprites;

	public int playedId;

	private SpriteRenderer myRenderer;
	private float animationVal;
	public float animationSpeed;

	void Awake()
	{
		myRenderer = transform.Find("Human").GetComponent<SpriteRenderer>();
		humanSprites = Resources.LoadAll<Sprite>("Sprites/ljudi");
	}

	public void Init(int x, int y, Color color)
	{
		this.x = x;
		this.y = y;
		myRenderer.color = color;
		playedId = -1;
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
	}

}
