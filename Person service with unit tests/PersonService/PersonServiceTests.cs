using FluentAssertions;
using Xunit;

namespace PersonService;

public class PersonServiceTests
{
    [Fact]
    public async Task TestAddAndGet()
    {
        var id = Guid.NewGuid();
        const string name = "First Person";
        const int age = 30;

        // Arrange
        var service = new PersonService();
        var person = new Person { Id = id, Name = name, Age = age };

        // Act
        await service.Add(person);
        var actual = await service.Get(id);

        // Assert
        actual.Name.Should().Be(name);
        actual.Age.Should().Be(age);
    }

    [Fact]
    public async Task TestUpdateAge()
    {
        const int originalAge = 30;
        const int newAge = 31;

        // Arrange
        var service = new PersonService();
        var person = new Person { Id = Guid.NewGuid(), Name = "First Person", Age = originalAge };
        await service.Add(person);

        // Act
        await service.UpdateAge(person.Id, newAge);
        var actual = await service.Get(person.Id);

        // Assert
        actual.Name.Should().Be(person.Name);
        actual.Age.Should().Be(newAge);
    }

    [Fact]
    public async Task TestDelete()
    {
        // Arrange
        var service = new PersonService();
        var id = Guid.NewGuid();
        await service.Add(new Person { Id = id, Name = "Person 1", Age = 30 });
        await service.Add(new Person { Id = Guid.NewGuid(), Name = "Person 2", Age = 30 });
        await service.Add(new Person { Id = Guid.NewGuid(), Name = "Person 3", Age = 30 });
        await service.Add(new Person { Id = Guid.NewGuid(), Name = "Person 4", Age = 30 });

        // Precondition check
        var before = await service.GetAll();
        before.Count().Should().Be(4);

        // Act
        await service.Delete(id);

        // Assert
        var after = await service.GetAll();
        after.Count().Should().Be(3);
    }
}