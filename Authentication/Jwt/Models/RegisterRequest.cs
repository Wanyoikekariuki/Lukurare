namespace Authentication.Jwt.Models
{
    public class RegisterRequest
    {
        public string Username { get; set; }

        //[Required]


        public string NationalID { get; set; }

        public string Domain { get; set; }
        public string Phone { get; set; }
        public string PhysicalAddress { get; set; }
        public string PostalAddress { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string EntityTypeName { get; set; }
    }
}
