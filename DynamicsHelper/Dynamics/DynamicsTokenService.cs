using System.Threading;
using System.Threading.Tasks;
using DynamicsHelper.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace DynamicsHelper.Dynamics
{
    public class DynamicsTokenService : IDynamicsTokenService
    {
        private readonly DynamicsSettings _dynamicsSettings;
        // TODO: Find out what is difference btw Mutex, Semaphore, SemaphoreSLim, monitor, lock
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        public DynamicsTokenService(IOptions<DynamicsSettings> dynamicsSettings)
        {
            _dynamicsSettings = dynamicsSettings.Value;
        }

        // TODO: Cancellation token 
        public virtual async Task<string> CreateCreateAuthorizationHeader()
        {
            await SemaphoreSlim.WaitAsync();
            // Token is cached by AuthenticationContext
            var clientCredentials = new ClientCredential(_dynamicsSettings.ClientId, _dynamicsSettings.ClientSecret);
            AuthenticationContext authenticationContext = new AuthenticationContext(_dynamicsSettings.Authority);
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(_dynamicsSettings.Resource, clientCredentials);
            SemaphoreSlim.Release();
            return authenticationResult.CreateAuthorizationHeader();
        }
    }
}
