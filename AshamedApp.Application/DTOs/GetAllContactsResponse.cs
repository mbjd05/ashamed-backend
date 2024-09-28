namespace AshamedApp.Application.DTOs;

public class GetAllContactsResponse(IEnumerable<ContactDto> contacts)
{
    public IEnumerable<ContactDto> ContactList { get; private set; } = contacts;
}