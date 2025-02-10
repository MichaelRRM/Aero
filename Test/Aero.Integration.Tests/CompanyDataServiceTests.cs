using Aero.Application;
using Aero.Base;
using Aero.MDH.DatabaseAccess;
using Aero.MDH.DatabaseAccess.BusinessEntities.DataServices;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;

namespace Aero.Integration.Tests;

[TestFixture]
[NonParallelizable]
public class CompanyDataServiceTests
{
    private static SqliteConnection _sharedConnection = null!;
    private static MdhDbContext _mdhDbContext = null!;
    private static ServiceProvider _serviceProvider = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _sharedConnection = new SqliteConnection("DataSource=:memory:");
        _sharedConnection.Open();

        var options = new DbContextOptionsBuilder<MdhDbContext>()
            .UseSqlite(_sharedConnection)
            .Options;
            
        _mdhDbContext = new MdhDbContext(options, Substitute.For<IUserService>());
        _mdhDbContext.Database.EnsureCreated();

        var services = new ServiceCollection();
        
        services.AddServices(Substitute.For<IConfiguration>());
        
        services.RemoveAll(typeof(MdhDbContext));
        services.AddScoped<MdhDbContext>(_ => _mdhDbContext);
        
        _serviceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _mdhDbContext.Dispose();
        _sharedConnection.Dispose();
        _serviceProvider.Dispose();
    }

    [SetUp]
    public void SetUp()
    {
        _mdhDbContext.CompanyData.RemoveRange(_mdhDbContext.CompanyData);
        _mdhDbContext.SaveChanges();
    }

    //[Test]
    public void SaveAsync_WithDataSaver_SavesCompanyData()
    {
        var service = _serviceProvider.GetRequiredService<CompanyDataService>();
        
        // Rest of test
    }
}