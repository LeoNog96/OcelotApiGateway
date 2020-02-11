namespace CatalogoWebApi.Jwt
{
    public class TokenConfigurations
    {
        public string SecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Days { get; set; }
    }
}