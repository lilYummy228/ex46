using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex46
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new Arena();
            List<Elemental> elementals = new List<Elemental>();
            elementals.Add(new FireElemental("Элеменаталь огня", 100, 20, 0));
            elementals.Add(new EarthElemental("Элементаль земли", 140, 20, 10));
            elementals.Add(new WaterElement("Элементаль воды", 80, 20, 0));
            elementals.Add(new WindElemental("Элементаль ветра", 100, 20, 0));
            elementals.Add(new DarkElemenatal("Элементаль тьмы", 100, 30, 0));
            elementals.Add(new LightElemental("Элементаль света", 100, 30, 0));

            for (int i = 0; i < elementals.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                elementals[i].ShowStats();
            }

            Elemental firstElemental = arena.ChooseFirstElemental(elementals);
            Elemental secondElemental = arena.ChooseSecondElemental(elementals);
            
            arena.Fight(firstElemental, secondElemental);

            
        }
    }
    class Arena
    {
        public Elemental ChooseFirstElemental(List<Elemental> elementals)
        {
            int elementalNumber;
            Console.Write("Выберете первого элементаля: ");

            if (int.TryParse(Console.ReadLine(), out elementalNumber))
            {
                if (elementalNumber <= elementals.Count && elementalNumber > 0)
                {
                    Elemental firstElemental = elementals[elementalNumber -1];
                    return firstElemental;
                }
                else
                {
                    Console.WriteLine("Такого элементаля нет...");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Неккоректный ввод...");
                return null;
            }
        }

        public Elemental ChooseSecondElemental(List<Elemental> elementals)
        {
            int elementalNumber;
            Console.Write("Выберете первого элементаля: ");

            if (int.TryParse(Console.ReadLine(), out elementalNumber))
            {
                if (elementalNumber <= elementals.Count && elementalNumber > 0)
                {
                    Elemental secondElemental = elementals[elementalNumber -1];
                    return secondElemental;
                }
                else
                {
                    Console.WriteLine("Такого элементаля нет...");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Неккоректный ввод...");
                return null;
            }
        }

        public void Fight(Elemental firstElemental, Elemental secondElemental)
        {
            Console.Clear();

            while (firstElemental.Health > 0 && secondElemental.Health > 0)
            {
                firstElemental.TakeDamage(secondElemental.Damage);
                secondElemental.TakeDamage(firstElemental.Damage);
                firstElemental.ShowCurrentHealth();
                secondElemental.ShowCurrentHealth();
            }
        }
    }

    class Elemental
    {
        public string Name;
        public int Health;
        public int Damage;
        public int Armor;

        public Elemental(string name, int health, int damage, int armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage - Armor;
        }

        public void ShowStats()
        {
            Console.WriteLine($"{Name}\n" +
                $"Здоровье: {Health}\n" +
                $"Наносимый урон: {Damage}\n" +
                $"Броня: {Armor}\n");
        }

        public void ShowCurrentHealth()
        {
            Console.WriteLine($"{Name}\nHP: {Health}");
        }
    }

    class FireElemental : Elemental
    {
        public FireElemental(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public void BurnArmor(int armor)
        {
            armor -= 20;
        }
    }

    class EarthElemental : Elemental
    {
        public EarthElemental(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public void PutStoneShield()
        {
            Armor += 20;
        }
    }

    class WaterElement : Elemental
    {
        public WaterElement(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public void Heal()
        {
            Health += 20;
        }
    }

    class WindElemental : Elemental
    {
        public WindElemental(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public void Dodge()
        {

        }
    }

    class DarkElemenatal : Elemental
    {
        public DarkElemenatal(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public void TakeLife()
        {

        }
    }

    class LightElemental : Elemental
    {
        public LightElemental(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public void Consecrate()
        {
            Damage += 20;
        }
    }
}
