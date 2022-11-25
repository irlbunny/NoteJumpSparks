using IPA.Loader;
using SiraUtil.Zenject;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace NoteJumpSparks.Managers
{
    internal class TrailPSManager : IInitializable
    {
        private readonly Assembly _assembly;

        // Yes, I know this is dumb for it to be a static.
        // I didn't know of any other good way to handle this, I'm sorry.
        public static GameObject prefab { get; private set; }

        public TrailPSManager(UBinder<Plugin, PluginMetadata> metadataBinder)
        {
            _assembly = metadataBinder.Value.Assembly;
        }

        public void Initialize()
        {
            if (prefab != null)
                return;

            using var mrs = _assembly.GetManifestResourceStream("NoteJumpSparks.Resources.content");
            var assetBundle = AssetBundle.LoadFromStream(mrs);

            prefab = assetBundle.LoadAsset<GameObject>("assets/prefabs/trailps.prefab");
            var trailPrefabRenderer = prefab.GetComponent<ParticleSystemRenderer>();
            var sparkleMaterial = Resources.FindObjectsOfTypeAll<Material>().Where(x => x.name == "Sparkle").FirstOrDefault();
            trailPrefabRenderer.sharedMaterial = sparkleMaterial;

            assetBundle.Unload(false);
        }
    }
}
