using ROLAP.Common.Model.Interfaces;
using ROLAP.Common.Model.Models;
using ROLAP.CubeConfiguration;
using ROLAP.Repository.Postgre;

namespace ROLAP.Process
{
    public class CubeProcess
    {
        private readonly IRepository repository = new PostgreRepository();
        public void Process(string mdx)
        {
            Console.WriteLine("Process start");
            var request = new Parser.Parser(mdx).GetCubeQuery(repository);
            switch (request.Type)
            {
                case CubeQueryType.Select:
                    {
                        new CubeProcessSelect(repository, request, MapperCubeConfiguration.GetCubeConfiguration(repository)).Process();
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
