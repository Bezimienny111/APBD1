using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler
{
    class Program
    {
        public static async Task Main(string[] args)  
        {
        try{
                if (args == null || args.Length == 0)
                {
                    throw new ArgumentNullException();

                }
                else
                {
                    string webURL = args[0];
                    try {
                        if (!testURL(webURL))
                        {
                            throw new ArgumentException();
                        }
                        else
                        {
                            HttpClient httpClietn = new HttpClient();

                            HttpResponseMessage resp = await httpClietn.GetAsync(webURL);
                            if (resp.IsSuccessStatusCode)
                            {
                                httpClietn.Dispose();
                                string reading = resp.Content.ReadAsStringAsync().Result;

                                if (Emails(reading).Equals("non"))
                                {
                                    Console.WriteLine("Nie znaleziono adresów email");
                                }
                                else Console.WriteLine(Emails(reading));
                            }


                            else Console.WriteLine("Błąd w czasie pobierania strony");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(String.Format("Errror!!!!  Zły URL"));
                    }
                }
            
            }
catch (Exception e)
{
                Console.WriteLine(String.Format("Error!!! Brak argumentu"));
}
        }


         static string Emails(string inURL)
        {
            string outString = "non";
            Regex emailReg = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            RegexOptions.IgnoreCase);

            MatchCollection matched = emailReg.Matches(inURL);
            StringBuilder sb = new StringBuilder();
            Hashtable hashtab = new Hashtable();

            foreach (Match emailMathc in matched) {
                string found = emailMathc.ToString();
                if (hashtab.Contains(found) == false)
                {
                    hashtab.Add(found, string.Empty);
                    sb.AppendLine(emailMathc.Value);
                }
                }
            
            StringBuilder test = new StringBuilder();
            if (!sb.Equals(test))
                outString = sb.ToString();


            return outString;
        }

        static bool testURL (string urlIN)
        {
            
            Uri testout;
            bool res = Uri.TryCreate(urlIN, UriKind.Absolute, out testout) && (testout.Scheme == Uri.UriSchemeHttps || testout.Scheme == Uri.UriSchemeHttp);
            return res;
        }
    }
}
