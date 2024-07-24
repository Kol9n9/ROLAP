namespace ROLAP.Core.Models.Interfaces;

public interface IDimensionContainer : IContainer
{
    ICubeItem GetItem();
}