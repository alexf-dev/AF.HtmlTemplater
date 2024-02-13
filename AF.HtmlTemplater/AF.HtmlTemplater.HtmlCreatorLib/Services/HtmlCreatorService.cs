using AF.HtmlTemplater.HtmlCreatorLib.Extensions;
using AF.HtmlTemplater.HtmlCreatorLib.Services.Interfaces;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace AF.HtmlTemplater.HtmlCreatorLib.Services
{
    public class HtmlCreatorService : IHtmlCreatorService
    {
        public string CreateHtml<T>(string template, string jsonData) 
        {
            var dataCollection = GetDeserializeCollectionFromJson<T>(jsonData);

            var templateData = new List<string>();
            using (var tempStr = new StreamReader(template))
            {
                string templateString;
                while ((templateString = tempStr.ReadLine()) != null)
                {
                    templateData.Add(templateString);
                }
            }

            return GetHtmlByTemplateData(templateData, dataCollection);
        }

        private string GetHtmlByTemplateData<T>(List<string> template, ICollection<T> dataCollection)
        {
            var result = new StringBuilder();

            for (int i = 0; i < template.Count;)
            {
                if (IsNeedParsingTemplateString(template[i]))
                {
                    i = CheckAndParsingTemplateStrings(template, i, dataCollection, ref result);
                }
                else
                {
                    result.AppendLine(template[i]);
                    i++;
                }
            }

            return result.ToString();
        }

        private int CheckAndParsingTemplateStrings<T>(List<string> template, int index, ICollection<T> dataCollection, ref StringBuilder inputResult)
        {
            var endIndex = index;

            if (template[index].Contains("{%") && template[index].Contains("%}") && template[index].Contains($"for {typeof(T).Name.ToLower()}"))
            {
                var endFor = template.FirstOrDefault(it => it.Contains("{%") && it.Contains("%}") && it.Contains($"endfor"));
                if (endFor != null)
                {
                    endIndex = template.IndexOf(endFor);

                    foreach (var data in dataCollection)
                    {
                        for (int i = index + 1; i < endIndex; i++)
                        {
                            if (IsNeedParsingTemplateString(template[i]))
                            {
                                inputResult.AppendLine(GetParsedTemplateString(data, template[i]));
                            }
                            else
                            {
                                inputResult.AppendLine(template[i]);
                            }
                        }
                    }
                }
            }

            return endIndex + 1;
        }

        private string GetParsedTemplateString<T>(T dataItem, string templateString)
        {
            var result = string.Empty;
            var afterString = string.Empty;

            var firstIndex = templateString.IndexOf('{');
            result = firstIndex > 0 ? templateString.Substring(0, firstIndex) : string.Empty;

            var lastIndex = templateString.LastIndexOf('}');
            afterString = lastIndex > 0 ? templateString.Substring(lastIndex + 1) : string.Empty;

            var dataString = templateString.GetSubstring(firstIndex, lastIndex);
            dataString = dataString.Replace("{", "");
            dataString = dataString.Replace("}", "");
            dataString = dataString.Trim();

            var items = dataString.Split('|').Select(it => it.Trim()).ToList();
            if (items.Any() && items.Count > 1)
            {
                result += GetParsedPropertyString(dataItem, items[0], items[1].Trim());
            }
            else
            {
                result += GetParsedPropertyString(dataItem, items[0]);
            }            

            return result += afterString;
        }

        private string GetParsedPropertyString<T>(T dataItem, string propertyString, string? modificator = null)
        {
            var result = string.Empty;

            foreach (var item in dataItem.GetType().GetProperties())
            {
                if (propertyString.Contains(item.Name.ToLower()))
                {
                    switch (modificator)
                    {
                        case "price":
                            {
                                decimal price = 0;
                                if (decimal.TryParse(item.GetValue(dataItem, null).ToString(), out price))
                                {
                                    result = string.Format(new CultureInfo("En-us"), "{0:c}", price);
                                }
                                break;
                            }
                        case "paragraph":
                        default:
                            {
                                result = item.GetValue(dataItem, null).ToString();
                                break;
                            }

                    }
                                        
                    break;
                }
            }            

            return result;
        }

        private bool IsNeedParsingTemplateString(string templateString)
        {
            if (string.IsNullOrWhiteSpace(templateString))
            {
                return false;
            }

            if (templateString.Contains('{') && templateString.Contains('}'))
            {
                return true;
            }

            return false;
        }

        private ICollection<T> GetDeserializeCollectionFromJson<T>(string jsonData)
        {
            var result = new List<T>();

            var json = File.ReadAllText(jsonData);
            if (!string.IsNullOrWhiteSpace(json))
            {
                var parsed = JsonConvert.DeserializeObject<Dictionary<string, ICollection<T>>>(json);
                if (parsed != null)
                {
                    foreach (var data in parsed)
                    {
                        result.AddRange(data.Value);
                    }
                }
            }

            return result.Distinct().ToList();
        }
    }
    
}
