using System;
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
	private AudioStream[] _audioPlayer;

	private TextureRect _panelConfig;
	private AudioStreamPlayer2D _soundGame;

	private string infoEN = "MADE WITH ❤ BY BETWEEN BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();

	private string infoES = "HECHO CON ❤ POR BETWEEN BYTE SOFTWARE " + "- " + DateTime.Now.Year.ToString();
	private bool _isPlaying = true;
	private bool _flagEN = true;
	private bool _isOpen = false;
	private string _language;
	private int _score = 0;
	private int _time = 10;


	Random random = new Random();

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

		var db = Colors.newColorEN.ToList();

		var colorRamdom = random.Next(0, db.Count);

		_btnGame[0].Modulate = new Color(db[5].Hex);
		_btnGame[1].Modulate = new Color(db[30].Hex);
		_btnGame[2].Modulate = new Color(db[3].Hex);
		_btnGame[3].Modulate = new Color(db[8].Hex);
		_btnGame[4].Modulate = new Color(db[20].Hex);
		_btnGame[5].Modulate = new Color(db[colorRamdom].Hex);

	}

	private void CheckColor()
	{


	}

	private void GenerateColor()
	{

	}

	private void ResetGame()
	{

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
			Debug.WriteLine("SPANISH");
		}
		else
		{
			_flagEN = true;
			LocalStorage.SaveData("language", "EN");
			ClosePanel();
			SoundClick();
			ChangeLabels("EN");
			Debug.WriteLine("ENGLISH");
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
			myLabel[13].Text = "FOLLOW US";
			myLabel[14].Text = infoEN;
			_btnConfig[1].TextureNormal = spriteConfing[3];

		}
		else
		{
			_language = language;
			myLabel[0].Text = "TIEMPO:";
			myLabel[2].Text = "PUNTOS:";
			myLabel[4].Text = "BASE DE DATOS: OK";
			myLabel[5].Text = "QUE COLOR ES";
			myLabel[13].Text = "SIGUENOS";
			myLabel[14].Text = infoES;
			_btnConfig[1].TextureNormal = spriteConfing[2];
		}
	}
}
