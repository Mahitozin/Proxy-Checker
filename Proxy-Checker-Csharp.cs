using Colorful;
using System.Drawing;
using System.Net;
using System.Text;
using Console = Colorful.Console;

namespace ProxyChecker
{
    class Program
    {
        public static void Main()
        {
            Console.ForegroundColor = Color.DarkMagenta;
            Console.WriteLine(@"______                      _____ _               _             
| ___ \                    /  __ \ |             | |            
| |_/ / __ _____  ___   _  | /  \/ |__   ___  ___| | _____ _ __ 
|  __/ '__/ _ \ \/ / | | | | |   | '_ \ / _ \/ __| |/ / _ \ '__|
| |  | | | (_) >  <| |_| | | \__/\ | | |  __/ (__|   <  __/ |   
\_|  |_|  \___/_/\_\\__, |  \____/_| |_|\___|\___|_|\_\___|_|   
                     __/ |                                      
                    |___/                                       
By Asterism
"); 
            Console.ResetColor();
            //Coloca um banner

            foreach (string x in File.ReadAllLines("proxys.txt")) //Pega todas as linhas do arquivo proxys.txt
            {
                DateTime dt = DateTime.Now;
                var watch = System.Diagnostics.Stopwatch.StartNew(); //Inicia um cronometo
                bool result = Checker.ProxyCheck(x.Split(':')[0], int.Parse(x.Split(':')[1])); //Verifica se a proxy esta viva
                watch.Stop(); //Finaliza o cronometo
                var elapsedMs = watch.ElapsedMilliseconds; //Pega o tempo que o cronometo foi executado em Milissegundo

                //Gambiarra total para deixar todo do mesmo tamanho
                string ip = x.Split(':')[0];
                string porta = x.Split(':')[1].ToString();
                for (int y = porta.Length; y != 5; y++)
                    porta += " ";
                for (int y = ip.Length; y != 15; y++)
                    ip += " ";

                if (result) //Verifica se o resultado do check for True ou False
                {
                    string dream = "[{0}] Ip -> {1} Porta -> {2} Tempo -> {3}s";
                    Formatter[] fruits = new Formatter[]
                    {
                        new Formatter("+", Color.Green),
                        new Formatter(ip, Color.Green),
                        new Formatter(porta, Color.Green),
                        new Formatter((DateTime.Now.Subtract(dt)).Seconds, Color.Green),
                    };

                    Console.WriteLineFormatted(dream, Color.Gray, fruits); //Escreve o texto de sucesso
                }
                else
                {
                    string dream = "[{0}] Ip -> {1} Porta -> {2} Tempo -> {3}s";
                    Formatter[] fruits = new Formatter[]
                    {
                        new Formatter("-", Color.Red),
                        new Formatter(ip, Color.Red),
                        new Formatter(porta, Color.Red),
                        new Formatter((DateTime.Now.Subtract(dt)).Seconds, Color.Red),
                    };

                    Console.WriteLineFormatted(dream, Color.Gray, fruits); //Escreve o texto de erro
                }
            }

            Console.ReadLine();
        }
    }

    public class Checker
    {
        private static readonly string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:68.0) Gecko/20100101 Firefox/68.0"; 

        public static bool ProxyCheck(string ipAddress, int port)
        {
            try
            {
                ICredentials credentials = CredentialCache.DefaultCredentials;
                IWebProxy proxy = new WebProxy(ipAddress, port); //Coloca o ip e a porta na proxy
                proxy.Credentials = credentials; 

                using (var wc = new WebClient())
                {
                    wc.Proxy = proxy; //Aplica a proxy o webcliente
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers.Add("User-Agent", UserAgent); //Coloca o UserAgent acima
                    string result = wc.DownloadString("http://checkip.dyndns.org/"); //Faz um request e pega o texto do site
                    return result.Contains(ipAddress); //Retorna o resultado do request
                }
            }
            catch
            {
                return false; //Retorna false se acontecer algum erro ao fazer o request
            }

        }
    }
}
