using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Contracts;
using UserManagement.Application.DTOs.Auth;
using UserManagement.Application.DTOs.Email;
using UserManagement.Application.Interfaces;
using UserManagement.Infrastructure.Persistence.Context;
using UserManagement.Infrastructure.Persistence.Identity;
using UserManagement.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository _repository;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IEmailService _emailService;
        private readonly JwtService _jwtService;
        private readonly ApplicationDbContext _context;

        private const string StoredProcedure =
            "erpsystem.sp_register_employee";


        public AuthService(
            UserManager<ApplicationUser> userManager,
            IGenericRepository repository,
            IPasswordGenerator passwordGenerator,
            IEmailService emailService,
            JwtService jwtService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _repository = repository;
            _passwordGenerator = passwordGenerator;
            _emailService = emailService;
            _jwtService = jwtService;
            _context = context;
        }
        public async Task<RegisterResponseDto> RegisterAsync(RegisterUserDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException(
                    "Registration data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.EmployeeName))
            {
                throw new ArgumentException(
                    "Employee name is required.");
            }


            if (string.IsNullOrWhiteSpace(dto.EmailAddress))
            {
                throw new ArgumentException(
                    "Email address is required.");
            }


            if (string.IsNullOrWhiteSpace(dto.MobileNumber))
            {
                throw new ArgumentException(
                    "Mobile number is required.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(
                    dto.MobileNumber,@"^[0-9]{10}$"))
            {
                throw new ArgumentException(
                    "Mobile number must be exactly 10 digits.");
            }

            var email = dto.EmailAddress.Trim();
            var mobileNumber = dto.MobileNumber.Trim();

            var existingUser =
                await _userManager.FindByEmailAsync(
                    dto.EmailAddress);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "A user with this email address already exists.");
            }

            var existingMobile = await _userManager.Users.FirstOrDefaultAsync(
            u => u.PhoneNumber == mobileNumber);

            if (existingMobile != null)
            {
                throw new InvalidOperationException(
                    "A user with this mobile number already exists.");
            }

            var employeeCodeResult =
                await _repository.ExecuteQueryAsync<EmployeeCodeResult>(
                    StoredProcedure,

                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "GetNextEmployeeCode"
                    });


            var employeeCode =
                employeeCodeResult
                    .FirstOrDefault()
                    ?.EmployeeCode;


            if (string.IsNullOrWhiteSpace(employeeCode))
            {
                throw new InvalidOperationException(
                    "Employee code could not be generated.");
            }

            var password =
                _passwordGenerator.GeneratePassword();

            var user = new ApplicationUser
            {
                UserName = employeeCode,
                Email = dto.EmailAddress,
                PhoneNumber = dto.MobileNumber,
                EmailConfirmed = false
            };


            var result = await _userManager.CreateAsync(user,password);

            if (!result.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description));

                throw new InvalidOperationException(
                    $"User registration failed. {errors}");
            }

            var resultFromSp =
                await _repository.ExecuteQueryAsync<RegisterEmployeeResult>(
                    StoredProcedure,

                    new StoredProcedureParameter
                    {
                        Name = "@Type",
                        Value = "Insert"
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@employee_name",
                        Value = dto.EmployeeName
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@employee_code",
                        Value = employeeCode
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@email_address",
                        Value = dto.EmailAddress
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@mobile_number",
                        Value = dto.MobileNumber
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@branch_id",
                        Value = dto.BranchId
                    },

                    new StoredProcedureParameter
                    {
                        Name = "@user_id",
                        Value = user.Id
                    }
                );

            var employee =
                resultFromSp.FirstOrDefault();

            if (employee == null)
            {
                await _userManager.DeleteAsync(user);

                throw new InvalidOperationException(
                    "Employee registration failed.");
            }

            var emailRequest = new EmailRequestDto
            {
                ToEmail = employee.EmailAddress,

                Subject =
                    "ERP System - Registration Successful",

                Body = $@"
                    <h2>Registration Successful</h2>

                    <p>
                        Your ERP System account has been
                        created successfully.
                    </p>

                    <p>
                        <strong>Username:</strong>
                        {employee.EmployeeCode}
                    </p>

                    <p>
                        <strong>Password:</strong>
                        {password}
                    </p>

                    <p>
                        Please login using the above credentials
                        and change your password after your
                        first login.
                    </p>

                    <p>
                        Regards,<br/>
                        ERP System
                    </p>"
            };


            await _emailService.SendEmailAsync(
                emailRequest);

            return new RegisterResponseDto
            {
                Message =
                    "User registered successfully.",

                UserId =
                    employee.UserId,

                EmployeeCode =
                    employee.EmployeeCode,

                EmployeeName =
                    employee.EmployeeName,

                EmailAddress =
                    employee.EmailAddress,

                MobileNumber =
                    employee.MobileNumber
            };
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                throw new ArgumentException(
                    "Username is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException(
                    "Password is required.");
            }

            var user =
                await _userManager.FindByNameAsync(
                    dto.Username);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "Invalid username or password.");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException(
                    "User account is deactivated.");
            }

            var passwordResult =
                await _userManager.CheckPasswordAsync(user,dto.Password);

            if (!passwordResult)
            {
                throw new InvalidOperationException(
                    "Invalid username or password.");
            }

            var token = await _jwtService.GenerateTokenAsync(user.Id);

            var refreshToken = await _jwtService.GenerateRefreshTokenAsync();

            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                CreatedDate = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
             await _context.SaveChangesAsync();
            
            return new LoginResponseDto
            {
                Message = "Login successful.",
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RefreshToken))
            {
                throw new ArgumentException(
                    "Refresh token is required.");
            }

            var refreshToken =
                await _context.RefreshTokens
                    .FirstOrDefaultAsync(
                        x => x.Token == dto.RefreshToken);

            if (refreshToken == null)
            {
                throw new InvalidOperationException(
                    "Invalid refresh token.");
            }

            if (refreshToken.IsRevoked)
            {
                throw new InvalidOperationException(
                    "Refresh token has been revoked.");
            }

            if (refreshToken.ExpiryDate <= DateTime.UtcNow)
            {
                throw new InvalidOperationException(
                    "Refresh token has expired.");
            }

            var user =
                await _userManager.FindByIdAsync(
                    refreshToken.UserId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "User not found.");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException(
                    "User account is deactivated.");
            }

            refreshToken.IsRevoked = true;

            var newAccessToken =
                await _jwtService.GenerateTokenAsync(
                    user.Id);

            var newRefreshToken =
                await _jwtService.GenerateRefreshTokenAsync();

            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(
                newRefreshTokenEntity);

            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                Message = "Token refreshed successfully.",
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentException(
                    "Refresh token is required.");
            }

            var token =
                await _context.RefreshTokens
                    .FirstOrDefaultAsync(
                        x => x.Token == refreshToken);

            if (token == null)
            {
                throw new InvalidOperationException(
                    "Invalid refresh token.");
            }

            if (token.IsRevoked)
            {
                throw new InvalidOperationException(
                    "Refresh token has already been revoked.");
            }

            token.IsRevoked = true;

            await _context.SaveChangesAsync();
        }
    }
}