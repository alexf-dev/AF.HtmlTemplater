using AF.HtmlTemplater.Data.Models.Dto;
using AF.HtmlTemplater.HtmlCreatorLib.Models;
using AF.HtmlTemplater.HtmlCreatorLib.Services;

namespace AF.HtmlTemplater.Api
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine(Environment.NewLine + @"<-- Start Templater API -->" + Environment.NewLine);
            Console.WriteLine(@"Enter the command (-help getting on help):" + Environment.NewLine);

            var inputDate = Console.ReadLine();

            while (inputDate.ToLower() != "-exit")
            {                
                if (string.IsNullOrWhiteSpace(inputDate))
                {
                    Console.WriteLine("Enter the command (-help getting on help):" + Environment.NewLine);
                }                
                else if (inputDate.Contains("-help"))
                {
                    Console.WriteLine("To get an HTML file, run the -convert command, specifying the following parameters with spaces:");
                    Console.WriteLine("Example:");
                    Console.WriteLine("-convert -dataFilePath: dataFile.txt -templateFilePath: templateFile.txt -outputFilePath: outputFile.html" + Environment.NewLine);
                }
                else if (!GetValidationCommandLine(inputDate))
                {
                    Console.WriteLine("Enter the correct command (-help getting on help):" + Environment.NewLine);
                }
                else if (GetValidationCommandLine(inputDate))
                {
                    var argsModel = GetParsedConsoleArgsModel(inputDate);

                    try
                    {
                        var creatorService = new HtmlCreatorService();

                        File.WriteAllText(argsModel.OutputFilePath, creatorService.CreateHtml<Product>(argsModel.TemplateFilePath, argsModel.DataFilePath));
                        Console.WriteLine(Environment.NewLine + $"Conversion was successful. The result is written to a file: {argsModel.OutputFilePath}" + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(Environment.NewLine + "An error occurred in the application. " + ex.Message + Environment.NewLine);
                        Console.WriteLine("Enter the command (-help getting on help):" + Environment.NewLine);
                    }
                }

                inputDate = Console.ReadLine();
            }
        }

        private static ConsoleArgsDto GetParsedConsoleArgsModel(string input)
        {
            var model = new ConsoleArgsDto();

            input = input.Trim();
            input = input.Replace("-convert", "");

            foreach (var item in input.Split('-', StringSplitOptions.RemoveEmptyEntries))
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }

                if (item.Contains("dataFilePath:"))
                {
                    model.DataFilePath = item.Replace("dataFilePath:", "").Trim();
                }
                else if (item.Contains("templateFilePath:"))
                {
                    model.TemplateFilePath = item.Replace("templateFilePath:", "").Trim();
                }
                else if (item.Contains("outputFilePath:"))
                {
                    model.OutputFilePath = item.Replace("outputFilePath:", "").Trim();
                }
            }

            return model;
        }

        private static bool GetValidationCommandLine(string inputDate)
        {
            if (inputDate.Contains("-exit") || inputDate.Contains("-help") || inputDate.Contains("-convert"))
            {
                if (inputDate.Contains("-convert"))
                {
                    if (inputDate.Contains("-dataFilePath") && inputDate.Contains("-templateFilePath") && inputDate.Contains("-outputFilePath"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}