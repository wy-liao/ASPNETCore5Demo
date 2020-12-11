using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ASPNETCore5Demo.Models
{
    public partial class CourseUpdateModel
    {
            public string Title { get; set; }
            public int Credits { get; set; }
            public DateTime? DateModified { get; set; }
    }
}