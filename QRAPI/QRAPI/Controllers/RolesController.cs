using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace QRAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        //[Authorize(Roles = "Admin,Employee")]
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public void CreateRoles()  //Bir action dır.
        {
            IdentityRole identityRole = new IdentityRole("Person");

            _roleManager.CreateAsync(identityRole).Wait();

            



        }
    }
}
