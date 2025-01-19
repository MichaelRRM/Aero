namespace Aero.Worker.WebApi.Models;

public record ArgumentDto(
    string Name, 
    string Description, 
    string Type, 
    string? DefaultValue
    );