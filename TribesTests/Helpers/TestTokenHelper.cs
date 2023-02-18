using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace TribesTests.Helpers
{
    public class TestTokenHelper
    {
        private readonly IConfiguration _config;

        public TestTokenHelper()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string restOfAddress = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) restOfAddress += @"/appsettings.json";
            else restOfAddress += @"\appsettings.json";

            _config = new ConfigurationBuilder()
                .AddJsonFile($"{currentDirectory}{restOfAddress}")
                .Build();
        }

        public string GenerateTestToken(int playerId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var expiration = DateTime.Now.AddMinutes(15);

            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.NameId, playerId.ToString())
            };

            var jwt = new JwtSecurityToken(issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signature);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public bool CheckTokenValidity(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            if (jwt == null) return false;

            string iss = jwt.Issuer;
            string aud = jwt.Audiences.FirstOrDefault();

            // JWT expiration is given in seconds. These must be added to "Epoch start" in order to get the actual expiration date and time. Epoch start is set to 1/1/1970 00:00:00. Each day is treated as having exactly 86,400 seconds.
            double addDaysToEpochStart = Convert.ToDouble
                (Int32.Parse(jwt.Claims.FirstOrDefault(c => c.Type == "exp")
                .ToString().Substring(5)))
                /86400;
            DateTime epochStart = new DateTime(1970, 1, 1);
            DateTime exp = epochStart.AddDays(addDaysToEpochStart);

            if (iss != "https://localhost:7038" ||
                aud != "https://localhost:7038" ||
                exp <= DateTime.Now) 
                return false;

            return true;
        }
    }
}