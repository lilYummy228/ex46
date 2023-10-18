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
                new Rogue("Разбойник", _maxHealth, _maxDamage),
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

        public void Fight()
        {
            Fighter firstFighter = ChooseFirstFighter();
            Fighter secondFighter = ChooseSecondFighter();

            if (firstFighter != secondFighter)
            {
                Console.Clear();
                firstFighter.ShowCurrentHealth();
                Console.WriteLine("\nПротив\n");
                secondFighter.ShowCurrentHealth();
                Console.WriteLine("\nНажмите любую клавишу чтобы начать битву...");
                Console.ReadKey();

                while (firstFighter.CurrentHealth > 0 && secondFighter.CurrentHealth > 0)
                {
                    Console.Clear();

                    firstFighter.TakeDamage(secondFighter.Damage);
                    firstFighter.UseAbility(secondFighter.Damage, secondFighter.MaxHealth, secondFighter.CurrentHealth);

                    secondFighter.TakeDamage(firstFighter.Damage);
                    secondFighter.UseAbility(firstFighter.Damage, firstFighter.MaxHealth, secondFighter.CurrentHealth);

                    firstFighter.ShowCurrentHealth();
                    firstFighter.ShowChangingsInHealth(secondFighter.Damage);
                    Console.WriteLine();

                    secondFighter.ShowCurrentHealth();
                    secondFighter.ShowChangingsInHealth(firstFighter.Damage);
                    Console.WriteLine();

                    if (firstFighter.CurrentHealth <= 0 && secondFighter.CurrentHealth <= 0)
                        Console.WriteLine("Ничья");
                    else if (firstFighter.CurrentHealth <= 0 && secondFighter.CurrentHealth > 0)
                        Console.WriteLine($"Победа за {secondFighter.Name}ом");
                    else if (firstFighter.CurrentHealth > 0 && secondFighter.CurrentHealth <= 0)
                        Console.WriteLine($"Победа за {firstFighter.Name}ом");

                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Двух одиннаковых бойцов выбрать нельзя");
            }

            Console.ReadKey();
            Console.Clear();
        }

        private Fighter ChooseFirstFighter()
        {
            Console.Write("Выберете первого бойца: ");

            if (int.TryParse(Console.ReadLine(), out int fighterIndex))
            {
                Fighter firstFighter = _fighters[fighterIndex - 1];
                return firstFighter;
            }

            return null;
        }

        private Fighter ChooseSecondFighter()
        {
            Console.Write("Выберете второго бойца: ");

            if (int.TryParse(Console.ReadLine(), out int fighterIndex))
            {
                Fighter secondFighter = _fighters[fighterIndex - 1];
                return secondFighter;
            }

            return null;
        }
    }

    class Fighter
    {
        public string Name;
        public int CurrentHealth;
        public int MaxHealth;
        public int Damage;

        public Fighter(string name, int health, int damage)
        {
            Name = name;
            MaxHealth = health;
            CurrentHealth = health;
            Damage = damage;
        }

        public virtual void ShowStats()
        {
            Console.WriteLine($"{Name}\nЗдоровье: {CurrentHealth}\nУрон: {Damage}");
        }

        public void ShowCurrentHealth()
        {
            Console.WriteLine($"{Name}\nЗдоровье: {CurrentHealth}");
        }

        public void ShowChangingsInHealth(int damage)
        {
            Console.WriteLine($"Нанесенный урон - {damage}");
        }

        public virtual void UseAbility(int damage, int maxHealth, int currentHealth) { }

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

        public override void UseAbility(int damage, int maxHealth, int currentHealth)
        {
            StealLife();
        }
    }

    class Rogue : Fighter
    {
        public Rogue(string name, int health, int damage) : base(name, health, damage)
        {
            InitialDamage = Damage;
        }

        public int InitialDamage { get; private set; }

        public void DealCriticalDamage()
        {
            int initialDamage = InitialDamage;
            Damage = initialDamage;
            Random random = new Random();
            int chance = random.Next(5); //20%
            int critDamage = Damage * 2;

            if (chance == 0)
                Damage = critDamage;               
        }

        public override void ShowStats()
        {
            base.ShowStats();
            Console.WriteLine($"{Name} часто опережает своего противника, получая преимущество в бою с ними в виде шанса на нанесение двойного урона при атаке\n");
        }

        public override void UseAbility(int damage, int maxHealth, int currentHealth)
        {
            DealCriticalDamage();
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

        public override void UseAbility(int damage, int maxHealth, int currentHealth)
        {
            BlockDamage(damage);
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

        public override void UseAbility(int damage, int maxHealth, int currentHealth)
        {
            EvadeDamage(damage);
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

        public override void UseAbility(int damage, int maxHealth, int currentHealth)
        {
            BurnEnemy();
        }
    }

    class Hunter : Fighter
    {
        public Hunter(string name, int health, int damage) : base(name, health, damage) { }

        public void HitToWounds(int currentHealth, int maxHealth)
        {
            int extraDamage = currentHealth / 8;

            if (currentHealth <= maxHealth / 2)
                IncreaseDamage(extraDamage);
            else if (currentHealth <= maxHealth / 3)
                IncreaseDamage(extraDamage);
            else if (currentHealth <= maxHealth / 4)
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

        public override void UseAbility(int damage, int maxHealth, int currentHealth)
        {
            HitToWounds(currentHealth, maxHealth);
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

        public override void UseAbility(int damage, int maxHealth, int currentHealth)
        {
            GetSpontaneousEffect();
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

        public override void UseAbility(int damage, int maxHealth, int currentHealth)
        {
            HealYourself();
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

        public override void UseAbility(int damage, int maxHealth, int currentHealth)
        {
            RiseAgain();
        }
    }
}
