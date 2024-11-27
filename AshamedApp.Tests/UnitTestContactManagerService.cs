using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using AshamedApp.Application.Services.Implementations;
using Moq;

namespace AshamedApp.Tests
{
    [TestClass]
    public class UnitTestContactManagerService
    {
        // Retrieve all contacts successfully
        [TestMethod]
        public void RetrieveAllContactsSuccessfully()
        {
            // Arrange
            var contactRepository = new Mock<IContactRepository>();
            var expectedContacts = new List<ContactDto> { new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "johndoe@example.com", PhoneNumber = "0600000000"} };
            contactRepository.Setup(repo => repo.GetAllContacts()).Returns(new GetAllContactsResponse(expectedContacts));
            var service = new ContactManagerService(contactRepository.Object);

            // Act
            var result = service.GetAllContacts().ContactList.ToList();

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expectedContacts, result);
        }

        #nullable disable
        [TestMethod]
        public void HandleNullResponseFromRepository()
        {
            // Arrange
            var contactRepository = new Mock<IContactRepository>();
            contactRepository.Setup(repo => repo.GetAllContacts()).Returns(null as GetAllContactsResponse);
            var service = new ContactManagerService(contactRepository.Object);

            // Act
            var result = service.GetAllContacts();

            // Assert
            Assert.IsNull(result);
        }
    }
}