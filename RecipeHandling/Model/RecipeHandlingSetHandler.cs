﻿using Jamie.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jamie.Model
{
    public class RecipeHandlingSetHandler
    {
        private RecipeDataSets _JamieDataSet;
        private static string filename = "ObservedUnits.txt";

        //Constructors
        internal RecipeHandlingSetHandler()
        {
            JamieDataSet = new RecipeDataSets();
            OpenLists();
            JamieDataSet.ViewSet();

        }
        internal RecipeHandlingSetHandler(bool ToBePopulatedWithDefaults)
        {
            JamieDataSet = new RecipeDataSets(ToBePopulatedWithDefaults);

        }

        //Properties
        public RecipeDataSets JamieDataSet
        {
            get
            {
                return _JamieDataSet;
            }

            set
            {
                _JamieDataSet = value;
            }
        }

        //Methods
        public void ClearLists()
        {
            JamieDataSet.ClearLists();
        }
        public void OpenLists()
        {
            using (Stream fs = new FileStream(filename, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(JamieDataSet.GetType());
                JamieDataSet = (RecipeDataSets)x.Deserialize(fs);
            }
        }
        public void SaveLists()
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(JamieDataSet.GetType());
                x.Serialize(fs, JamieDataSet);
            }

        }
    }
}