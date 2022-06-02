using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Common.Infrastructure;
using CimcikSozluk.Common.Infrastructure.Exceptions;
using CimcikSozluk.Common.Models.Queries;
using CimcikSozluk.Common.Models.RequestModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CimcikSozluk.Api.Application.Features.Commands.User;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var dbUser = await _userRepository.GetSingleAsync(i => i.EmailAddress == request.EmailAddress);

        if (dbUser is null) throw new DatabaseValidationException("User not found");

        var pass = PasswordEncryptor.Encrpt(request.Password);

        if (dbUser.Password != pass) throw new DatabaseValidationException("Password or email is wrong");

        if (!dbUser.EmailConfirmed) throw new DatabaseValidationException("Email address is not confirmed yet");

        var result = _mapper.Map<LoginUserViewModel>(dbUser);

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
            new Claim(ClaimTypes.Email, dbUser.EmailAddress),
            new Claim(ClaimTypes.Name, dbUser.UserName),
            new Claim(ClaimTypes.GivenName, dbUser.FirstName),
            new Claim(ClaimTypes.Surname, dbUser.LastName)
        };

        result.Token = GenerateToken(claims);

        return result;
    }

    private string GenerateToken(Claim[] claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthConfig:SecurityKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(10);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiry,
            signingCredentials: creds,
            notBefore: DateTime.Now);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}