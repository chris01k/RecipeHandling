using RecipeHandling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace RecipeHandling
{
    class Program
    {
        static void Main(string[] args)
        {

            UnitList UL = new UnitList();

            Console.WriteLine("UnitList");
            Console.WriteLine();
            foreach (Unit ListItem in UL.Units)
                Console.WriteLine(ListItem);

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(UL.GetType());
            x.Serialize(Console.Out, UL);
            ;

            Console.ReadLine();

        }
    }
}

