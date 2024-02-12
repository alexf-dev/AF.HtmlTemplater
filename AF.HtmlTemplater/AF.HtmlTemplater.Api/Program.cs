
namespace AF.HtmlTemplater.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Templater API created!");
            var inputDate = Console.ReadLine();

            while (inputDate.ToLower() != "exit")
            {
                if (string.IsNullOrWhiteSpace(inputDate))
                {
                    Console.WriteLine("Input a command, please...");
                    inputDate = Console.ReadLine();
                }
                else
                {
                    //work code
                    inputDate = Console.ReadLine();
                }
            }
        }
    }
}