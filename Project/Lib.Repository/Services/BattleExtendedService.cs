using Lib.Repository.Entities;

namespace Lib.Repository.Services;

public static class BattleExtendedService
{
    public static Battle SimulateBattle(Monster monsterA, Monster monsterB)
    {
        var attacker = DetermineFirstAttacker(monsterA, monsterB);
        var defender = attacker.Id == monsterA.Id ? monsterB : monsterA;

        int attackerHp = attacker.Hp;
        int defenderHp = defender.Hp;

        while (true)
        {
            int damage = Math.Max(1, attacker.Attack - defender.Defense);
            defenderHp -= damage;

            if (defenderHp <= 0)
                break;

            int counterDamage = Math.Max(1, defender.Attack - attacker.Defense);
            attackerHp -= counterDamage;

            if (attackerHp <= 0)
            {
                var temp = attacker;
                attacker = defender;
                break;
            }
        }

        return new Battle
        {
            MonsterA = monsterA.Id,
            MonsterB = monsterB.Id,
            Winner = attacker.Id
        };
    }

    private static Monster DetermineFirstAttacker(Monster m1, Monster m2)
    {
        if (m1.Speed > m2.Speed) return m1;
        if (m2.Speed > m1.Speed) return m2;
        return m1.Attack >= m2.Attack ? m1 : m2;
    }
}
