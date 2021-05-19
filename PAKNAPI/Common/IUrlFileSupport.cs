namespace PAKNAPI.Common
{
    internal interface IUrlFileSupport
    {
        string UrlHdsdCV { get; set; }
        string UrlHdsdLdcb { get; set; }
        string UrlHdsdQtht { get; set; }
        string UrlHdsdDb { get; set; }
    }
    public class UrlFileSupport : IUrlFileSupport
    {
        public string UrlHdsdCV { get; set; }
        public string UrlHdsdLdcb { get; set; }
        public string UrlHdsdQtht { get; set; }
        public string UrlHdsdDb { get; set; }
    }
}