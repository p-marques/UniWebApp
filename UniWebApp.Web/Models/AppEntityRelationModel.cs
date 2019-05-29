namespace UniWebApp.Web.Models
{
    public class AppEntityRelationModel
    {
        public int Id { get; set; }

        public AppEntityModel RelatedEntity { get; set; }

        public string Description { get; set; }
    }
}
