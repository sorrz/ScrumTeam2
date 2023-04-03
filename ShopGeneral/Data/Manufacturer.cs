using System.ComponentModel.DataAnnotations;

namespace ShopGeneral.Data;

public class Manufacturer
{
    public int Id { get; set; }
    [MaxLength(50)] public string Name { get; set; }

    [MaxLength(50)] public string Icon { get; set; }
    [MaxLength(150)] public string EmailReport { get; set; }

    //public Manufacturer(int id, string name,string icon, string emailReport)
    //{
    //    Id = id;
    //    Name = name;
    //    Icon = icon;
    //    EmailReport = emailReport;
    //}

}