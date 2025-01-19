namespace Aero.Worker.WebApi.Models;

public record ModuleDto(
    string Name, 
    string Description, 
    IEnumerable<ArgumentDto> Arguments
    );