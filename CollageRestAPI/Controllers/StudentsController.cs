﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CollageRestAPI.Models;
using CollageRestAPI.Providers;

namespace CollageRestAPI.Controllers
{
    [RoutePrefix("api/Student")]
    public class StudentsController : ApiController
    {
        /*=======================================
        =========== GET METHODS =================
        =======================================*/

        // GET api/Student
        public List<StudentModel> GetStudents()
        {
            return BaseRepository.Instance.Students;
        }

        // GET api/Student/id
        [Route("{id}")]
        public StudentModel GetStudent(int id)
        {
            return BaseRepository.Instance.Students.Single(x => x.Index == id);
        }

        // GET api/Student/id/Grades
        [Route("{id}/Grades")]
        public List<GradeModel> GetGrades(int id)
        {
            return BaseRepository.Instance.Students.Single(x => x.Index == id).Grades;
        }

        // GET api/Student/id/Grades/issueYear/issueMonth/issueDay (eg. /Grades/2016/01/03 )
        [Route("{id}/Grades/{issueYear}/{issueMonth}/{issueDay}")]
        public List<GradeModel> GetGradeByDay(int id, int issueYear, int issueMonth, int issueDay)
        {
            var student = BaseRepository.Instance.Students.Find(x => x.Index == id);
            DateTime incomingData = new DateTime(issueYear, issueMonth, issueDay);
            return student.Grades.Where(x => x.IssueDateTime == incomingData).ToList();
        }

        // GET api/Student/id/Grades/courseName
        [Route("{id}/Grades/{courseName}")]
        public List<GradeModel> GetGradesByCourse(int id, string courseName)
        {
            var course = BaseRepository.Instance.Courses.Single(x => x.CourseName == courseName);
            var student = BaseRepository.Instance.Students.Single(x => x.Index == id);
            return course.Grades.Where(x => x.Student == student).ToList();
        }

        /*=======================================
        =========== POST METHODS ================
        =======================================*/

        // POST api/Student
        public HttpResponseMessage PostStudents([FromBody]List<StudentModel> studentsToCreate)
        {
            //studentsToCreate.ForEach(x => x.Index = IdProvider.Instance.GetId());
            BaseRepository.Instance.Students.AddRange(studentsToCreate);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Url.Content("~/api/Student"));
            return response;
        }

        // POST api/Student/id/Grades
        [Route("{id}/Grades")]
        public HttpResponseMessage PostGrades(int id, [FromBody]List<GradeModel> gradesToAdd)
        {
            var student = BaseRepository.Instance.Students.Single(x => x.Index == id);
            gradesToAdd.ForEach(x => x.Student = student);
            student.Grades.AddRange(gradesToAdd);
            return Request.CreateResponse(HttpStatusCode.Created);
        }

        /*=======================================
        =========== PUT METHODS =================
        =======================================*/

        // PUT api/Student/id
        [Route("{id}")]
        public HttpResponseMessage PutStudent(int id, [FromBody]StudentModel studentToUpdate)
        {
            int studentIndex = BaseRepository.Instance.Students.FindIndex(x => x.Index == id);
            BaseRepository.Instance.Students[studentIndex] = studentToUpdate;
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/Student/id/Grades/issueYear/issueMonth/issueDay (eg. /Grades/2016/01/03 )
        [Route("{id}/Grades/{issueYear}/{issueMonth}/{issueDay}")]
        public HttpResponseMessage PutGrade(int id, int issueYear, int issueMonth, int issueDay, [FromBody]GradeModel gradeToUpdate)
        {
            var student = BaseRepository.Instance.Students.Find(x => x.Index == id);
            DateTime incomingData = new DateTime(issueYear, issueMonth, issueDay);
            int gradeIndex = student.Grades.FindIndex(x => x.IssueDateTime == incomingData);
            student.Grades[gradeIndex] = gradeToUpdate;
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /*=======================================
        =========== DELETE METHODS ==============
        =======================================*/

        // DELETE api/Student/id
        [Route("{id}")]
        public HttpResponseMessage DeleteStudent(int id)
        {
            var studentsList = BaseRepository.Instance.Students;
            studentsList.Remove(studentsList.Single(x => x.Index == id));
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/Student/id/Grades/issueYear/issueMonth/issueDay (eg. /Grades/2016/01/03 )
        [Route("{id}/Grades/{issueYear}/{issueMonth}/{issueDay}")]
        public HttpResponseMessage DeleteGrade(int id, int issueYear, int issueMonth, int issueDay)
        {
            var student = BaseRepository.Instance.Students.Single(x => x.Index == id);
            DateTime incomingData = new DateTime(issueYear, issueMonth, issueDay);
            student.Grades.Remove(student.Grades.Single(x => x.IssueDateTime == incomingData));
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
