namespace BE.Common
{
    public class CustomerDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public List<VehicleDTO>? Vehicles { get; set; }
    }
}
