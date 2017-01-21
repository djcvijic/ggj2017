using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	#region Singleton

	private static GameController instance;
	public static GameController I
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameController>();
			}
			return instance;
		}
	}

	#endregion

	public RectTransform infoPanel;
	public Text infoText;

	private float timeToRestartGame;

	public enum State
	{
		StartingGame,
		Playing,
		EndGame
	}

	public State CurrentState { get; private set; }
	public bool IsPlaying { get { return CurrentState == State.Playing; } }

	void Start()
	{
		SwithToStartingGame();
	}


	public void SwithToStartingGame()
	{
		CurrentState = State.StartingGame;
		infoText.text = "Activate players, press SPACE when ready";
		infoPanel.gameObject.SetActive(true);
		StandsController.I.StartNewGame();
	}

	public void SwitchToPlaying()
	{
		CurrentState = State.Playing;
		infoPanel.gameObject.SetActive(false);
		PlayerController.I.PrepareForStart();
	}

	public void SwitchToEndGame(int winner)
	{
		timeToRestartGame = 3f;
		CurrentState = State.EndGame;
		infoText.text = (winner >= 0 ? ("Player " + (winner + 1) + " won!") : "Nobody won!" ) + " Press SPACE to restart.";
		infoPanel.gameObject.SetActive(true);
	}

	void Update()
	{
		if (CurrentState == State.StartingGame)
		{
			if (Input.GetKeyDown(KeyCode.Space) && PlayerController.I.Players.Exists(p => p.isActive))
			{
				SwitchToPlaying();
			}
		}
		else if (CurrentState == State.EndGame)
		{
			timeToRestartGame -= Time.deltaTime;
			if (timeToRestartGame <= 0f && Input.GetKeyDown(KeyCode.Space))
			{
				SwithToStartingGame();
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SwithToStartingGame();
		}
	}

}
