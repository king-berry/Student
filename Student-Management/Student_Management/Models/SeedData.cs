
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Student_Management.Data;
using Student_Management.Models;
using System;
using System.Linq;

namespace Student_Management.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new Student_ManagementContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<Student_ManagementContext>>()))
        {
            // Look for any movies.
            if (context.Class.Any())
            {
                return;   // db has been seeded
            }

            var classs = new Class[]
           {
                new Class { Name="T2210M"},
                new Class { Name="T2210A"},
                new Class { Name = "T2210E"},
           };
            foreach (Class c in classs)
            {
                context.Class.Add(c);
            }
            context.SaveChanges();

            var students = new Student[]
           {
                new Student { Id="Th1007", Name="Dua Hau", Image="/uploads/dua-hau.png", ClassId = classs.Single(c => c.Name == "T2210A").Id},
                new Student { Id="Th1008", Name="Cam", Image="/uploads/qua-cam.jpg", ClassId = classs.Single(c => c.Name == "T2210M").Id},
                new Student { Id="Th1009", Name="Chanh", Image="/uploads/qua-chanh.jpg", ClassId = classs.Single(c => c.Name == "T2210E").Id},
                new Student { Id="Th1010", Name="Cam 2", Image="/uploads/qua-cam.jpg", ClassId = classs.Single(c => c.Name == "T2210A").Id},
                new Student { Id="Th1011", Name="Chanh 2", Image="/uploads/qua-chanh.jpg", ClassId = classs.Single(c => c.Name == "T2210A").Id},
           };
            foreach (Student s in students)
            {
                context.Student.Add(s);
            }
            context.SaveChanges();



            // var attendances = new Attendance[]
            //{
            //     new Attendance { Name="T2210M"},
            //     new Attendance { Name="T2210A"},
            //     new Attendance { Name = "T2210E"},
            //};
            // foreach (Attendance a in attendances)
            // {
            //     context.Attendance.Add(a);
            // }
            // context.SaveChanges();

        }
    }
}