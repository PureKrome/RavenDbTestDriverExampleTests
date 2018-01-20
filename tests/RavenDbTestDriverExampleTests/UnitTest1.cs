using System;
using System.Linq;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.TestDriver;
using Xunit;

namespace RavenDbTestDriverExampleTests
{
    public class UnitTest1 : RavenTestDriver<CustomRavenServerLocator>
    {
        public class TestDocumentByName : AbstractIndexCreationTask<TestDocument>
        {
            public TestDocumentByName()
            {
                Map = docs => from doc in docs
                              select new {doc.Name};
                Indexes.Add(x => x.Name, FieldIndexing.Search);
            }
        }

        public class TestDocument
        {
            public string Name { get; set; }
        }

        [Fact]
        public void MyFirstTest()
        {
            using (var store = GetDocumentStore())
            {
                store.ExecuteIndex(new TestDocumentByName());
                using (var session = store.OpenSession())
                {
                    session.Store(new TestDocument {Name = "Hello world!"});
                    session.Store(new TestDocument {Name = "Goodbye..."});
                    session.SaveChanges();
                }

                WaitForIndexing(store); //If we want to query documents sometime we need to wait for the indexes to catch up
                //WaitForUserToContinueTheTest(store); //Sometimes we want to debug the test itself, this redirect us to the studio
                using (var session = store.OpenSession())
                {
                    var query = session.Query<TestDocument, TestDocumentByName>().Where(x => x.Name == "hello").ToList();
                    Assert.Single(query);
                }
            }
        }
    }
}