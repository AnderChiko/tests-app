using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Test.Core.Interfaces.Security;
using Test.Core.Models.Constants;
using Test.Core.Models.Security;
using Test.Core.Extensions;
using Test.Core.Exceptions;

namespace Test.WebApi.PolicyHandlers
{
    public class LoginSessionValidationHandler : AuthorizationHandler<LoginSessionValidationRequirement>
    {
        // TODO: Replace this with LoggingManager
        ILogger<LoginSessionValidationHandler> _logger;
       // ISessionContextProvider _sessionContextProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<TokenProviderOptions> _tokenProviderOptions;
        private readonly ITokenManager _tokenManager;
       // private readonly IConnectionProvider _communicationProvider;

        public LoginSessionValidationHandler(ILoggerFactory loggerFactory,
          //  ISessionContextProvider sessionContextProvider,
            IHttpContextAccessor httpContextAccessor,
            IOptions<TokenProviderOptions> tokenProviderOptions,
            ITokenManager tokenManager
          // IConnectionProvider communicationProvider
          )
        {
            _logger = loggerFactory.CreateLogger<LoginSessionValidationHandler>();
           // _sessionContextProvider = sessionContextProvider;
            _httpContextAccessor = httpContextAccessor;
            _tokenProviderOptions = tokenProviderOptions;
            _tokenManager = tokenManager;
           // _communicationProvider = communicationProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, LoginSessionValidationRequirement requirement)
        {
            Microsoft.Extensions.Primitives.StringValues authorizationHeader;

            // https://stackoverflow.com/questions/58565574/reading-the-authorizationfiltercontext-in-netcore-api-3-1
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                try
                {
                    var hasValue = httpContext.Request.Headers.TryGetValue(HttpHeaders.Authorization, out authorizationHeader);

                    //if we can't get the auth headers fail auth
                    if (!hasValue)
                    {
                        _logger.LogWarning($"Request without Authorization Header. {httpContext.GetContextDescription()}");
                        context.Fail();
                        return;
                    }

                    var authorizationHeaders = authorizationHeader.First().Split(",").Select(h => h.Trim()).ToList();
                    var bearerToken = authorizationHeaders.First(x => x.StartsWith("bearer", StringComparison.OrdinalIgnoreCase));

                    // if we don't have a token fail auth
                    if (bearerToken == null)
                    {
                        _logger.LogWarning($"No Bearer Token. {httpContext.GetContextDescription()}");
                        Fail(context, requirement, httpContext);
                        return;
                    }

                    var token = bearerToken.Substring("bearer ".Length).Trim();

                    if (string.IsNullOrWhiteSpace(token))
                    {
                        _logger.LogWarning($"Bearer Token Bare. {httpContext.GetContextDescription()}");
                        Fail(context, requirement, httpContext);
                        return;
                    }

                    //_sessionContextProvider.SecurityContext.BearerToken = token;

                    // TODO: Validate Bearer Token
                    ValidateToken(token);
                    var jwtTokenMain = new JwtSecurityToken(token);
                    var userName = jwtTokenMain.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                    //_sessionContextProvider.SecurityContext.UserName = userName;
                   // _sessionContextProvider.ConnectionString = _communicationProvider.GetConnectionStringByName(RequestContextNames.IHISPMI);

                    foreach (var authHeader in authorizationHeaders)
                    {
                        if (!authHeader.StartsWith("Bearer "))
                        {
                            var parts = authHeader.Split(" ");
                            if (parts.Length != 2)
                            {
                                _logger.LogWarning($"Authentication header invalid format, not enough parts. Header: {authHeader}");
                            }
                            else
                            {
                                var jwtToken = new JwtSecurityToken(token);
                                // TODO: Find a way to get Refresh token info data in.
                                //_sessionContextProvider.SecurityContext.ExternalTokens.Add(new TokenInfo()
                                //{
                                //    Provider = parts[0],
                                //    Token = parts[1]
                                //});
                            }
                        }
                    }

                    context.Succeed(requirement);
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error in the login session handler");
                    Fail(context, requirement, httpContext);
                    return;
                }
            }

            context.Fail();
        }

        private void ValidateToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = _tokenManager.GetTokenValidationParameters();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                // Check if we can read it first. The idea is not to throw exceptions to handle workflow, but this check doesn't seem to fully fix that limitation...
                if (!tokenHandler.CanReadToken(token))
                {
                    throw new CoreSecurityException(RequestContextNames.Primary,
                        $"Can't read token. Token: {token}", "Can't read token.", token);
                }

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validToken);

                if (!(validToken is JwtSecurityToken) || principal == null)
                {
                    throw new CoreSecurityException(RequestContextNames.Primary,
                        $"Can't validate principal or type of token. Token: {token}", "Can't validate principal or type of token.", token);
                }
            }
            catch (SecurityTokenValidationException ex)
            {
                throw new CoreSecurityException(RequestContextNames.Primary,
                    ex, $"Error validating token. Token: {token}", "Error validating token.", token);
            }
        }

        private void Fail(AuthorizationHandlerContext context, LoginSessionValidationRequirement requirement, HttpContext authorizationFilterContext)
        {
            // TODO: Logging

            // Currently, standard .Net core is to return a 401 if the Authorization Handler fails. 
            // If we need to override this in the future, including sending a structured body,
            // the async code below would be needed. One can comment out hte body piece if desired.
            //httpContext.Response?.OnStarting(async () =>
            //{
            //    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            //    await response.Body.WriteAsync(message, 0, message.Length); only when you want to pass a message
            //});

            context.Fail();
        }
    }
}
