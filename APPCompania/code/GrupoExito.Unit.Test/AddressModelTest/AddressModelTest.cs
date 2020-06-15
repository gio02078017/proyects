using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Utilities.Resources;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.AddressModelTest
{
    public class AddressModelTest : BaseAddressModelTest
    {
        [Fact]
        public async Task GetCitiesSuccessful()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            var cities = new List<City>
            {
                new City { Id = "any" }
            };

            CitiesFilter parameters = new CitiesFilter
            {
                Pickup = "true"
            };

            var response = new CitiesResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Cities = cities
            };

            AddressService.Setup(_ => _.GetCities(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetCities(parameters);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response.Cities.Any());
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task GetCitiesFailed()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            CitiesFilter parameters = new CitiesFilter
            {
                Pickup = "true"
            };

            var response = new CitiesResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.GetCities(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetCities(parameters);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            AddressService.VerifyAll();
        }

        [Fact]
        public async Task AddAddressSuccessful()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            UserAddress address = new UserAddress();

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.AddAddress(address)).ReturnsAsync(response);

            // Action
            var actual = await Model.AddAddress(address);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task AddAddressFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            UserAddress address = new UserAddress();

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.AddAddress(address)).ReturnsAsync(response);

            // Action
            var actual = await Model.AddAddress(address);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetAddressSuccessful()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            var addresses = new List<UserAddress>
            {
                new UserAddress { CityId = "any" }
            };

            var response = new AddressResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Addresses = addresses
            };

            AddressService.Setup(_ => _.GetAddress()).ReturnsAsync(response);

            // Action
            var actual = await Model.GetAddress();

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response.Addresses.Any());
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task GetAddressesFailed()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            var response = new AddressResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Addresses = new List<UserAddress>()
            };

            AddressService.Setup(_ => _.GetAddress()).ReturnsAsync(response);

            // Action
            var actual = await Model.GetAddress();

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            AddressService.VerifyAll();
        }

        [Fact]
        public async Task UpdateAddressSuccessful()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            UserAddress address = new UserAddress();

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.UpdateAddress(address)).ReturnsAsync(response);

            // Action
            var actual = await Model.UpdateAddress(address);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task UpdateAddressFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            UserAddress address = new UserAddress();

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.UpdateAddress(address)).ReturnsAsync(response);

            // Action
            var actual = await Model.UpdateAddress(address);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteAddressSuccessful()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            UserAddress address = new UserAddress();

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.DeleteAddress(address)).ReturnsAsync(response);

            // Action
            var actual = await Model.DeleteAddress(address);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task DeleteAddressFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            UserAddress address = new UserAddress();

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.DeleteAddress(address)).ReturnsAsync(response);

            // Action
            var actual = await Model.DeleteAddress(address);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task CoverageAddressSuccessful()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            LocationAddress location = new LocationAddress();

            var response = new CoverageAddressResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.CoverageAddress(location)).ReturnsAsync(response);

            // Action
            var actual = await Model.CoverageAddress(location);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task CoverageAddressFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            LocationAddress location = new LocationAddress();

            var response = new CoverageAddressResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.CoverageAddress(location)).ReturnsAsync(response);

            // Action
            var actual = await Model.CoverageAddress(location);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AutoCompleteAddressSuccessful()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            List<Prediction> address = new List<Prediction>
            {
                new Prediction() { Description = "Any" }
            };
            var response = new AddressPredictionResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Predictions = address
            };

            AddressService.Setup(_ => _.AutoCompleteAddress("Any")).ReturnsAsync(response);

            // Action
            var actual = await Model.AutoCompleteAddress("Any");

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(actual.Predictions.Any());
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task AutoCompleteAddressFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            var response = new AddressPredictionResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.AutoCompleteAddress("Any")).ReturnsAsync(response);

            // Action
            var actual = await Model.AutoCompleteAddress("Any");

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public void ValidateFieldsSuccessfull()
        {
            var validateFieldsResponse = string.Empty;
            var userAddress = new UserAddress()
            {
                AddressComplete = "Any",
                CellPhone = "3015557889",
                AddressKey = "Any",
                AddressName = "Any",
                City = "Any",
                CityId = "Any",
                AditionalInformationAddress = "Any",
                DependencyId = "Any",
                Description = "Any",
                HasCoverage = true,
                IsDefaultAddress = false,
                LastName = "Any",
                Latitude = 0,
                Location = "Any",
                Longitude = 0,
                Name = "Any",
                Neighborhood = "Any",
                StateId = "Any"
            };

            // Action
            var actual = Model.ValidateFields(userAddress);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateFieldsRequiredFieldsFailed()
        {
            var validateFieldsResponse = string.Empty;
            var userAddress = new UserAddress()
            {
                AddressComplete = "Any",
                CellPhone = "Any",
                AddressKey = "Any",
                AddressName = "Any",
                City = "Any",
                AditionalInformationAddress = "Any",
                DependencyId = "Any",
                Description = "Any",
                HasCoverage = true,
                IsDefaultAddress = false,
                LastName = "Any",
                Latitude = 0,
                Location = "Any",
                Longitude = 0,
                Name = "Any",
                Neighborhood = "Any",
                StateId = "Any"
            };

            // Action
            var actual = Model.ValidateFields(userAddress);

            // Assert
            Assert.NotEmpty(actual);
            Assert.Equal(actual, AppMessages.RequiredFieldsText);
        }

        [Fact]
        public void GetCitiesNotReturnSuccessfull()
        {
            var validateFieldsResponse = string.Empty;
            IList<City> cities = new List<City>();

            // Action
            var actual = Model.GetCity("Any", cities);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public void GetCitiesReturnSuccessfull()
        {
            var validateFieldsResponse = string.Empty;
            IList<City> cities = new List<City>
            {
                new City { Id = "Any", Name = "Any" }
            };

            // Action
            var actual = Model.GetCity("Any", cities);

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void GetShortCityIdNotReturnSuccessfull()
        {
            var validateFieldsResponse = string.Empty;
            IList<City> cities = new List<City>();

            // Action
            var actual = Model.GetShortCityId("Any", cities);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void GetShortCityIdReturnSuccessfull()
        {
            var validateFieldsResponse = string.Empty;
            IList<City> cities = new List<City>
            {
                new City { Id = "Any", Name = "Any" }
            };

            // Action
            var actual = Model.GetShortCityId("Any", cities);

            // Assert
            Assert.NotEmpty(actual);
        }

        [Fact]
        public void GetCityNameNotReturnSuccessfull()
        {
            var validateFieldsResponse = string.Empty;
            IList<City> cities = new List<City>();

            // Action
            var actual = Model.GetCityName("Any", cities);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void GetCityNameReturnSuccessfull()
        {
            var validateFieldsResponse = string.Empty;
            IList<City> cities = new List<City>
            {
                new City { Id = "Any", Name = "Any" }
            };

            // Action
            var actual = Model.GetCityName("Any", cities);

            // Assert
            Assert.NotEmpty(actual);
        }

        [Fact]
        public async Task GetStoreSuccessful()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            StoreParameters parameters = new StoreParameters();

            var response = new StoreResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.GetStores(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetStores(parameters);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task GetStoreFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            StoreParameters parameters = new StoreParameters();

            var response = new StoreResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.GetStores(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetStores(parameters);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetStoresSuccessful()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            SearchStoresParameters parameters = new SearchStoresParameters();

            var response = new StoreResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.GetStores(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetStores(parameters);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task GetStoresFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            SearchStoresParameters parameters = new SearchStoresParameters();

            var response = new StoreResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.GetStores(parameters)).ReturnsAsync(response);

            // Action
            var actual = await Model.GetStores(parameters);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task SetDefaultAddressSuccessful()
        {

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            UserAddress address = new UserAddress();

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.SetDefaultAddress(address)).ReturnsAsync(response);

            // Action
            var actual = await Model.SetDefaultAddress(address);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task SetDefaultAddressFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            UserAddress address = new UserAddress();

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            AddressService.Setup(_ => _.SetDefaultAddress(address)).ReturnsAsync(response);

            // Action
            var actual = await Model.SetDefaultAddress(address);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public void ValidateFieldsAtHomeSuccessfull()
        {
            var validateFieldsResponse = string.Empty;
            var userAddress = new UserAddress()
            {
                AddressComplete = "Any",
                CellPhone = "Any",
                AddressKey = "Any",
                AddressName = "Any",
                City = "Any",
                CityId = "Any",
                AditionalInformationAddress = "Any",
                DependencyId = "Any",
                Description = "Any",
                HasCoverage = true,
                IsDefaultAddress = false,
                LastName = "Any",
                Latitude = 0,
                Location = "Any",
                Longitude = 0,
                Name = "Any",
                Neighborhood = "Any",
                StateId = "Any"
            };

            // Action
            var actual = Model.ValidateFieldsAtHome(userAddress);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateFieldsAtHomeFieldsFailed()
        {
            var validateFieldsResponse = string.Empty;
            var userAddress = new UserAddress()
            {
                CellPhone = "Any",
                AddressKey = "Any",
                AddressName = "Any",
                City = "Any",
                AditionalInformationAddress = "Any",
                DependencyId = "Any",
                Description = "Any",
                HasCoverage = true,
                IsDefaultAddress = false,
                LastName = "Any",
                Latitude = 0,
                Location = "Any",
                Longitude = 0,
                Name = "Any",
                Neighborhood = "Any",
                StateId = "Any"
            };

            // Action
            var actual = Model.ValidateFieldsAtHome(userAddress);

            // Assert
            Assert.NotEmpty(actual);
            Assert.Equal(actual, AppMessages.RequiredFieldsText);
        }

        [Fact]
        public void ValidateFieldsInStoreSuccessfull()
        {
            var validateFieldsResponse = string.Empty;
            var store = new Store()
            {
                CityId = "1",
                Id = "1"
            };

            // Action
            var actual = Model.ValidateFieldsInStore(store);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateFieldsInStoreFailed()
        {
            var validateFieldsResponse = string.Empty;
            var store = new Store()
            {
                CityId = string.Empty,
                Id = string.Empty
            };

            // Action
            var actual = Model.ValidateFieldsInStore(store);

            // Assert
            Assert.NotEmpty(actual);
            Assert.Equal(actual, AppMessages.RequiredFieldsText);
        }
    }
}
