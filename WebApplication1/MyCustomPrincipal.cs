using System.Security.Principal;

public class MyCustomPrincipal : GenericPrincipal
{
    public MyCustomPrincipal(IIdentity identity, string[] roles, string apid, string firstName, string lastName, string unitCode)
        : base(identity, roles)
    {
        APID = apid;
        FirstName = firstName;
        LastName = lastName;
        UnitCode = unitCode;
    }

    public string APID { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string UnitCode { get; private set; }
    public string Name { get { return FirstName + " " + LastName + ", " + UnitCode; } }
}