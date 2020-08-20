using EasySoccer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.BLL.Infra.DTO
{
    public class PersonUserResponse
    {
        public Guid PersonId { get; set; }
        public Guid? UserId { get; set; } = null;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public PersonUserResponse(Person person, User user)
        {
            if (person != null)
            {
                PersonId = person.Id;
                Name = person.Name;
                Email = person.Email;
                Phone = person.Phone;
            }
            if (user != null)
            {
                UserId = user.Id;
            }
        }
        public PersonUserResponse()
        {

        }
        public PersonUserResponse(Person person)
        {
            if (person != null)
            {
                PersonId = person.Id;
                Name = person.Name;
                Email = person.Email;
                Phone = person.Phone;
            }
        }
    }
}
