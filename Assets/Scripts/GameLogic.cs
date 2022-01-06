using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{

	[Header("Set in Instructor")]
	public Text Turn;
	public Text Score;
	public int numPlayers = 2;
	public Vector3 startPosition = new Vector3(-10, 13.5f, 10);
	public AudioClip sound;

	private Player[] players;
	private string scoreText = ""; // For creating text of Score table
	private int turn = 0;
	private Rigidbody RB;

	// Use this for initialization
	void Start () {
		players = new Player[numPlayers];
		for(int i = 0; i < numPlayers; i++)
        {
			players[i] = new Player("Player " + (i + 1));
        }
	}
	
	// Update is called once per frame
	void Update () {
		scoreText = "";
		for(int i = 0; i < numPlayers; i++)
        {
			if (i > 0) scoreText += "\n";
			scoreText += players[i].Name;
			scoreText += ": " + players[i].ScoredBalls;
        }
		Score.text = scoreText;
		Turn.text = players[turn].Name;
	}

	public void NextTurn()
    {
		if (turn == numPlayers - 1) turn = 0;
		else turn++;
    }
	public int OtherPlayer()
	{
		if (turn == numPlayers - 1) return 0;
		else return turn + 1;
	}
	public void Loss()
    {
		WinnerName.winnerName = players[OtherPlayer()].Name;
		SceneManager.LoadScene("EndGameScene");
	}
	public void Win()
	{
		WinnerName.winnerName = players[turn].Name;
		SceneManager.LoadScene("EndGameScene");
	}
	public void Goal(GameObject ball)
	{
		if (players[turn].typeBalls == Player.GetTypeBalls(ball)) players[turn].ScoredBalls++;
		else if (Player.GetTypeBalls(ball) != Player.TypeBalls.Empty)
		{
			NextTurn();
			Goal(ball);
		}
		else if (Player.GetTypeBalls(ball) == Player.TypeBalls.Empty && players[turn].ScoredBalls != 7) Loss();
		else Win();
	}
	private void OnTriggerEnter(Collider other) {
		if (other.tag != "Player")
		{
			if (players[turn].typeBalls == Player.TypeBalls.Empty)
            {
				players[turn].typeBalls = Player.GetTypeBalls(other.gameObject);
				if (players[turn].typeBalls == Player.TypeBalls.Empty) Loss();
				if (players[turn].typeBalls == Player.TypeBalls.Solid) players[OtherPlayer()].typeBalls = Player.TypeBalls.Striped;
				else players[OtherPlayer()].typeBalls = Player.TypeBalls.Solid;
			}
			Goal(other.gameObject);
			AudioSource.PlayClipAtPoint(sound, other.gameObject.transform.position);
			Destroy(other.gameObject);
		}
		else
		{
			NextTurn ();
			other.gameObject.transform.position = startPosition;
			RB =  other.gameObject.GetComponent<Rigidbody>();
			RB.velocity = new Vector3(0, 0, 0);
		}
	}
}

struct Player
{
	public int ScoredBalls;
	public string Name;
	public TypeBalls typeBalls;
	public Player(string name = "Player")
    {
		this.Name = name;
		this.ScoredBalls = 0;
		this.typeBalls = TypeBalls.Empty;
    }
	public static TypeBalls GetTypeBalls(GameObject ball)
    {
		if (int.Parse(ball.name) >= 1 && int.Parse(ball.name) <= 7) return TypeBalls.Solid;
		else if (int.Parse(ball.name) >= 9 && int.Parse(ball.name) <= 15) return TypeBalls.Striped;
		else return TypeBalls.Empty;
	}
	public enum TypeBalls
    {
		Empty,
		Solid,
		Striped
    }
}
