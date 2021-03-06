﻿using UnityEngine;
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

	public GameObject hitParticlePrefab;

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
			} while (Players.Exists(p => p.x == x && p.y == y));
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

	private bool IsJoystickStartButton(KeyCode kcode)
	{
		return kcode == KeyCode.JoystickButton9
			|| kcode == KeyCode.Joystick1Button9
			|| kcode == KeyCode.Joystick2Button9
			|| kcode == KeyCode.Joystick3Button9
			|| kcode == KeyCode.Joystick4Button9
			|| kcode == KeyCode.Joystick5Button9
			|| kcode == KeyCode.Joystick6Button9
			|| kcode == KeyCode.Joystick7Button9
			|| kcode == KeyCode.Joystick8Button9;
	}

	private bool IsJoystickSelectButton(KeyCode kcode)
	{
		return kcode == KeyCode.JoystickButton8
			|| kcode == KeyCode.Joystick1Button8
			|| kcode == KeyCode.Joystick2Button8
			|| kcode == KeyCode.Joystick3Button8
			|| kcode == KeyCode.Joystick4Button8
			|| kcode == KeyCode.Joystick5Button8
			|| kcode == KeyCode.Joystick6Button8
			|| kcode == KeyCode.Joystick7Button8
			|| kcode == KeyCode.Joystick8Button8;
	}

	public void CheckPlayers()
	{
		// activate new players
		if (GameController.I.IsStarting && Input.anyKeyDown)
		{
			foreach(KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
			{
				if (kcode == KeyCode.Escape || kcode == KeyCode.Space
					|| IsJoystickStartButton(kcode) || IsJoystickSelectButton(kcode)
					|| (kcode >= KeyCode.JoystickButton0 && kcode <= KeyCode.JoystickButton19))
					continue;
				if (Input.GetKeyDown(kcode) && !Players.Exists(p => p.isActive && p.keyCode == kcode))
				{
					HumanPlayer activatedPlayer;
					// try to find previous position of this key
					activatedPlayer = Players.Find(p => p.keyCode == kcode);
					if (activatedPlayer == null)
					{
						// if can't find it, find any innactive one
						activatedPlayer = Players.Find(p => !p.isActive);
					}
					// if found it
					if (activatedPlayer != null)
					{
						activatedPlayer.keyCode = kcode;
						activatedPlayer.isActive = true;
						activatedPlayer.isStanding = true;
						activatedPlayer.justActivated = true;

						var seat = StandsView.I.At(activatedPlayer.x, activatedPlayer.y);
						seat.playedId = Players.FindIndex(p => p == activatedPlayer);
						seat.InvertColor();
						seat.DeactivateAccessories();
						awkwardnessSliderBacks [seat.playedId].color = Color.white;
						seat.PlayYeah();
					}
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
				var seat = StandsView.I.At(player.x, player.y);
				if (player.isStanding)
					seat.PlayYeah();
				else
					seat.PlaySitDown();
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
					player.isActive = false;
					player.isStanding = false;
					var seat = StandsView.I.At(player.x, player.y);

					seat.playedId = -1;
					seat.ResetColor();
					seat.PlaySitDown();
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
						Instantiate(hitParticlePrefab, StandsView.I.At(player.x, player.y).transform.position, Quaternion.identity);
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
				GameController.I.SwitchToEndGame(Players.Find(p => !p.isDead));
			}
			else if (alivePlayers == 0)
			{
				GameController.I.SwitchToEndGame(activePlayers == 1 ? Players.Find(p => p.isActive) : null);
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
