namespace ROLAP.Processor.Interfaces;

public interface IProcessor
{
    Task ProcessQuery(string query);
}