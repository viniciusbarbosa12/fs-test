using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Lib.Repository.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BattleExtendedController : BaseApiController
{
    private readonly IBattleOfMonstersRepository _repository;

    public BattleExtendedController(IBattleOfMonstersRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Add([FromBody] Battle battle)
    {
        if (battle == null)
            return BadRequest("Missing ID");

        if (battle?.MonsterA == null || battle?.MonsterB == null)
            return BadRequest("Missing ID");

        var monsterIds = new[] { battle.MonsterA ?? 0, battle.MonsterB ?? 0 };

        var monsters = await _repository.Monsters.GetByIdsAsync(monsterIds);
        if (monsters.Count != 2)
            return BadRequest("One or both monsters not found.");

        var monsterA = monsters.FirstOrDefault(m => m.Id == battle.MonsterA);
        var monsterB = monsters.FirstOrDefault(m => m.Id == battle.MonsterB);

        if (monsterA == null || monsterB == null)
            return BadRequest("One or both monsters not found.");


        var resolvedBattle = BattleExtendedService.SimulateBattle(monsterA, monsterB);

        await _repository.Battles.AddAsync(resolvedBattle);
        await _repository.Save();

        return Ok(resolvedBattle);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var battle = await _repository.Battles.FindAsync(id);
        if (battle == null) return NotFound();

        await _repository.Battles.RemoveAsync(id);
        await _repository.Save();
        return NoContent();
    }
}
