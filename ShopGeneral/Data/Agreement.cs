using System.ComponentModel.DataAnnotations;

namespace ShopGeneral.Data;

public class Agreement
{
    public int Id { get; set; }

    [MaxLength(30)] public string Name { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }

    public List<AgreementRow> AgreementRows { get; set; } = new();
}

public class AgreementRow
{
    public int Id { get; set; }
    public string? ManufacturerMatch { get; set; }
    public string? ProductMatch { get; set; }
    public string? CategoryMatch { get; set; }

    public decimal PercentageDiscount { get; set; }
}