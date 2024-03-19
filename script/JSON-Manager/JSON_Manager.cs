using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Newtonsoft.Json;

public partial class JSON_Manager : Node
{
	[Export]
	private TextEdit[] inputData;
	private Label txtLabel;
	private string nameFile = "Naruto-JSON.json";
	private string _path;
	private bool _isSelect = false;

	public override void _Ready()
	{
		_path = PathManager.GetPath(nameFile);
		CreateJSON();

		txtLabel = GetNode<Label>("txtPath");
		txtLabel.Text = _path;

		var buttonsControls = GetNode<Node>("Controls");
		var scene = "res://scene/LiteDB-Manager.tscn";
		buttonsControls.Call("LoadScene", scene);

		LOAD();
	}

	private void CreateJSON()
	{
		_path = PathManager.GetPath(nameFile);

		try
		{
			if (!File.Exists(_path))
			{
				using (FileStream fs = File.Create(_path))

					Debug.Print("File created: " + _path);

				var narutoList = Naruto.characters;
				string json = JsonConvert.SerializeObject(narutoList, Formatting.Indented);
				File.WriteAllText(_path, json);
			}
			else
			{
				Debug.Print("File already exists: " + _path);

			}
		}
		catch (Exception e)
		{
			Debug.Print("Error creating or saving JSON file: " + e.Message);
		}
	}

	private void LOAD()
	{
		int PANEL_MARGIN = 10;
		var json = File.ReadAllText(_path);

		var data = JsonConvert.DeserializeObject<List<Naruto>>(json);

		var scroll = GetNode<ScrollContainer>("ScrollContainer");
		var color = scroll.GetNode<ColorRect>("ColorRect");

		var yPos = 0;

		foreach (var character in data)
		{
			var panel = new Panel();

			panel.Position = new Vector2(0, 0);

			panel.Size = new Vector2(300, 40);

			color.AddChild(panel);

			var idLabel = new Label();
			idLabel.Position = new Vector2(10, 10);
			idLabel.Text = character.IdCharacter.ToString();
			panel.AddChild(idLabel);

			var nameLabel = new Label();
			nameLabel.Position = new Vector2(70, 10);
			nameLabel.Text = character.Name;
			panel.AddChild(nameLabel);

			var clanLabel = new Label();
			clanLabel.Position = new Vector2(171, 10);
			clanLabel.Text = character.Clan;
			panel.AddChild(clanLabel);

			var ageLabel = new Label();
			ageLabel.Position = new Vector2(259, 10);
			ageLabel.Text = character.Age.ToString();
			panel.AddChild(ageLabel);

			// var deleteButton = new Button();
			// deleteButton.Text = "Delete";
			// panel.AddChild(deleteButton);

			// var getButton = new Button();
			// getButton.Text = "Get";
			// panel.AddChild(getButton);

			yPos += 42;
			panel.Position = new Vector2(10, yPos);

		}

	}
	private void POST()
	{
		if (File.Exists(_path))
		{
			var json = File.ReadAllText(_path);

			var characters = JsonConvert.DeserializeObject<List<Naruto>>(json).Count;

			Naruto obj = new Naruto
			{
				IdCharacter = characters + 1,
				Name = inputData[1].Text.ToUpper(),
				Clan = inputData[2].Text.ToUpper(),
				Age = Convert.ToInt32(inputData[3].Text),
				Avatar = $"https://api.dicebear.com/7.x/micah/png?seed=img{characters + 1}&radius=50&backgroundColor=d1d4f9"
			};

			SaveJSON(obj);

			ResetFields();

			Debug.Print("Data saved successfully");
		}
		else
		{
			Debug.Print("File not found");
		}
	}

	private async void GET()
	{
		_isSelect = true;
		var json = File.ReadAllText(_path);

		var data = JsonConvert.DeserializeObject<List<Naruto>>(json);

		var query = data.FirstOrDefault(x => x.IdCharacter == (string.IsNullOrEmpty(inputData[0].Text) ? 1 : Convert.ToInt32(inputData[0].Text)));

		if (query == null)
		{

			txtLabel.Text = "Data not found";

			await Task.Delay(1000);

			txtLabel.Text = _path;
		}
		else
		{
			inputData[0].Text = query.IdCharacter.ToString();
			inputData[1].Text = query.Name;
			inputData[2].Text = query.Clan;
			inputData[3].Text = query.Age.ToString();
		}
	}

	private async void PUT()
	{
		var json = File.ReadAllText(_path);

		var data = JsonConvert.DeserializeObject<List<Naruto>>(json);

		if (string.IsNullOrEmpty(inputData[0].Text))
		{
			txtLabel.Text = "Id is required";

			await Task.Delay(1000);

			txtLabel.Text = _path;
			return;
		}
		else
		{
			if (_isSelect == true)
			{
				var query = data.Where(x => x.IdCharacter == Convert.ToInt32(inputData[0].Text)).FirstOrDefault();

				query.Name = inputData[1].Text.ToUpper();
				query.Clan = inputData[2].Text.ToUpper();
				query.Age = Convert.ToInt32(inputData[3].Text);

				string jsonToSave = JsonConvert.SerializeObject(data, Formatting.Indented);

				File.WriteAllText(_path, jsonToSave);

				ResetFields();

				Debug.Print("Data updated successfully");
			}
			else
			{
				txtLabel.Text = "Select the data first";

				await Task.Delay(1000);

				txtLabel.Text = _path;
			}
		}
	}

	private async void DELETE()
	{
		var json = File.ReadAllText(_path);

		var data = JsonConvert.DeserializeObject<List<Naruto>>(json);

		if (string.IsNullOrEmpty(inputData[0].Text))
		{
			txtLabel.Text = "Id is required";

			await Task.Delay(1000);

			txtLabel.Text = _path;
			return;
		}
		else
		{
			if (_isSelect == true)
			{
				var query = data.Where(x => x.IdCharacter == Convert.ToInt32(inputData[0].Text)).FirstOrDefault();

				data.Remove(query);

				string jsonToSave = JsonConvert.SerializeObject(data, Formatting.Indented);

				File.WriteAllText(_path, jsonToSave);

				ResetFields();

				Debug.Print("Data deleted successfully");

			}
			else
			{
				txtLabel.Text = "Select the data first";

				await Task.Delay(1000);

				txtLabel.Text = _path;
			}
		}
	}

	private void ReadJSON()
	{
		try
		{
			var json = File.ReadAllText(_path);
			var data = JsonConvert.DeserializeObject<List<Naruto>>(json);
		}
		catch (Exception e)
		{
			Debug.Print("Error: " + e.Message.ToString());
		}
	}

	private void SaveJSON(Naruto obj)
	{
		try
		{
			List<Naruto> existingData = new List<Naruto>();

			if (File.Exists(_path))
			{
				var json = File.ReadAllText(_path);
				existingData = JsonConvert.DeserializeObject<List<Naruto>>(json);
			}

			existingData.Add(obj);

			string jsonToSave = JsonConvert.SerializeObject(existingData, Formatting.Indented);

			File.WriteAllText(_path, jsonToSave);

			GD.Print("Data saved successfully");

		}
		catch (Exception e)
		{
			GD.Print("Error: " + e.Message);
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