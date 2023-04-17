using ROLAP.Model.CubeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROLAP.Parser.InterpreterModel
{
    internal class MemberItem : TupleItem
    {
        public List<string> Hierarchy { get; set; } = new List<string>();
        public string FuncName { get; set; }

        public override List<TupleItem> Run()
        {
            return new List<TupleItem> { this as TupleItem };
        }
        private Tuple<ROLAP.Model.Models.CubeMemberType,Guid> GetKey()
        {
            ROLAP.Model.Models.CubeMemberType hierarchyType = ROLAP.Model.Models.CubeMemberType.Unknown;
            string firstHierarchy = Hierarchy.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(firstHierarchy)) throw new Exception("");
            if(Hierarchy.Count < 2) throw new Exception("");
            if (firstHierarchy.ToLower().StartsWith("dimension"))
            {
                hierarchyType = ROLAP.Model.Models.CubeMemberType.Dimension;
            }
            else if (firstHierarchy.ToLower().StartsWith("Measure")){
                hierarchyType = ROLAP.Model.Models.CubeMemberType.Measure;
            }
            string lastHierarchy = Hierarchy.LastOrDefault();
            Guid key = Guid.Empty;
            if (lastHierarchy.StartsWith("&"))
            {
                key = new Guid(lastHierarchy.Substring(1));
            } 
            else
            {

            }
            return new Tuple<ROLAP.Model.Models.CubeMemberType, Guid>(hierarchyType, key);
        }
        internal override List<CubeMemberRequest> GetCubeMemberRequest()
        {
            var key = GetKey();
            return new List<CubeMemberRequest>
            {
                new CubeMemberRequest
                {
                    Id = key.Item2,
                    Type = key.Item1
                }
            };
        }
    }
}
