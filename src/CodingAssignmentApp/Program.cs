// See https://aka.ms/new-console-template for more information

using System.IO.Abstractions;
using CodingAssignmentLib;
using CodingAssignmentLib.Abstractions;

using System.Xml;
// using Newtonsoft.Json;

Console.WriteLine("Coding Assignment!");

do
{
    Console.WriteLine("\n---------------------------------------\n");
    Console.WriteLine("Choose an option from the following list:");
    Console.WriteLine("\t1 - Display");
    Console.WriteLine("\t2 - Search");
    Console.WriteLine("\t3 - Exit");

    switch (Console.ReadLine())
    {
        case "1":
            Display();
            break;
        case "2":
            Search();
            break;
        case "3":
            return;
        default:
            return;
    }
} while (true);


void Display()
{   
    Console.WriteLine("\n---------------------------------------\n");
    Console.WriteLine("Choose type of file:");
    Console.WriteLine("\t1 - Json");
    Console.WriteLine("\t2 - XML");
    Console.WriteLine("\t3 - CSV");
    Console.WriteLine("\t4 - Exit");

    switch (Console.ReadLine())
    {
        case "1":
        // Display Json File
            string jsonPath = CheckJsonFile();
            JsonFileDisplay(jsonPath);
            break;
        case "2":
        // Display XML File
            readXML();
            break;
        case "3":
        // Display CSV File
            readCSV();
            return;

        case "4":
        // Exit
            return;
        default:
            return;
    }

    
}

void readCSV()
{
    Console.WriteLine("Enter the name of the file to display its content:");
    var fileName = Console.ReadLine()!;
    Console.WriteLine("Name of file: {0}", fileName);

    var fileUtility = new FileUtility(new FileSystem());
    var dataList = Enumerable.Empty<Data>();

    if (fileUtility.GetExtension(fileName) == ".csv")
    {
        dataList = new CsvContentParser().Parse(fileUtility.GetContent(fileName));
    }

    Console.WriteLine("Data:");

    foreach (var data in dataList)
    {
        Console.WriteLine($"Key:{data.Key} Value:{data.Value}");
    }
}

void Search()
{
    Console.WriteLine("Enter the key to search.");
}

void readXML()
{
    Console.Write("Enter name of XML file: ");
    String fileName = Console.ReadLine();
    string filePath = "C:\\Users\\pss32\\Desktop\\illuminaTest\\src\\CodingAssignmentApp\\data\\" + fileName + ".xml";
    
    XmlDocument xmlDoc = new XmlDocument();
    xmlDoc.Load(filePath);

    // Check XML Path
    Console.WriteLine($"XML Path: {filePath}");

    XmlNodeList? xmlNodeList = xmlDoc.DocumentElement.SelectNodes("/Datas/Data");

    Console.WriteLine("Data:");
    foreach(XmlNode xmlNode in xmlNodeList)
    {
        string nodeKey = xmlNode.SelectSingleNode("Key").InnerText;
        string nodeValue = xmlNode.SelectSingleNode("Value").InnerText;
        // Print Data
        Console.WriteLine($"Key: {nodeKey} Value: {nodeValue}");
    }
}

string CheckJsonFile()
{
    START:
    // Location of data folder
    string path = "C:\\Users\\pss32\\Desktop\\illuminaTest\\src\\CodingAssignmentApp\\data\\";
    Console.Write("Enter name of JSON file: ");
    
    var response = Console.ReadLine();
    // Console.WriteLine("Name of File: {0}", response);

    response = path + response + ".json";
    Console.WriteLine("Json File Path: {0}", response);

    if (File.Exists(response))
    {
        return response;
    }
    else 
    {
        Console.Write("File does not exist. Please try again.");
        goto START;
    } 
}

void JsonFileDisplay(string jsonFileInput)
{
    string jsonString = File.ReadAllText(jsonFileInput);
    // dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText(jsonFileInput));
    // Console.WriteLine($"Key: {jsonFile["Key"]}");
    // Console.WriteLine($"Key:{jsonFile.Key} Value:{jsonFile.Value}");
    
    // JArray myArray = (JArray)jsonFile["myArray"];

    // foreach (JObject item in myArray)
    // {
    //     string key = (string)item["Key"];
    //     string value = (string)item["Value"];
    //     Console.WriteLine($"Key: {key} Value: {value}");
        
    // }

    Console.WriteLine($"Json String : \n{jsonString}\n");
    // JsonData[] jsonData = JsonConvert.DeserializeObject<JsonData[]>(jsonString);
    // Console.WriteLine(jsonData.Key);

    // use the built in Json deserializer to convert the string to a list of Person objects
    var jsonData = System.Text.Json.JsonSerializer.Deserialize<List<JsonData>>(jsonString);
    Console.WriteLine("Data:");

    foreach (var value in jsonData)
    {
        Console.WriteLine($"Key: {value.Key} Value: {value.Value}");
    }
    
}

public class JsonData 
{
    public string Key { get; set; }
    public string Value { get; set; }
}

