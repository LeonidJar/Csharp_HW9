using System;
using System.IO;
using System.Text.Json;
using System.Xml;

class Program
{
    static void Main()
    {

        string json = "{\"Id\":\"123\",\"DateOfRegistration\":\"2012-10-21T00:00:00+05:30\",\"Status\":0}";
        //@"{ ""name"": ""John"", ""age"": 30, ""city"": ""New York"" }";

        using (JsonDocument document = JsonDocument.Parse(json))
        {
            XmlElement rootElement = CreateXmlElement(document.RootElement);

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.AppendChild(xmlDoc.ImportNode(rootElement, true));

            xmlDoc.Save("output.xml");
        }
    }

    public static XmlElement CreateXmlElement(JsonElement jsonElement)
    {
        XmlDocument xmlDoc = new XmlDocument();

        XmlElement element = xmlDoc.CreateElement(jsonElement.ValueKind.ToString());

        switch (jsonElement.ValueKind) 
        { 
            case JsonValueKind.Object:

                foreach (JsonProperty property in jsonElement.EnumerateObject())
                {
                    XmlElement propertyElement = CreateXmlElement(property.Value);
                    propertyElement.SetAttribute("name", property.Name); 
                    element.AppendChild(xmlDoc.ImportNode(propertyElement, true));
                }
                break;

            case JsonValueKind.Array:
                foreach (JsonElement arrayElement in jsonElement.EnumerateArray())
                {
                    XmlElement arrayItemElement = CreateXmlElement(arrayElement);
                    element.AppendChild(arrayItemElement);
                }
                break;

            case JsonValueKind.String:
                element.InnerText = jsonElement.GetString();
                break;

            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
                element.InnerText = jsonElement.GetRawText();
                break;

            case JsonValueKind.Null:
                element.SetAttribute("null", "true");
                break;
        }

        return element;
    }
}
