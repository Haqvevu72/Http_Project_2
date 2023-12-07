using System.Net;


var _CLIENT = new HttpClient();
while (true)
{
    Console.Write("Write Letter: ");
    var Letter = Console.ReadLine();


    var result = await _CLIENT.GetStringAsync($"http://localhost:27001/?q={Letter}");

    Console.WriteLine(result);
}
