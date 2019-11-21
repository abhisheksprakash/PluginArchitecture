using System;
using Core.Personal.Plugin;

namespace Core.Personal.SamplePlugin
{
    public class SamplePlugin : IPlugin
    {
        public string Name { get { return "SamplePlugin"; } }

        public void RunJob()
        {
            Console.WriteLine("Hello.!! This is a SamplePlugin.");
        }
    }
}
