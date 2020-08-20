using Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Server
{
    public static class ReflectionHelper
    {
        public static IEnumerable<Assembly> LoadAssemblies(string relativePath)
        {
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            if (!Directory.Exists(folder))
            {
                if (Log.IsEnabled(Serilog.Events.LogEventLevel.Warning))
                {
                    Log.Warning("{0} directory doesn't exist.", folder);
                }
            }

            var loadedAssemblies = new List<Assembly>();
            foreach (string file in Directory.GetFiles(folder))
            {
                try
                {
                    if (file.EndsWith(".dll"))
                    {
                        var assembly = Assembly.LoadFrom(file);
                        AppDomain.CurrentDomain.Load(assembly.GetName());
                        loadedAssemblies.Add(assembly);

                        if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
                        {
                            Log.Information("{0} was loaded.", Path.GetFileName(file));
                        }
                    }
                }
                catch(Exception ex)
                {
                    if (Log.IsEnabled(Serilog.Events.LogEventLevel.Error))
                    {
                        Log.Error(ex, "{0} was not loaded.", Path.GetFileName(file));
                    }
                }
            }
            return loadedAssemblies;
        }

        public static IEnumerable<Type> FindClassesOfType<T>(this IEnumerable<Assembly> assemblies) where T : class
        {
            var originalType = typeof(T);
            return
                from a in assemblies
                from t in a.GetTypes()
                where t.IsClass && !t.IsInterface && !t.IsAbstract && originalType.IsAssignableFrom(t)
                select t;
        }

        public static IEnumerable<IPluginStartup> GetPluginStartups(this IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .FindClassesOfType<IPluginStartup>()
                .Select(startupType => (IPluginStartup)Activator.CreateInstance(startupType))
                .OrderBy(startup => startup.Order);
        }
    }
}
