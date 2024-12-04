namespace GylleneDroppen.Api.Utilities.Interfaces;

public interface IConfigProvider<out T> where T : class, new()
{
    T GetConfig();
}