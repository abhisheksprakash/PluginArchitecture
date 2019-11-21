using System;

namespace Core.Personal.Plugin
{
    public interface IPlugin
    {
        string Name { get; }
        void RunJob();
    }
}