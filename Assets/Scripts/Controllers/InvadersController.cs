using System.Collections;
using UnityEngine;

namespace SpaceInvaders.Controllers
{
    public class InvadersController : MonoBehaviour
    {
        [SerializeField] private int _gridRows;
        [SerializeField] private int _gridCols;
        [SerializeField] private float _gridSpacing;
        [SerializeField] private GameObject[] _invaderTypeArray;
        private GameObject[] _invadersArray;
        private GameObject[] _invaderRowArray;
        [SerializeField] private int _invadersRemaining;
        private int invaderIndex;

        private void Awake()
        {
            _invadersArray = new GameObject[_gridRows * _gridCols];
            _invaderRowArray = new GameObject[_gridRows];
        }

        private void OnEnable()
        {
            StartCoroutine(CreateGrid(_gridRows,_gridCols));
        }

        private IEnumerator CreateGrid(int rows, int columns)
        {
            if (invaderIndex < rows)
            {
                for (int row = 0; row < rows; row++)
                {
                    GameObject newRow = new GameObject ($"Invader Row {row}");
                    _invaderRowArray[row] = newRow;
                
                    for (int col = 0; col < columns; col++)
                    {
                        yield return new WaitForSeconds(.025f);
                        Vector3 spawnPosition = new Vector3(transform.position.x + _gridSpacing * col, transform.position.y + _gridSpacing * row, transform.position.z);
                        GameObject newInvader = SpawnInvader(spawnPosition, invaderIndex, newRow.transform);
                        _invadersArray[col] = newInvader;
                    }
                    
                    invaderIndex++;
                }
            }
            StartCoroutine(StartMoveInterval());
        }

        private GameObject SpawnInvader(Vector3 position, int typeIndex, Transform parent)
        {   
            GameObject newInvader = Instantiate(_invaderTypeArray[typeIndex], position, Quaternion.identity);
            newInvader.transform.SetParent(parent);
            _invadersRemaining++;
            return newInvader;
        }

        private IEnumerator StartMoveInterval()
        {
            float moveInterval = 60/_invadersRemaining;

            while (true)
            {
                yield return new WaitForSeconds(moveInterval);

                foreach (var row in _invaderRowArray)
                {
                    Move(row);
                } 
            }  
        }

        private void Move(GameObject objectToMove)
        {
            objectToMove.transform.position += new Vector3 (.1f,0f,0f);
        }
    }
}
