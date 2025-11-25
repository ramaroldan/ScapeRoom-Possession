using UnityEngine;

namespace PadlockSystem
{
    public class SpinnerScript : MonoBehaviour
    {
        [Header("Padlock Controller Reference")]
        [SerializeField] private PadlockController _padlockController = null;

        [Header("Combination Settings")]
        private int spinnerNumber;
        private int spinnerLimit;

        [Header("Padlock Row")]      
        [SerializeField] private PadlockRow _row = PadlockRow.row1;

        private enum PadlockRow { row1, row2, row3, row4 }

        private void Awake()
        {
            spinnerNumber = 1;
            spinnerLimit = 9;
        }
        private void Start()
        {
            // Si no está asignado manualmente, buscarlo en la escena
            if (_padlockController == null)
            {
                //if ( sceneName == "HospitalRoom")
                //{
                    GameObject padlockGO = GameObject.Find("Padlock_Controller_A1");
                //    break;
                //}
                //if (sceneName == "Morgue")
                //{
                //    GameObject padlockGO = GameObject.Find("Padlock_Controller_A2");
                //    break;
                //}
                //if ( sceneName == "Museum")
                //{
                //    GameObject padlockGO = GameObject.Find("Padlock_Controller_A3");
                //    break;
                //}

                
                if (padlockGO != null)
                {
                    _padlockController = padlockGO.GetComponent<PadlockController>();
                    Debug.Log("<color=green>✔ PadlockController asignado dinámicamente.</color>");
                }
                else
                {
                    Debug.LogError("❌ No se encontró 'Padlock_Controller_A1' en la escena.");
                }
            }
        }
        void OnMouseDown()
        {
            transform.Rotate(0, 0, transform.rotation.z + 40);
            _padlockController.SpinSound();
            Rotate();
        }

        void Rotate()
        {
            if (spinnerNumber <= spinnerLimit - 1)
            {
                spinnerNumber++;
            }
            else
            {
                spinnerNumber = 1;
            }

            switch (_row)
            {
                case PadlockRow.row1:
                    _padlockController.combinationRow1 = spinnerNumber;
                    _padlockController.CheckCombination();
                    break;
                case PadlockRow.row2:
                    _padlockController.combinationRow2 = spinnerNumber;
                    _padlockController.CheckCombination();
                    break;
                case PadlockRow.row3:
                    _padlockController.combinationRow3 = spinnerNumber;
                    _padlockController.CheckCombination();
                    break;
                case PadlockRow.row4:
                    _padlockController.combinationRow4 = spinnerNumber;
                    _padlockController.CheckCombination();
                    break;
            }
        }
    }
}


