using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using Core.Personal.Plugin;
using System.Reflection;

namespace Core.Personal
{
    public partial class Think
    {
        private int TimerCount { get; set; }
        public Dictionary<string, IPlugin> Plugins { get; private set; }
        public Timer ThinkTimer { get; set; }
        
        private readonly string PluginLocation = AppDomain.CurrentDomain.BaseDirectory;        

        public Think()
        {

        }

        public void Poll(object state)
        {
            try
            {
                var autoEvent = (AutoResetEvent)state;
                
                // test to make sure the constant ping is working
                WriteToFile(string.Format("Callback called {0}.{1}", TimerCount, Environment.NewLine));

                // load all the plugins that implement IPlugin
                // TODO: for now i have added a SamplePlugin as reference for test, should remove it.
                LoadPlugins();

                // TODO: create the threadpools for each plugin
                CreateThreadPoolsForPlugins();

                // check if there are new jobs in the DB for each plugin loaded, 
                // if there is; then run it using RunJob().
                // CheckForNewJobs();

                ThinkTimer.Change(5000, 1000);
                TimerCount++;                

                // for now the timer is set to expire after 2 runs. we can set it to infinite.
                if (TimerCount == 1)
                {                    
                    autoEvent.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        private void CreateThreadPoolsForPlugins()
        {
            foreach (var plugin in Plugins)
            {
                var threadCount = ConfigUtil.Get<int>(string.Format("{0}.ThreadCount", plugin.Key));
            }
        }

        private void WriteToFile(string text)
        {
            try
            {
                File.AppendAllText(string.Format(@"{0}output.txt", AppDomain.CurrentDomain.BaseDirectory), text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void LoadPlugins()
		{
			string[] dllFileNames = null;

			if(Directory.Exists(PluginLocation))
			{
				dllFileNames = Directory.GetFiles(PluginLocation, "*.dll");

				var assemblies = new List<Assembly>(dllFileNames.Length);
				foreach(string dllFile in dllFileNames)
				{
					AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
					Assembly assembly = Assembly.Load(an);
					assemblies.Add(assembly);
				}

				Type pluginType = typeof(IPlugin);
				var pluginTypes = new List<Type>();
				foreach(Assembly assembly in assemblies)
				{
					if(assembly != null)
					{
						Type[] types = assembly.GetTypes();

						foreach(Type type in types)
						{
							if(type.IsInterface || type.IsAbstract)
							{
								continue;
							}
							else
							{
								if(type.GetInterface(pluginType.FullName) != null)
								{
									pluginTypes.Add(type);
								}
							}
						}
					}
				}
				
                Plugins = new Dictionary<string, IPlugin>();
				foreach(Type type in pluginTypes)
				{
					var plugin = (IPlugin)Activator.CreateInstance(type);

                    if(!Plugins.ContainsKey(plugin.Name))
                    {
                        Plugins.Add(plugin.Name, plugin);
                    }
				}
			}
		}
    }
}