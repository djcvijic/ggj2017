using UnityEngine;
using System.Collections;

public class SingleSeat : MonoBehaviour
{

	public int x { get; private set; }
	public int y { get; private set; }

	private Renderer myRenderer;

	void Awake()
	{
		myRenderer = GetComponentInChildren<Renderer>();
	}

	public void Init(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	void Update()
	{
		var val = StandsController.I.Value(x, y);
		myRenderer.material.color = new Color(val, 0.5f, 0.5f, 0.5f);
	}

}
