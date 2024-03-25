using Unity.Mathematics;
using UnityEngine;

namespace SpaceInvaders.Controllers
{
    public class InvadersController : MonoBehaviour
    {
        [SerializeField] private int _maxRows;
        [SerializeField] private int _maxCols;
        [SerializeField] private GameObject[] _invaderRowsArray;
        [SerializeField] private float _rowSpacing;
        [SerializeField] private float _colSpacing;
        private GameObject[] _invaderControllersRemaining;
        private int _invaderTypeIndex;
        
        private void OnEnable()
        {
            SpawnInvaders();
        }

        private void SpawnInvaders()
        {

            _invaderControllersRemaining = new GameObject[_maxRows * _maxCols];
            
            if (_invaderTypeIndex < _maxRows)
            {
                for (int row = 0; row < _maxRows; row++)
                {
                GameObject newRow = new GameObject($"InvaderRow{row}");
                newRow.transform.SetParent(transform);
                newRow.transform.position = transform.position + new Vector3(0f,_rowSpacing * row,0f);                

                    for (int col = 0; col < _maxCols; col++)
                    {
                        GameObject newInvader = Instantiate(_invaderRowsArray[_invaderTypeIndex],transform.position,quaternion.identity, transform.parent);
                        newInvader.transform.SetParent(newRow.transform);
                        newInvader.transform.position = newRow.transform.position + new Vector3(_colSpacing * col, 0f, 0f);
            
                        _invaderControllersRemaining[col] = newInvader;
                    }
                
                _invaderTypeIndex++;

                }   
            }
            Debug.Log("Number of spawned invaders: " + _invaderControllersRemaining.Length);
        }
       
    }
}
