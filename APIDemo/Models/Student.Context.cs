﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APIDemo.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class StudentDB : DbContext
    {
        public StudentDB()
            : base("name=StudentDB")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<StudentProfile> StudentProfile { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }
    
        public virtual ObjectResult<StudentProfile> StudentProfile_Sel(string id_operator, string id_value, string name_operator, string name_value, string coupon_operator, string coupon_value, string height_operator, Nullable<decimal> height_value, Nullable<decimal> height_value2, string weight_operator, Nullable<decimal> weight_value, Nullable<decimal> weight_value2, string gender_value, string blood_value)
        {
            var id_operatorParameter = id_operator != null ?
                new ObjectParameter("Id_operator", id_operator) :
                new ObjectParameter("Id_operator", typeof(string));
    
            var id_valueParameter = id_value != null ?
                new ObjectParameter("Id_value", id_value) :
                new ObjectParameter("Id_value", typeof(string));
    
            var name_operatorParameter = name_operator != null ?
                new ObjectParameter("Name_operator", name_operator) :
                new ObjectParameter("Name_operator", typeof(string));
    
            var name_valueParameter = name_value != null ?
                new ObjectParameter("Name_value", name_value) :
                new ObjectParameter("Name_value", typeof(string));
    
            var coupon_operatorParameter = coupon_operator != null ?
                new ObjectParameter("Coupon_operator", coupon_operator) :
                new ObjectParameter("Coupon_operator", typeof(string));
    
            var coupon_valueParameter = coupon_value != null ?
                new ObjectParameter("Coupon_value", coupon_value) :
                new ObjectParameter("Coupon_value", typeof(string));
    
            var height_operatorParameter = height_operator != null ?
                new ObjectParameter("Height_operator", height_operator) :
                new ObjectParameter("Height_operator", typeof(string));
    
            var height_valueParameter = height_value.HasValue ?
                new ObjectParameter("Height_value", height_value) :
                new ObjectParameter("Height_value", typeof(decimal));
    
            var height_value2Parameter = height_value2.HasValue ?
                new ObjectParameter("Height_value2", height_value2) :
                new ObjectParameter("Height_value2", typeof(decimal));
    
            var weight_operatorParameter = weight_operator != null ?
                new ObjectParameter("Weight_operator", weight_operator) :
                new ObjectParameter("Weight_operator", typeof(string));
    
            var weight_valueParameter = weight_value.HasValue ?
                new ObjectParameter("Weight_value", weight_value) :
                new ObjectParameter("Weight_value", typeof(decimal));
    
            var weight_value2Parameter = weight_value2.HasValue ?
                new ObjectParameter("Weight_value2", weight_value2) :
                new ObjectParameter("Weight_value2", typeof(decimal));
    
            var gender_valueParameter = gender_value != null ?
                new ObjectParameter("Gender_value", gender_value) :
                new ObjectParameter("Gender_value", typeof(string));
    
            var blood_valueParameter = blood_value != null ?
                new ObjectParameter("Blood_value", blood_value) :
                new ObjectParameter("Blood_value", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<StudentProfile>("StudentProfile_Sel", id_operatorParameter, id_valueParameter, name_operatorParameter, name_valueParameter, coupon_operatorParameter, coupon_valueParameter, height_operatorParameter, height_valueParameter, height_value2Parameter, weight_operatorParameter, weight_valueParameter, weight_value2Parameter, gender_valueParameter, blood_valueParameter);
        }
    
        public virtual ObjectResult<StudentProfile> StudentProfile_Sel(string id_operator, string id_value, string name_operator, string name_value, string coupon_operator, string coupon_value, string height_operator, Nullable<decimal> height_value, Nullable<decimal> height_value2, string weight_operator, Nullable<decimal> weight_value, Nullable<decimal> weight_value2, string gender_value, string blood_value, MergeOption mergeOption)
        {
            var id_operatorParameter = id_operator != null ?
                new ObjectParameter("Id_operator", id_operator) :
                new ObjectParameter("Id_operator", typeof(string));
    
            var id_valueParameter = id_value != null ?
                new ObjectParameter("Id_value", id_value) :
                new ObjectParameter("Id_value", typeof(string));
    
            var name_operatorParameter = name_operator != null ?
                new ObjectParameter("Name_operator", name_operator) :
                new ObjectParameter("Name_operator", typeof(string));
    
            var name_valueParameter = name_value != null ?
                new ObjectParameter("Name_value", name_value) :
                new ObjectParameter("Name_value", typeof(string));
    
            var coupon_operatorParameter = coupon_operator != null ?
                new ObjectParameter("Coupon_operator", coupon_operator) :
                new ObjectParameter("Coupon_operator", typeof(string));
    
            var coupon_valueParameter = coupon_value != null ?
                new ObjectParameter("Coupon_value", coupon_value) :
                new ObjectParameter("Coupon_value", typeof(string));
    
            var height_operatorParameter = height_operator != null ?
                new ObjectParameter("Height_operator", height_operator) :
                new ObjectParameter("Height_operator", typeof(string));
    
            var height_valueParameter = height_value.HasValue ?
                new ObjectParameter("Height_value", height_value) :
                new ObjectParameter("Height_value", typeof(decimal));
    
            var height_value2Parameter = height_value2.HasValue ?
                new ObjectParameter("Height_value2", height_value2) :
                new ObjectParameter("Height_value2", typeof(decimal));
    
            var weight_operatorParameter = weight_operator != null ?
                new ObjectParameter("Weight_operator", weight_operator) :
                new ObjectParameter("Weight_operator", typeof(string));
    
            var weight_valueParameter = weight_value.HasValue ?
                new ObjectParameter("Weight_value", weight_value) :
                new ObjectParameter("Weight_value", typeof(decimal));
    
            var weight_value2Parameter = weight_value2.HasValue ?
                new ObjectParameter("Weight_value2", weight_value2) :
                new ObjectParameter("Weight_value2", typeof(decimal));
    
            var gender_valueParameter = gender_value != null ?
                new ObjectParameter("Gender_value", gender_value) :
                new ObjectParameter("Gender_value", typeof(string));
    
            var blood_valueParameter = blood_value != null ?
                new ObjectParameter("Blood_value", blood_value) :
                new ObjectParameter("Blood_value", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<StudentProfile>("StudentProfile_Sel", mergeOption, id_operatorParameter, id_valueParameter, name_operatorParameter, name_valueParameter, coupon_operatorParameter, coupon_valueParameter, height_operatorParameter, height_valueParameter, height_value2Parameter, weight_operatorParameter, weight_valueParameter, weight_value2Parameter, gender_valueParameter, blood_valueParameter);
        }
    
        public virtual ObjectResult<StudentProfile_summary_Result> StudentProfile_summary(string columnName)
        {
            var columnNameParameter = columnName != null ?
                new ObjectParameter("columnName", columnName) :
                new ObjectParameter("columnName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<StudentProfile_summary_Result>("StudentProfile_summary", columnNameParameter);
        }
    }
}
