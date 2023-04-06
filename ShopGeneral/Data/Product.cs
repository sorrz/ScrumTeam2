﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ShopGeneral.Data;

public class ProductObject
{
    public int Id { get; set; }
    [MaxLength(50)] public string Name { get; set; }
    [XmlElement]
    public Category Category { get; set; }
    [XmlElement]
    public Manufacturer Manufacturer { get; set; }

    public int BasePrice { get; set; }
    [XmlElement]
    public DateTime AddedUtc { get; set; }
    [MaxLength(100)] public string ImageUrl { get; set; }
}