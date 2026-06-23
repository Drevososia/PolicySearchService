using FastMember;
using PolicySearchService.Models.CSV;

namespace PolicySearchService.DataReaders;

public class AttachedPersonReaderBuilder : IDataReaderBuilder<AttachedPerson>
{
    public ObjectReader BuildReader(IEnumerable<AttachedPerson> records)
    {
        return ObjectReader.Create(records, "ENP", "SMO", "SMO_NAME",
            "FAM", "IM", "OT", "GENDER", "DR", "DATE_DEATH",
            "ADDRESS_REGION", "ADDRESS_DISTRICT", "ADDRESS_LOCALITY", "ADDRESS_STREET",
            "ADDRESS_HOME", "ADDRESS_HOUSING", "ADDRESS_FLAT", "ACTUAL_MO",
            "DATE_ATTACHED", "DATE_DETACHED", "SITE", "SITE_TYPE", "SITE_PODR",
            "ACTUALITY", "INSURANCE_DATE_END");
    }
}
