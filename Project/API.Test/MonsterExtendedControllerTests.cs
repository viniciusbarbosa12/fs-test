using API.Controllers;
using FluentAssertions;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.Test;

public class MonsterExtendedControllerTests
{
    private Mock<IBattleOfMonstersRepository> _repository;

    [Fact]
    public async Task Post_OnSuccess_ImportCsvToMonster()
    {
        _repository = new Mock<IBattleOfMonstersRepository>();
        var csv = "name,attack,defense,speed,hp,imageUrl\nMonster1,10,5,7,100,http://image";

        var file = CreateFakeCsv(csv);

        _repository.Setup(r => r.Monsters.AddAsync(It.IsAny<IEnumerable<Monster>>()))
                   .Returns(Task.CompletedTask);
        _repository.Setup(r => r.Save()).ReturnsAsync(1);

        var controller = new MonsterController(_repository.Object);
        var result = await controller.ImportCsv(file);

        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Monster()
    {
        _repository = new Mock<IBattleOfMonstersRepository>();
        var csv = "name,attack,defense,speed,hp,imageUrl\nMonster1,10,5,7,100,http://image";

        var file = CreateFakeCsv(csv, "monsters.txt"); 

        var controller = new MonsterController(_repository.Object);
        var result = await controller.ImportCsv(file);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Column()
    {
        _repository = new Mock<IBattleOfMonstersRepository>();
        var csv = "wrongcol,attack,defense,speed,hp,imageUrl\n?,10,5,7,100,http://image";

        var file = CreateFakeCsv(csv);

        var controller = new MonsterController(_repository.Object);
        var result = await controller.ImportCsv(file);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    private IFormFile CreateFakeCsv(string content, string fileName = "monsters.csv")
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);
        return new FormFile(stream, 0, bytes.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };
    }
}
