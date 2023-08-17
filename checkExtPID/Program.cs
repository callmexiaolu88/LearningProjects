// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;

Console.WriteLine("Hello, World!");
using var jsonreader = new StreamReader("ext.json");
var jsontxt = jsonreader.ReadToEnd();
var exts = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(jsontxt);

List<dynamic> result = new List<dynamic>();
using var pidreader = new StreamReader("pid.txt");
while(!pidreader.EndOfStream){
    var pdi = pidreader.ReadLine();
    var rl= exts.Where(ex => ex.IdHex == pdi.ToUpper());
    result.AddRange(rl);
}
Console.WriteLine(result.Count);
Console.WriteLine(JsonConvert.SerializeObject(result));
