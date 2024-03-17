using Godot;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public partial class LiteDB_Manager : Node
{
	[Export]
	private TextEdit[] inputData;
	private Label txtLabel;
	private string nameFile = "Characters.db";
	private string _path;
	private bool _isSelect = false;
	public override void _Ready()
	{
		_path = PathManager.GetPath(nameFile);
		CreateDB();

		txtLabel = GetNode<Label>("txtPath");
		txtLabel.Text = _path;

		var buttonsControls = GetNode<Node>("Controls");
		var scene = "res://scene/SQLITE-Manager.tscn";
		buttonsControls.Call("LoadScene", scene);

		LOAD();
	}

	private void CreateDB()
	{
		_path = PathManager.GetPath(nameFile);

		try
		{
			if (!File.Exists(_path))
			{
				using (var db = new LiteDatabase(_path))
				{
					var collection = db.GetCollection<Naruto>("Naruto");


					foreach (var item in Naruto.characters)
					{
						collection.Insert(item);
					}
				}

				Debug.Print("Database created successfully");
			}
			else
			{
				Debug.Print("File already exists: " + _path);

			}
		}
		catch (Exception e)
		{
			Debug.Print("Error creating or saving the database: " + e.Message);
		}
	}

	private void LOAD()
	{
		using (var db = new LiteDatabase(PathManager.GetPath(_path)))
		{
			var collection = db.GetCollection<Naruto>("Naruto");

			var query = collection.FindAll();

			if (query == null)
			{
				Debug.Print("Data not found");
			}
			else
			{
				foreach (var item in query)
				{
					Debug.Print($"Id: {item.IdNaruto} - Name: {item.Name} - Clan: {item.Clan} - Age: {item.Age}");
				}

				var list = $"Id: {query.FirstOrDefault().IdNaruto} - Name: {query.FirstOrDefault().Name} - Clan: {query.FirstOrDefault().Clan} - Age: {query.FirstOrDefault().Age}";
				txtLabel.Text = list;
			}
		}
	}

	private void POST()
	{
		using (var db = new LiteDatabase(PathManager.GetPath(_path)))
		{
			var collection = db.GetCollection<Naruto>("Naruto");

			collection.Insert(
				new Naruto
				{
					Name = inputData[1].Text.ToUpper(),
					Clan = inputData[2].Text.ToUpper(),
					Age = Convert.ToInt32(inputData[3].Text),
					Avatar = $"https://api.dicebear.com/7.x/micah/png?seed=img{collection.Count() + 1}&radius=50&backgroundColor=d1d4f9"
				});

			ResetFields();
			Debug.Print("Data saved successfully");
		}
	}

	private async void GET()
	{
		using (var db = new LiteDatabase(PathManager.GetPath(_path)))
		{
			var collection = db.GetCollection<Naruto>("Naruto");

			if (string.IsNullOrEmpty(inputData[0].Text))
			{
				txtLabel.Text = "Id is required";

				inputData[0].Text = "";

				_isSelect = false;

				await Task.Delay(1000);

				txtLabel.Text = _path;
				return;
			}
			else
			{
				var id = new ObjectId(inputData[0].Text);

				var query = collection.FindById(id);

				if (query == null)
				{
					txtLabel.Text = "Data not found";



					await Task.Delay(1000);

					txtLabel.Text = _path;
				}
				else
				{
					inputData[0].Text = query.IdNaruto.ToString();
					inputData[1].Text = query.Name;
					inputData[2].Text = query.Clan;
					inputData[3].Text = query.Age.ToString();
					_isSelect = true;
				}

			}
		}
	}

	private async void PUT()
	{
		using (var db = new LiteDatabase(PathManager.GetPath(_path)))
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
				if (_isSelect == true)
				{
					var collection = db.GetCollection<Naruto>("Naruto");

					var obj = new Naruto
					{
						IdNaruto = new ObjectId(inputData[0].Text),
						Name = inputData[1].Text.ToUpper(),
						Clan = inputData[2].Text.ToUpper(),
						Age = Convert.ToInt32(inputData[3].Text)
					};

					collection.Update(obj);

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
	}

	private async void DELETE()
	{
		using (var db = new LiteDatabase(PathManager.GetPath(_path)))
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
				if (_isSelect == true)
				{
					var collection = db.GetCollection<Naruto>("Naruto");

					var id = new ObjectId(inputData[0].Text);

					collection.Delete(id);

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
