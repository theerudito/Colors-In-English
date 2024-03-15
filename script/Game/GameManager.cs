using System;
using System.Diagnostics;
using System.Linq;
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
	private Button[] _btnGame;
	[Export]
	private AudioStream[] _audioPlayer;
	private TextureRect _panelConfig;
	private AudioStreamPlayer2D _soundGame;
	private string infoEN = "MADE BY BETWEEN BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();
	private string infoES = "HECHO POR BETWEEN BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();
	private bool _isPlaying = true;
	private bool _flagEN = true;
	private bool _isOpen = false;
	private bool _premium = true;
	private string _language;
	private int _score = 0;
	private int _timeGame = 10;
	private int _timeButtons = 2;

	CurrentScene currentScene = new CurrentScene();

	public override void _Ready()
	{
		var currentScene = GetNode<CurrentScene>("/root/CurrentScene");

		currentScene.SceneName = "res://scene/JSON-Manager.tscn";

		myLabel[9].Visible = false;

		var time = GetNode<Timer>("timeGame");
		TimerManager(time);

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
		if (LocalStorage.LoadData("score") == null)
		{
			_score = 0;
			LocalStorage.SaveData("score", _score.ToString());
		}
		else
		{
			_score = LocalStorage.LoadDataInt("score");
			myLabel[3].Text = _score.ToString();
		}
		#endregion SCORE

		_panelConfig = GetNode<TextureRect>("panelConfig");
		_panelConfig.Visible = false;
		_soundGame = GetNode<AudioStreamPlayer2D>("soundGame");

		GenerateColor();
	}

	private void CheckColor(int index)
	{
		var selected = GetNode<Panel>("Panel Buttons").GetChild<Button>(index);

		if (myLabel[6].Text == selected.Text)
		{
			Score(true);
			LocalStorage.SaveData("score", _score.ToString());
			if (_isPlaying == true)
			{
				_soundGame.Stream = _audioPlayer[0];
				_soundGame.Play();
			}
			for (int i = 0; i < _btnGame.Length; i++)
			{
				_btnGame[i].Disabled = true;
			}
			ResetGame();
		}
		else
		{
			Score(false);
			LocalStorage.SaveData("score", _score.ToString());
			if (_isPlaying == true)
			{
				_soundGame.Stream = _audioPlayer[1];
				_soundGame.Play();
			}
			_btnGame[index].Disabled = true;
		}
	}

	public void GenerateColor()
	{
		Random random = new Random();

		if (_language == "EN")
		{
			var colors = Colors.newColorEN.ToList();

			var colorRandomIndex = random.Next(0, colors.Count);

			var selectColor = colors[colorRandomIndex];

			if (selectColor.Name.Length > 10)
			{
				myLabel[6].AddThemeFontSizeOverride("font_size", 20);
			}
			else
			{
				myLabel[6].AddThemeFontSizeOverride("font_size", 30);
			}

			myLabel[6].Text = selectColor.Name;

			int[] MyIndex = new int[] { 1, 2, 3, 4, 5, selectColor.IdColor - 1 };

			MyIndex = MyIndex.OrderBy(x => random.Next()).ToArray();

			for (int i = 0; i < _btnGame.Length; i++)
			{
				_btnGame[i].Modulate = new Color(colors[MyIndex[i]].Hex);
				_btnGame[i].Text = colors[MyIndex[i]].Name;
				_btnGame[i].AddThemeColorOverride("font_color", new Color("Transparent"));
				_btnGame[i].Disabled = false;
			}
		}
		else
		{
			var colors = Colors.newColorES.ToList();

			var colorRandomIndex = random.Next(0, colors.Count);

			var selectColor = colors[colorRandomIndex];

			myLabel[6].Text = selectColor.Name;

			int[] MyIndex = new int[] { 1, 2, 3, 4, 5, selectColor.IdColor - 1 };

			MyIndex = MyIndex.OrderBy(x => random.Next()).ToArray();

			for (int i = 0; i < _btnGame.Length; i++)
			{
				//Debug.Print("COLOR " + colors[MyIndex[i]].Name + " " + colors[MyIndex[i]].IdColor);
				_btnGame[i].Modulate = new Color(colors[MyIndex[i]].Hex);
				_btnGame[i].Text = colors[MyIndex[i]].Name;
				_btnGame[i].AddThemeColorOverride("font_color", new Color("Transparent"));
				_btnGame[i].Disabled = false;
			}
		}
	}

	private async void ResetGame()
	{
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		var face = GetNode<Panel>("Panel Faces").GetChild<Sprite2D>(0);
		face.Texture = spriteFaces[0];
		GenerateColor();
		_timeGame = 10;
		myLabel[1].Modulate = new Color("White");
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
			GenerateColor();
		}
		else
		{
			_flagEN = true;
			LocalStorage.SaveData("language", "EN");
			ClosePanel();
			SoundClick();
			ChangeLabels("EN");
			Debug.Print("ENGLISH");
			GenerateColor();
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
		if (_isPlaying == true)
		{
			_soundGame.Stream = _audioPlayer[2];
			_soundGame.Play();
		}
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
			myLabel[9].Text = "YOU ARE PREMIUM";
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
			myLabel[9].Text = "ERES PREMIUM";
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

	private void TimerManager(Timer timer)
	{
		timer.Start();
		myLabel[1].Text = _timeGame.ToString();
		_timeGame -= 1;
	}

	private void CronometerOne()
	{
		var timerGame = GetNode<Timer>("timeGame");

		if (_timeGame < 7)
		{
			if (_isPlaying == true)
			{
				_soundGame.Stream = _audioPlayer[3];
				_soundGame.Play();
			}
			myLabel[1].Modulate = new Color("Red");
		}

		if (_timeGame <= 0)
		{
			_timeGame = 10;
			timerGame.Start();

			GenerateColor();
			_soundGame.Stream = _audioPlayer[3];
			myLabel[1].Modulate = new Color("White");
			_soundGame.Stop();
		}
		else
		{
			TimerManager(timerGame);
		}
	}

	private void CheckPremium()
	{
		var scene = GetNode<CurrentScene>("/root/CurrentScene");
		scene.SceneName = "res://scene/JSON-Manager.tscn";

		if (_premium == true)
		{
			_premium = false;
			myLabel[9].Visible = true;

		}
		else
		{
			_premium = true;
			myLabel[9].Visible = false;
		}
		if (_isPlaying == true)
		{
			_soundGame.Stream = _audioPlayer[0];
			_soundGame.Play();
		}
	}

}

