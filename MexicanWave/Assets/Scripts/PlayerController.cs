using UnityEngine;
using System.Collections.Generic;

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

	public List<HumanPlayer> players;
	public List<HumanPlayer> activePlayers;

	void Awake()
	{
		players = new List<HumanPlayer>()
		{
			new HumanPlayer() { keyCode = KeyCode.A },
			new HumanPlayer() { keyCode = KeyCode.L },
			new HumanPlayer() { keyCode = KeyCode.F },
			new HumanPlayer() { keyCode = KeyCode.J },
		};
	}

	public void ActivatePlayers(int count)
	{
		var centerX = StandsController.I.width / 2;
		var centerY = StandsController.I.height / 2;
		activePlayers = players.GetRange(0, count);
		activePlayers.ForEach(p => p.x = p.y = 0);
		for (int i = 0; i < activePlayers.Count; i++)
		{
			var player = activePlayers[i];
			int x, y;
			do
			{
				x = Random.Range(centerX - 4, centerX + 4);
				y = Random.Range(centerY - 2, centerY + 2);
			} while (activePlayers.Exists(p => p.x == x && p.y == y));
			player.x = x;
			player.y = y;
			var seat = StandsView.I.At(x, y);
			seat.playedId = i;
			seat.InvertColor();
			Debug.Log("NEW PLAYER " + x + " " + y);
		}
	}

	public float StandingValue(int id)
	{
		return activePlayers[id].isStanding ? 1f : 0f;
	}

	public void CheckPlayers()
	{
		for (int i = 0; i < activePlayers.Count; i++)
		{
			var player = activePlayers[i];
			if (Input.GetKeyDown(player.keyCode))
			{
				player.isStanding = !player.isStanding;
			}

			// check if player is in right position
			var val = StandsController.I.Seats[player.x, player.y];
			if ((val <= 0.1f && player.isStanding) || (val >= 0.9f && !player.isStanding))
			{
				Debug.Log(player.isStanding ? "STANDING!" : "SITTING!");
			}
		}
	}
}
