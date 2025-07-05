using System.Globalization;
using API.Models;
using CsvHelper;
using Lib.Repository.Entities;
using Lib.Repository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MonsterExtendedController : BaseApiController
{
    private readonly IBattleOfMonstersRepository _repository;

    public MonsterExtendedController(IBattleOfMonstersRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        var monsters = await _repository.Monsters.GetAllAsync();
        return Ok(monsters);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, [FromBody] Monster monster)
    {
        var existing = await _repository.Monsters.FindAsync(id);
        if (existing == null)
        {
            return NotFound($"The monster with ID = {id} not found.");
        }

        existing.Name = monster.Name;
        existing.Attack = monster.Attack;
        existing.Defense = monster.Defense;
        existing.Speed = monster.Speed;
        existing.Hp = monster.Hp;
        existing.ImageUrl = monster.ImageUrl;

        _repository.Monsters.Update(id, existing);
        await _repository.Save();

        return Ok();
    }
}
