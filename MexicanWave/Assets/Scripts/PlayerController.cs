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

	public List<HumanPlayer> Players { get; private set; }
	public List<HumanPlayer> ActivePlayers { get; private set; }

	public float maxAwkwardness = 1f;

	void Awake()
	{
		Players = new List<HumanPlayer>()
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
		ActivePlayers = Players.GetRange(0, count);
		ActivePlayers.ForEach(p => p.x = p.y = 0);
		for (int i = 0; i < ActivePlayers.Count; i++)
		{
			var player = ActivePlayers[i];
			int x, y;
			do
			{
				x = Random.Range(centerX - 4, centerX + 4);
				y = Random.Range(centerY - 2, centerY + 2);
			} while (ActivePlayers.Exists(p => p.x == x && p.y == y));
			player.x = x;
			player.y = y;
			var seat = StandsView.I.At(x, y);
			seat.playedId = i;
			seat.InvertColor();
			player.isStanding = false;
			player.awkwardness = 0f;
			player.isDead = false;
			Debug.Log("NEW PLAYER " + x + " " + y);
		}
	}

	public float StandingValue(int id)
	{
		return ActivePlayers[id].isStanding ? 1f : 0f;
	}

	public void CheckPlayers()
	{
		for (int i = 0; i < ActivePlayers.Count; i++)
		{
			var player = ActivePlayers[i];
			if (player.isDead)
				continue;
			
			if (Input.GetKeyDown(player.keyCode))
			{
				player.isStanding = !player.isStanding;
			}

			// check if player is in right position
			var val = StandsController.I.Seats[player.x, player.y];
			if ((val <= 0.3f && player.isStanding) || (val >= 0.7f && !player.isStanding))
			{
				player.awkwardness += Time.deltaTime;
				Debug.Log(player.awkwardness + "  " + (player.isStanding ? "STANDING!" : "SITTING!"));
				if (player.awkwardness >= maxAwkwardness)
				{
					player.isDead = true;
					Debug.Log("DEAD");
				}
			}
		}
	}
}
