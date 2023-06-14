using Archz.Employee.Api.Services;
using Archz.Core.Validation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Archz.Employee.Api.Models
{
    public class Employee : AbstractValidatableObject
    {
        public int EmployeeId { get; set; }

        [MinLength(2, ErrorMessage = "Minimum 2 characters required"), MaxLength(50, ErrorMessage = "Maximum 50 characters allowed")]
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public int Age { get; set; }


        public override async Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext,
         CancellationToken cancellation)
        {
            if (Salary < 10000)
                _errors.Add(new ValidationResult("Salary cannot be less than $10,000", new[] { nameof(Salary) }));

            if (Age < 18)
                _errors.Add(new ValidationResult($"Age: {Age} not allowed to work. Minimum age is 18 or more", new[] { nameof(Age) }));

            //get the required service injected
            //var employeeService = validationContext.GetService<IEmployeeService>();

            //// Database call through service for validation
            //var isExist = await employeeService.IsEmployeeExist(EmployeeId);
            //if (isExist)
            //{
            //    _errors.Add(new ValidationResult("EmployeeId exist", new[] { nameof(EmployeeId) }));
            //}

            return _errors;
        }
    }


public static class EmployeeEndpoints
{
	public static void MapEmployeeEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Employee").WithTags(nameof(Employee));

        group.MapGet("/", () =>
        {
            return new [] { new Employee() };
        })
        .WithName("GetAllEmployees")
        .WithOpenApi();

        group.MapGet("/{id}", (int id) =>
        {
            //return new Employee { ID = id };
        })
        .WithName("GetEmployeeById")
        .WithOpenApi();

        group.MapPut("/{id}", (int id, Employee input) =>
        {
            return TypedResults.NoContent();
        })
        .WithName("UpdateEmployee")
        .WithOpenApi();

        group.MapPost("/", (Employee model) =>
        {
            //return TypedResults.Created($"/Employees/{model.ID}", model);
        })
        .WithName("CreateEmployee")
        .WithOpenApi();

        group.MapDelete("/{id}", (int id) =>
        {
            //return TypedResults.Ok(new Employee { ID = id });
        })
        .WithName("DeleteEmployee")
        .WithOpenApi();
    }
}}
