using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using AshamedApp.Application.Services;

namespace ContactsApplication.Infrastructure.Services;

public class ContactManagerService(IContactRepository contactRepository) : IContactManagerService
{
    //use repository to get data
    public GetAllContactsResponse GetAllContacts()
    {
        var contacts = contactRepository.GetAllContacts();
        return contacts;
    }
}