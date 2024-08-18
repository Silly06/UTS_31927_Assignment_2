using Moq;
using NUnit.Framework;
using PetPlayApp.Server.Db;
using PetPlayApp.Server.Services;
using PetPlayApp.Server.Services.Abstractions;

#nullable enable

namespace PetPlayApp.Test.Services
{
    [TestFixture]
    public class RepositoryProviderServiceTests
    {
        private Mock<DatabaseContext> _databaseContextMock;
        private RepositoryProviderService _repositoryProviderService;

        [SetUp]
        public void Setup()
        {
            _databaseContextMock = new Mock<DatabaseContext>();
            _repositoryProviderService = new RepositoryProviderService(_databaseContextMock.Object);
        }

        [Test]
        public void GetRepository_ValidType_ReturnsRepository()
        {
            var repository = _repositoryProviderService.GetRepository<SampleEntity>();

            Assert.That(repository, Is.Not.Null);
            Assert.That(repository, Is.InstanceOf<IRepository<SampleEntity>>());
        }

        private class SampleEntity
        {
        }
    }
}
