using UnityEngine;
using System.Collections;

public class SingleSeat : MonoBehaviour
{
	public int x { get; private set; }
	public int y { get; private set; }
	public StandsView view { get; private set; }
	public Sprite[] humanSprites;

	private SpriteRenderer myRenderer;

	void Awake()
	{
		myRenderer = transform.Find("Human").GetComponent<SpriteRenderer>();
		humanSprites = Resources.LoadAll<Sprite>("Sprites/ljudi");
		//Debug.Log(humanSprites.Length);
	}

	public void Init(int x, int y, StandsView view)
	{
		this.x = x;
		this.y = y;
		this.view = view;
	}

	void Update()
	{
		var val = StandsController.I.Value(x, y);
		//myRenderer.material.color = new Color(val, 0.5f, 0.5f, 0.5f);

		int index = Mathf.FloorToInt(humanSprites.Length * val);
		//Debug.Log(index);
		myRenderer.sprite = humanSprites[index];
	}

}
