using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Aero.Application.Workers.TennaxiaDataCollection.Modules;

public class TestModule : AbstractModule
{
    public override string Code => "TestModule";
    public override string Name => "Test Module";
    public override string Description => "This is a test module designed to easily test execution, logging, behaviour in front end, etc.";

    public StringArgument TestArgument { get; } = new(
        name: "TestArgument",
        isRequired: true,
        description: "Test argument that will be logged."
    );
    
    public DateOnlyArgument ProcessDate { get; } = new(
        name: "ProcessDate",
        isRequired: false,
        defaultValue: DateOnly.FromDateTime(DateTime.Today),
        description: "A test date that will simply be logged"
    );
    
    private readonly ILogger<TestModule> _logger;
    private readonly IConfiguration _configuration;

    public TestModule(ILogger<TestModule> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public override async Task RunAsync()
    {
        var testArgument = TestArgument.GetValue(_configuration);
        var processDate = ProcessDate.GetValue(_configuration);
        _logger.LogInformation($"Running test module with argument {testArgument}");
        _logger.LogInformation($"Module will run at date {processDate:yyyy-MM-dd}");
        await Task.CompletedTask;
    }
}