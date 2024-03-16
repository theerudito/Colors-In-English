using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Godot;
using Newtonsoft.Json;

public partial class JSON_Manager : Node
{
	[Export]
	private TextEdit[] inputData;
	private string nameFile = "Naruto.json";
	private string _path;
	public override void _Ready()
	{
		CreateJSON();
		var buttonsControls = GetNode<Node>("Controls");
		var scene = "res://scene/main.tscn";
		buttonsControls.Call("LoadScene", scene);
	}

	public void CreateJSON()
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

	public void POST()
	{
		if (File.Exists(_path))
		{
			var json = File.ReadAllText(_path);
			var characters = JsonConvert.DeserializeObject<List<Naruto>>(json).Count;

			Naruto obj = new Naruto
			{
				IdCharacter = characters + 1,
				Name = inputData[0].Text.ToUpper(),
				Clan = inputData[1].Text.ToUpper(),
				Age = Convert.ToInt32(inputData[2].Text),
				Avatar = $"https://api.dicebear.com/7.x/micah/png?seed=img{characters + 1}&radius=50&backgroundColor=d1d4f9"
			};

			SaveJSON(obj);
		}
		else
		{
			GD.Print("File not found");
		}
	}

	public void GET()
	{
		var json = File.ReadAllText(_path);
		var data = JsonConvert.DeserializeObject<List<Naruto>>(json);

		var query = data.Where(x => x.IdCharacter == 1).FirstOrDefault();

		inputData[0].Text = query.Name;
		inputData[1].Text = query.Clan;
		inputData[2].Text = query.Age.ToString();
	}


	public void ReadJSON()
	{
		try
		{
			var json = File.ReadAllText(_path);
			var data = JsonConvert.DeserializeObject<List<Naruto>>(json);
			foreach (var item in data)
			{
				GD.Print("IdCharacter: ", item.IdCharacter);
				GD.Print("Name: ", item.Name);
				GD.Print("Clan: ", item.Clan);
				GD.Print("Age: ", item.Age);
			}

		}
		catch (System.Exception e)
		{
			GD.Print("Error: ", e.Message);
		}
	}

	public void SaveJSON(Naruto obj)
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
}