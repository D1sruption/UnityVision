using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityVision
{
    public class Loader
    {
        //This class is what injectors use to initialize the hack
        static UnityEngine.GameObject gameObject;

        private static void Load()
        {
            new Main().Load();
        }
    }
}
