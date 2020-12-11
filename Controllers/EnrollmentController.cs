using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCore5Demo.Models;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;

namespace ASPNETCore5Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly ContosoUniversityContext db;
        public EnrollmentController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<Enrollment>> GetEnrollments()
        {
            return db.Enrollments.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Enrollment> GetEnrollmentById(int id)
        {
            return db.Enrollments.Find(id);
        }

        [HttpPost("")]
        public ActionResult<Enrollment> PostEnrollment(Enrollment model)
        {
            db.Enrollments.Add(model);
            db.SaveChanges();
            return Created("/api/Enrollments/" + model.EnrollmentId, model);
        }

        [HttpPut("{id}")]
        public IActionResult PutEnrollment(int id, EnrollmentUpdateModel model)
        {
            var c = db.Enrollments.Find(id);
            c.InjectFrom(model);
            db.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Enrollment> DeleteEnrollmentById(int id)
        {
            db.Database.ExecuteSqlRaw($"DELETE FROM db.Enrollment WHERE EnrollmentID={id}");
            return null;
        }
    }
}