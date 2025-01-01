using System.Data.Common;

namespace AdventOfCode.AoC2015;

public class Day22
{
    [Solution(2015,22,1, false)]
    [Solution(2015,22,2, true)]
    public int Part1(string[] input, bool hardMode)
    {
        var bossHitPoints = int.Parse(input[0].Split(": ")[1]);
        var bossDamage = int.Parse(input[1].Split(": ")[1]);

        var minManaSpent = int.MaxValue;

        nextMove(new GameState(bossHitPoints, bossDamage), true);

        return minManaSpent;

        void nextMove(GameState gameState, bool isWizardMove)
        {
            gameState.tickEffects();

            if (hardMode && isWizardMove)
            {
                gameState.HitPoints--;
            }

            if (gameState.wizardWon())
            {
                if (gameState.ManaSpent < minManaSpent)
                {
                    minManaSpent = gameState.ManaSpent;
                }
                return;
            }
            if (gameState.wizardDead() || gameState.ManaSpent > minManaSpent)
            {
                return;
            }

            if (isWizardMove)
            {
                var nextState = gameState;
                nextState.missiles();
                nextMove(nextState, false);
                nextState = gameState;
                nextState.drain();
                nextMove(nextState, false);
                if (gameState.ShieldTurns == 0)
                {
                    nextState = gameState;
                    nextState.shield();
                    nextMove(nextState, false);
                }
                if (gameState.PoisonTurns == 0)
                {                    
                    nextState = gameState;
                    nextState.poison();
                    nextMove(nextState, false);
                }
                if (gameState.RegenTurns == 0)
                {                    
                    nextState = gameState;
                    nextState.regen();
                    nextMove(nextState, false);
                }
                
            }
            else
            {
                gameState.bossTurn();
                nextMove(gameState, true);
            }
        }

    }


    public record struct GameState (int BossHitPoints, int BossDamage, int HitPoints = 50, int Mana = 500, int PoisonTurns = 0, int RegenTurns = 0, int ShieldTurns = 0, int ManaSpent = 0)
    {
        public bool wizardWon()
        {
            return BossHitPoints <= 0 && Mana >= 0 && HitPoints > 0;
        }

        public bool wizardDead()
        {
            return Mana <= 0 || HitPoints <=0;
        }

        public void tickEffects()
        {
            if (PoisonTurns > 0)
            {
                PoisonTurns--;
                BossHitPoints -=3;
            }
            if (ShieldTurns > 0)
            {
                ShieldTurns--;
            }
            if (RegenTurns > 0)
            {
                Mana += 101;
                RegenTurns--;
            }
        }

        public void bossTurn()
        {
            HitPoints -= BossDamage - (ShieldTurns > 0 ? 7 : 0);            
        }

        private void spendMana(int amount)
        {
            Mana -= amount;
            ManaSpent += amount;
            if (Mana < 0) Mana = -10000;
        }

        public void missiles()
        {
            spendMana(53);
            BossHitPoints -= 4;
        }

        public void drain()
        {
            spendMana(73);
            HitPoints += 2;
            BossHitPoints -=2;
        }

        public void shield()
        {
            spendMana(113);            
            ShieldTurns = 6;
        }

        public void poison()
        {
            spendMana(173);
            PoisonTurns = 6;
        }

        public void regen()
        {
            spendMana(229);
            RegenTurns = 5;
        }
    }

}