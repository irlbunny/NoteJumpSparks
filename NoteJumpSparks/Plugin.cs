using HarmonyLib;
using IPA;
using IPA.Loader;
using NoteJumpSparks.Installers;
using SiraUtil.Attributes;
using SiraUtil.Zenject;
using System.Reflection;
using IPALogger = IPA.Logging.Logger;

namespace NoteJumpSparks
{
    [Plugin(RuntimeOptions.DynamicInit), Slog]
    public class Plugin
    {
        internal const string HARMONYID = "com.github.ItsKaitlyn03.NoteJumpSparks";

        internal static IPALogger Log { get; private set; }
        internal static Harmony HarmonyInstance { get; private set; } = new(HARMONYID);

        [Init]
        public Plugin(IPALogger logger, PluginMetadata metadata, Zenjector zenjector)
        {
            Log = logger;

            zenjector.UseLogger(logger);

            zenjector.Install(Location.App, container =>
            {
                container.BindInstance(new UBinder<Plugin, PluginMetadata>(metadata));
            });

            zenjector.Install<NJSMenuInstaller>(Location.Menu);
        }

        [OnEnable]
        public void OnEnable()
        {
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnDisable]
        public void OnDisable()
        {
            HarmonyInstance.UnpatchSelf();
        }
    }
}
