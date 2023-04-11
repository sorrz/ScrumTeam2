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

        public Dictionary<char, string> SingularTable = new Dictionary<char, string>()
        {
            {'1', "I"},
            {'2', "II"},
            {'3', "III"},
            {'4', "IV"},
            {'5', "V"},
            {'6', "VI"},
            {'7', "VII"},
            {'8', "VIII"},
            {'9', "IX"}
        };
        public Dictionary<int, string> TensTable = new Dictionary<int, string>()
        {
            {'1', "X"},
            {'2', "XX"},
            {'3', "XXX"},
            {'4', "XL"},
            {'5', "L"},
            {'6', "LX"},
            {'7', "LXX"},
            {'8', "LXXX"},
            {'9', "XC"}
        };
        public Dictionary<int, string> HundredsTable = new Dictionary<int, string>()
        {
            {'1', "C"},
            {'2', "CC"},
            {'3', "CCC"},
            {'4', "CD"},
            {'5', "D"},
            {'6', "DC"},
            {'7', "DCC"},
            {'8', "DCCC"},
            {'9', "CM"}
        };
        public Dictionary<int, string> ThousandsTable = new Dictionary<int, string>()
        {
            {'1', "M"},
            {'2', "MM"},
            {'3', "MMM"},
            {'4', "MMMM"},
            {'5', "MMMMM"},
            {'6', "MMMMMM"},
            {'7', "MMMMMMM"},
            {'8', "MMMMMMMM"},
            {'9', "MMMMMMMMM"}
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
            if (!(1 < number) && !(number < 9999))
            {
                return "Provide a number between 1 and 9999 please.";
            }
            var listOfNumbers = number.ToString().ToList();
            List<string> RomanNumber = new List<string>();

            var counter = 0;
            listOfNumbers.Reverse();
            for (int i = 0; i < listOfNumbers.Count; i++)
            {
                string value;
                if (i == 3)
                {
                    if (ThousandsTable.TryGetValue(listOfNumbers[i], out value))
                        RomanNumber.Insert(0, value);
                }
                if (i == 2)
                {
                    if (HundredsTable.TryGetValue(listOfNumbers[i], out value))
                        RomanNumber.Insert(0, value);
                }
                if (i == 1)
                {
                    if (TensTable.TryGetValue(listOfNumbers[i], out value))
                        RomanNumber.Insert(0, value);
                }
                if (i == 0)
                {
                    if (SingularTable.TryGetValue(listOfNumbers[i], out value))
                        RomanNumber.Insert(0, value);
                }
            }

            return string.Join("",RomanNumber);
        }
    }
}
