namespace PhoneCentre.Data.Databases
{
    public class DbOptions 
    {
        public const string SectionName = "DbOptions";
        public string UserId { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string IPaddress { get; set; }
        public string Port { get; set; }
    }
}
