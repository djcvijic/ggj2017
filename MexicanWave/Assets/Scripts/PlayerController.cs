using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	#region Singleton

	private static PlayerController instance;
	public static PlayerController I
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<PlayerController>();
			}
			return instance;
		}
	}

	#endregion

	public List<HumanPlayer> Players { get; private set; }

	public List<Slider> awkwardnessSliders;
	public List<Image> awkwardnessSliderBacks;
	public List<Text> awkwardnessSliderText;

	public float maxAwkwardness = 1f;

	void Awake()
	{
		Players = new List<HumanPlayer>();
		for (int i = 0; i < 8; i++)
		{
			Players.Add(new HumanPlayer() { keyCode = KeyCode.None });
		}
	}

	public void PickPlayers()
	{
		var centerX = StandsController.I.width / 2;
		var centerY = StandsController.I.height / 2;
		Players.ForEach(p => p.x = p.y = 0);
		for (int i = 0; i < Players.Count; i++)
		{
			var player = Players[i];
			int x, y;
			do
			{
				x = Random.Range(centerX - 4, centerX + 4);
				y = Random.Range(centerY - 2, centerY + 2);
			} while (Players.Exists(p => p.isActive && p.x == x && p.y == y));
			player.x = x;
			player.y = y;
			var seat = StandsView.I.At(x, y);
			player.isStanding = false;
			seat.DeactivateAccessories();
			player.awkwardness = 0f;
			player.isDead = false;
			player.isActive = false;
			player.keyCode = KeyCode.None;
		}

		for (int i = 0; i < awkwardnessSliders.Count; i++)
		{
			var player = Players[i];
			awkwardnessSliders[i].value = 0f;
			awkwardnessSliderText[i].color = Color.black;
			awkwardnessSliderBacks[i].color = player.isActive ? Color.white : Color.gray;
		}
	}

	public void PrepareForStart()
	{
		for (int i = 0; i < Players.Count; i++)
		{
			var player = Players[i];
			if (player.isActive)
			{
				player.isStanding = false;
			}
		}
	}

	public float StandingValue(int id)
	{
		return Players[id].isStanding ? 1f : 0f;
	}

	public void CheckPlayers()
	{
		// activate new players
		if (GameController.I.IsStarting && Input.anyKeyDown)
		{
			foreach(KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
			{
				if (kcode == KeyCode.Escape || kcode == KeyCode.Space 
					|| (kcode >= KeyCode.JoystickButton0 && kcode <= KeyCode.JoystickButton19))
					continue;
				if (Input.GetKeyDown(kcode) && !Players.Exists(p => p.keyCode == kcode))
				{
					Debug.Log("KeyCode down: " + kcode);
					var activatedPlayer = Players.Find(p => !p.isActive);
					activatedPlayer.keyCode = kcode;
					activatedPlayer.isActive = true;
					activatedPlayer.isStanding = true;
					activatedPlayer.justActivated = true;

					var seat = StandsView.I.At(activatedPlayer.x, activatedPlayer.y);
					seat.playedId = Players.FindIndex(p => p == activatedPlayer);
					seat.InvertColor();
					seat.DeactivateAccessories();
					awkwardnessSliderBacks[seat.playedId].color = Color.white;
				}
			}
		}

		int alivePlayers = 0, activePlayers = 0;
		for (int i = 0; i < Players.Count; i++)
		{
			var player = Players[i];
			if (player.isDead || !player.isActive)
				continue;
			
			if (Input.GetKeyDown(player.keyCode) && !player.justActivated)
			{
				player.isStanding = !player.isStanding;
			}

			// do we need to deactivate this one
			if (GameController.I.IsStarting && Input.GetKeyDown(player.keyCode))
			{
				if (player.justActivated)
				{
					player.justActivated = false;
				}
				else
				{
					Debug.Log("DEACTIVATE" + player.keyCode);
					player.isActive = false;
					player.isStanding = false;
					var seat = StandsView.I.At(player.x, player.y);

					seat.playedId = -1;
					player.keyCode = KeyCode.None;
					seat.ResetColor();
					awkwardnessSliderBacks [i].color = Color.gray;
				}
			}

			// check if player is in right position
			if (GameController.I.IsPlaying && player.isActive)
			{
				var val = StandsController.I.Seats [player.x, player.y];
				if ((val <= 0.3f && player.isStanding) || (val >= 0.7f && !player.isStanding))
				{
					player.awkwardness += Time.deltaTime;
					if (player.awkwardness >= maxAwkwardness)
					{
						player.isDead = true;
						awkwardnessSliderText[i].color = Color.white;
					}
				}
			}
			if (!player.isDead && player.isActive) { alivePlayers++; }
			if (player.isActive) { activePlayers++; }
		}
		// check for end game
		if (GameController.I.IsPlaying)
		{
			if (alivePlayers == 1 && activePlayers > 1)
			{
				var winner = Players.FindIndex(p => !p.isDead);
				GameController.I.SwitchToEndGame(winner);
			}
			else if (alivePlayers == 0)
			{
				GameController.I.SwitchToEndGame(activePlayers > 1 ? -1 : -2);
			}
			return;
		}
	}

	void Update()
	{
		for (int i = 0; i < Players.Count; i++)
		{
			var player = Players[i];
			awkwardnessSliders[i].value = player.awkwardness / maxAwkwardness;

			awkwardnessSliderText[i].text = player.isDead ? "DEAD" :
				(player.isActive ? player.keyCode.ToString() : "ANY KEY");
		}
	}
}
