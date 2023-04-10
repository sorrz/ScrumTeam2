using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopGeneral.Services
{
    public class RomanService : IRomanService
    {
        public Dictionary<char, int> TranslateTable = new Dictionary<char, int>() {
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000}
        };
        
        public int? ConvertFromRoman(string roman)
        {
            var romanList = roman.ToList();
            var counter = 0;
            var total = 0;
            List<int> numberList = new List<int>();
            romanList.ForEach(r =>
            {
                if (TranslateTable.TryGetValue(r, out int value))
                {
                    numberList.Add(value);
                }
                //else statment setting total to null, whitch checks below and return null from method!
            });
            numberList.ForEach(n =>
            {
                if(numberList.Count == counter + 1)
                {
                    total += n;
                }
                else
                {
                    if (n >= numberList[counter + 1])
                    {
                        total += n;
                    }
                    else
                    {
                        total -= n;
                    }
                }
                counter++;
            });
            return total;

        }

        public string ConvertToRoman(int number)
        {
            throw new NotImplementedException();
        }
    }
}
