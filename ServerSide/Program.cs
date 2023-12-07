using System.Net;
using System.Text.Json;

// Cache
Dictionary<string, int> Cache = new Dictionary<string, int>();

// DataBase 
string FilePath = "C:\\Users\\Elgun\\source\\repos\\Http_Project_2\\ServerSide\\Database.json";
string JsonString = File.ReadAllText(FilePath);
Dictionary<string, int> DataBase = JsonSerializer.Deserialize<Dictionary<string, int>>(JsonString);

// Http Listener
var url = "http://localhost:27001/";

var _LISTENER = new HttpListener();
_LISTENER.Prefixes.Add(url);
_LISTENER.Start();

// Listening
while (true)
{

    var context = await _LISTENER.GetContextAsync();

    var request = context.Request;
    var response = context.Response;

    response.Headers.Add("Content-Type", "text/plain");
    response.StatusCode = (int)HttpStatusCode.OK;

    var queryStringValue = request.QueryString["q"]?.ToString();

    bool IsFound = false;
    for (int i = 0; i < Cache.Count; i++)
    {
        if (Cache.ElementAt(i).Key == queryStringValue)
        {
            using (var sw = new StreamWriter(response.OutputStream))
            {
                await sw.WriteLineAsync($"From Cache -> {Cache.ElementAt(i).Key}: {Cache.ElementAt(i).Value}");
            }
            IsFound = true;
            break;
        }
    }
    if (!IsFound)
    {
        for (int i = 0; i < DataBase.Count; i++)
        {
            if (DataBase.ElementAt(i).Key == queryStringValue)
            {
                Cache.Add(DataBase.ElementAt(i).Key, DataBase.ElementAt(i).Value);
                using (var sw = new StreamWriter(response.OutputStream))
                {
                    await sw.WriteLineAsync($"From Database -> {DataBase.ElementAt(i).Key}: {DataBase.ElementAt(i).Value}");
                }
                IsFound = true;
                break;
            }
        }
        if (!IsFound)
        {
            using (var sw = new StreamWriter(response.OutputStream))
            {
                await sw.WriteLineAsync($"Not Exist In Database ...");
            }
        }
    }
}

