﻿using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Scheduling.Domain;
using Scheduling.GraphQl.Types;
using Scheduling.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduling.GraphQl
{
    public class Querys : ObjectGraphType
    {
        public Querys(IHttpContextAccessor httpContext, DataBaseRepository dataBaseRepository)
        {

            Name = "Query";
            Field<UserType>(
                "GetCurrentUser",
                arguments: new QueryArguments(
                    new QueryArgument<DateGraphType> { Name = "CalendarDay", Description = "Selected day" }
                    ),
                resolve: context =>
                {
                    string email = httpContext.HttpContext.User.Claims.First(claim => claim.Type == "Email").Value.ToString();
                    User user = dataBaseRepository.Get(email);

                    user.ComputedProps = new ComputedProps();
                    user.ComputedProps.AddPermission(dataBaseRepository.GetPermission(user.Id));
                    user.ComputedProps.AddTeams(dataBaseRepository.GetUserTeams(user.Id));


                    System.DateTime? selectedDay = context.GetArgument<System.DateTime?>("CalendarDay");
                    if (selectedDay.HasValue)
                    {
                        var a = dataBaseRepository.GetTimerHistory(user.Id)
                            .Where(r => r.StartTime.Value.ToShortDateString() == selectedDay.Value.Date.ToShortDateString());

                        user.ComputedProps.AddTimerHistory(new List<TimerHistory>(a.OfType<TimerHistory>()));

                    }
                    else
                        user.ComputedProps.AddTimerHistory(dataBaseRepository.GetTimerHistory(user.Id));

                    return user;
                }
            ).AuthorizeWith("Authenticated");

            Field<ListGraphType<UserType>>(
                "GetAllUsers",
                arguments: null,
                resolve: context =>
                {
                    return dataBaseRepository.Get();
                }
            ).AuthorizeWith("Manager");

            Field<ListGraphType<TeamType>>(
                "GetTeams",
                arguments: null,
                resolve: context =>
                {
                    string email = httpContext.HttpContext.User.Claims.First(claim => claim.Type == "Email").Value.ToString();
                    User user = dataBaseRepository.Get(email);
                    return dataBaseRepository.GetListOfAvailableTeams(user.Id);
                },
                description: "Get list of available teams."
            ).AuthorizeWith("Manager");


            Field<ListGraphType<TeamType>>(
                "GetUserTeams",
                arguments: null,
                resolve: context =>
                {
                    string email = httpContext.HttpContext.User.Claims.First(claim => claim.Type == "Email").Value.ToString();
                    User user = dataBaseRepository.Get(email);
                    return dataBaseRepository.GetUserTeams(user.Id);
                }
            ).AuthorizeWith("Authenticated");

            Field<ListGraphType<UserType>>(
                "GetTeamUsers",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "TeamId", Description = "Team id."}    
                ),
                resolve: context =>
                {
                    int teamId = context.GetArgument<int>("TeamId");
                    return dataBaseRepository.GetTeamUsers(teamId);
                }

            ).AuthorizeWith("Authenticated");

            Field<ListGraphType<StringGraphType>>(
                "GetAllPermissions",
                arguments: null,
                resolve: context =>
                {
                    return dataBaseRepository.GetAllPermissions();
                }
            ).AuthorizeWith("Manager");

            Field<ListGraphType<VacationRequestType>>(
                "GetCurrentUserRequests",
                arguments: null,
                resolve: context =>
                {
                    string email = httpContext.HttpContext.User.Claims.First(claim => claim.Type == "Email").Value.ToString();
                    User user = dataBaseRepository.Get(email);
                    int id = user.Id;
                    return dataBaseRepository.GetUserVacationRequests(user.Id);
                }
            ).AuthorizeWith("Authenticated");

            Field<ListGraphType<VacationResponseType>>(
                "GetVacationRequestInfo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "RequestID" }
                ),
                resolve: context =>
                {
                    int requestId = context.GetArgument<int>("RequestID");
                    return dataBaseRepository.GetVacationRequestResponses(requestId);
                }
            ).AuthorizeWith("Authenticated");

            Field<ListGraphType<VacationRequestType>>(
                "GetRequestsForConsideration",
                arguments: null,
                resolve: context =>
                {
                    string email = httpContext.HttpContext.User.Claims.First(claim => claim.Type == "Email").Value.ToString();
                    User user = dataBaseRepository.Get(email);
                    int id = user.Id;
                    return dataBaseRepository.GetRequestsForConsideration(id);
                }
            ).AuthorizeWith("Manager");

            Field<ListGraphType<VacationRequestType>>(
                "GetConsideredRequests",
                arguments: null,
                resolve: context =>
                {
                    string email = httpContext.HttpContext.User.Claims.First(claim => claim.Type == "Email").Value.ToString();
                    User user = dataBaseRepository.Get(email);
                    int id = user.Id;
                    return dataBaseRepository.GetConsideredRequests(id);
                }
            ).AuthorizeWith("Manager");

            FieldAsync<ListGraphType<TimerHistoryType>, IReadOnlyCollection<TimerHistory>>(
                "GetTimerHistories",
                resolve: ctx =>
                {
                    return dataBaseRepository.GetTimerHistory();
                }).AuthorizeWith("Authenticated");

            Field<UserType>(
                "GetCurrentUserId",
                arguments: null,
                resolve: context =>
                {
                    string email = httpContext.HttpContext.User.Claims.First(claim => claim.Type == "Email").Value.ToString();
                    User user = dataBaseRepository.Get(email);
                    return user;
                }
            ).AuthorizeWith("Authenticated");

            Field<ListGraphType<UserType>>(
                "GetUsersOnVacation",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<DateTimeGraphType>> { Name="Date" }
                ),
                resolve: context =>
                {
                    string email = httpContext.HttpContext.User.Claims.First(claim => claim.Type == "Email").Value.ToString();
                    User user = dataBaseRepository.Get(email);

                    user.ComputedProps = new ComputedProps();
                    user.ComputedProps.AddTeams(dataBaseRepository.GetUserTeams(user.Id));

                    DateTime DateToCheck = context.GetArgument<DateTime>("Date");

                    List<User> teammatesOnVacation = new List<User>();

                    user.ComputedProps.Teams.ForEach((team) => {
                        dataBaseRepository.GetTeamUsers(team.Id).ForEach((user) => {
                            dataBaseRepository.GetUserVacationRequests(user.Id).ToList().ForEach((request) => {
                                if(request.FinishDate >= DateToCheck && request.StartDate <= DateToCheck)
                                {
                                    if (teammatesOnVacation.Contains(user))
                                        return;
                                    teammatesOnVacation.Add(user);
                                }
                            });
                        });
                    });

                    return teammatesOnVacation;
                }
            ).AuthorizeWith("Authenticated");

            Field<DecimalGraphType>(
                "GetAvailableVacationDays",
                arguments: null,
                resolve: context =>
                {
                    string email = httpContext.HttpContext.User.Claims.First(claim => claim.Type == "Email").Value.ToString();
                    User user = dataBaseRepository.Get(email);

                    user.ComputedProps = new ComputedProps();
                    user.ComputedProps.AddVacationRequests(dataBaseRepository.GetUserVacationRequests(user.Id).ToList());

                    DateTime currentDate = DateTime.Now;
                    int vacationDaysSum = user.ComputedProps.VacationRequests
                    .Select(request => (Start: request.StartDate, Finish: request.FinishDate)) //returns only the date pair
                    .Where(date => //selects request the has the same month
                        (date.Finish.Year == currentDate.Year && date.Finish.Month == currentDate.Month) ||
                        (date.Start.Year == currentDate.Year && date.Start.Month == currentDate.Month))
                    .Select(dates => //crops dates to be within the month
                    {
                        var newDates = (Start: dates.Start, Finish: dates.Finish);// makes a copy

                        if (newDates.Start.Month < currentDate.Month)
                            newDates.Start = new DateTime(currentDate.Year, currentDate.Month, 1);
                        if (newDates.Finish.Month > currentDate.Month)
                            newDates.Finish = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

                        return newDates;
                    })
                    .Sum(dates => dates.Finish.DayOfYear - dates.Start.DayOfYear + 1);

                    int availableVacationDaysPerMonth = 2;
                    if (availableVacationDaysPerMonth - vacationDaysSum < 0)
                        return 0;
                    return availableVacationDaysPerMonth - vacationDaysSum;
                }
            ).AuthorizeWith("Authenticated");

            Field<ListGraphType<CalendarEventType>>(
                "GetCurrentUserEvents",
                arguments: null,
                resolve: context =>
                {
                    string email = httpContext.HttpContext.User.Claims.First(claim => claim.Type == "Email").Value.ToString();
                    User user = dataBaseRepository.Get(email);
                    int id = user.Id;
                    return dataBaseRepository.GetUserEvents(user.Id);
                }
            ).AuthorizeWith("Authenticated");
        }
    }
}
