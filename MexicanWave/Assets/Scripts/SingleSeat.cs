using UnityEngine;
using System.Collections;

public class SingleSeat : MonoBehaviour
{

	public int x { get; private set; }
	public int y { get; private set; }
	public StandsView view { get; private set; }
	public Color color { get; private set; }

	private Renderer myRenderer;

	void Awake()
	{
		myRenderer = GetComponentInChildren<Renderer>();
	}

	public void Init(int x, int y, StandsView view)
	{
		this.x = x;
		this.y = y;
		this.view = view;
		this.color = new Color(Random.value, Random.value, Random.value);
	}

	void Update()
	{
		var val = StandsController.I.Value(x, y);
		myRenderer.material.color = new Color(color.r, color.g, color.b, Mathf.Clamp(val, 0.1f, 1f));
	}

}
