using Complex.LinqProvider.Entities;
using Complex.LinqProvider.Provider;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LinqProvider.Tests
{
    public class ProviderTests
    {
        private readonly EntitySet<Employee> _employees;
        private readonly EntitySet<Office> _offices;
        private readonly EntitySet<Country> _countries;
        
        public ProviderTests()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = configuration.GetConnectionString("linqprovider");

            _employees = new EntitySet<Employee>(connectionString);
            _offices = new EntitySet<Office>(connectionString);
            _countries = new EntitySet<Country>(connectionString);
        }

        [Fact]
        public void GetCountriesList()
        {
            //Arrange
            var countries = _countries.ToList();

            //Act

            //Assert
            Assert.NotNull(countries);
            Assert.Equal(5, countries.Count);
        }

        [Fact]
        public void GetOfficesByCountryId()
        {
            //Arrange
            var expectedOffices = new int[]{1, 2};
            var actualOffices = _offices.Where(o => o.CountryId == 1).ToList();

            //Act

            //Assert
            Assert.NotNull(actualOffices);
            Assert.Equal(expectedOffices,  actualOffices.Select(o => o.Id));
        }

        [Fact]
        public void GetOfficesByName()
        {
            //Arrange
            var expectedOffices = new[]
            {
                new Office
                {
                    Id = 4, 
                    Name = "EPAM Japan", 
                    OfficeChief = "Hayao Miyazaki",
                    CountryId = 3
                },
                new Office
                {
                    Id = 5,
                    Name = "EPAM Japan",
                    OfficeChief = "Hideo Kodzima",
                    CountryId = 3
                }
            };
            var actualOffices = _offices.Where(o => o.Name == "EPAM Japan").ToList();

            //Act

            //Assert
            Assert.NotNull(actualOffices);
            Assert.Equal(expectedOffices, actualOffices);
        }

        [Fact]
        public void GetCountriesExceptOne()
        {
            //Arrange
            var expectedCountries = new int[] { 1, 2, 3, 4 };
            var actualCountries = _countries.Where(c => c.Id != 5).ToList();

            //Act

            //Assert
            Assert.NotNull(actualCountries);
            Assert.Equal(expectedCountries, actualCountries.Select(c => c.Id));
        }

        [Fact]
        public void GetCountriesWithIdGreaterThanTwo()
        {
            //Arrange
            var expectedCountries = new int[] { 3, 4, 5 };
            var actualCountries = _countries.Where(c => c.Id > 2).ToList();

            //Act

            //Assert
            Assert.NotNull(actualCountries);
            Assert.Equal(expectedCountries, actualCountries.Select(c => c.Id));
        }

        [Fact]
        public void GetCountriesWithIdGreaterOrEqualThanTwo()
        {
            //Arrange
            var expectedCountries = new int[] { 2, 3, 4, 5 };
            var actualCountries = _countries.Where(c => c.Id >= 2).ToList();

            //Act

            //Assert
            Assert.NotNull(actualCountries);
            Assert.Equal(expectedCountries, actualCountries.Select(c => c.Id));
        }

        [Fact]
        public void GetCountriesWithIdLessThanTwo()
        {
            //Arrange
            var expectedCountries = new int[] { 1 };
            var actualCountries = _countries.Where(c => c.Id < 2).ToList();

            //Act

            //Assert
            Assert.NotNull(actualCountries);
            Assert.Equal(expectedCountries, actualCountries.Select(c => c.Id));
        }

        [Fact]
        public void GetCountriesWithIdLessOrEqualThanTwo()
        {
            //Arrange
            var expectedCountries = new int[] { 1, 2};
            var actualCountries = _countries.Where(c => c.Id <= 2).ToList();

            //Act

            //Assert
            Assert.NotNull(actualCountries);
            Assert.Equal(expectedCountries, actualCountries.Select(c => c.Id));
        }

        [Fact]
        public void GetEmployeeByFirstNameAndByLastName()
        {
            //Arrange
            var expectedEmployee = new [] { 
                new Employee
                {
                    Id = 8,
                    FirstName = "Bill",
                    LastName = "Gates",
                    OfficeId = 2
                }
            };
            var actualEmployee = _employees
                .Where(e => e.FirstName == "Bill" && 
                            e.LastName == "Gates")
                .ToList();

            //Act

            //Assert
            Assert.NotNull(actualEmployee);
            Assert.Equal(expectedEmployee, actualEmployee);
        }
    }
}
