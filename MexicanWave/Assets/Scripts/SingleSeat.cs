using UnityEngine;
using System.Collections;

public class SingleSeat : MonoBehaviour
{
	public int x { get; private set; }
	public int y { get; private set; }
	public Sprite[] humanSprites;

	private SpriteRenderer myRenderer;

	void Awake()
	{
		myRenderer = transform.Find("Human").GetComponent<SpriteRenderer>();
		humanSprites = Resources.LoadAll<Sprite>("Sprites/ljudi");
		//Debug.Log(humanSprites.Length);
	}

	public void Init(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	void Update()
	{
		var val = StandsController.I.Value(x, y);
		//myRenderer.material.color = new Color(val, 0.5f, 0.5f, 0.5f);

		int index = Mathf.FloorToInt(humanSprites.Length * val);
		if (index >= humanSprites.Length) index = humanSprites.Length - 1;
		//Debug.Log(index);
		myRenderer.sprite = humanSprites[index];
	}

}
