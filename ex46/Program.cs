using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace ex46
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new Arena();
            arena.ShowAllFighters();
            arena.Fight();
        }
    }

    class Arena
    {
        private List<Fighter> _fighters;
        private int _maxHealth;
        private int _maxDamage;

        public Arena()
        {
            _maxHealth = 1000;
            _maxDamage = 100;
            _fighters = new List<Fighter>
            {
                new Warlock("Чернокнижник", _maxHealth, _maxDamage, _maxDamage/5),
                new Rogue("Разбойник", _maxHealth, _maxDamage, _maxDamage*2),
                new Warrior("Воин", _maxHealth, _maxDamage, _maxDamage/2),
                new Paladin("Паладин", _maxHealth, _maxDamage),
                new Mage("Маг", _maxHealth, _maxDamage),
                new Hunter("Охотник", _maxHealth, _maxDamage),
                new Shaman("Шаман", _maxHealth, _maxDamage),
                new Druid("Друид", _maxHealth, _maxDamage),
                new Priest("Жрец", _maxHealth, _maxDamage)
            };  
        }

        public void ShowAllFighters()
        {
            int index = 1;

            foreach (Fighter fighter in _fighters)
            {
                Console.Write($"{index}. ");
                fighter.ShowStats();
                index++;
            }
        }

        public Fighter ChooseFirstFighter()
        {
            Console.Write("Выберете первого бойца: ");

            if (int.TryParse(Console.ReadLine(), out int fighterIndex))
            {
                Fighter firstFighter = _fighters[fighterIndex - 1];
                return firstFighter;
            }

            return null;
        }

        public Fighter ChooseSecondFighter()
        {
            Console.Write("Выберете второго бойца: ");

            if (int.TryParse(Console.ReadLine(), out int fighterIndex))
            {
                Fighter secondFighter = _fighters[fighterIndex - 1];
                return secondFighter;
            }

            return null;
        }

        public void Fight()
        {
            Fighter firstFighter = ChooseFirstFighter();
            Fighter secondFighter = ChooseSecondFighter();

            while(firstFighter.CurrentHealth > 0 && secondFighter.CurrentHealth > 0)
            {
                Console.Clear();
                firstFighter.TakeDamage(secondFighter.Damage);
                secondFighter.TakeDamage(firstFighter.Damage);
                firstFighter.ShowCurrentHealth();
                Console.WriteLine();
                secondFighter.ShowCurrentHealth();
                Console.ReadKey();
            }
        }
    }

    class Fighter
    {
        public Fighter(string name, int health, int damage)
        {
            Name = name;
            MaxHealth = health;
            CurrentHealth = health;
            Damage = damage;
        }

        public string Name { get; private set; }
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; private set; }
        public int Damage { get; set; }

        public virtual void ShowStats()
        {
            Console.WriteLine($"{Name}\nЗдоровье: {CurrentHealth}\nУрон: {Damage}");
        }

        public void ShowCurrentHealth()
        {
            Console.WriteLine($"{Name}\nЗдоровье: {CurrentHealth}");
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }
    }

    class Warlock : Fighter
    {
        public Warlock(string name, int health, int damage, int lifesteal) : base(name, health, damage)
        {
            Lifesteal = lifesteal;
        }

        public int Lifesteal { get; private set; }

        public void StealLife()
        {
            CurrentHealth += Lifesteal;

            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} имеет способность высасывать жизненную энергию из врага при нанесении урона\n");
        }
    }

    class Rogue : Fighter
    {
        public Rogue(string name, int health, int damage, int criticalDamage) : base(name, health, damage)
        {
            CriticalDamage = criticalDamage;
        }

        public int CriticalDamage { get; private set; }

        public void DealCriticalDamage()
        {
            Random random = new Random();
            int chance = random.Next(5); //20%

            if (chance == 0)
                Damage = CriticalDamage;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} часто опережает своего противника, получая преимущество в бою с ними в виде шанса на нанесение двойного урона при атаке\n");
        }
    }

    class Warrior : Fighter
    {
        public Warrior(string name, int health, int damage, int armor) : base(name, health, damage)
        {
            Armor = armor;
        }

        public int Armor { get; private set; }

        public void BlockDamage(int damage)
        {
            damage -= Armor;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Благодаря своим тяжелым доспехам и военному мастерству, {Name} меньше получает урона от противников\n");
        }
    }

    class Paladin : Fighter
    {
        public Paladin(string name, int health, int damage) : base(name, health, damage) { }

        public void EvadeDamage(int damage)
        {
            Random random = new Random();
            int chance = random.Next(20); //5%

            if (chance == 0)
                damage = 0;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} имеет шанс полностью заблокировать атаку противника благодаря божественному щиту\n");
        }
    }

    class Mage : Fighter
    {
        public Mage(string name, int health, int damage) : base(name, health, damage)
        {
            HitCount = 0;
        }

        public int HitCount { get; private set; }

        public void BurnEnemy()
        {
            HitCount++;

            for (int i = 0; i < HitCount; i++)
                Damage += Damage / 10;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} способен держать в страхе своих врагов, не желающих быть сожженными дотла\n");
        }
    }

    class Hunter : Fighter
    {
        public Hunter(string name, int health, int damage) : base(name, health, damage) { }

        public void HitToWounds(int health, int maxHealth)
        {
            int extraDamage = health / 8;

            if (health <= maxHealth / 2)
                IncreaseDamage(extraDamage);
            else if (health <= maxHealth / 3)
                IncreaseDamage(extraDamage);
            else if (health <= maxHealth / 4)
                IncreaseDamage(extraDamage);
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"У {Name}а меткий не только выстрел, но и взор, благодаря которому он может находить слабые места противников и пользоваться этим\n");
        }

        private void IncreaseDamage(int extraDamage)
        {
            Damage += extraDamage;
        }
    }

    class Shaman : Fighter
    {
        public Shaman(string name, int health, int damage) : base(name, health, damage) { }

        public void GetSpontaneousEffect()
        {
            Random random = new Random();
            int chance = random.Next(4); //25%
            int randomValue = random.Next(MaxHealth / 20, MaxHealth / 10);

            if (chance == 0)
                CurrentHealth += randomValue;
            else if (chance == 1)
                Damage += randomValue;
            else if (chance == 2)
                CurrentHealth -= randomValue;
            else if (chance == 3)
                Damage -= randomValue;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Силы стихий часто несут за собой такие же стихийные последствия для {Name}а, чем он и пользуется \n");
        }
    }

    class Druid : Fighter
    {
        public Druid(string name, int health, int damage) : base(name, health, damage) { }

        public void HealYourself()
        {
            if (CurrentHealth <= MaxHealth / 2)
                CurrentHealth += CurrentHealth / 6;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Связь {Name}а с лесом помогает ему оставаться невредимым даже на поле сражений\n");
        }
    }

    class Priest : Fighter
    {
        public Priest(string name, int health, int damage) : base(name, health, damage) { }

        public void RiseAgain()
        {
            if (CurrentHealth <= 0)
                CurrentHealth += MaxHealth / 5;
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"Достойный {Name}, примкнувший к силам света, никогда не оставит ни себя, ни своих союзников умирать на поле боя\n");
        }
    }
}
