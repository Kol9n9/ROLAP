namespace ROLAP.Process.Interfaces;

public interface IProcessor
{
    Task<string> ProcessQuery(string query);
}