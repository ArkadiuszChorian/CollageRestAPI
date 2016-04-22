﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CollageRestAPI.Models
{
    //[DataContract(IsReference = true)]
    public class GradeModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public double Value { get; set; }
        public DateTime IssueDateTime { get; set; }
        [IgnoreDataMember]
        public StudentModel Student { get; set; } = new StudentModel();
        [IgnoreDataMember]
        public CourseModel Course { get; set; } = new CourseModel();
    }
}