using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LottoServiceHost.ServiceReference1;

namespace LottoServiceHost
{
    internal class Program
    {
        private static ServiceReference1.LottoClient lc;

        static void Main(string[] args)
        {
            Console.WriteLine("Service is running...");

            lc = new ServiceReference1.LottoClient();
            lc.LottoGame();
            Console.ReadLine();

        }
    }
}
