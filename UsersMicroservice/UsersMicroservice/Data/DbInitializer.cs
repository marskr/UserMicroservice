using System.Linq;
using UsersMicroservice.Models;

namespace UsersMicroservice.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new Users[]
            {
            new Users{Id=1, Username="Marvin", Email="markor@op.pl", Password="KFSE43SDQ", Salt="abc", AuthToken="token1"},
            new Users{Id=2, Username="Jarvin", Email="jarkor@op.pl", Password="KFPO43SDQ", Salt="abc", AuthToken="token1"},
            new Users{Id=3, Username="Darvin", Email="darkor@op.pl", Password="KFSE4HJDQ", Salt="abc", AuthToken="token1"}
            };
            foreach (Users user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();
            
            //var permissions = new Permissions[]
            //{
            //    new Permissions{Id = 101, Name = "}
            //};
            //foreach (Course c in courses)
            //{
            //    context.Courses.Add(c);
            //}
            //context.SaveChanges();

            /*var enrollments = new Enrollment[]
            {
            new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
            new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
            new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
            new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
            new Enrollment{StudentID=3,CourseID=1050},
            new Enrollment{StudentID=4,CourseID=1050},
            new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
            new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
            new Enrollment{StudentID=6,CourseID=1045},
            new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
            };
            foreach (Enrollment e in enrollments)
            {
                context.Enrollments.Add(e);
            }
            context.SaveChanges();*/
        }
    }
}
