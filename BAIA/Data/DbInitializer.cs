using BAIA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAIA.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BAIA_DB_Context context)
        {

            context.Database.EnsureCreated();
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }
            var users = new User[]
            {
                new User{Name="Youssef",CompanyName="Cross Workers",Email="youssef@gmail.com",Password="youssef12345",PhoneNumber=0100100},
                new User{Name="Mostafa",CompanyName="Valeo",Email="mostafa@gmail.com",Password="mostafa12345",PhoneNumber=0100200},
                new User{Name="Nada",CompanyName="HSBC",Email="nada@gmail.com",Password="nada12345",PhoneNumber=0100300},
                new User{Name="Mariam",CompanyName="CIB",Email="mariam@gmail.com",Password="mariam12345",PhoneNumber=0100400},
                new User{Name="Batoul",CompanyName="QNB",Email="batoul@gmail.com",Password="batoul12345",PhoneNumber=0100500},
                new User{Name="Nour",CompanyName="Anghami",Email="nour@gmail.com",Password="nour12345",PhoneNumber=0100600},
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
            var projects = new Project[]
            {
                new Project{ProjectTitle="Spotify Music Player",ProjectDescription="It's a music player.....",Domain="Music Players",OrganizationName="Spotify",User=users[0]},
                new Project{ProjectTitle="Facebook",ProjectDescription="It's a social media.....",Domain="Social Media",OrganizationName="Meta",User=users[1]},
                new Project{ProjectTitle="Instagram",ProjectDescription="It's a social media.....",Domain="Social Media",OrganizationName="Meta",User=users[3]},
                new Project{ProjectTitle="BAIA",ProjectDescription="It's an Assistant for BAs who.....",Domain="Not Specified",OrganizationName="FCIS",User=users[5]}
            };
            foreach (Project p in projects)
            {
                context.Projects.Add(p);
            }
            context.SaveChanges();

            
        }
    }
}
