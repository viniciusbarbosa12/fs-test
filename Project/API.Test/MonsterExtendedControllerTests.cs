using System.Diagnostics;
using API.Controllers;
using API.Test.Fixtures;
using FluentAssertions;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Test;

public class MonsterExtendedControllerTests
{
    private readonly Mock<IBattleOfMonstersRepository> _repository;

    [Fact]
    public async Task Post_OnSuccess_ImportCsvToMonster()
    {
        // @TODO missing implementation
        Assert.True(false);
    }

    [Fact]
    public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Monster()
    {
        // @TODO missing implementation
        Assert.True(false);
    }

    [Fact]
    public async Task Post_BadRequest_ImportCsv_With_Nonexistent_Column()
    {
        // @TODO missing implementation
        Assert.True(false);
    }
}
