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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAll()
    {
        return Ok();
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Update(int id, [FromBody] Monster monster)
    {
        return NoContent();
    }
}
