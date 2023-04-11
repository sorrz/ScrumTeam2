using ShopGeneral.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAdmin.Commands
{
    public class Roman : ConsoleAppBase
    {
        private readonly IRomanService _romanService;

        public Roman(IRomanService romanService)
        {
            _romanService = romanService;
        }

        public void Fromroman()
        {
            var romanNumber = "MCMDXXVI";
            var result = _romanService.ConvertFromRoman(romanNumber);
            Console.WriteLine(result);
            Console.ReadKey();
        }
        public void Toroman()
        {
            var testNumber = 2023;
            var result = _romanService.ConvertToRoman(testNumber);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
