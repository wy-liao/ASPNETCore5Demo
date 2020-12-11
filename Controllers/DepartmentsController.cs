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
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext db;
        public DepartmentsController(ContosoUniversityContext db)
        {
            this.db = db;
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<Department>> GetDepartments()
        {
            //不追蹤 省記憶體 效能較好
            return db.Departments.AsNoTracking().ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Course>> GetDepartmentsCourse(int id)
        {
            return db.Departments.Include(p => p.Courses).First(p => p.DepartmentId == id).Courses.ToList();

            //return db.Courses.Where(p => p.DepartmentId == id).ToList();
        }

        [HttpGet("ddl")]
        public ActionResult<IEnumerable<DepartmentDropDown>> GetDepartmentDropDown()
        {
            return db.DepartmentDropDown.FromSqlInterpolated($"SELECT DepartmentID, Name FROM dbo.Department").ToList();
        }
        
        [HttpPost("")]
        public ActionResult<Department> PostDepartment(Department model)
        {
            // 資料庫有Insert成功，但頁面會有錯誤訊息500Error
            // var insertCount = db.Departments
            //                   .FromSqlRaw("EXECUTE dbo.Department_Insert {0},{1},{2},{3}", model.Name, model.Budget, model.StartDate, model.InstructorId)
            //                   .ToList();
            var Name = model.Name;
            var Budget = model.Budget;
            var StartDate = model.StartDate;
            var InstructorId = model.InstructorId;
            var insertCount = db.Departments
                .FromSqlInterpolated($"EXECUTE dbo.Department_Insert {Name},{Budget},{StartDate},{InstructorId}")
                .ToList();
            return Created("/api/Department/" + model.DepartmentId, model);
        }

        [HttpPost("InMod")]
        public ActionResult<Department> PostDepartmentMod(Department model)
        {
            model.DateModified=DateTime.Now;
            db.Departments.Add(model);
            db.SaveChanges();
            return Created("/api/Department/" + model.DepartmentId, model);
        }
        
        [HttpPut("{id}")]
        public IActionResult PutDepartment(int id, Department model)
        {
            var DepartmentID = id;
            var Name = model.Name;
            var Budget = model.Budget;
            var StartDate = model.StartDate;
            var InstructorId = model.InstructorId;
            //var RowVersion_Original = model.RowVersion;
            var FindRowVersion = db.Departments.Find(id);
            var RowVersion_Original = FindRowVersion.RowVersion;
            var insertCount = db.Departments
                .FromSqlInterpolated($"EXECUTE dbo.Department_Update {DepartmentID}, {Name}, {Budget}, {StartDate}, {InstructorId}, {RowVersion_Original}")
                .ToList();
            
            return NoContent();
        }

        [HttpPut("UpMod/{id}")]
        public IActionResult PutDepartmentMod(int id, Department model)
        {
            var c = db.Departments.Find(id);
            model.DateModified=DateTime.Now;
            c.InjectFrom(model);
            db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Department> DeleteDepartmentById(int id, Department model)
        {
            var DepartmentID = id;
            //var RowVersion_Original = model.RowVersion;
            var FindRowVersion = db.Departments.Find(id);
            var RowVersion_Original = FindRowVersion.RowVersion;
            var insertCount = db.Departments
                .FromSqlInterpolated($"EXECUTE dbo.Department_Delete {DepartmentID}, {RowVersion_Original}")
                .ToList();
            return null;
        }
        
        [HttpDelete("IsDel/{id}")]
        public ActionResult<Department> DeleteDepartmentByIsDel(int id)
        {
            var DateModified=DateTime.Now;
            var IsDeleted=true;
            db.Database.ExecuteSqlRaw($"UPDATE Department SET DateModified={DateModified}, IsDeleted={IsDeleted} WHERE DepartmentID={id}");
            
            return null;
        }
        
        [HttpGet("vwDepartmentCourseCount")]
        public ActionResult<IEnumerable<Department>> GetDepartmentCourseCount()
        {
            var vw = db.Departments.FromSqlRaw($"SELECT * FROM vwDepartmentCourseCount").AsNoTracking().ToList();
            return vw;
        }

    }
}