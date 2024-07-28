using ROLAP.Core.Models.Interfaces.CubeItem;

namespace ROLAP.Core.Models.Interfaces.Container;

public interface IDimensionContainer : IContainer
{
    ICubeItem GetItem();
}