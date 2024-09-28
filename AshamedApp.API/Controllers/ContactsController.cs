using AshamedApp.Application.DTOs;
using AshamedApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AshamedApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController(IContactManagerService contactService)
{
    [HttpGet]
    public GetAllContactsResponse GetAllContacts()
    {
        return contactService.GetAllContacts();
    }
}