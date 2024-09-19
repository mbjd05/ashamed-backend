using ContactsApplication.Application.DTOs;
using ContactsApplication.Application.Repositories;
using ContactsApplication.Application.Services;

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