namespace Configuration
{
    public interface IConfiguration
    {
        public string this[string key]
        {
            get; set;
        }
    }
}
