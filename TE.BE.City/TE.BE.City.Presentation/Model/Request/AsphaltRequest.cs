namespace TE.BE.City.Presentation.Model.Request
{
    public class AsphaltRequest : BaseModel
    {
        //A via é asfaltada?
        public bool IsPaved { get; set; }
        //A via possui buracos ou crateras?
        public bool HasHoles { get; set; }
        // Há calçadas pavimentadas?
        public bool HasPavedSidewalks { get; set; }
    }
}
