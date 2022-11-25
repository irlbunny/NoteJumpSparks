using HarmonyLib;
using NoteJumpSparks.Behaviours;
using NoteJumpSparks.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace NoteJumpSparks.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapObjectsInstaller), nameof(BeatmapObjectsInstaller.InstallBindings))]
    internal class BeatmapObjectsInstallerInstallBindings
    {
        private static List<NoteController> _injectedPrefabs = new();

        private static void Prefix(
            GameNoteController ____normalBasicNotePrefab,
            GameNoteController ____proModeNotePrefab,
            GameNoteController ____burstSliderHeadNotePrefab, // ?? What is this?
            BurstSliderGameNoteController ____burstSliderNotePrefab,
            BombNoteController ____bombNotePrefab)
        {
            InjectPrefab(____normalBasicNotePrefab);
            InjectPrefab(____proModeNotePrefab);
            InjectPrefab(____burstSliderHeadNotePrefab); // Trust me, I have no fucking idea what this is. But, we'll inject TrailPS into it anyways.
            InjectPrefab(____burstSliderNotePrefab);
            InjectPrefab(____bombNotePrefab);
        }

        private static void InjectPrefab(NoteController noteController)
        {
            if (!_injectedPrefabs.Contains(noteController))
            {
                Plugin.Log?.Debug($"Injecting TrailPS into {noteController.gameObject.name}.");

                var trailPS = UnityEngine.Object.Instantiate(TrailPSManager.prefab);
                trailPS.transform.SetParent(noteController.transform, true);

                var trailNoteVisuals = noteController.gameObject.AddComponent<TrailNoteVisuals>();
                trailNoteVisuals.noteController = noteController;
                trailNoteVisuals.noteMovement = noteController.gameObject.GetComponent<NoteMovement>();
                trailNoteVisuals.trailPS = trailPS.GetComponent<ParticleSystem>();

                _injectedPrefabs.Add(noteController);
            }
        }
    }
}
