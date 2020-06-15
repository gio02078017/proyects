namespace GrupoExito.DataAgent.Services.Base
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Constants;
    using GrupoExito.Utilities.Contracts.Base;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class SecurityService : BaseApiService, ISecurityService
    {
        public SecurityService(IDeviceManager deviceManager) 
            : base(deviceManager)
        {
            DeviceManager = deviceManager;
        }

        public new async Task SaveKey()
        {
            CryptographyService cryptographyService = new CryptographyService();
            DiffieHellmanService diffieHellmanService = new DiffieHellmanService();
            string salt = cryptographyService.CreateSalt();
            var saltBytes = cryptographyService.GetBytes(salt);
            string clientPrivateKey = cryptographyService.CreateDerivedKey(AppServiceConfiguration.OuterStrength, saltBytes);
            string clientPublicKey = BitConverter.ToString(diffieHellmanService.GetPublicKey(cryptographyService.GetBytes(clientPrivateKey)));

            string endpoint = AppServiceConfiguration.SaveKeyEndPoint;
            var request = new ServerSecret { PublicKey = clientPublicKey };
            var result = await HttpClientBaseService.PostAsync(endpoint, request);

            if (result.IsSuccessStatusCode)
            {
                string data = result.Content.ReadAsStringAsync().Result;
                ServerSecret secret = JsonService.Deserialize<ServerSecret>(data);

                byte[] byteServerPublicKey = cryptographyService.GetBytes(secret.PublicKey);
                byte[] byteClientPrivateKey = cryptographyService.GetBytes(clientPrivateKey);

                byte[] sharedKey = diffieHellmanService.GetSharedSecret(byteClientPrivateKey, byteServerPublicKey);
                DeviceManager.SaveAccessPreference(ConstPreferenceKeys.SharedKey, BitConverter.ToString(sharedKey));
            }
            else
            {
                throw new Exception(result.ReasonPhrase);
            }
        }
    }
}
