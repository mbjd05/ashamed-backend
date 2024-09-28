using AshamedApp.Application.DTOs;

namespace AshamedApp.Application.Repositories;

public interface IContactRepository
{
    GetAllContactsResponse GetAllContacts();
    // Task<ContactDto> GetContactByIdAsync(int id);
    // Task AddContactAsync(ContactDto contact);
    // Task UpdateContactAsync(ContactDto contact);
    // Task DeleteContactAsync(int id);
}