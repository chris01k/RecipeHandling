using RecipeHandling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace RecipeHandling
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Unit> UnitList = new List<Unit> (30);

            UnitList.Add(new Unit("Kilogramm", "kg", "Masse"));
            UnitList.Add(new Unit("Liter", "l", "Volumen"));
            UnitList.Add(new Unit("Stück", "st", "Anzahl"));
            UnitList.Add(new Unit("Milliliter", "ml", "Volumen"));
            UnitList.Add(new Unit("Meter", "m", "Länge"));

            UnitList.Remove((Unit) UnitList[4]);

            UnitList.Add(new Unit("Milliliter", "ml", "Volumen"));


            Console.WriteLine("UnitList");
            Console.WriteLine();
            foreach (Unit ListItem in UnitList)
                Console.WriteLine(ListItem);

            UnitList.Remove(new Unit("", "st", ""));


            Console.WriteLine("UnitList");
            Console.WriteLine();
            foreach (Unit ListItem in UnitList)
                Console.WriteLine(ListItem);



            Console.ReadLine();
        }
    }
}
