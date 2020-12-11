using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCore5Demo.Models;
using Omu.ValueInjecter;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCore5Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ContosoUniversityContext db;
        public PersonController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<Person>> GetPersons()
        {
            return db.People.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Person> GetPersonById(int id)
        {
            return db.People.Find(id);
        }

        [HttpPost("")]
        public ActionResult<Person> PostPerson(Person model)
        {
            db.People.Add(model);
            db.SaveChanges();
            return Created("/api/Person/" + model.Id, model);
        }

        [HttpPost("InMod")]
        public ActionResult<Person> PostPersonMod(Person model)
        {
            model.DateModified=DateTime.Now;
            db.People.Add(model);
            db.SaveChanges();
            return Created("/api/Person/" + model.Id, model);
        }

        [HttpPut("{id}")]
        public IActionResult PutPerson(int id, Person model)
        {
            var c = db.People.Find(id);
            c.InjectFrom(model);
            db.SaveChanges();
            return NoContent();
        }

        [HttpPut("UpMod/{id}")]
        public IActionResult PutPersonMod(int id, Person model)
        {
            var c = db.People.Find(id);
            model.DateModified=DateTime.Now;
            c.InjectFrom(model);
            db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Person> DeletePersonById(int id)
        {
            db.Database.ExecuteSqlRaw($"DELETE FROM db.OfficeAssignment WHERE InstructorID={id}");
            return null;
        }

        
        [HttpDelete("IsDel/{id}")]
        public ActionResult<Person> DeletePersonByIsDel(int id)
        {
            var DateModified=DateTime.Now;
            var IsDeleted=true;
            db.Database.ExecuteSqlRaw($"UPDATE Person SET DateModified={DateModified}, IsDeleted={IsDeleted} WHERE ID={id}");
            
            return null;
        }
    }
}