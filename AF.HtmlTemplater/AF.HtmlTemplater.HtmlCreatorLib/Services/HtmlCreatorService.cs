using AF.HtmlTemplater.HtmlCreatorLib.Services.Interfaces;
using Newtonsoft.Json;
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

        private string GetHtmlByTemplateData<T>(ICollection<string> template, ICollection<T> dataCollection)
        {
            var result = new StringBuilder();

            return result.ToString();
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
