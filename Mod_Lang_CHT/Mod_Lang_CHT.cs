using ICities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColossalFramework.Plugins;
using System.Reflection;
using System.Diagnostics;

namespace Mod_Lang_CHT
{
    public class Mod_Lang_CHT : IUserMod
    {
        private string locale_name = "zh-tw";

        public string Name
        {
            get {
                try
                {
                    Assembly asm = Assembly.GetExecutingAssembly();
                    Stream st = asm.GetManifestResourceStream(asm.GetName().Name+"."+locale_name+".locale");

#if (DEBUG)
                        DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("File size: {0}", st.Length));
#endif
                    FileStream dst = File.OpenWrite("Files\\Locale\\"+locale_name+".locale");

                    byte[] buffer = new byte[8 * 1024];
                    int len=0;
                    while ((len = st.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        dst.Write(buffer, 0, len);
                    }
                    dst.Close();
                    st.Close();

#if (DEBUG)
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("File write to: {0}", Path.GetFullPath(dst.Name)));
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("Current Language: {0}", ColossalFramework.Globalization.LocaleManager.defaultLanguage));
#endif
                    ColossalFramework.Globalization.LocaleManager.ForceReload();

                    string[] locales = ColossalFramework.Globalization.LocaleManager.instance.supportedLocaleIDs;
                    for (int i = 0; i < locales.Length; i++)
                    {
#if (DEBUG)
                            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("Locale index: {0}, ID: {1}", i, locales[i]));
#endif
                        if (locales[i].Equals(locale_name))
                        {
#if (DEBUG)
                                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("Find locale {0} at index: {1}", locale_name, i));
#endif
                            ColossalFramework.Globalization.LocaleManager.instance.LoadLocaleByIndex(i);
                        }
                    }
#if (DEBUG)
                        DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, String.Format("Post Setting Language: {0}", ColossalFramework.Globalization.LocaleManager.defaultLanguage));
#endif
                }
                catch (Exception e)
                {
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, e.ToString());
                }
                return "Chinese Traditional localization mod"; 
            }
        }

        public string Description
        {
            get { return "Here is where I define my mod"; }
        }
    }
}
