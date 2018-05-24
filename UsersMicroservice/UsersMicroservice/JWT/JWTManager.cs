using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UsersMicroservice.Logs;

namespace UsersMicroservice.JWT
{
    public class JWTManager
    {
        public readonly string _key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb3" +
                                      "37591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
        
        public string ReturnJWT(DateTime expirationDate, int permissionId_i, int userId_i)
        {
            // Create Security key  using private key above:
            // not that latest version of JWT using Microsoft namespace instead of System
            SymmetricSecurityKey securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

            // Also note that securityKey length should be >256b
            // so you have to make sure that your private key has a proper length
            SigningCredentials credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // Finally create a Token
            JwtHeader header = new JwtHeader(credentials);

            // Some PayLoad that contain information about the  customer
            JwtPayload payload = new JwtPayload
            {
                { "expirationDate", expirationDate},
                { "permissionId", permissionId_i },
                { "userId", userId_i }
            };
            
            JwtSecurityToken secToken = new JwtSecurityToken(header, payload);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            ErrInfLogger.LockInstance.InfoLog("Token created.");
            return handler.WriteToken(secToken);
        }
    }
}
