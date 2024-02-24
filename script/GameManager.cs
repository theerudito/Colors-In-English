using System;
using System.Collections.Generic;
using System.Linq;
using ColorsInEnglish.script;
using Godot;


public partial class GameManager : Node
{
	[Export]
	private Label myLabel;
	[Export]
	private Label pathLabel;
	[Export]
	private Label _labelOne;
	[Export]
	private Label _labelTimer;
	[Export]
	private Label _labelScore;

	private TouchScreenButton _btnOne;
	private TouchScreenButton _btnTwo;
	private TouchScreenButton _btnThree;
	private TouchScreenButton _btnFour;
	private TouchScreenButton _btnFive;
	private TouchScreenButton _btnSix;

	private Sprite2D _faceHappy;
	private Sprite2D _faceSad;
	private Sprite2D _faceBoss;

	private Timer _timer;

	private string info = "Made with ❤ by Between Byte Software® " + "- " + DateTime.Now.Year.ToString();
	private string _correctColor = "GreenYellow";
	private string _inCorrectColor = "Red";

	private string[] myColors = new string[6];

	private int _idColor = 0;

	Random random = new Random();

	public override void _Ready()
	{
		_labelOne = GetNode<Label>("labelOne");
		myLabel = GetNode<Label>("myLabel");
		pathLabel = GetNode<Label>("pathLabel");
		_labelTimer = GetNode<Label>("labelTimer").GetChild<Label>(0);
		_labelScore = GetNode<Label>("labelScore").GetChild<Label>(0);

		_btnOne = GetNode<TouchScreenButton>("btnOne");
		_btnTwo = GetNode<TouchScreenButton>("btnTwo");
		_btnThree = GetNode<TouchScreenButton>("btnThree");
		_btnFour = GetNode<TouchScreenButton>("btnFour");
		_btnFive = GetNode<TouchScreenButton>("btnFive");
		_btnSix = GetNode<TouchScreenButton>("btnSix");

		_faceHappy = GetNode<Sprite2D>("faceHappy");
		_faceSad = GetNode<Sprite2D>("faceSad");
		_faceBoss = GetNode<Sprite2D>("faceBoss");

		_timer = GetNode<Timer>("myTimer");

		_labelOne.AddThemeColorOverride("font_color", new Color("Black"));

		GetNode<Label>("infoDeveloper").Text = info;

		_faceHappy.Visible = false;
		_faceSad.Visible = false;

		GenerateColor();

	}


	private void CheckColor()
	{

		var color = myLabel.Text;
		var labelColor = new Color(color);
		GD.Print("Color: " + color);

		TouchScreenButton[] buttons = new TouchScreenButton[] { _btnOne, _btnTwo, _btnThree, _btnFour, _btnFive, _btnSix };

		GD.Print(buttons);

		bool colorMatch = false;

		foreach (var button in buttons)
		{
			if (button.Modulate == labelColor)
			{
				colorMatch = true;
				break;
			}
		}

		if (colorMatch)
		{
			_faceHappy.Visible = true;
			_faceSad.Visible = false;
			_faceBoss.Visible = false;
			GenerateColor();
		}
		else
		{
			_faceHappy.Visible = false;
			_faceSad.Visible = true;
			_faceBoss.Visible = false;
		}


	}


	private void GenerateColor()
	{
		var colorRamdom = random.Next(0, Word.GetData().Count);

		var selectColor = Word.GetData().Where(x => x.IdWord == colorRamdom).FirstOrDefault();

		if (selectColor == null) return;

		_idColor = selectColor.IdWord;
		myLabel.Text = selectColor.Color;

		List<int> MyIndex = new List<int> { 1, 2, 3, 4, 5, _idColor };

		MyIndex = MyIndex.OrderBy(x => random.Next()).ToList();

		TouchScreenButton[] buttons = new TouchScreenButton[] { _btnOne, _btnTwo, _btnThree, _btnFour, _btnFive, _btnSix };

		for (int i = 0; i < MyIndex.Count; i++)
		{
			int index = MyIndex[i] - 1;
			buttons[i].Modulate = new Color(Word.GetData()[index].Color);
		}
		_faceHappy.Visible = false;
		_faceSad.Visible = false;
		_faceBoss.Visible = true;
	}

	private void OpenBrowser(string url) => OS.ShellOpen(url);
}
