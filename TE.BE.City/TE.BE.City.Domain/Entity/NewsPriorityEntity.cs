using TE.BE.City.Infra.CrossCutting.Enum;

namespace TE.BE.City.Domain.Entity
{
    public class NewsPriorityEntity : BaseEntity
    {
        public int OccurrenceId { get; set; }
        public TypeIssue OccurrenceType { get; set; }
        public int Weight { get; set; }
        public int Score { get; set; }
        public string Address { get; set; }
    }
}
