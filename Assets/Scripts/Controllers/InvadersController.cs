using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace SpaceInvaders.Controllers
{
    public class InvadersController : MonoBehaviour
    {
        [SerializeField] private int _gridRows;
        [SerializeField] private int _gridCols;
        [SerializeField] private float _gridSpacing;
        [SerializeField] private GameObject[] _invaderTypeArray;
        [SerializeField] private int movementStep;
        [SerializeField] private float moveDistanceSide;
        [SerializeField] private float moveDistanceDown;
        private GameObject[] _invadersArray;
        private GameObject[] _invaderRowArray;
        private int _activeInvadersRemaining;
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

            for (int row = 0; row < rows; row++)
            {
                GameObject newRow = new GameObject ($"Invader Row {row}");
                _invaderRowArray[row] = newRow;
            
                for (int col = 0; col < columns; col++)
                {
                    Vector3 spawnPosition = new Vector3(transform.position.x + _gridSpacing * col, transform.position.y + _gridSpacing * row, transform.position.z);
                    GameObject newInvader = SpawnInvader(spawnPosition, invaderIndex, newRow.transform);
                    _invadersArray[col] = newInvader;
                    yield return new WaitForSeconds(.025f);
                    ActivateInvader(newInvader);
                }
                
                invaderIndex++;
            }

            StartCoroutine(StartMoveInterval());

        }

        private GameObject SpawnInvader(Vector3 position, int typeIndex, Transform parent)
        {   
            GameObject newInvader = Instantiate(_invaderTypeArray[typeIndex], position, Quaternion.identity);
            newInvader.transform.SetParent(parent);
            newInvader.SetActive(false);
            return newInvader;
        }

        private void ActivateInvader(GameObject invaderToActivate)
        {
            invaderToActivate.SetActive(true);
            _activeInvadersRemaining++;
        }

        private IEnumerator StartMoveInterval()
        {
            movementStep = 8;
            bool moveRight = true;
            float moveInterval = 60/_activeInvadersRemaining;

            while (true)
            {

                if (moveRight)
                {
                    yield return new WaitForSeconds(moveInterval);

                    foreach (var row in _invaderRowArray)
                    {
                        yield return new WaitForSeconds(.1f);
                        row.transform.position += new Vector3 (moveDistanceSide,0f,0f);
                    } 

                    movementStep++;
                    Debug.Log(movementStep);
                }

                if (!moveRight)
                {
                    yield return new WaitForSeconds(moveInterval);

                    foreach (var row in _invaderRowArray)
                    {
                        yield return new WaitForSeconds(.1f);
                        row.transform.position -= new Vector3 (moveDistanceSide,0f,0f);
                        
                    } 

                    movementStep--;
                    Debug.Log(movementStep);
                }

                if (movementStep == 16)
                {   
                    yield return new WaitForSeconds(moveInterval);
                    
                    foreach (var row in _invaderRowArray)
                    {
                        yield return new WaitForSeconds(.1f);
                        row.transform.position += new Vector3 (0f,-moveDistanceDown,0f);
                    }

                    yield return new WaitForSeconds(moveInterval);

                    foreach (var row in _invaderRowArray)
                    {
                        yield return new WaitForSeconds(.1f);
                        row.transform.position -= new Vector3 (moveDistanceSide,0f,0f);   
                    }

                    moveRight = false; 
                }

                if (movementStep == 0)
                {   
                    yield return new WaitForSeconds(moveInterval);
                    
                    foreach (var row in _invaderRowArray)
                    {
                        yield return new WaitForSeconds(.1f);
                        row.transform.position += new Vector3 (0f,-moveDistanceDown,0f);
                    }

                    yield return new WaitForSeconds(moveInterval);

                    foreach (var row in _invaderRowArray)
                    {
                        yield return new WaitForSeconds(.1f);
                        row.transform.position += new Vector3 (moveDistanceSide,0f,0f);   
                    }

                    moveRight = true; 
                }
                
            }  
        }

    }
}
