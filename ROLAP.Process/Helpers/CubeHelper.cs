using ROLAP.Common.Model.Helpers;
using ROLAP.Common.Model.Models;
using ROLAP.Common.Model.Models.Meta;

namespace ROLAP.Process.Helpers;

public static class CubeHelper
{
    public static List<List<List<ICubeMeta>>> GetCubeQueryMetas(List<ICubeMeta> cubeMetas, CubeQuery query)
    {
        List<List<List<ICubeMeta>>> res = new List<List<List<ICubeMeta>>>();
        List<CubeDimensionMeta> allDimensions = cubeMetas.OfType<CubeDimensionMeta>().ToList();
        List<CubeMeasureMeta> allMeasures = cubeMetas.OfType<CubeMeasureMeta>().ToList();
        foreach (var axis in query.Axes)
        {
            List<List<ICubeMeta>> axisMetas = new List<List<ICubeMeta>>();
            foreach (var tuple in axis.Set.Tuples)
            {
                List<ICubeMeta> tupleMetas = new List<ICubeMeta>();
                foreach (var member in tuple.Members)
                {
                    if (member.Type == CubeMemberType.Dimension)
                    {
                        var dimension = MetaHelper.GetDimensionWithValueMeta(allDimensions, member.Names);
                        if (dimension == null) throw new Exception("Dimension dont find");
                        tupleMetas.Add(dimension);
                    } 
                    else if (member.Type == CubeMemberType.Measure)
                    {
                        var measure = MetaHelper.GetMeasureMeta(allMeasures, member.Names);
                        if (measure == null) throw new Exception("Measure dont find");
                        tupleMetas.Add(measure);
                    }
                }
                axisMetas.Add(tupleMetas);
            }
            
            res.Add(axisMetas);
        }

        return res;
    }

    public static List<List<ICubeMeta>> GenerateQuery(List<List<List<ICubeMeta>>> axesMeta, List<ICubeMeta> prevMetas = null)
    {
        List<List<ICubeMeta>> result = new List<List<ICubeMeta>>();
        if (axesMeta.Count > 1)
        {
            List<List<ICubeMeta>> prev = new List<List<ICubeMeta>>();
            foreach (var prevItem in axesMeta[axesMeta.Count-1])
            {
                prev.AddRange(MultiplyMetas(prevItem,prevMetas));
            }

            foreach (var pr in prev)
            {
                result.AddRange(GenerateQuery(axesMeta.Take(axesMeta.Count-1).ToList(),pr));
            }
            
        }
        else
        {
            foreach (var cubeMeta in axesMeta[0])
            {
                result.Add(MultiplyMetas(cubeMeta,prevMetas).SelectMany(x => x).ToList());
            }
        }

        return result;

    }

    private static List<List<ICubeMeta>> MultiplyMetas(List<ICubeMeta> prevItem, List<ICubeMeta> cubeMetas)
    {
        List<List<ICubeMeta>> res = new List<List<ICubeMeta>>();
        if (cubeMetas == null) return new List<List<ICubeMeta>>
        {
            new List<ICubeMeta>(prevItem)
        };
        foreach (var cubeMeta in cubeMetas)
        {
            List<ICubeMeta> cubeMetaRes = new List<ICubeMeta>();
            prevItem.AddRange(cubeMetas);
            cubeMetaRes.AddRange(prevItem);;
            res.Add(cubeMetaRes);
        }
        return res;
    }
}