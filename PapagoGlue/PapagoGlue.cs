using System;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PapagoGlue
{
	class ApiPass {
		// Strangely, if someone remove setter here, JsonSerializer
		// does not work.
		public string Id { get; set; }
		public string Secret { get; set; }

		public static ApiPass FromFile(string path) {
			using (StreamReader f = File.OpenText(path)) {
				string conf = f.ReadToEnd();
				return JsonSerializer.Deserialize<ApiPass>(conf);
			}

		}
	}

    public class PapagoGlue
    {
		// Papago Api URL
		const String url = "https://openapi.naver.com/v1/papago/n2mt";

		// TODO: use proper path
		const ApiPass apiPass = ApiPass.FromFile("../../../key.json");
		HttpClient client;

		public PapagoGlue() {
			client = new HttpClient();
		}

		// FIXME: this function wiil crash if there's no proper Id
		// and Secret in key.json.
		// TODO: Handle Errors appropriately (error handling not present)
		async public Task<String> Translate(String s) {
			var request = new HttpRequestMessage(new HttpMethod("POST"), url);
			var d = new Dictionary<String, String>();
			{
				d.Add("source", "en");
				d.Add("target", "ko");
				d.Add("text", s);
			}
			var con = new FormUrlEncodedContent(d);
			
			// naver open api auth
			request.Headers.Add("X-Naver-Client-Id", apiPass.Id);
			request.Headers.Add("X-Naver-Client-Secret", apiPass.Secret);
			request.Content = con;

			var response = await client.SendAsync(request);
			var content = await response.Content.ReadAsStringAsync();
			var rjson = JsonDocument.Parse(content);

			var translatedText =  rjson.RootElement
				.GetProperty("message")
				.GetProperty("result")
				.GetProperty("translatedText");

			return translatedText.ToString();
		}
    }
}
