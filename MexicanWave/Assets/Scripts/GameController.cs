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

	public CanvasGroup infoPanel;
	public Text infoText;
	public CanvasGroup introBackground;
	public CanvasGroup logoPanel;
	public CanvasGroup introPanel;
	public CanvasGroup firstIntroPanel;
	public CanvasGroup secondIntroPanel;
	public CanvasGroup pressAnyKeyIntro;

	public enum State
	{
		Logo,
		Intro,
		StartingGame,
		Playing,
		EndGame
	}

	public State CurrentState { get; private set; }
	public bool IsFinishedIntro { get { return CurrentState > State.Intro; } }
	public bool IsPlaying { get { return CurrentState == State.Playing; } }
	public bool IsStarting { get { return CurrentState == State.StartingGame; } }

	void Start()
	{
		introBackground.alpha = 1f;
		logoPanel.alpha = 0f;
		introPanel.alpha = 0f;
		infoPanel.alpha = 1f;
		firstIntroPanel.alpha = secondIntroPanel.alpha = pressAnyKeyIntro.alpha = 0f;
		SwitchToLogo();
	}

	public void SwitchToLogo()
	{
		CurrentState = State.Logo;
		Go.to(logoPanel, 2.5f, new GoTweenConfig()
			.floatProp("alpha", 1f)
			.setDelay(0.5f)
			.onComplete(
				t => Go.to(logoPanel, 1f, new GoTweenConfig()
					.floatProp("alpha", 0f)
					.setDelay(1.5f)
					.onComplete(t2 => SwitchToIntro())
				)
			)
		);
	}

	public void SwitchToIntro()
	{
		CurrentState = State.Intro;
		Go.to(introPanel, 1.5f, new GoTweenConfig().floatProp("alpha", 1f));
		Go.to(firstIntroPanel, 2f, new GoTweenConfig().floatProp("alpha", 1f).setDelay(0.1f));
		Go.to(secondIntroPanel, 2f, new GoTweenConfig().floatProp("alpha", 1f).setDelay(6.5f));
		Go.to(pressAnyKeyIntro, 2f, new GoTweenConfig().floatProp("alpha", 1f).setDelay(11f));
	}

	public void EndIntro()
	{
		SwithToStartingGame();
		Go.to(introPanel, 1f, new GoTweenConfig()
			.floatProp("alpha", 0f)
		);
		Go.to(introBackground, 1f, new GoTweenConfig().floatProp("alpha", 0f));
	}

	public void SwithToStartingGame()
	{
		CurrentState = State.StartingGame;
		infoText.text = "Activate players, press SPACE/START when ready";
		Go.to(infoPanel, 0.4f, new GoTweenConfig().floatProp("alpha", 1f));
		StandsController.I.StartNewGame();
		WaveGenerator.I.Reset();
	}

	public void SwitchToPlaying()
	{
		CurrentState = State.Playing;
		Go.to(infoPanel, 0.4f, new GoTweenConfig().floatProp("alpha", 0f));
		PlayerController.I.PrepareForStart();
		WaveController.I.StartFirstWave();
	}

	public void SwitchToEndGame(HumanPlayer winner)
	{
		CurrentState = State.EndGame;
		infoText.text = ((winner != null && !winner.isDead)
			? ("Player " + winner.keyCode + " won!") 
			: (winner == null ? "Nobody won!" : "Nice practice!")) 
			+ " Press SPACE/START to restart.";
		Go.to(infoPanel, 0.4f, new GoTweenConfig().floatProp("alpha", 1f));
	}

	void Update()
	{
		if (CurrentState == State.Intro)
		{
			if (Input.anyKeyDown)
			{
				EndIntro();
			}
		}
		else if (CurrentState == State.StartingGame)
		{
			if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton9))
				&& PlayerController.I.Players.Exists(p => p.isActive))
			{
				SwitchToPlaying();
			}
		}
		else if (CurrentState == State.EndGame)
		{
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton9))
			{
				infoPanel.alpha = 0f;
				SwithToStartingGame();
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SwithToStartingGame();
		}
	}

}
