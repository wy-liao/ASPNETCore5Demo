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
    public class CourseInstructorController : ControllerBase
    {
        private readonly ContosoUniversityContext db;
        public CourseInstructorController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<CourseInstructor>> GetCourseInstructors()
        {
            return db.CourseInstructors.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<CourseInstructor> GetCourseInstructorById(int id)
        {
            return db.CourseInstructors.Find(id);
        }

        [HttpPost("")]
        public ActionResult<CourseInstructor> PostCourseInstructor(CourseInstructor model)
        {
            db.CourseInstructors.Add(model);
            db.SaveChanges();
            return Created("/api/CourseInstructor/" + model.InstructorId, model);
        }

        [HttpPut("{id}")]
        public IActionResult PutCourseInstructor(int id, CourseInstructor model)
        {
            var c = db.CourseInstructors.Find(id);
            c.Instructor=model.Instructor;
            db.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<CourseInstructor> DeleteCourseInstructorById(int id)
        {
            db.Database.ExecuteSqlRaw($"DELETE FROM db.CourseInstructor WHERE InstructorID={id}");
            return null;
        }
    }
}