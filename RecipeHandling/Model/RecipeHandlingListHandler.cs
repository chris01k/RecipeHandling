﻿using Jamie.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jamie.Model
{
    public class RecipeHandlingListHandler
    {
        public RecipeHandlingList RHL;
        private static string filename = "ObservedUnits.txt";
        

        internal RecipeHandlingListHandler()
        {
            RHL = new RecipeHandlingList();
            OpenList();
            RHL.ShowList();

        }

        internal RecipeHandlingListHandler(bool ToBePopulatedWithDefaults)
        {
            RHL = new RecipeHandlingList(ToBePopulatedWithDefaults);

        }

        public void OpenList()
        {
            using (Stream fs = new FileStream(filename, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(RHL.GetType());
                RHL = (RecipeHandlingList)x.Deserialize(fs);
            }
        }

        public void SaveList()
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(RHL.GetType());
                x.Serialize(fs, RHL);
            }

        }



    }
}