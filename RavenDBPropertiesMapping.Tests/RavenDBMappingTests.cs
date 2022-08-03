using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide.Operations;
using RavenDBPropertiesMapping.DomainModels;
using System;
using Xunit;

namespace RavenDBPropertiesMapping.Tests
{
    public class RavenDBMappingTests
    {
        [Fact(DisplayName = "Load a class with backfield and readonly list with same session it was created")]
        public void ClassWithBackFieldAndReadOnlyList_WhenLoadWithSameSessionItWasCreated_ShouldReturnSameModel()
        {
            //Arrange
            var session = CreateSession();
            var domainModel = new ClassWithBackFieldAndReadOnlyList("Random data");
            domainModel.AddFakeModelToList(new FakeModel { FakeProperty = "FakeProperty1" });
            domainModel.AddFakeModelToList(new FakeModel { FakeProperty = "FakeProperty2" });

            session.Store(domainModel);
            session.SaveChanges();

            //Act
            var ravenDbFindModel = session.Load<ClassWithBackFieldAndReadOnlyList>(domainModel.Id);

            //Assert
            Assert.Equal(domainModel.AnotherProperty, ravenDbFindModel.AnotherProperty);
            Assert.Equal(domainModel.MyList, ravenDbFindModel.MyList);
        }

        [Fact(DisplayName = "Load a class with init only property with same session it was created")]
        public void ClassWithInitOnlyProperty_WhenLoadWithSameSessionItWasCreated_ShouldReturnSameDateValue()
        {
            //Arrange
            var session = CreateSession();
            var domainModel = new ClassWithInitOnlyProperty();

            session.Store(domainModel);
            session.SaveChanges();

            //Act
            var ravenDbFindModel = session.Load<ClassWithInitOnlyProperty>(domainModel.Id);

            //Assert
            Assert.Equal(domainModel.CreatedDate, ravenDbFindModel.CreatedDate);
        }

        [Fact(DisplayName = "Load a class with backfield and readonly list with different session it was created")]
        public void ClassWithBackFieldAndReadOnlyList_WhenLoadWithDifferentSessionItWasCreated_ShouldReturnSameModel()
        {
            //Arrange
            var session = CreateSession();
            var domainModel = new ClassWithBackFieldAndReadOnlyList("Random data");
            domainModel.AddFakeModelToList(new FakeModel { FakeProperty = "FakeProperty1" });
            domainModel.AddFakeModelToList(new FakeModel { FakeProperty = "FakeProperty2" });

            session.Store(domainModel);
            session.SaveChanges();

            var session2 = CreateSession();

            //Act
            var ravenDbFindModel = session2.Load<ClassWithBackFieldAndReadOnlyList>(domainModel.Id);

            //Assert
            Assert.Equal(domainModel.AnotherProperty, ravenDbFindModel.AnotherProperty);
            Assert.Equal(domainModel.MyList, ravenDbFindModel.MyList);
        }

        [Fact(DisplayName = "Load a class with init only property with different session it was created")]
        public void ClassWithInitOnlyProperty_WhenLoadWithDifferentSessionItWasCreated_ShouldReturnSameDateValue()
        {
            //Arrange
            var session = CreateSession();
            var domainModel = new ClassWithInitOnlyProperty();

            session.Store(domainModel);
            session.SaveChanges();

            var session2 = CreateSession();

            //Act
            var ravenDbFindModel = session2.Load<ClassWithInitOnlyProperty>(domainModel.Id);

            //Assert
            Assert.Equal(domainModel.CreatedDate, ravenDbFindModel.CreatedDate);
        }


        private static IDocumentSession CreateSession()
        {
            var store = new DocumentStore()
            {
                Urls = new string[] { "http://localhost:8080" },
                Database = "Test"
            };

            store.Initialize();

            //Avoid to create a database manually (tests only)
            TryToCreateDatabase(store);

            return store.OpenSession();
        }

        private static void TryToCreateDatabase(IDocumentStore store)
        {
            try
            {
                store.Maintenance.Server.Send(new CreateDatabaseOperation(new Raven.Client.ServerWide.DatabaseRecord(store.Database)));
            }
            catch (Exception)
            {
            }
        }
    }
}