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

	public enum State
	{
		StartingGame,
		Playing,
		EndGame
	}

	public State CurrentState { get; private set; }
	public bool IsPlaying { get { return CurrentState == State.Playing; } }
	public bool IsStarting { get { return CurrentState == State.StartingGame; } }

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
		WaveGenerator.I.Reset();
	}

	public void SwitchToPlaying()
	{
		CurrentState = State.Playing;
		infoPanel.gameObject.SetActive(false);
		PlayerController.I.PrepareForStart();
		WaveController.I.StartFirstWave();
	}

	public void SwitchToEndGame(HumanPlayer winner)
	{
		CurrentState = State.EndGame;
		infoText.text = ((winner != null && !winner.isDead)
			? ("Player " + winner.keyCode + " won!") 
			: (winner == null ? "Nobody won!" : "Nice practice!")) 
			+ " Press SPACE to restart.";
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
			if (Input.GetKeyDown(KeyCode.Space))
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
