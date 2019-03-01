using MCServerWrapper.Messages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MCServerWrapper.Plugins
{
    internal class PluginLoader
    {
        public Dictionary<IPlugin, bool> plugins = new Dictionary<IPlugin, bool>();
        public IEnumerable<IPlugin> Enabled => plugins.Where(x => x.Value).Select(x => x.Key);
        public IEnumerable<IPlugin> Disabled => plugins.Where(x => !x.Value).Select(x => x.Key);

        public string PluginPath { get; set; }

        private static readonly Color red = Color.FromArgb(0xf4, 0x47, 0x47);

        public PluginLoader(string path)
        {
            PluginPath = path;
        }

        public (int dlls, int loaded, int failed) Load()
        {
            if (!Directory.Exists(PluginPath)) return (0, 0, 0);
            var dlls = Directory.GetFiles(PluginPath).Where(x => Path.GetExtension(x) == ".dll").ToList();
            if (dlls.Count == 0) return (0, 0, 0);

            int success = 0;
            int failure = 0;

            foreach (string dll in dlls)
            {
                Assembly assembly = Assembly.Load(File.ReadAllBytes(Path.GetFullPath(dll)));
                var pluginTypes = assembly.GetTypes().Where(x => typeof(IPlugin).IsAssignableFrom(x));
                foreach (Type t in pluginTypes)
                {
                    try
                    {
                        plugins.Add((IPlugin)Activator.CreateInstance(t), true);
                        success++;
                    }
                    catch
                    {
                        failure++;
                    }
                }
            }

            return (dlls.Count, success, failure);
        }

        public void Unload()
        {
            plugins.Clear();
        }

        public (int dlls, int loaded, int failed) Reload()
        {
            Unload();
            return Load();
        }

        public void EnableAll()
        {
            foreach (IPlugin p in plugins.Keys)
            {
                plugins[p] = true;
            }
        }

        public void DisableAll()
        {
            foreach (IPlugin p in plugins.Keys)
            {
                plugins[p] = false;
            }
        }

        public void Enable(IPlugin plugin)
        {
            if (plugins.ContainsKey(plugin))
            {
                plugins[plugin] = true;
            }
        }

        public void Disable(IPlugin plugin)
        {
            if (plugins.ContainsKey(plugin))
            {
                plugins[plugin] = false;
            }
        }

        /// <summary>
        /// Called when the server starts
        /// </summary>
        /// <param name="console">The server GUI - provides access to the underlying server process</param>
        public void OnStart(IServerConsole console)
        {
            foreach (IPlugin plugin in Enabled)
            {
                try
                {
                    plugin.OnStart(console);
                }
                catch (Exception ex)
                {
                    console.DisplayLine($"Error in method OnStart for plugin {plugin.Name}: {ex}", red);
                }
            }
        }

        /// <summary>
        /// Called when the server exits
        /// </summary>
        /// <param name="console">The server GUI - provides access to the underlying server process</param>
        public void OnExit(IServerConsole console)
        {
            foreach (IPlugin plugin in Enabled)
            {
                try
                {
                    plugin.OnExit(console);
                }
                catch (Exception ex)
                {
                    console.DisplayLine($"Error in method OnExit for plugin {plugin.Name}: {ex}", red);
                }
            }
        }

        /// <summary>
        /// Called when a chat message is sent
        /// </summary>
        /// <param name="console">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The chat message</param>
        public void OnChatMessage(IServerConsole console, ServerChatMessage message)
        {
            foreach (IPlugin plugin in Enabled)
            {
                try
                {
                    plugin.OnChatMessage(console, message);
                }
                catch (Exception ex)
                {
                    console.DisplayLine($"Error in method OnChatMessage for plugin {plugin.Name}: {ex}", red);
                }
            }
        }

        /// <summary>
        /// Called when the server receives an error message
        /// </summary>
        /// <param name="console">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The error message</param>
        public void OnErrorMessage(IServerConsole console, ServerErrorMessage message)
        {
            foreach (IPlugin plugin in Enabled)
            {
                try
                {
                    plugin.OnErrorMessage(console, message);
                }
                catch (Exception ex)
                {
                    console.DisplayLine($"Error in method OnErrorMessage for plugin {plugin.Name}: {ex}", red);
                }
            }
        }

        /// <summary>
        /// Called when the server receives a success message
        /// </summary>
        /// <param name="console">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The success message</param>
        public void OnSuccessMessage(IServerConsole console, ServerSuccessMessage message)
        {
            foreach (IPlugin plugin in Enabled)
            {
                try
                {
                    plugin.OnSuccessMessage(console, message);
                }
                catch (Exception ex)
                {
                    console.DisplayLine($"Error in method OnSuccessMessage for plugin {plugin.Name}: {ex}", red);
                }
            }
        }

        /// <summary>
        /// Called when the server receives a message that isn't otherwise categorized
        /// </summary>
        /// <param name="console">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The uncategorized message</param>
        public void OnOtherMessage(IServerConsole console, ServerMessage message)
        {
            foreach (IPlugin plugin in Enabled)
            {
                try
                {
                    plugin.OnOtherMessage(console, message);
                }
                catch (Exception ex)
                {
                    console.DisplayLine($"Error in method OnOtherMessage for plugin {plugin.Name}: {ex}", red);
                }
            }
        }

        /// <summary>
        /// Called when a player joins the server
        /// </summary>
        /// <param name="console">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The connection message</param>
        public void OnPlayerConnect(IServerConsole console, ServerConnectionMessage message)
        {
            foreach (IPlugin plugin in Enabled)
            {
                try
                {
                    plugin.OnPlayerConnect(console, message);
                }
                catch (Exception ex)
                {
                    console.DisplayLine($"Error in method OnPlayerConnect for plugin {plugin.Name}: {ex}", red);
                }
            }
        }

        /// <summary>
        /// Called when a player disconnects from the server
        /// </summary>
        /// <param name="console">The server GUI - provides access to the underlying server process</param>
        /// <param name="message">The disconnection message</param>
        public void OnPlayerDisconnect(IServerConsole console, ServerConnectionMessage message)
        {
            foreach (IPlugin plugin in Enabled)
            {
                try
                {
                    plugin.OnPlayerDisconnect(console, message);
                }
                catch (Exception ex)
                {
                    console.DisplayLine($"Error in method OnPlayerDisconnect for plugin {plugin.Name}: {ex}", red);
                }
            }
        }
    }
}
