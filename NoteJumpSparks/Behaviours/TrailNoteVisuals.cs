using UnityEngine;

namespace NoteJumpSparks.Behaviours
{
    internal class TrailNoteVisuals : MonoBehaviour, INoteControllerDidInitEvent, INoteControllerNoteDidStartJumpEvent, INoteControllerNoteDidStartDissolvingEvent
    {
        private bool _trailPSEmitting;

        public NoteController noteController;
        public NoteMovement noteMovement;
        public ParticleSystem trailPS;

        public bool trailPSEnabled
        {
            get => _trailPSEmitting;
            set
            {
                var emission = trailPS.emission;
                emission.enabled = value;
                _trailPSEmitting = value;
            }
        }

        public virtual void Awake()
        {
            noteController.didInitEvent.Add(this);
            noteController.noteDidStartJumpEvent.Add(this);
            noteController.noteDidStartDissolvingEvent.Add(this);

            trailPSEnabled = false;
        }

        public virtual void OnDestroy()
        {
            if (noteController != null)
            {
                noteController.didInitEvent.Remove(this);
                noteController.noteDidStartJumpEvent.Remove(this);
                noteController.noteDidStartDissolvingEvent.Remove(this);
            }
        }

        public void HandleNoteControllerDidInit(NoteControllerBase noteController)
        {
            var main = trailPS.main;
            main.startColor = Color.white;
        }

        public void HandleNoteControllerNoteDidStartJump(NoteController noteController)
        {
            trailPSEnabled = false;
        }

        public void HandleNoteControllerNoteDidStartDissolving(NoteControllerBase noteController, float duration)
        {
            trailPSEnabled = false;
        }

        private void Update()
        {
            if (noteMovement.movementPhase == NoteMovement.MovementPhase.MovingOnTheFloor && !trailPSEnabled && transform.localPosition.z < 30f)
                trailPSEnabled = true;
        }
    }
}
