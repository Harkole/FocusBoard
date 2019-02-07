using FocusBoardCore.Interfaces;
using FocusBoardCore.Models;
using FocusBoardCore.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace FocusBoardCore.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository repository;
        private readonly JwtIssuerOptions jwtOptions;

        /// <summary>
        /// Handles validation of the Actor and generation of the Token
        /// </summary>
        /// <param name="tokenRepository"></param>
        public TokenService(ITokenRepository tokenRepository, IOptions<JwtIssuerOptions> jwtIssuerOptions)
        {
            repository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
            jwtOptions = jwtIssuerOptions.Value ?? throw new ArgumentNullException(nameof(jwtIssuerOptions));
        }

        /// <summary>
        /// Takes the Actor object, validates the properties and then retrieves
        /// the Authentication values from the repository, turning them in to a
        /// JWT token
        /// </summary>
        /// <param name="actor">The Actor object to validate</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An <code>object</code> containing the token and expires_in values</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ActorToken> GetClaimsIdentityAsync(ActorLogin actor, CancellationToken cancellationToken = default(CancellationToken))
        {
            ActorToken token = null;

            // Validate the Model, the results will be collected in the ICollection, but if everything is OK then isValid = true
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(actor, new ValidationContext(actor), validationResults, true);

            if (isValid)
            {
                Authentication auth = await repository.GetClaimsValuesAsync(actor, cancellationToken);

                try
                {
                    if (null == auth)
                    {
                        // Get the generated token and encode it, then set as the return value
                        string encodedJwt = GenerateEncodedToken(auth);
                        token = new ActorToken { Token = encodedJwt, ExpiresIn = (int)jwtOptions.ValidFor.TotalSeconds };
                    }
                }
                catch (ArgumentException argEx) // Catches ArgumentNullException too
                {
                    // Reset the token response to null, any error is considered a security failure
                    token = null;

                    Trace.WriteLine(argEx.Message);
#if DEBUG
                    Trace.WriteLine(argEx.StackTrace);
#endif
                }
                catch (Microsoft.IdentityModel.Tokens.SecurityTokenCompressionFailedException tokenFailedEx)
                {
                    // The token may have been valid, but something went wrong so ensure the security fails
                    token = null;

                    Trace.WriteLine(tokenFailedEx.Message);
#if DEBUG
                    Trace.WriteLine(tokenFailedEx.StackTrace);
#endif
                }
            }
            else
            {
                // As each validation error will aggregated, loop over the results
                foreach(ValidationResult vr in validationResults)
                {
                    // Throw an exception for the first error, ignoring any others that are present
                    throw new ArgumentNullException(((string[])vr.MemberNames)[0], vr.ErrorMessage);
                }
            }

            // Return the Token object
            return token;
        }

        /// <summary>
        /// Provides a new token to replace an old or expiring one. Passing
        /// Claims to this method will generate a new Token so only call
        /// form Authenticated Controllers/Endpoints
        /// </summary>
        /// <param name="claims">A list of claims required to populate the new Token</param>
        /// <returns></returns>
        /// <remarks>Only call from authenticated end points</remarks>
        public ActorToken RenewClaimsIdentity(ClaimsPrincipal claims)
        {
            ActorToken token;

            try
            {
                Authentication auth = new Authentication
                {
                    Email = claims.FindFirst(ClaimTypes.Email)?.Value,
                    PrimaryId = claims.FindFirst(ClaimTypes.PrimarySid)?.Value,
                    PrimaryGroupId = claims.FindFirst(ClaimTypes.PrimaryGroupSid)?.Value,
                    RoleId = claims.FindFirst(ClaimTypes.Role)?.Value,
                    Alias = claims.FindFirst(ClaimTypes.Actor)?.Value,
                    Hidden = bool.Parse(claims.FindFirst(ClaimTypes.Anonymous)?.Value),
                };

                // Generate and assign the new token values
                string encodedToken = GenerateEncodedToken(auth);
                token = new ActorToken { Token = encodedToken, ExpiresIn = (int)jwtOptions.ValidFor.TotalSeconds };
            }
            catch (ArgumentException argEx) // Catches ArgumentNullException
            {
                token = null;

                Trace.WriteLine($"{argEx.ParamName} threw an ArgumentException: {argEx.Message}");
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenCompressionFailedException tokenFailedEx)
            {
                token = null;

                Trace.WriteLine(tokenFailedEx.Message);
            }
            catch (FormatException formatEx)
            {
                token = null;

                Trace.WriteLine(formatEx.Message);
            }

            return token;
        }

        /// <summary>
        /// Takes the Authentication object and returns the JWT encoded string
        /// to use in the authentication Token, All Claims are built here.
        /// </summary>
        /// <param name="auth">The Authentication object to use for the Claims</param>
        /// <returns>Encoded JWT string</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Microsoft.IdentityModel.Tokens.SecurityTokenEncryptionFailedException"></exception>
        private string GenerateEncodedToken(Authentication auth)
        {
            string response = string.Empty;

            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(auth.Email, "Token"), new[]
            {
                // User claim details
                new Claim(ClaimTypes.Email, auth.Email),
                new Claim(ClaimTypes.PrimarySid, auth.PrimaryId),
                new Claim(ClaimTypes.PrimaryGroupSid, auth.PrimaryGroupId),
                new Claim(ClaimTypes.Role, auth.RoleId),
                new Claim(ClaimTypes.Actor, auth.Alias),
                new Claim(ClaimTypes.Anonymous, auth.Hidden.ToString()),

                // System claims
                new Claim(JwtRegisteredClaimNames.Jti, jwtOptions.JtiGenerator().Result),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)jwtOptions.IssuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            // If the Identity isn't null we can build the token
            if (null != identity)
            {
                JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: jwtOptions.Issuer,
                    audience: jwtOptions.Audience,
                    claims: identity.Claims,
                    notBefore: jwtOptions.NotBefore,
                    expires: jwtOptions.Expiration,
                    signingCredentials: jwtOptions.SigningCredentials
                );

                // Encode the token values
                response = new JwtSecurityTokenHandler().WriteToken(jwt);
            }

            return response;
        }
    }
}
