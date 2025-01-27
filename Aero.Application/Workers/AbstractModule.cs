using Aero.Base.Attributes;

namespace Aero.Application.Workers;

[Injectable]
public abstract class AbstractModule : IModule
{
    public abstract string Code { get; }
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract Task RunAsync();
}