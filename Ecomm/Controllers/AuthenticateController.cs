using Ecomm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecomm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]//this is same code of Identity Razor no any diff in it
    public class AuthenticateController : ControllerBase // ye sabhi cheezen 2.0 ki identity me Account nam ke folder me aati thi hamare pass, ki usermanager kis prakar 1 user ko create karega
    {
        private readonly UserManager<AppUser> _userManager; //UserManager class for Creating users in identity
        private readonly RoleManager<IdentityRole> _roleManager; //RoleManager for creating roles using IdentityRole
        private readonly IConfiguration _configuration; // Configuration declared here

        public AuthenticateController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)//User Ko create karne ke liye db me exist db ko chk karna badega phele using by UserManager Class, is method ko ham FromBody se access karenge
        {
            var userExist = await _userManager.FindByNameAsync(model.UserName);
            if (userExist != null) //agr user mil jata hai to
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error Message", Message = "Username already Exists" });
            AppUser user = new AppUser() //AppUser class is created here, badme ham usermanage class ke throw appuser create karenge using CreateAsync method
            {
                UserName = model.UserName,//UserName is same as Email here
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString() //Security Stamp me hamne GUID add kar di
            };
            var result = await _userManager.CreateAsync(user, model.Password);//CreateAsync me hamare pass 2 cheezen aati hai user & pass
            if (!result.Succeeded) // after result succeded StatusCode generated here
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                { Status = "Error Message", Message = "User Created Failed" });
            }
            else
            {//Here first User Admin ban jaega and 2nd User UserRole me assign ho jaega
                if (!await _roleManager.RoleExistsAsync("Admin"))//agar Admin Role Exist nahi karta hai to hamara Admin role ban jana chaiye yahan
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin")); //here we only Create a Admin Role in IdentityRole
                    await _userManager.AddToRoleAsync(user, "Admin"); //here we assigned first default user to Admin Role
                }
                else
                {
                    if (!await _roleManager.RoleExistsAsync("User")) //here we check Roles by user name
                    {
                        await _roleManager.CreateAsync(new IdentityRole("User")); //agar User Exist nahi karta hai to user ke naam se 1 user ban jana chaiye
                        await _userManager.AddToRoleAsync(user, "User");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "User"); //here craeted user goes to User Role that's created in if condition
                    }
                }
            }
            return Ok(new Response { Status = "Success", Message = "User Created Successfully" });
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)//User hamara db me exist karta hai ke nahi karta
        {
            var user = await _userManager.FindByNameAsync(model.UserName);//here FindByNameAsync treat as a User Name
            if (user != null && await _userManager.
                CheckPasswordAsync(user, model.Password))//jaise user nikala tha vese hi pass bhi nikal liya UserManager se, CheckPasswordAsync means jo pass hamne dala hai vo sahi hai ke nahi hai
            {
                var userRoles = await _userManager.GetRolesAsync(user);//dono hi userfind agar mil jata hai to ye login successful ki or jaega, login successful ki or jane ke liye hamein ek tocken generate karna hai user ke liye, sabse phele ham user nikalenge kunki ho sakta hai user kisi perticular role me ho, vahi role se related user ham yahan nikal rahe hain, for knowing role of user nikala hua //ho sakta hai user admin & user dono role me ho
                var authCliams = new List<Claim>//here riteriving authCliams, here Claims are added for JwtAuthentication
                    {
                        new Claim(ClaimTypes.Name,user.UserName), //here inserting new Claim in authCliams, here ClaimTypes is Name getting from user.UserName
                        new Claim(JwtRegisteredClaimNames.Jti, //JwtRegisteredClaimNames ke liye nai Guid genrate ho jaegi here
                        Guid.NewGuid().ToString())
                    };
                foreach (var UserRole in userRoles) //here userRoles gotted
                {
                    authCliams.Add(new Claim(ClaimTypes.Role, UserRole)); //userRoles is provided to authCliams here using ClaimTypes like UseName added before
                }

                var authSignInKey = new SymmetricSecurityKey //authSignInKey me new SymmetricSecurityKey use karni hai hamein yahan //Symmetric Key hamari appsetting.json file me hai uske liye hamne Encoding use ki hai
                    (Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                var toekn = new JwtSecurityToken( //heme yahan Token gen karna hai jo hamein yahan milna chaiye
                    issuer: _configuration["JWT:ValidIssuer"],//we can check this _configuration from appsettings.json
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1), //in expires add an hour
                    claims: authCliams,
                    signingCredentials: new SigningCredentials
                    (authSignInKey, SecurityAlgorithms.HmacSha256) //Secure Hash Algorithm is Putted here
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(toekn)
                }); //here Token gen and sented also
            }
            return Unauthorized(); //If User is not Authenticated/Null/NotFound/IncorectPass then user make unauthorized and sent it, otherwise generate a token
        }
    } //Now Succefully Authentication Tokken is Generated, after getting Authentication Tokken when user put values in Tokken, then signin key sent it from here
} //after this we check Users and Roles for checking Login Functionality, here LoggedInUser is Gulshan, 1st run proj then go to for Login
//form-data of Body of postman for putting data in KeyValue Pair and raw for Json Formate
