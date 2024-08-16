using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace Architecture.Tests;

public class ArchitectureTest
{
    private const string DomainNamespace = "Domain";
    private const string ApplicationNamespace = "Application";
    private const string InfrastructureNamespace = "Infrastructure";
    private const string WebApiNamespace = "WebApi";

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Domain_Should_Not_HaveAnyDependency()
    {
        //Arrange
        var assembly = Assembly.LoadFrom("Domain.dll");
        //Act
        var testResult = Types.InAssembly(assembly).ShouldNot()
        .HaveDependencyOnAny(ApplicationNamespace, InfrastructureNamespace, WebApiNamespace)
        .GetResult();
        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Test]
    public void Application_Should_HaveDependencyOnDomain(){
        //Arrange
        var assembly = Assembly.LoadFrom("Application.dll");
        //Act
        var testResult = Types.InAssembly(assembly).Should().HaveDependencyOn(DomainNamespace).GetResult();
        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Test]
    public void Infrastructure_Should_HaveDependencyOnDomainAndApplication(){
        //Arrange
        var assembly = Assembly.LoadFrom("Infrastructure.dll");
        //Act
        var testResult = Types.InAssembly(assembly).Should()
        .HaveDependencyOnAll(DomainNamespace,ApplicationNamespace).GetResult();
        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Test]
    public void WebApi_Should_HaveDependencyOnApplication(){
        //Arrange
        var assembly = Assembly.LoadFrom("WebApi.dll");
        //Act
        var testResult = Types.InAssembly(assembly).Should().HaveDependencyOn(ApplicationNamespace).GetResult();
        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Test]
    public void Application_Should_Not_HaveDependencyOnInfrastructureAndWebApi(){
        //Arrange
        var assembly = Assembly.LoadFrom("Application.dll");
        //Act
        var testResult = Types.InAssembly(assembly).ShouldNot()
        .HaveDependencyOnAll(InfrastructureNamespace,WebApiNamespace).GetResult();
        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
}