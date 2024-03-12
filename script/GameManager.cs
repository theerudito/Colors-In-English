using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Godot;


public partial class GameManager : Node
{
	[Export]
	private Label[] myLabel;
	[Export]
	private Texture2D[] spriteFaces;
	[Export]
	private Texture2D[] spriteConfing;
	[Export]
	private TouchScreenButton[] _btnConfig;
	[Export]
	private TouchScreenButton[] _btnGame;
	[Export]
	private Label[] _labelButtons;
	[Export]
	private AudioStream[] _audioPlayer;
	private TextureRect _panelConfig;
	private AudioStreamPlayer2D _soundGame;
	private string infoEN = "MADE BY BETWEEN BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();
	private string infoES = "HECHO POR BETWEEN BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();
	private bool _isPlaying = true;
	private bool _flagEN = true;
	private bool _isOpen = false;
	private string _language;
	private int _score = 0;
	private int _time = 10;


	public override void _Ready()
	{

		#region SOUND
		if (LocalStorage.LoadData("sound") == null)
		{

			LocalStorage.SaveData("sound", "ON");
			_isPlaying = true;
			_btnConfig[0].TextureNormal = spriteConfing[0];
		}
		else
		{
			if (LocalStorage.LoadData("sound") == "OFF")
			{
				_isPlaying = false;
				_btnConfig[0].TextureNormal = spriteConfing[1];
			}
			else
			{
				_isPlaying = true;
				_btnConfig[0].TextureNormal = spriteConfing[0];
			}
		}
		#endregion SOUND

		#region LANGUAGE
		if (LocalStorage.LoadData("language") == null)
		{
			_flagEN = true;
			ChangeLabels("EN");
		}
		else
		{
			if (LocalStorage.LoadData("language") == "ES")
			{
				_flagEN = false;
				ChangeLabels("ES");
			}
			else
			{
				_flagEN = true;
				ChangeLabels("EN");
			}
		}
		#endregion LANGUAGE

		#region SCORE
		// if (LocalStorage.LoadData("score") == null)
		// {
		// 	_score = 0;
		// 	LocalStorage.SaveData("score", _score.ToString());
		// }
		// else
		// {
		// 	_score = LocalStorage.LoadDataInt("score");
		// 	myLabel[3].Text = _score.ToString();
		// }
		#endregion SCORE

		_panelConfig = GetNode<TextureRect>("panelConfig");
		_panelConfig.Visible = false;
		_soundGame = GetNode<AudioStreamPlayer2D>("soundGame");

		GenerateColor();
	}

	private void CheckColor()
	{

		Score(true);
		GenerateColor();
		Debug.Print("BUTTON 1");

		var panel = GetNode<Panel>("Panel Faces").GetChild<Sprite2D>(0);
		panel.Texture = spriteFaces[1];

		ResetGame();


		// if (myLabel[6].Text == _labelButtons[0].Text)
		// {
		// 	Score(true);
		// 	ResetGame();
		// 	GenerateColor();
		// 	Debug.Print("BUTTON 1");
		// }
		// else if (myLabel[6].Text == _labelButtons[1].Text)
		// {
		// 	Score(true);
		// 	ResetGame();
		// 	GenerateColor();
		// 	Debug.Print("BUTTON 2");
		// }
		// else if (myLabel[6].Text == _labelButtons[2].Text)
		// {
		// 	Score(true);
		// 	ResetGame();
		// 	GenerateColor();
		// 	Debug.Print("BUTTON 3");
		// }
		// else if (myLabel[6].Text == _labelButtons[3].Text)
		// {
		// 	Score(true);
		// 	ResetGame();
		// 	GenerateColor();
		// 	Debug.Print("BUTTON 4");
		// }
		// else if (myLabel[6].Text == _labelButtons[4].Text)
		// {
		// 	Score(true);
		// 	ResetGame();
		// 	GenerateColor();
		// 	Debug.Print("BUTTON 5");
		// }
		// else if (myLabel[6].Text == _labelButtons[5].Text)
		// {
		// 	Score(true);
		// 	ResetGame();
		// 	GenerateColor();
		// 	Debug.Print("BUTTON 6");
		// }
		// else
		// {
		// 	Score(false);
		// 	Debug.Print("WRONG");
		// }

	}

