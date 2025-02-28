# New API instruction
Follow these instructions to create a new API in the project.

## General Requirements
- The API should be implemented using the .NET Core framework
- The API should be RESTful
- Use RESTful conventions for API design. For example, use HTTP methods for CRUD operations, and use HTTP status codes to indicate the status of the request
- Use Swagger to document the API
- Validate all user inputs and sanitize data
- Implement logging and monitoring for security events

## Example Code
The following is an example of a simple API that returns a list of items.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc; 

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly List<string> _items = new List<string>
        { 
            "item1", "item2", "item3", "item4", "item5", 
            "item6", "item7", "item8", "item9", "item10",
            "item11", "item12", "item13", "item14", "item15"
        };

        // GET: api/items?page=1&pageSize=5
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Page and pageSize must be greater than 0.");

            var pagedItems = _items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(pagedItems);
        }
    }
}
```

