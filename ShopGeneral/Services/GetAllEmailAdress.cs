using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopGeneral.Data;

namespace ShopGeneral.Services
{
    public class GetAllEmailAdress : Manufacturer
    {
        public static List<string> RetrieveAllEmailAddresses(List<Manufacturer> manufacturers)
        {
            // Filter out the manufacturers that don't have an email address
            var manufacturersWithEmail = manufacturers.Where(m => !string.IsNullOrEmpty(m.EmailReport));

            // Extract the email addresses
            var emailAddresses = manufacturersWithEmail.Select(m => m.EmailReport).ToList();

            return emailAddresses;
        }

    }
}
