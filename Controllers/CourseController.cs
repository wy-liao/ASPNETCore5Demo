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
    public class CourseController : ControllerBase
    {
        private readonly ContosoUniversityContext db;
        public CourseController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<Course>> GetCourses()
        {
            return db.Courses.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Course> GetCourseById(int id) //Query
        {
            return db.Courses.Find(id);
        }

        [HttpPost("InMod")]
        public ActionResult<Course> PostCourse(Course model) //Insert
        {
            model.DateModified=DateTime.Now;
            db.Courses.Add(model);
            db.SaveChanges();
            return Created("/api/Course/" + model.CourseId, model);
        }

        [HttpPut("UpMod/{id}")]
        public IActionResult PutCourse(int id, CourseUpdateModel model) //Update
        {
            var c = db.Courses.Find(id);
            // c.Credits=model.Credits;
            // c.Title=model.Title;
            model.DateModified=DateTime.Now;
            c.InjectFrom(model);
            db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Course> DeleteCourseById(int id) //Delete
        {
            var c = db.Courses.Find(id);
            db.Remove(c);
            db.SaveChanges();

            db.Database.ExecuteSqlRaw($"DELETE FROM db.Course WHERE CourseID={id}");

            return Ok(c);
        }

        [HttpDelete("IsDel/{id}")]
        public ActionResult<Course> DeleteCourseByIsDel(int id)
        {
            var DateModified=DateTime.Now;
            var IsDeleted=true;
            db.Database.ExecuteSqlRaw($"UPDATE Course SET DateModified={DateModified}, IsDeleted={IsDeleted} WHERE CourseID={id}");
            
            return null;
        }
        

        [HttpGet("credits/{credit}")]
        public ActionResult<IEnumerable<Course>> GetCourseByCredit(int credit) //Query By credit field
        {
            return db.Courses.Where(p => p.Credits == credit).ToList();
        }

        [HttpDelete("all")]
        public ActionResult<Course> DeleteCourseAll()
        {
            db.Database.ExecuteSqlRaw($"DELETE FROM db.Course");
            return null;
        }
        
        [HttpGet("vwCourseStudents")]
        public ActionResult<IEnumerable<Course>> GetCourseStudents()
        {
            
            return db.Courses
                  .FromSqlInterpolated($"SELECT * FROM db.vwCourseStudents")
                  .ToList();
        }

        [HttpGet("vwCourseStudentCount")]
        public ActionResult<IEnumerable<Course>> GetCourseStudentCount()
        {
            return db.Courses
                  .FromSqlInterpolated($"SELECT * FROM db.vwCourseStudentCount")
                  .ToList();
        }
        
        
        
    }
}