using API.Controllers;
using FluentAssertions;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace API.Test;

public class BattleExtendedControllerTests
{
    private readonly Mock<IBattleOfMonstersRepository> _repository;

    public BattleExtendedControllerTests()
    {
        _repository = new Mock<IBattleOfMonstersRepository>();
    }

    [Fact]
    public async Task Post_OnNoMonsterFound_When_StartBattle_With_NonexistentMonster()
    {
        _repository.Setup(r => r.Monsters.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                   .ReturnsAsync(new List<Monster>());

        var controller = new BattleExtendedController(_repository.Object);

        var result = await controller.Add(new Battle { MonsterA = 1, MonsterB = 2 });

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterAWinning()
    {
        var monsterA = new Monster { Id = 1, Name = "A", Attack = 10, Defense = 5, Speed = 10, Hp = 10 };
        var monsterB = new Monster { Id = 2, Name = "B", Attack = 1, Defense = 1, Speed = 1, Hp = 10 };

        _repository.Setup(r => r.Monsters.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                   .ReturnsAsync(new List<Monster> { monsterA, monsterB });
        _repository.Setup(r => r.Battles.AddAsync(It.IsAny<Battle>())).ReturnsAsync((EntityEntry<Battle>?)null!);
        _repository.Setup(r => r.Save()).ReturnsAsync(1);

        var controller = new BattleExtendedController(_repository.Object);
        var result = await controller.Add(new Battle { MonsterA = 1, MonsterB = 2 });

        var ok = Assert.IsType<OkObjectResult>(result);
        var battle = Assert.IsType<Battle>(ok.Value);
        Assert.Equal(1, battle.Winner);
    }

    [Fact]
    public async Task Post_OnSuccess_Returns_With_MonsterBWinning()
    {
        var monsterA = new Monster { Id = 1, Name = "A", Attack = 1, Defense = 1, Speed = 1, Hp = 10 };
        var monsterB = new Monster { Id = 2, Name = "B", Attack = 10, Defense = 5, Speed = 10, Hp = 10 };

        _repository.Setup(r => r.Monsters.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                   .ReturnsAsync(new List<Monster> { monsterA, monsterB });
        _repository.Setup(r => r.Battles.AddAsync(It.IsAny<Battle>())).ReturnsAsync((EntityEntry<Battle>?)null!);
        _repository.Setup(r => r.Save()).ReturnsAsync(1);

        var controller = new BattleExtendedController(_repository.Object);
        var result = await controller.Add(new Battle { MonsterA = 1, MonsterB = 2 });

        var ok = Assert.IsType<OkObjectResult>(result);
        var battle = Assert.IsType<Battle>(ok.Value);
        Assert.Equal(2, battle.Winner);
    }

    [Fact]
    public async Task Post_MonsterA_Wins_When_SameSpeed_AndHigherAttack()
    {
        var monsterA = new Monster { Id = 1, Name = "A", Attack = 10, Defense = 1, Speed = 5, Hp = 10 };
        var monsterB = new Monster { Id = 2, Name = "B", Attack = 5, Defense = 1, Speed = 5, Hp = 10 };

        _repository.Setup(r => r.Monsters.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                   .ReturnsAsync(new List<Monster> { monsterA, monsterB });
        _repository.Setup(r => r.Battles.AddAsync(It.IsAny<Battle>())).ReturnsAsync((EntityEntry<Battle>?)null!);
        _repository.Setup(r => r.Save()).ReturnsAsync(1);

        var controller = new BattleExtendedController(_repository.Object);
        var result = await controller.Add(new Battle { MonsterA = 1, MonsterB = 2 });

        var ok = Assert.IsType<OkObjectResult>(result);
        var battle = Assert.IsType<Battle>(ok.Value);
        Assert.Equal(1, battle.Winner);
    }

    [Fact]
    public async Task Post_MonsterB_Wins_When_SameSpeed_AndHigherAttack()
    {
        var monsterA = new Monster { Id = 1, Name = "A", Attack = 5, Defense = 1, Speed = 5, Hp = 10 };
        var monsterB = new Monster { Id = 2, Name = "B", Attack = 10, Defense = 1, Speed = 5, Hp = 10 };

        _repository.Setup(r => r.Monsters.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                   .ReturnsAsync(new List<Monster> { monsterA, monsterB });
        _repository.Setup(r => r.Battles.AddAsync(It.IsAny<Battle>())).ReturnsAsync((EntityEntry<Battle>?)null!);
        _repository.Setup(r => r.Save()).ReturnsAsync(1);

        var controller = new BattleExtendedController(_repository.Object);
        var result = await controller.Add(new Battle { MonsterA = 1, MonsterB = 2 });

        var ok = Assert.IsType<OkObjectResult>(result);
        var battle = Assert.IsType<Battle>(ok.Value);
        Assert.Equal(2, battle.Winner);
    }

    [Fact]
    public async Task Post_MonsterA_Wins_When_SameDefense_AndHigherSpeed()
    {
        var monsterA = new Monster { Id = 1, Name = "A", Attack = 10, Defense = 5, Speed = 10, Hp = 10 };
        var monsterB = new Monster { Id = 2, Name = "B", Attack = 10, Defense = 5, Speed = 5, Hp = 10 };

        _repository.Setup(r => r.Monsters.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                   .ReturnsAsync(new List<Monster> { monsterA, monsterB });
        _repository.Setup(r => r.Battles.AddAsync(It.IsAny<Battle>())).ReturnsAsync((EntityEntry<Battle>?)null!);
        _repository.Setup(r => r.Save()).ReturnsAsync(1);

        var controller = new BattleExtendedController(_repository.Object);
        var result = await controller.Add(new Battle { MonsterA = 1, MonsterB = 2 });

        var ok = Assert.IsType<OkObjectResult>(result);
        var battle = Assert.IsType<Battle>(ok.Value);
        Assert.Equal(1, battle.Winner);
    }

    [Fact]
    public async Task Delete_OnSuccess_RemovesBattle()
    {
        var battle = new Battle { Id = 10 };

        _repository.Setup(r => r.Battles.FindAsync(10)).ReturnsAsync(battle);
        _repository.Setup(r => r.Battles.RemoveAsync(10)).ReturnsAsync((EntityEntry<Battle>?)null!);
        _repository.Setup(r => r.Save()).ReturnsAsync(1);

        var controller = new BattleExtendedController(_repository.Object);
        var result = await controller.Delete(10);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_Returns404_When_BattleNotFound()
    {
        _repository.Setup(r => r.Battles.FindAsync(10)).ReturnsAsync((Battle?)null);

        var controller = new BattleExtendedController(_repository.Object);
        var result = await controller.Delete(10);

        result.Should().BeOfType<NotFoundResult>();
    }
}
