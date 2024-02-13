namespace AF.HtmlTemplater.HtmlCreatorLib.Services.Interfaces
{
    public interface IHtmlCreatorService
    {
        /// <summary>
        /// Метод возвращает HTML код построенный по шаблону и данным (template - путб к файлу шаблона, jsonData - путь к файлу с данными)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="template"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public string CreateHtml<T>(string template, string jsonData);
    }
}
