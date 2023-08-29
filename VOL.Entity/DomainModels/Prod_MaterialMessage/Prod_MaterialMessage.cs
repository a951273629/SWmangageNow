/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果数据库字段发生变化，请在代码生器重新生成此Model
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOL.Entity.SystemModels;

namespace VOL.Entity.DomainModels
{
    [Entity(TableCnName = "物料信息档案",TableName = "Prod_MaterialMessage")]
    public partial class Prod_MaterialMessage:BaseEntity
    {
        /// <summary>
       ///序号
       /// </summary>
       [Key]
       [Display(Name ="序号")]
       [Column(TypeName="bigint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public long ID { get; set; }

       /// <summary>
       ///物料编码
       /// </summary>
       [Display(Name ="物料编码")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Code { get; set; }

       /// <summary>
       ///物料名称
       /// </summary>
       [Display(Name ="物料名称")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Name { get; set; }

       /// <summary>
       ///物料分类
       /// </summary>
       [Display(Name ="物料分类")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Classify { get; set; }

       /// <summary>
       ///物料规则
       /// </summary>
       [Display(Name ="物料规则")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Regular { get; set; }

       /// <summary>
       ///物料单位
       /// </summary>
       [Display(Name ="物料单位")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Unit { get; set; }

       /// <summary>
       ///物料类型
       /// </summary>
       [Display(Name ="物料类型")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Type { get; set; }

       /// <summary>
       ///物料来源
       /// </summary>
       [Display(Name ="物料来源")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Source { get; set; }

       /// <summary>
       ///状态
       /// </summary>
       [Display(Name ="状态")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? Status { get; set; }

       /// <summary>
       ///操作
       /// </summary>
       [Display(Name ="操作")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Operation { get; set; }

       
    }
}