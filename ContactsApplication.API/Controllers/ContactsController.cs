using System.Dynamic;
using ContactsApplication.Application.DTOs;
using ContactsApplication.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApplication.API.Controllers;

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