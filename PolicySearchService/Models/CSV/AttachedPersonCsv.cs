using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace PolicySearchService.Models.CSV;

public class AttachedPerson
{
    public string? ENP { get; set; }
    public string? SMO { get; set; }
    public string? SMO_NAME { get; set; }
    public string? FAM { get; set; }
    public string? IM { get; set; }
    public string? OT { get; set; }
    public int? GENDER { get; set; }
    public DateTime? DR { get; set; }
    public DateTime? DATE_DEATH { get; set; }
    public string? ADDRESS_REGION { get; set; }
    public string? ADDRESS_DISTRICT { get; set; }
    public string? ADDRESS_LOCALITY { get; set; }
    public string? ADDRESS_STREET { get; set; }
    public string? ADDRESS_HOME { get; set; }
    public string? ADDRESS_HOUSING { get; set; }
    public string? ADDRESS_FLAT { get; set; }
    public string? ACTUAL_MO { get; set; }
    public DateTime? DATE_ATTACHED { get; set; }
    public DateTime? DATE_DETACHED { get; set; }
    public string? SITE { get; set; }
    public string? SITE_TYPE { get; set; }
    public string? SITE_PODR { get; set; }
    public bool? ACTUALITY { get; set; }
    public DateTime? INSURANCE_DATE_END { get; set; }
}

public sealed class AttachedPersonMap : ClassMap<AttachedPerson>
{
    public AttachedPersonMap()
    {
        var dateConverter = new RussianDateConverter();
        Map(x => x.ENP).Index(0);
        Map(x => x.SMO).Index(1);
        Map(x => x.SMO_NAME).Index(2);
        Map(x => x.FAM).Index(3);
        Map(x => x.IM).Index(4);
        Map(x => x.OT).Index(5);
        Map(x => x.GENDER).Index(6);
        Map(x => x.DR).Index(7).TypeConverter(dateConverter);
        Map(x => x.DATE_DEATH).Index(8).TypeConverter(dateConverter);
        Map(x => x.ADDRESS_REGION).Index(9);
        Map(x => x.ADDRESS_DISTRICT).Index(10);
        Map(x => x.ADDRESS_LOCALITY).Index(11);
        Map(x => x.ADDRESS_STREET).Index(12);
        Map(x => x.ADDRESS_HOME).Index(13);
        Map(x => x.ADDRESS_HOUSING).Index(14);
        Map(x => x.ADDRESS_FLAT).Index(15);
        Map(x => x.ACTUAL_MO).Index(16);
        Map(x => x.DATE_ATTACHED).Index(17).TypeConverter(dateConverter);
        Map(x => x.DATE_DETACHED).Index(18).TypeConverter(dateConverter);
        Map(x => x.SITE).Index(19);
        Map(x => x.SITE_TYPE).Index(20);
        Map(x => x.SITE_PODR).Index(21);
        Map(x => x.ACTUALITY).Index(22);
        Map(x => x.INSURANCE_DATE_END).Index(23).TypeConverter(dateConverter);
    }
}

public class RussianDateConverter : ITypeConverter
{
    private static readonly string[] DateFormats =
    [
        "dd.MM.yyyy", "d.M.yyyy", "dd.MM.yy", "d.M.yy",
        "dd.MM.yyyy H:mm:ss", "dd.MM.yyyy HH:mm:ss",
        "d.M.yyyy H:mm:ss", "d.M.yyyy HH:mm:ss",
        "dd.MM.yyyy H:mm", "dd.MM.yyyy HH:mm"
    ];

    public object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        var trimmed = text.Trim();
        if (DateTime.TryParseExact(trimmed, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            return dt;

        return null;
    }

    public string? ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        return value switch
        {
            DateTime dt => dt.ToString("yyyy-MM-dd"),
            _ => null
        };
    }
}
