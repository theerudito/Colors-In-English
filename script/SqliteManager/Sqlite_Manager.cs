using ColorsInEnglish.script;
using Godot;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public partial class Sqlite_Manager : Node
{

	[Export]
	private TextEdit[] inputData;
	private Label txtLabel;
	private string nameFile = "Naruto-SQLITE.db";
	private string _path;
	private bool _isSelect = false;

	public override void _Ready()
	{
		_path = PathManager.GetPath(nameFile);

		txtLabel = GetNode<Label>("txtPath");
		txtLabel.Text = _path;

		var buttonsControls = GetNode<Node>("Controls");
		var scene = "res://scene/API-Manager.tscn";
		buttonsControls.Call("LoadScene", scene);

		LOAD();
	}
	private void LOAD()
	{
		var db = new GameContext();

		var query = db.Naruto.ToList();

		foreach (var item in query)
		{
			Debug.Print($"Id: {item.IdCharacter} - Name: {item.Name} - Clan: {item.Clan} - Age: {item.Age} - Avatar: {item.Avatar}");
		}
	}

	private void POST()
	{
		var db = new GameContext();

		var count = db.Naruto.Count();

		var obj = new Naruto
		{
			Name = inputData[1].Text.ToUpper(),
			Clan = inputData[2].Text.ToUpper(),
			Age = Convert.ToInt32(inputData[3].Text),
			Avatar = $"https://api.dicebear.com/7.x/micah/png?seed=img{count + 1}&radius=50&backgroundColor=d1d4f9"
		};

		db.Naruto.Add(obj);
		db.SaveChanges();

		ResetFields();

		Debug.Print("Data saved successfully");

	}

	private async void GET()
	{
		using (var db = new GameContext())
		{
			if (string.IsNullOrEmpty(inputData[0].Text))
			{
				txtLabel.Text = "Id is required";

				inputData[0].Text = "";

				await Task.Delay(1000);

				txtLabel.Text = _path;
				return;
			}
			else
			{
				var find = db.Naruto.Find(Convert.ToInt32(inputData[0].Text));

				if (find != null)
				{
					inputData[0].Text = find.IdCharacter.ToString();
					inputData[1].Text = find.Name;
					inputData[2].Text = find.Clan;
					inputData[3].Text = find.Age.ToString();

					_isSelect = true;
				}
				else
				{
					txtLabel.Text = "Data not found";

					await Task.Delay(1000);

					txtLabel.Text = _path;
				}
			}
		}
	}

	private async void PUT()
	{
		using (var db = new GameContext())
		{
			if (string.IsNullOrEmpty(inputData[0].Text))
			{
				txtLabel.Text = "Id is required";

				inputData[0].Text = "";

				await Task.Delay(1000);

				txtLabel.Text = _path;
				return;
			}
			else
			{
				var find = db.Naruto.Find(Convert.ToInt32(inputData[0].Text));

				if (find != null)
				{
					find.Name = inputData[1].Text.ToUpper();
					find.Clan = inputData[2].Text.ToUpper();
					find.Age = Convert.ToInt32(inputData[3].Text);

					db.SaveChanges();

					_isSelect = false;

					ResetFields();

					Debug.Print("Data updated successfully");
				}
				else
				{
					txtLabel.Text = "Data not found";

					await Task.Delay(1000);

					txtLabel.Text = _path;
				}
			}
		}
	}

	private async void DELETE()
	{
		using (var db = new GameContext())
		{
			if (string.IsNullOrEmpty(inputData[0].Text))
			{
				txtLabel.Text = "Id is required";

				inputData[0].Text = "";

				await Task.Delay(1000);

				txtLabel.Text = _path;
				return;
			}
			else
			{
				var find = db.Naruto.Find(Convert.ToInt32(inputData[0].Text));

				if (find != null)
				{
					db.Naruto.Remove(find);

					db.SaveChanges();

					_isSelect = false;

					ResetFields();

					Debug.Print("Data deleted successfully");
				}
				else
				{
					txtLabel.Text = "Data not found";

					await Task.Delay(1000);

					txtLabel.Text = _path;
				}
			}
		}
	}

	private void ResetFields()
	{
		foreach (var item in inputData)
		{
			item.Text = "";
		}
		_isSelect = false;
	}


}
