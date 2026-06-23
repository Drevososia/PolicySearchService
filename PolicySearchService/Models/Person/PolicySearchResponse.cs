using PolicySearchService.Models.Person;

namespace PolicySearchService.Models.Person;

public class PolicySearchResponse
{
    public string? FAM { get; set; }
    public string? IM { get; set; }
    public string? OT { get; set; }
    public int? W { get; set; }
    public DateTime? DR { get; set; }
    public DateTime? DS { get; set; }
    public string? SMO { get; set; }
    public string? SPOL { get; set; }
    public string? NPOL { get; set; }
    public string? ENP { get; set; }
    public int? OPDOC { get; set; }

    public PolicySearchResponse() { }

    public PolicySearchResponse(Person person)
    {
        FAM = person.FAM;
        IM = person.IM;
        OT = person.OT;
        W = person.W;
        DR = person.DR;
        DS = person.DS;
        SMO = person.SMO;
        SPOL = person.SPOL;
        NPOL = person.NPOL;
        ENP = person.ENP;
        OPDOC = person.OPDOC;
    }
}
