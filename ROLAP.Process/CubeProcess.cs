using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;
using ROLAP.Mapping;
using ROLAP.Process.Models;
using ROLAP.Repository.Postgre;
using ROLAP.TestLocalRepository;

namespace ROLAP.Process
{
    public class CubeProcess
    {
        private readonly IRepository repository = new PostgreRepository();
        private CubeMeta _cubeMeta;
        public void Process(string mdx)
        {
            Console.WriteLine("Process start");
            _cubeMeta = (new MappingCubeConfiguration(repository)).GetCubeMeta("");
            var request = new Parser.Parser(mdx).GetCubeQuery(new MappingCubeConfiguration(repository));
            switch (request.Type)
            {
                case CubeQueryType.Select:
                    {
                        new CubeProcessSelect(repository, request, _cubeMeta).Process();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            
        }
       
    }
}
