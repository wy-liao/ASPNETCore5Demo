using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCore5Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCore5Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeAssignmentController : ControllerBase
    {
        private readonly ContosoUniversityContext db;
        public OfficeAssignmentController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<OfficeAssignment>> GetOfficeAssignments()
        {
            return db.OfficeAssignments.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<OfficeAssignment> GetOfficeAssignmentById(int id)
        {
            return  db.OfficeAssignments.Find(id);
        }

        [HttpPost("")]
        public ActionResult<OfficeAssignment> PostOfficeAssignment(OfficeAssignment model)
        {
            db.OfficeAssignments.Add(model);
            db.SaveChanges();
            return Created("/api/OfficeAssignment/" + model.InstructorId, model);
        }

        [HttpPut("{id}")]
        public IActionResult PutOfficeAssignment(int id, OfficeAssignment model)
        {
            var c = db.OfficeAssignments.Find(id);
            c.Instructor=model.Instructor;
            c.Location=model.Location;
            db.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<OfficeAssignment> DeleteOfficeAssignmentById(int id)
        {
            db.Database.ExecuteSqlRaw($"DELETE FROM db.OfficeAssignment WHERE InstructorID={id}");
            return null;
        }
    }
}