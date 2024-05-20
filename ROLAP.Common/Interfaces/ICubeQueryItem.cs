using ROLAP.Common.Model;

namespace ROLAP.Common.Interfaces;

public interface ICubeQueryItem
{
    ICubeQueryItem Execute(Cube cube);
}