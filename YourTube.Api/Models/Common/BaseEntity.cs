namespace YourTube.Api.Models.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastEditTime { get; set; }
    }
}
