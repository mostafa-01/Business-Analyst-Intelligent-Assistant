﻿using BAIA.Models;
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
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            /*if (context.Users.Any())
            {
                return;   // DB has been seeded
            }
            var users = new User[]
            {
                new User{Name="Youssef",CompanyName="Cross Workers",Email="youssef@gmail.com",Password="youssef12345",PhoneNumber="0100100"},
                new User{Name="Mostafa",CompanyName="Valeo",Email="mostafa@gmail.com",Password="mostafa12345",PhoneNumber="0100200"},
                new User{Name="Nada",CompanyName="HSBC",Email="nada@gmail.com",Password="nada12345",PhoneNumber="0100300"},
                new User{Name="Mariam",CompanyName="CIB",Email="mariam@gmail.com",Password="mariam12345",PhoneNumber="0100400"},
                new User{Name="Batoul",CompanyName="QNB",Email="batoul@gmail.com",Password="batoul12345",PhoneNumber="0100500"},
                new User{Name="Nour",CompanyName="Anghami",Email="nour@gmail.com",Password="nour12345",PhoneNumber="0100600"},
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
            var projects = new Project[]
            {
                new Project{ProjectTitle="Spotify Music Player",ProjectDescription="It's a music player.....",Domain="Music Players",OrganizationName="Spotify",SystemActors="End-User" , User=users[0]},
                new Project{ProjectTitle="Facebook",ProjectDescription="It's a social media.....",Domain="Social Media",OrganizationName="Meta",SystemActors="End-User" , User=users[0]},
                new Project{ProjectTitle="Instagram",ProjectDescription="It's a social media.....",Domain="Social Media",OrganizationName="Meta", SystemActors="End-User" , User=users[3]},
                new Project{ProjectTitle="BAIA",ProjectDescription="It's an Assistant for BAs who.....",Domain="Not Specified",OrganizationName="FCIS", SystemActors="BA,StackHolder" ,User=users[5]}
            };
            foreach (Project p in projects) // Projects are added in reverse!!
            {
                context.Projects.Add(p);
            }
            context.SaveChanges();
            var meetings = new Meeting[]
            {
                new Meeting{MeetingTitle="First Meeting",
                    MeetingDescription="In this meeting we talked about.....",
                    MeetingPersonnel="Ahmed Elsayed, Mohamed Ahmed",
                    AudioReference="ASRModule/audio_wac/batoul_meeting.wav",
                    ASR_Text="Speaker A: Good afternoon, it's a pleasure to meet with you today sirSpeaker B: the pleasure is mine",
                    Project=projects[0] },
                new Meeting{MeetingTitle="Second Meeting",
                    MeetingDescription="In this meeting we talked about.....",
                    MeetingPersonnel="Ahmed Elmohamady, Mohamed Sayed",
                    AudioReference="ASRModule/audio_wac/batoul_meeting.wav",
                    ASR_Text="Speaker A: Good afternoon, it's a pleasure to meet with you today sirSpeaker B: the pleasure is mine",
                    Project=projects[0] },
                new Meeting{MeetingTitle="Third Meeting",
                    MeetingDescription="In this meeting we talked about.....",
                    MeetingPersonnel="Hamdy Elsayed, Youssef Ahmed",
                    AudioReference="ASRModule/audio_wac/batoul_meeting.wav",
                    ASR_Text="Speaker A: Good afternoon, it's a pleasure to meet with you today sirSpeaker B: the pleasure is mine",
                    Project=projects[0] }
            };
            foreach (Meeting m in meetings)
            {
                context.Meetings.Add(m);
            }
            context.SaveChanges();
            var services = new Service[]
            {
                new Service{ServiceTitle="Login Page",Meeting=meetings[0]},
                new Service{ServiceTitle="Home Page",Meeting=meetings[0]},
                new Service{ServiceTitle="Logout Button",Meeting=meetings[1]},
                new Service{ServiceTitle="Home Page",Meeting=meetings[1]},
                new Service{ServiceTitle="Profile Page",Meeting=meetings[2]},
                new Service{ServiceTitle="Login Page",Meeting=meetings[2]}
            };
            foreach (Service s in services)
            {
                context.Services.Add(s);
            }
            context.SaveChanges();
            var serviceDetails = new ServiceDetail[]
            {
                new ServiceDetail{ServiceDetailString="login page should be the first page the user sees when he first opens the application",Timestamp="1:30",Service=services[0]},
                new ServiceDetail{ServiceDetailString="It should provide two textboxes one for the username and one for the password",Timestamp="5:21",Service=services[0]},
                new ServiceDetail{ServiceDetailString="It should have a register button",Timestamp="10:32",Service=services[0]},

                new ServiceDetail{ServiceDetailString="Home page should be the first page the user sees when he login",Timestamp="1:30",Service=services[1]},
                new ServiceDetail{ServiceDetailString="It should provide last accessed item",Timestamp="5:21",Service=services[1]},
                new ServiceDetail{ServiceDetailString="It should have my profile button",Timestamp="10:32",Service=services[1]},
                
                new ServiceDetail{ServiceDetailString="logout button must navigate the user to the login page",Timestamp="3:35",Service=services[2]},
                new ServiceDetail{ServiceDetailString="Any data must be saved before the user logs out",Timestamp="6:69",Service=services[2]},

                new ServiceDetail{ServiceDetailString="Home page must contain logout button",Timestamp="1:30",Service=services[3]},
                new ServiceDetail{ServiceDetailString="It must be interactive",Timestamp="5:21",Service=services[3]},
                //new ServiceDetail{ServiceDetailString="It isn't necessary to provide last accessed item",Timestamp="5:21",Service=services[3]},

                new ServiceDetail{ServiceDetailString="Profile page should should show all data related to the user",Timestamp="1:30",Service=services[4]},
                new ServiceDetail{ServiceDetailString="It should allow the user to edit his\\her data",Timestamp="5:21",Service=services[4]},


                new ServiceDetail{ServiceDetailString="login page must be secured against any hack",Timestamp="1:40",Service=services[5]},
                new ServiceDetail{ServiceDetailString="The user can't login with his username",Timestamp="6:20",Service=services[5]},
                new ServiceDetail{ServiceDetailString="It should prevent user from loging in if any of the textboxes is invalid",Timestamp="8:30",Service=services[5]}
            };
            foreach (ServiceDetail sd in serviceDetails)
            {
                context.ServiceDetails.Add(sd);
            }
            context.SaveChanges();*/

            /*var userStories = new UserStory[]
            {
                new UserStory{UserStoryTitle="First User Story",UserStoryDescription="As a supply planner, I need the ability to plan dependent forecasted items as independent SKUs, so that I can include those items in my MSP / distribution requirements plan.I will know it’s achieved when I can pull in demand forecast of a parent SKU, identify the bill of materials, andcreate a sourcing plan for dependent SKUs.....",
                    Meeting=meetings[0] }
            };
            foreach (UserStory us in userStories)
            {
                context.UserStories.Add(us);
            }
            context.SaveChanges();*/



        }
    }
}
