namespace App.Areas.Crm.Enums
{
    public enum Gender
    {
        Female = 1,
        Male = 2
    }

    public enum ContactCategory
    {
        Person = 0,
        Company = 1
    }

    public enum EmailCategory
    {
        Work = 1,
        Personal = 2,
        Other = 3
    }

    public enum Salutation
    {
        Mr = 1,
        Mrs = 2,
        Ms = 3,
        Dr = 4,
        Prof = 5,
        Other = 6
    }

    public enum CivilStatus
    {
        Other = 1,
        Single = 2,
        Married = 3,
        Divorced = 4
    }

    public enum CompanyCategory
    {
        Limited = 1,
        Ngo = 2,
        Other = 3
    }

    public enum AddressCategory
    {
        Home = 1,
        Work = 2,
        Other = 0
    }

    public enum ContactEventCategory
    {
        Birthday = 0,
        Anniversary = 1,
        Engaged = 2
    }

    public enum PhoneCategory
    {
        Mobile = 0,
        Office = 1,
        Home = 2,
        Fax = 3,
        Other = 4
    }

    public enum IdentificationCategory
    {
        Nin = 0,
        Passport = 1,
        DrivingPermit = 2,
        VillageCard = 3,
        Nssf = 4,
        Other = 5
    }
}
