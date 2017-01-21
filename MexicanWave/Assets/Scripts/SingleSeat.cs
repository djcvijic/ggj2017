using UnityEngine;
using System.Collections;

public class SingleSeat : MonoBehaviour
{
	public StandsView view;
	public int x { get; private set; }
	public int y { get; private set; }
	public Sprite[] humanSprites;

	public int playedId;

	private Transform myHuman;
	private SpriteRenderer myRenderer;
	private Vector3 defaultPosition;
	private float animationVal;
	public float animationSpeed;

	void Awake()
	{
		myHuman = transform.Find("Human");
		myRenderer = myHuman.GetComponent<SpriteRenderer>();
		humanSprites = Resources.LoadAll<Sprite>("Sprites/ljudi");
		defaultPosition = new Vector3(0, -1.1f, 0);
	}

	public void Init(StandsView view, int x, int y, Color color)
	{
		this.view = view;
		this.x = x;
		this.y = y;

		myRenderer.color = color;

		myHuman.transform.localPosition = defaultPosition;
		myHuman.transform.Translate(
			(float)((Random.value - 0.5) * view.humanJitterHorizontal),
			(float)((Random.value - 0.5) * view.humanJitterVertical),
			0);

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
