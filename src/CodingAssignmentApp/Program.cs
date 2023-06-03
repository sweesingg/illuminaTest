// See https://aka.ms/new-console-template for more information

using System.IO.Abstractions;
using CodingAssignmentLib;
using CodingAssignmentLib.Abstractions;

using System.Xml;
// using Newtonsoft.Json;

// File Path (Change if necessary)
string currentDir = Directory.GetCurrentDirectory() + "\\data\\";
string searchTerm = "";

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
            JsonFileDisplay(jsonPath, false);
            break;
        case "2":
        // Display XML File
            readXML("", false);
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

void Search()
{
    Console.WriteLine("Enter the key to search.");
    // string searchTerm = @"aaaaa"; 
    // json search term: 75knWnMBov
    // xml search term: YvXYLQtn7V
    // csv search term: aaaaa

    searchTerm = Console.ReadLine();
    Console.WriteLine(" ");
    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(currentDir);
    IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);  
    
    // Search contents of file
    var queryMatchingFiles = from file in fileList let fileText = GetFileText(file.FullName) where checkStrings(fileText, searchTerm) select file.FullName;

    // Execute Query
    // Console.WriteLine("The Key {0} was found in: ", searchTerm);
    foreach(string filename in queryMatchingFiles)
    {
        var fileUtility = new FileUtility(new FileSystem());

        // JSON
        if (fileUtility.GetExtension(filename)== ".json")
        {
            Console.WriteLine("File Type: JSON");
            JsonFileDisplay(filename, true);

        }
        // XML
        if (fileUtility.GetExtension(filename)== ".xml")
        {
            Console.WriteLine("File Type: XML");
            readXML(filename, true);
        }
        // CSV
        if (fileUtility.GetExtension(filename)== ".csv")
        {
            Console.WriteLine("File Type: CSV");
            // TO DO
        }

        // Print file path
        Console.WriteLine("File Name: {0} \n", filename);
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

string GetFileText(string name)
{
    string fileContents = String.Empty;

    if (System.IO.File.Exists(name))
    {
        fileContents = System.IO.File.ReadAllText(name);
    }

    return fileContents;
}

bool checkStrings(string fileText, string searchTerm)
{
    bool result = false;

    // Method that converts text to lower cases
    if (fileText.ToLower().Contains(searchTerm.ToLower()))
    {
        return result = true;
    }

    return result;
}

void readXML(string fileName, bool searchMode)
{
    string filePath = "";

    if (searchMode == false){
        Console.Write("Enter name of XML file: ");
        fileName = Console.ReadLine();
        Console.WriteLine(" ");
        filePath = currentDir + fileName + ".xml";
    }
    else if (searchMode == true)
    {
        filePath = fileName;
        // Console.WriteLine("CHECK FILE PATH IN XML: {0}", filePath);
    }
    
    XmlDocument xmlDoc = new XmlDocument();
    xmlDoc.Load(filePath);

    // Check XML Path
    // Console.WriteLine($"XML Path: {filePath}");

    XmlNodeList? xmlNodeList = xmlDoc.DocumentElement.SelectNodes("/Datas/Data");

    Console.WriteLine("XML Data:");
    foreach(XmlNode xmlNode in xmlNodeList)
    {
        string nodeKey = xmlNode.SelectSingleNode("Key").InnerText;
        string nodeValue = xmlNode.SelectSingleNode("Value").InnerText;
        
        // Search mode 
        if (searchMode == true)
        {
            if (checkStrings(searchTerm, nodeKey))
            {
                Console.WriteLine($"Key: {nodeKey} Value: {nodeValue}");
                break;
            }
        }
        // Show Data
        Console.WriteLine($"Key: {nodeKey} Value: {nodeValue}");
    }
}

string CheckJsonFile()
{
    START:
    Console.Write("Enter name of JSON file: ");
    var fileName = Console.ReadLine();
    Console.WriteLine(" ");
    string filePath = currentDir + fileName + ".json";
    // Console.WriteLine("Json File Path: {0}", filePath);

    if (File.Exists(filePath))
    {
        return filePath;
    }
    else 
    {
        Console.Write("File does not exist. Please try again.");
        goto START;
    } 
}

void JsonFileDisplay(string jsonFileInput, bool searchMode)
{
    string jsonString = File.ReadAllText(jsonFileInput);
    // Console.WriteLine($"Json String : \n{jsonString}\n");

    // Built in Json deserializer to convert the string to a list of JsonData objects
    var jsonData = System.Text.Json.JsonSerializer.Deserialize<List<JsonData>>(jsonString);
    Console.WriteLine("Json Data:");
    
    foreach (var info in jsonData)
    {
        // Search Mode
        if (searchMode == true)
        {
            if (checkStrings(searchTerm, info.Key))
            {
                Console.WriteLine($"Key: {info.Key} Value: {info.Value}");
                break;
            }
        }
        // Print Json Data
        Console.WriteLine($"Key: {info.Key} Value: {info.Value}");
    }
    
}

public class JsonData 
{
    public string Key { get; set; }
    public string Value { get; set; }
}


