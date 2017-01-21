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
	public List<HumanPlayer> ActivePlayers { get; private set; }

	public List<Slider> awkwardnessSliders;
	public List<Image> awkwardnessSliderBacks;
	public List<Text> awkwardnessSliderText;

	public float maxAwkwardness = 1f;

	void Awake()
	{
		Players = new List<HumanPlayer>()
		{
			new HumanPlayer() { keyCode = KeyCode.A },
			new HumanPlayer() { keyCode = KeyCode.S },
			new HumanPlayer() { keyCode = KeyCode.D },
			new HumanPlayer() { keyCode = KeyCode.F },
			new HumanPlayer() { keyCode = KeyCode.G },
			new HumanPlayer() { keyCode = KeyCode.H },
			new HumanPlayer() { keyCode = KeyCode.J },
			new HumanPlayer() { keyCode = KeyCode.K },
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
		}

		for (int i = 0; i < awkwardnessSliders.Count; i++)
		{
			awkwardnessSliders[i].value = 0f;
			awkwardnessSliderText[i].color = Color.black;
			awkwardnessSliderText[i].text = Players[i].keyCode.ToString();
			awkwardnessSliderBacks[i].color = i < count ? Color.white : Color.gray;
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
				if (player.awkwardness >= maxAwkwardness)
				{
					player.isDead = true;
					awkwardnessSliderText[i].text = "DEAD";
					awkwardnessSliderText[i].color = Color.white;
				}
			}
		}
	}

	void Update()
	{
		for (int i = 0; i < ActivePlayers.Count; i++)
		{
			awkwardnessSliders[i].value = ActivePlayers[i].awkwardness / maxAwkwardness;
		}
	}
}
