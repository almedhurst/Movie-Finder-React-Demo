using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieFinder.Web.Middleware;
using NSubstitute.ExceptionExtensions;

namespace MovieFinder.Web.Tests.Middleware;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async void InvokeAsync_WhenNoException_CallRequestDelegate()
    {
        //Arrange
        var mockLogger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var mockHostEnvironment = Substitute.For<IHostEnvironment>();
        var mockRequestDelegate = Substitute.For<RequestDelegate>();
        var subject = new ExceptionMiddleware(mockRequestDelegate, mockLogger, mockHostEnvironment);
        
        //Act
        await subject.InvokeAsync(new DefaultHttpContext());
        
        //Arrange
        mockRequestDelegate.Received();
    }

    [Fact]
    public async void InvokeAsync_WhenException_LoggerLogErrorIsCalled()
    {
        //Arrange
        var mockLogger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var mockHostEnvironment = Substitute.For<IHostEnvironment>();
        var mockRequestDelegate = Substitute.For<RequestDelegate>();
        mockRequestDelegate.Invoke(Arg.Any<HttpContext>()).Throws(new Exception("A test error"));
        var subject = new ExceptionMiddleware(mockRequestDelegate, mockLogger, mockHostEnvironment);
        
        //Act
        await subject.InvokeAsync(new DefaultHttpContext());
        
        //Arrange
        mockLogger.Received().LogError(Arg.Any<Exception>(), "A test error");
    }

    [Fact]
    public async void InvokeAsync_WhenException_ContentTypeShouldBeJson()
    {
        //Arrange
        var mockLogger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var mockHostEnvironment = Substitute.For<IHostEnvironment>();
        var mockRequestDelegate = Substitute.For<RequestDelegate>();
        mockRequestDelegate.Invoke(Arg.Any<HttpContext>()).Throws(new Exception("A test error"));
        var subject = new ExceptionMiddleware(mockRequestDelegate, mockLogger, mockHostEnvironment);
        var mockContext = new DefaultHttpContext();
        
        //Act
        await subject.InvokeAsync(mockContext);
        
        //Arrange
        mockContext.Response.ContentType.ShouldBe("application/json");
    }

    [Fact]
    public async void InvokeAsync_WhenException_StatusCodeShouldBeFiveHundred()
    {
        //Arrange
        var mockLogger = Substitute.For<ILogger<ExceptionMiddleware>>();
        var mockHostEnvironment = Substitute.For<IHostEnvironment>();
        var mockRequestDelegate = Substitute.For<RequestDelegate>();
        mockRequestDelegate.Invoke(Arg.Any<HttpContext>()).Throws(new Exception("A test error"));
        var subject = new ExceptionMiddleware(mockRequestDelegate, mockLogger, mockHostEnvironment);
        var mockContext = new DefaultHttpContext();
        
        //Act
        await subject.InvokeAsync(mockContext);
        
        //Arrange
        mockContext.Response.StatusCode.ShouldBe(500);
    }
}