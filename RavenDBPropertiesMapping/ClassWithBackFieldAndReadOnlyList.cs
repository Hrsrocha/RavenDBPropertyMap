namespace RavenDBPropertiesMapping.DomainModels
{
    public class ClassWithBackFieldAndReadOnlyList
    {
        public string Id { get; set; }
        public string AnotherProperty { get; private set; }
        private List<FakeModel> _myList;
        public IReadOnlyList<FakeModel> MyList => _myList.AsReadOnly();

        public ClassWithBackFieldAndReadOnlyList(string anotherProperty)
        {
            AnotherProperty = anotherProperty;
            _myList = new List<FakeModel>();
        }

        public void AddFakeModelToList(FakeModel fakeModel)
        {
            _myList.Add(fakeModel);
        }
    }

    public class FakeModel
    {
        public string FakeProperty { get; set; }
    }
}
