// Global using directives

global using System.Data.Common;
global using System.Net;
global using Eclipseworks.Application.Commands.CreateProject;
global using Eclipseworks.Application.Commands.CreateProjectTask;
global using Eclipseworks.Application.Queries.ListProjects;
global using Eclipseworks.Application.Queries.ListProjectTasks;
global using Eclipseworks.Domain.Constants;
global using Eclipseworks.Domain.Enums;
global using Eclipseworks.Infrastructure;
global using Eclipseworks.Tests.Utils.ObjectMothers;
global using Eclipseworks.WebApi.Tests.Utils.Clients;
global using Eclipseworks.WebApi.Tests.Utils.Factories;
global using FluentAssertions;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.AspNetCore.TestHost;
global using Microsoft.Data.Sqlite;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.DependencyInjection.Payloads;
global using Microsoft.Extensions.Hosting;
global using Refit;
global using Xunit;