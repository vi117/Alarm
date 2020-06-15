using System;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Papago
{
	public class ApiPass
	{
		public string Id { get; set; }
		public string Secret { get; set; }

		public static ApiPass FromFile(string path)
		{
			using (StreamReader f = File.OpenText(path))
			{
				string conf = f.ReadToEnd();
				return JsonSerializer.Deserialize<ApiPass>(conf);
			}

		}
	}

	public class NotAuthorizedException : Exception { }

	public class PapagoGlue
	{
		// Papago Api URL
		const string url = "https://openapi.naver.com/v1/papago/n2mt";

		private ApiPass apiPass;

		private HttpClient client;

		public PapagoGlue(ApiPass apiPass)
		{
			this.apiPass = apiPass;
			client = new HttpClient();
		}

		// TODO: Handle Errors appropriately (error handling not present)
		async public Task<String> Translate(String s)
		{
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

			// send request
			var response = await client.SendAsync(request);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var rjson = JsonDocument.Parse(content);

				var translatedText = rjson.RootElement
					.GetProperty("message")
					.GetProperty("result")
					.GetProperty("translatedText");

				return translatedText.ToString();
			}
			else
			{
				throw new NotAuthorizedException();
			}
		}
	}
}
