namespace ShoppingApp.WebApi.Jwt
{
    // JWT token içinde kullanılacak claim isimlerini merkezi bir şekilde tanımlayan yardımcı sınıf
    public static class JwtClaimNames
    {
        // Kullanıcı ID'sini temsil eden claim ismi
        public const string Id = "Id";

        // Kullanıcı e-posta adresini temsil eden claim ismi
        public const string Email = "Email";

        // Kullanıcı adını temsil eden claim ismi
        public const string FirstName = "FirstName";

        // Kullanıcı soyadını temsil eden claim ismi
        public const string LastName = "LastName";

        // Kullanıcı rolünü temsil eden claim ismi
        public const string Role = "Role";
    }
}
