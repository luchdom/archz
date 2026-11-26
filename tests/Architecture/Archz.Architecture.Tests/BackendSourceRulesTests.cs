using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using OrdersProgram = Archz.Orders.Api.Program;
using ProductsProgram = Archz.Products.Api.Program;
using UsersProgram = Archz.Users.Api.Program;

namespace Archz.Architecture.Tests;

public sealed class BackendSourceRulesTests
{
    private static readonly IReadOnlyList<BackendSourceFile> Sources = BackendSourceFile.LoadCompilerInputs();

    [Fact]
    public void Mvc_json_request_types_are_top_level_records_in_matching_request_files()
    {
        var bodies = GetJsonBodies<UsersProgram>()
            .Concat(GetJsonBodies<ProductsProgram>())
            .Concat(GetJsonBodies<OrdersProgram>())
            .ToArray();

        Assert.NotEmpty(bodies);
        var violations = bodies
            .Where(body => !IsRequestRecord(body.ControllerAssembly, body.ParameterType))
            .Select(body => $"{body.DisplayName}: put {body.ParameterType.FullName} in a top-level *Request record class under Controllers/Requests in {body.ControllerAssembly.GetName().Name}.")
            .Order(StringComparer.Ordinal)
            .ToArray();
        Assert.True(violations.Length == 0, string.Join(Environment.NewLine, violations));
    }

    [Fact]
    public void Each_handwritten_backend_file_contains_at_most_one_top_level_class()
    {
        Assert.NotEmpty(Sources);
        var violations = Sources
            .Where(source => !source.IsGenerated)
            .Select(source => (source.Path, Types: source.TopLevelClasses.Select(type => type.Identifier.ValueText).ToArray()))
            .Where(source => source.Types.Length > 1)
            .Select(source => $"{source.Path}: {string.Join(", ", source.Types)}")
            .Order(StringComparer.Ordinal)
            .ToArray();
        Assert.True(violations.Length == 0, string.Join(Environment.NewLine, violations));
    }

    private static IReadOnlyList<(string? DisplayName, Assembly ControllerAssembly, Type ParameterType)> GetJsonBodies<TProgram>()
        where TProgram : class
    {
        var assemblyName = typeof(TProgram).Assembly.GetName().Name;
        var contentRoot = Sources.First(source => source.AssemblyName == assemblyName).ProjectDirectory;
        using var factory = new MvcArchitectureFactory<TProgram>(contentRoot);

        return factory.Services.GetRequiredService<IActionDescriptorCollectionProvider>()
            .ActionDescriptors.Items
            .OfType<ControllerActionDescriptor>()
            .Where(AcceptsJson)
            .SelectMany(action => action.Parameters
                .Where(parameter => parameter.BindingInfo?.BindingSource == BindingSource.Body)
                .Select(parameter => (action.DisplayName, action.ControllerTypeInfo.Assembly, parameter.ParameterType)))
            .Distinct()
            .ToArray();
    }

    private static bool AcceptsJson(ControllerActionDescriptor action)
    {
        MediaTypeCollection contentTypes = [];
        foreach (var constraint in action.FilterDescriptors.Select(descriptor => descriptor.Filter).OfType<IApiRequestMetadataProvider>())
            constraint.SetContentTypes(contentTypes);

        return contentTypes.Count == 0 || contentTypes
            .Select(text => MediaTypeHeaderValue.Parse(text))
            .Any(mediaType => mediaType.MatchesMediaType("application/json") ||
                mediaType.MatchesMediaType("text/json") ||
                mediaType.Suffix.Equals("json", StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsRequestRecord(Assembly controllerAssembly, Type type)
    {
        var name = type.Name.Split('`')[0];
        if (type.Assembly != controllerAssembly || type.IsNested || !name.EndsWith("Request", StringComparison.Ordinal))
            return false;

        var requestPath = Path.Combine("Controllers", "Requests", $"{name}.cs");
        var metadataType = type.IsConstructedGenericType ? type.GetGenericTypeDefinition() : type;
        return Sources
            .Where(source => source.AssemblyName == controllerAssembly.GetName().Name &&
                Path.GetRelativePath(source.ProjectDirectory, source.Path) == requestPath)
            .SelectMany(source => source.TopLevelClasses)
            .Any(declaration => declaration is RecordDeclarationSyntax &&
                BackendSourceFile.MetadataFullName(declaration) == metadataType.FullName);
    }
}
