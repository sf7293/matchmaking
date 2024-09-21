using FluentAssertions;
using Xunit;

namespace Rovio.MatchMaking.Tests;

public class SessionFactoryTests
{
    private readonly SessionFactory _sessionFactory = new(); 
    
    [Fact]
    // MethodName_StateUnderTest_ExpectedBehavior
    public async Task Creat_CreateANewSession_NewSessionIsCreated()
    {
        // 1. Arrange
        
        // 2. Act
        var result = _sessionFactory.Create();

        // 3. Assert
        result.Should().NotBeNull();
    }
}