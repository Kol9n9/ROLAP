namespace ROLAP.Process.Interfaces;

public interface IProcessor
{
    Task ProcessQuery(string query);
}