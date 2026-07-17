using System.Reflection;
using Archz.Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrdersProgram = Archz.Orders.Api.Program;
using ProductsProgram = Archz.Products.Api.Program;
using SharedEntity = Archz.SharedKernel.SeedWork.Entity;
using UsersProgram = Archz.Users.Api.Program;

namespace Archz.Architecture.Tests;

public sealed class ArchitectureRulesTests
{
    private static readonly Assembly UsersAssembly = typeof(UsersProgram).Assembly;
    private static readonly Assembly ProductsAssembly = typeof(ProductsProgram).Assembly;
    private static readonly Assembly OrdersAssembly = typeof(OrdersProgram).Assembly;
    private static readonly Assembly ApplicationCoreAssembly = typeof(BaseController).Assembly;
    private static readonly Assembly SharedKernelAssembly = typeof(SharedEntity).Assembly;

    [Fact]
    public void Service_projects_do_not_reference_each_other()
    {
        var serviceAssemblies = new[]
        {
            UsersAssembly,
            ProductsAssembly,
            OrdersAssembly
        };

        foreach (var assembly in serviceAssemblies)
        {
            var otherServiceNames = serviceAssemblies
                .Where(other => other != assembly)
                .Select(other => other.GetName().Name)
                .ToArray();

            var referencedServices = assembly.GetReferencedAssemblies()
                .Select(reference => reference.Name)
                .Where(reference => otherServiceNames.Contains(reference))
                .ToArray();

            Assert.Empty(referencedServices);
        }
    }

    [Fact]
    public void Users_and_products_controllers_use_api_controller_and_versioned_routes()
    {
        var controllers = new[] { UsersAssembly, ProductsAssembly }
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
            .Where(type => !type.IsAbstract)
            .ToArray();

        Assert.NotEmpty(controllers);

        foreach (var controller in controllers)
        {
            Assert.EndsWith("Controller", controller.Name);
            Assert.NotNull(controller.GetCustomAttribute<ApiControllerAttribute>());

            var route = controller.GetCustomAttribute<RouteAttribute>();
            Assert.NotNull(route);
            Assert.Contains("v{version:apiVersion}", route.Template);
        }
    }

    [Fact]
    public void Domain_types_do_not_depend_on_application_infrastructure_or_controllers()
    {
        var forbiddenNamespaceParts = new[]
        {
            ".Application",
            ".Controllers",
            ".Infra",
            ".Startup",
            ".Settings"
        };

        var domainTypes = new[] { UsersAssembly, ProductsAssembly }
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.Namespace?.Contains(".Domain") == true)
            .ToArray();

        Assert.NotEmpty(domainTypes);

        var violations = domainTypes
            .SelectMany(type => GetReferencedTypes(type)
                .Where(reference => reference.Namespace is not null)
                .Where(reference => forbiddenNamespaceParts.Any(part => reference.Namespace!.Contains(part)))
                .Select(reference => $"{type.FullName} -> {reference.FullName}"))
            .Distinct()
            .OrderBy(violation => violation)
            .ToArray();

        Assert.Empty(violations);
    }

    [Fact]
    public void Command_and_query_handlers_live_in_matching_application_namespaces()
    {
        var serviceAssemblies = new[] { UsersAssembly, ProductsAssembly };
        var requestHandlerInterface = typeof(IRequestHandler<,>);

        var handlerTypes = serviceAssemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Where(type => type.GetInterfaces().Any(IsRequestHandlerInterface))
            .ToArray();

        Assert.NotEmpty(handlerTypes);

        var violations = handlerTypes
            .Where(type =>
                type.Name.EndsWith("CommandHandler", StringComparison.Ordinal)
                    ? type.Namespace?.Contains(".Application.Commands.") != true
                    : type.Name.EndsWith("QueryHandler", StringComparison.Ordinal)
                        ? type.Namespace?.Contains(".Application.Queries.") != true
                        : true)
            .Select(type => type.FullName)
            .ToArray();

        Assert.Empty(violations);

        bool IsRequestHandlerInterface(Type candidate) =>
            candidate.IsGenericType &&
            candidate.GetGenericTypeDefinition() == requestHandlerInterface;
    }

    [Fact]
    public void Building_blocks_do_not_reference_service_assemblies()
    {
        var serviceAssemblyNames = new[]
        {
            UsersAssembly.GetName().Name,
            ProductsAssembly.GetName().Name,
            OrdersAssembly.GetName().Name
        };

        var violations = new[] { ApplicationCoreAssembly, SharedKernelAssembly }
            .SelectMany(assembly => assembly.GetReferencedAssemblies()
                .Where(reference => serviceAssemblyNames.Contains(reference.Name))
                .Select(reference => $"{assembly.GetName().Name} -> {reference.Name}"))
            .ToArray();

        Assert.Empty(violations);
    }

    private static IEnumerable<Type> GetReferencedTypes(Type type)
    {
        return type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            .Select(field => field.FieldType)
            .Concat(type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(property => property.PropertyType))
            .Concat(type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .SelectMany(constructor => constructor.GetParameters().Select(parameter => parameter.ParameterType)))
            .Concat(type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(method => !method.IsSpecialName)
                .SelectMany(method => method.GetParameters().Select(parameter => parameter.ParameterType).Append(method.ReturnType)))
            .Concat(type.GetInterfaces())
            .Concat(type.BaseType is null ? [] : [type.BaseType])
            .SelectMany(ExpandGenericArguments)
            .Where(reference => reference.Assembly == type.Assembly);
    }

    private static IEnumerable<Type> ExpandGenericArguments(Type type)
    {
        yield return type;

        if (!type.IsGenericType)
        {
            yield break;
        }

        foreach (var argument in type.GetGenericArguments().SelectMany(ExpandGenericArguments))
        {
            yield return argument;
        }
    }
}
