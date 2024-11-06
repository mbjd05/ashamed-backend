using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;

namespace AshamedApp.Application.Services.Implementations;

public class ContactManagerService(IContactRepository contactRepository) : IContactManagerService
{
    //use repository to get data
    public GetAllContactsResponse GetAllContacts()
    {
        var contacts = contactRepository.GetAllContacts();
        return contacts;
    }
}