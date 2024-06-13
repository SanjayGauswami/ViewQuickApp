using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using ViewQuickApp.Server.Core.DbContext;
using ViewQuickApp.Server.Core.Dtos;
using ViewQuickApp.Server.Core.Dtos.Auth;
using ViewQuickApp.Server.Core.Entities;
using ViewQuickApp.Server.Core.OtherObject;
using ViewQuickApp.Server.Interface;
using ViewQuickApp.Server.MapperProfile;


namespace ViewQuickApp.Server.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly ViewQuickAppDbContext _viewQuickAppDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogServices _logServices;
        private readonly IMapper mapper;


        public AuthServices(ViewQuickAppDbContext viewQuickAppDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, ILogServices logServices)
        {
            _viewQuickAppDbContext = viewQuickAppDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            this.mapper = mapper;
            _logServices = logServices;
        }


        public async Task<GenralResponseDto> SeedRoleAsync()
        {
            bool isOwnerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.OWNER);
            bool isAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.ADMIN);
            bool isManagerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.MANGER);
            bool isUserRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.USER);


            if (isOwnerRoleExist && isAdminRoleExist && isUserRoleExist && isManagerRoleExist)
            {
                return new GenralResponseDto()
                {
                    statusCode = 400,
                    isSucceed = false,
                    message = "Already Seed Role.."
                };
            }

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.OWNER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.MANGER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRole.USER));

            return new GenralResponseDto()
            {
                statusCode = 200,
                isSucceed = true,
                message = "Role seed successfully"
            };
        }

        public async Task<GenralResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExisttUser = await _userManager.FindByNameAsync(registerDto.UserName);

            if (isExisttUser != null)
            {
                return new GenralResponseDto()
                {
                    isSucceed = false,
                    statusCode = 409,
                    message = "User is Already Exist.."
                };
            }

            var myUser = mapper.Map<ApplicationUser>(registerDto);

            var createUser = await _userManager.CreateAsync(myUser, registerDto.Password);

            if (!createUser.Succeeded)
            {
                var errorMessage = "User creadation is failed because";
                foreach (var error in createUser.Errors)
                {
                    errorMessage += " # " + error.Description;

                    return new GenralResponseDto()
                    {
                        isSucceed = false,
                        statusCode = 400,
                        message = errorMessage
                    };
                }

            }

            // add default role

            await _userManager.AddToRoleAsync(myUser, StaticUserRole.USER);
            await _logServices.SaveNewLog(myUser.UserName, "New Register..");

            return new GenralResponseDto()
            {
                isSucceed = true,
                statusCode = 200,
                message = "User is Register successfully.." 

            };
        }


        public async Task<LoginServicesResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
            {
                return null;
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordCorrect)
            {
                return null;
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var newToken = await GenerateJwtTokenAsync(user);
            var userinfo = GenerateUserInfoObject(user, userRoles);
            await _logServices.SaveNewLog(user.UserName, "New Login..");

            return new LoginServicesResponseDto()
            {
                NewToken = newToken,
                Userinfo = userinfo
            };
        }



        public async Task<GenralResponseDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateDto updateDto)
        {
            var user = await _userManager.FindByNameAsync(updateDto.UserName);

            if (user == null)
            {
                return new GenralResponseDto()
                {
                    isSucceed = false,
                    statusCode = 404,
                    message = "UserName is Invalid.."
                };
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            // ADMIN and  OWNER  change role
            if (User.IsInRole(StaticUserRole.ADMIN))
            {

                // if user is Admin
                if (updateDto.NewRole == RoleType.USER || updateDto.NewRole == RoleType.MANAGER)
                {
                    // Admin can change role all except owner

                    if (userRoles.Any(e => e.Equals(StaticUserRole.OWNER) || e.Equals(StaticUserRole.ADMIN)))
                    {
                        return new GenralResponseDto()
                        {
                            isSucceed = false,
                            statusCode = 403,
                            message = "You are not allowed to change this role"
                        };
                    }
                    else
                    {
                        await _userManager.RemoveFromRolesAsync(user, userRoles);
                        await _userManager.AddToRoleAsync(user, updateDto.NewRole.ToString());
                        await _logServices.SaveNewLog(user.UserName, "UserRole Updated..");
                        return new GenralResponseDto()
                        {
                            isSucceed = true,
                            statusCode = 200,
                            message = "Role is Updated successfully.."
                        };
                    }

                }
                else
                {
                    return new GenralResponseDto()
                    {
                        isSucceed = false,
                        statusCode = 403,
                        message = "You are not allowed Update this User Role"
                    };

               }


            }
            else
            {
                // user owner

                if(userRoles.Any(e => e.Equals(StaticUserRole.OWNER)))
                {
                    return new GenralResponseDto()
                    {
                        isSucceed = false,
                        statusCode = 403,
                        message = "You are not allowed Update this User Role"
                    };
                }
                else
                {
                    await _userManager.RemoveFromRolesAsync(user, userRoles);
                    await _userManager.AddToRoleAsync(user,updateDto.NewRole.ToString());
                    await _logServices.SaveNewLog(user.UserName, "Role Updated..");

                    return new GenralResponseDto()
                    {
                        isSucceed = true,
                        statusCode = 200,
                        message = "Role is Updated successfully.."
                    };
                }   
            }  
        }

        public async  Task<LoginServicesResponseDto?> MeAsync(MeDto meDto)
        {
            ClaimsPrincipal handler = new JwtSecurityTokenHandler().ValidateToken(meDto.Token, new TokenValidationParameters()
            {

                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer =_configuration["jwt:ValidIssuer"],
                ValidAudience = _configuration["jwt:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]))

            }, out SecurityToken securityToken);


            string decodeusername = handler.Claims.First(e=>e.Type == ClaimTypes.Name).Value;
            if (decodeusername == null)
                return null;

            var user = await _userManager.FindByNameAsync(decodeusername);

            if (user == null)
                return new LoginServicesResponseDto()
                {
                    NewToken = "sanjay",
                    

                };


            var userRoles = await _userManager.GetRolesAsync(user);
            var newToken = await GenerateJwtTokenAsync(user);
            var userinfo = GenerateUserInfoObject(user, userRoles);
            await _logServices.SaveNewLog(user.UserName, "New Token..");

            return new LoginServicesResponseDto()
            {
                NewToken = newToken,
                Userinfo = userinfo
            };

        }

        public async Task<IEnumerable<UserInfoResult>> GetUserListAsync()
        {
            var data = _userManager.Users.ToList();

            List<UserInfoResult> userInfoResults = new List<UserInfoResult>();

            foreach(var user in data)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userinfo = GenerateUserInfoObject(user,roles);
                userInfoResults.Add(userinfo);

            }

            return userInfoResults;
        }

        public async Task<UserInfoResult> GetUserDetailsByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = GenerateUserInfoObject(user, roles);
            return userInfo;

        }

        public async Task<IEnumerable<string>> GetUsernameListAsync()
        {
            var userNames = await _userManager.Users.Select(e=>e.UserName).ToListAsync();

            return userNames;
        }



        // Token Generate..
        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim("JWTID", Guid.NewGuid().ToString())
                };

            foreach (var UserRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, UserRole));
            }


            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));

            var TokenObject = new JwtSecurityToken(

                issuer: _configuration["jwt:Validissuer"],
                audience: _configuration["jwt:Validaudience"],
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: new SigningCredentials(authkey, SecurityAlgorithms.HmacSha256)

                );

            var Token = new JwtSecurityTokenHandler().WriteToken(TokenObject);

            return Token;
        }


        // UserInfoObject..
        private UserInfoResult GenerateUserInfoObject(ApplicationUser user, IEnumerable<string> Roles)
        {
            return new UserInfoResult()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                CreateAt = user.CreatedAt,
                Roles = Roles

            };


        }

    }

}
