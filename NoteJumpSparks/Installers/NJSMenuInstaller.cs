using NoteJumpSparks.Managers;
using Zenject;

namespace NoteJumpSparks.Installers
{
    internal class NJSMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            // Managers
            Container.BindInterfacesAndSelfTo<TrailPSManager>().AsSingle();
        }
    }
}
