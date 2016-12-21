namespace urednistvo.ModelsDB
{
    public class Rating
    {
        public int TextId { get; set; }
        public int PersonId { get; set; }
        public bool? WebPublishable { get; set; }
        public int Rate { get; set; }
        public string SectionTitle { get; set; }

        public virtual User Person { get; set; }
        public virtual Text Text { get; set; }
    }
}