using Godot;
using Newtonsoft.Json;
using System.Diagnostics;

public partial class API_Manager : Node2D
{
	private string url = "https://reqres.in/api/users";
	[Export]
	private Label[] myTexts;

	public class Data
	{
		public int id { get; set; }
		public string email { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string avatar { get; set; }
	}

	public class Root
	{
		public Data data { get; set; }
	}

	public override void _Ready()
	{
		var buttonsControls = GetNode<Node>("Controls");
		var scene = "res://scene/main.tscn";
		buttonsControls.Call("LoadScene", scene);

		GET();
	}

	public void GET()
	{
		var fetch = new System.Net.Http.HttpClient();

		var response = fetch.GetAsync($"{url}/1").Result;

		var json = response.Content.ReadAsStringAsync().Result;

		var data = JsonConvert.DeserializeObject<Root>(json).data;

		myTexts[0].GetChild<LineEdit>(0).Text = data.id.ToString();
		myTexts[1].Text = data.first_name;
		myTexts[2].Text = data.last_name;
		myTexts[3].Text = data.email;

		//var texture = LoadImage(data.avatar);

		var image = GetNode<TextureRect>("img");

		//LoadImage(data.avatar);
	}

	public async void LoadImage(string url)
	{
		var imagen = "https://i.postimg.cc/qMKQ98Yg/perro.jpg";

		var fetch = new System.Net.Http.HttpClient();

		var response = fetch.GetAsync(imagen).Result;

		response.EnsureSuccessStatusCode();

		//Debug.Print(response.Content.Headers.ContentType.MediaType);

		var bytes = await response.Content.ReadAsByteArrayAsync();

		//Debug.Print(bytes.Length.ToString());

		var image = new Image();

		var error = image.LoadPngFromBuffer(bytes);

		if (error == Error.Ok)
		{
			Debug.Print("Imagen cargada correctamente");
		}
		else
		{
			GD.Print("Error cargando la imagen:", error);
		}
	}
}
