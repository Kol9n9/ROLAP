using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROLAP.Common.Model.Models.Meta;

namespace ROLAP.Common.Model.Models
{
    /// <summary>
    /// Модель для запроса куба
    /// </summary>
    public class CubeQuery : ICubeQueryNode
    {
        /// <summary>
        /// Тип запроса
        /// </summary>
        public CubeQueryType Type { get; set; }
        /// <summary>
        /// Оси
        /// </summary>
        public List<CubeQueryAxis> Axes { get; set; } = new List<CubeQueryAxis>(); 
        /// <summary>
        /// Наименование куба
        /// </summary>
        public string CubeName { get; set; }
    }
    /// <summary>
    /// Ось
    /// </summary>
    public class CubeQueryAxis : ICubeQueryNode
    {
        /// <summary>
        /// Набор кортежей
        /// </summary>
        public CubeQuerySet Set { get; set; }
        /// <summary>
        /// Номер на оси
        /// </summary>
        public int AxisNumber { get; set; }
    }
    /// <summary>
    /// Набор кортежей
    /// </summary>
    public class CubeQuerySet : ICubeQueryNode
    {
        /// <summary>
        /// Кортежи
        /// </summary>
        public List<CubeQueryTuple> Tuples { get; set; } = new List<CubeQueryTuple>();
    }
    /// <summary>
    /// Кортеж
    /// </summary>
    public class CubeQueryTuple : ICubeQueryNode
    {
        /// <summary>
        /// Члены кортежа
        /// </summary>
        public List<CubeQueryMember> Members { get; set; } = new List<CubeQueryMember>();
    }
    /// <summary>
    /// Член кортежа
    /// </summary>
    public class CubeQueryMember : ICubeQueryNode
    {
        /// <summary>
        /// Тип члена
        /// </summary>
        public CubeMemberType Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Names { get; set; }
    }

    public class CubeQueryValue
    {
        public object Value { get; set; }
        public List<ICubeMeta> MetaInfo { get; set; }
        //public List<CubeMetaItem2> Dimensions { get; set; }
        //public CubeMetaItem2 Measure { get; set; }
    }
    
    public enum CubeMemberType
    {
        Unknown = -1,
        Dimension,
        Measure
    }
    public enum CubeQueryType
    {
        Unknown = -1,
        Select
    }

    public interface ICubeQueryNode { }
}
