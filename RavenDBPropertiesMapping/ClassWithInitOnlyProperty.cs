namespace RavenDBPropertiesMapping.DomainModels
{
    public class ClassWithInitOnlyProperty
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; }

        public ClassWithInitOnlyProperty()
        {
            CreatedDate = DateTime.Now;
        }
    }
}
