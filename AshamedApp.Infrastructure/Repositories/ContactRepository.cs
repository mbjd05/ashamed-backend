using AshamedApp.Application.DTOs;
using AshamedApp.Application.Repositories;
using AshamedApp.Domain.Models;

namespace ContactsApplication.Infrastructure.Repositories;

public class ContactRepository() : IContactRepository
    {
        // public async Task<IEnumerable<ContactDto>> GetAllContactsAsync()
        // {
        //     return await context.Contacts
        //         .Select(c => new ContactDto
        //         {
        //             Id = c.Id,
        //             FirstName = c.FirstName,
        //             LastName = c.LastName,
        //             Email = c.Email,
        //             PhoneNumber = c.PhoneNumber
        //         })
        //         .ToListAsync();
        // }
        //
        // public async Task<ContactDto> GetContactByIdAsync(int id)
        // {
        //     var contact = await context.Contacts.FindAsync(id);
        //     if (contact == null) return null;
        //
        //     return new ContactDto
        //     {
        //         Id = contact.Id,
        //         FirstName = contact.FirstName,
        //         LastName = contact.LastName,
        //         Email = contact.Email,
        //         PhoneNumber = contact.PhoneNumber
        //     };
        // }
        //
        // public async Task AddContactAsync(ContactDto contactDto)
        // {
        //     var contact = new Contact
        //     {
        //         FirstName = contactDto.FirstName,
        //         LastName = contactDto.LastName,
        //         Email = contactDto.Email,
        //         PhoneNumber = contactDto.PhoneNumber
        //     };
        //
        //     context.Contacts.Add(contact);
        //     await context.SaveChangesAsync();
        // }
        //
        // public async Task UpdateContactAsync(ContactDto contactDto)
        // {
        //     var contact = await context.Contacts.FindAsync(contactDto.Id);
        //     if (contact == null) return;
        //
        //     contact.FirstName = contactDto.FirstName;
        //     contact.LastName = contactDto.LastName;
        //     contact.Email = contactDto.Email;
        //     contact.PhoneNumber = contactDto.PhoneNumber;
        //
        //     context.Contacts.Update(contact);
        //     await context.SaveChangesAsync();
        // }
        //
        // public async Task DeleteContactAsync(int id)
        // {
        //     var contact = await context.Contacts.FindAsync(id);
        //     if (contact != null)
        //     {
        //         context.Contacts.Remove(contact);
        //         await context.SaveChangesAsync();
        //     }
        // }

        public GetAllContactsResponse GetAllContacts()
        {
            //dummy library of contacts
            var contacts = new List<ContactDto>
            {
                new ContactDto
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@example.com",
                    PhoneNumber = "1234567890"
                },
                new ContactDto
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "janedoe@example.com",
                    PhoneNumber = "0987654321"
                }
            };
            //returning the list of contacts
            return new GetAllContactsResponse(contacts);

        }
    }