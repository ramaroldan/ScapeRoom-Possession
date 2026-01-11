using NavKeypad;
using System.Collections.Generic;
using UnityEngine;

public class SceneHints : MonoBehaviour
{
    private RoomHintProvider hintProvider;

    [SerializeField] private string sceneName = "Scene";
    void Start()
    {
        hintProvider = FindObjectOfType<RoomHintProvider>();
        switch (sceneName)
        {
            case "HospitalRoom":
                HospitalRoom();
                break;
            case "Morgue":
                Morgue();
                break;
            case "Museum":
                Museum();
                break;
                // Agregar más escenas y sus pistas aquí si es necesario
        }
    }

    private void HospitalRoom()
    {
        hintProvider.RegisterPuzzleHints(0, nameof(PocketWatchHint), new List<string>
        {
                "hint_patient_decline",
                "hint_nurse_steals",
                "hint_bathroom"
        });

        hintProvider.RegisterPuzzleHints(1, nameof(TvScreenHint), new List<string>
        {
                "hint_hospital_tv_1",
                "hint_hospital_tv_2"
        });

        hintProvider.RegisterPuzzleHints(2, nameof(DrawerLockHint), new List<string>
        {
                "hint_hospital_drawer_1",
                "hint_hospital_drawer_2"
        });

        hintProvider.RegisterPuzzleHints(3, nameof(DripperMainHint), new List<string>
        {
                "hint_hospital_dripper_1",
                "hint_hospital_dripper_2"

        });
    }
    private void Museum()
    {
        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(0, nameof(CriptexCodePanel), new List<string>
        {
               "hint_museum_criptex_1",
               "hint_museum_criptex_2",
        });
        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(1, nameof(SlidingPuzzleManager), new List<string>
        {
                "hint_museum_sliding_1",
                "hint_museum_sliding_2",
        });
        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(2, nameof(DoorLockByPadlock), new List<string>
        {
                "hint_museum_padlock_1",
                "hint_museum_padlock_2",
        });
        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(3, nameof(RitualBoxManager), new List<string>
        {
                "hint_museum_ritual_1",
                "hint_museum_ritual_2",
        });
    }
    private void Morgue()
    {   // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(0, nameof(PcController), new List<string>
        {
                "hint_morgue_pc_1",
                "hint_morgue_pc_2",
                "hint_morgue_pc_3"
        });
        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(1, nameof(PowerBoxCover), new List<string>
            {
                "hint_morgue_powerbox_1",
                "hint_morgue_powerbox_2",

            });

        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(2, nameof(Keypad), new List<string>
            {
                "hint_morgue_keypad_1",
                "hint_morgue_keypad_2",
                "hint_morgue_keypad_3"
            });
    }

}
