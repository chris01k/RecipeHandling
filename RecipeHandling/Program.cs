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


            string MenuInput = "";
            UnitList UL = new UnitList();


            while (MenuInput!="Q")
            {
                Console.WriteLine();
                Console.WriteLine("\nMenü");
                Console.WriteLine("----");

                Console.WriteLine("A Add Unit");
                Console.WriteLine("C Clear Unit List");
                Console.WriteLine("O Read Unit List");
                Console.WriteLine("S Save Unit List");
                Console.WriteLine("V View Unit List");
                Console.WriteLine("X View XML File");
                Console.WriteLine();
                Console.WriteLine("Q Quit");

                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine();

                switch (MenuInput)
                {
                    case "O":
                        UL.OpenList();
                        break;
                    case "S":
                        UL.SaveList();
                        break;
                    case "V":
                        UL.ShowList();
                        break;
                    case "X":
                        UL.ViewXML();
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }
                
            }


            Console.WriteLine("UnitList");


            Console.WriteLine();
            foreach (Unit ListItem in UL.Units)
                Console.WriteLine(ListItem);

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(UL.GetType());
            x.Serialize(Console.Out, UL);




        }
    }
}