	private void GenerateColor()
	{
		Random random = new Random();

		var db = Colors.newColorEN.ToList();

		var colorRamdom = random.Next(1, db.Count);

		var selectColor = db.Where(x => x.IdColor == colorRamdom).FirstOrDefault();

		myLabel[6].Text = selectColor.Name;

		List<int> MyIndex = new List<int> { 1, 2, 3, 4, 5, selectColor.IdColor };

		MyIndex = MyIndex.OrderBy(x => random.Next()).ToList();

		//Debug.Print("ANSWER " + selectColor.Name + " " + selectColor.IdColor);

		foreach (var item in MyIndex)
		{
			//Debug.Print(item.ToString());
		}

		//myLabel[6].AddThemeFontSizeOverride("normal", 6);


		// if (myLabel[6].Text.Length < 7)
		// {
		// 	myLabel[6].AddThemeFontSizeOverride("bold", 20);
		// 	GD.Print("20");
		// }
		// else if (selectColor.Name.Length < 10)
		// {
		// 	myLabel[6].AddThemeFontSizeOverride("normal", 6);
		// 	GD.Print("10");
		// }
		// else
		// {
		// 	myLabel[6].AddThemeFontSizeOverride("normal", 8);
		// 	GD.Print("8");
		// }

		for (int i = 0; i < _btnGame.Length; i++)
		{

			//Debug.Print(db[MyIndex[i]].Name);
			_btnGame[i].Modulate = new Color(db[MyIndex[i]].Hex);
			_labelButtons[i].Text = db[MyIndex[i]].Name;
		}
	}

	private void ResetGame()
	{
		var timer = GetNode<Timer>("myTimer");
		timer.Start();

		_time = 100;

		var face = GetNode<Panel>("Panel Faces").GetChild<Sprite2D>(0);
		face.Texture = spriteFaces[0];
		GenerateColor();
	}

	private void ChangeLanguage()
	{
		if (_flagEN == true)
		{
			_flagEN = false;
			LocalStorage.SaveData("language", "ES");
			ClosePanel();
			SoundClick();
			ChangeLabels("ES");
			Debug.Print("SPANISH");
		}
		else
		{
			_flagEN = true;
			LocalStorage.SaveData("language", "EN");
			ClosePanel();
			SoundClick();
			ChangeLabels("EN");
			Debug.Print("ENGLISH");
		}
	}

	private void ChangeSound()
	{
		if (_isPlaying == true)
		{
			_isPlaying = false;
			_btnConfig[0].TextureNormal = spriteConfing[1];
			LocalStorage.SaveData("sound", "OFF");
			ClosePanel();
			SoundClick();
		}
		else
		{
			_isPlaying = true;
			_btnConfig[0].TextureNormal = spriteConfing[0];
			LocalStorage.SaveData("sound", "ON");
			ClosePanel();
			SoundClick();
		}
	}

	private void OpenPanel()
	{
		SoundClick();
		if (_isOpen == false)
		{
			_panelConfig.Visible = true;
			_btnConfig[2].TextureNormal = spriteConfing[4];
			_isOpen = true;
		}
		else
		{
			ClosePanel();
			_btnConfig[2].TextureNormal = spriteConfing[5];
		}
	}

	private void ClosePanel()
	{
		_panelConfig.Visible = false;
		_isOpen = false;
		_btnConfig[2].TextureNormal = spriteConfing[5];
	}

	private void OpenBrowser(string url)
	{
		SoundClick();
		OS.ShellOpen(url);
	}

	private void SoundClick()
	{
		_soundGame.Stream = _audioPlayer[3];
		_soundGame.Play();
	}

	private void ChangeLabels(string language)
	{
		if (_flagEN == true)
		{
			_language = language;
			myLabel[0].Text = "TIME:";
			myLabel[2].Text = "SCORE:";
			myLabel[4].Text = "DATABASE: OK";
			myLabel[5].Text = "WHATS COLOR IS";
			myLabel[7].Text = "FOLLOW US";
			myLabel[8].Text = infoEN;
			_btnConfig[1].TextureNormal = spriteConfing[3];

		}
		else
		{
			_language = language;
			myLabel[0].Text = "TIEMPO:";
			myLabel[2].Text = "PUNTOS:";
			myLabel[4].Text = "BASE DE DATOS: OK";
			myLabel[5].Text = "QUE COLOR ES";
			myLabel[7].Text = "SIGUENOS";
			myLabel[8].Text = infoES;
			_btnConfig[1].TextureNormal = spriteConfing[2];
		}
	}
	private void Score(bool correct)
	{
		var panel = GetNode<Panel>("Panel Faces").GetChild<Sprite2D>(0);
		if (correct == true)
		{
			_score += 10;
			myLabel[3].Text = _score.ToString();
			panel.Texture = spriteFaces[1];
			Debug.Print("SUMANDO");
		}
		else
		{
			_score -= 5;
			if (_score < 0)
			{
				_score = 0;
			}
			else
			{
				myLabel[3].Text = _score.ToString();
				panel.Texture = spriteFaces[2];
				Debug.Print("RESTANDO");
			}
		}
	}
}
