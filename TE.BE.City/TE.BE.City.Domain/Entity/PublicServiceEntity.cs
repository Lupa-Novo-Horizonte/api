namespace TE.BE.City.Domain.Entity
{
    public class PublicServiceEntity : BaseEntity
    {
        public PublicServiceEntity()
        {
            Status = new StatusEntity();
            User = new UserEntity();
        }

        public int Service { get; set; }
    }
}
