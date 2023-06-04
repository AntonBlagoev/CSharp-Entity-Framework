﻿namespace CarDealer.DTOs.Export;

using System.Xml.Serialization;

[XmlType("part")]
public class ExportPartDto
{
    [XmlAttribute("name")]
    public string Name { get; set; } = null!;

    [XmlAttribute("price")]
    public decimal Price { get; set; }
}
