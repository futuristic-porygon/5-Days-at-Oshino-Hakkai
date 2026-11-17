
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<ItemDropper> slots;
    [SerializeField] private ItemDragger piecePrefab;
    [SerializeField] private Transform pieceSpawnPoints;


    [SerializeField] private MicrogameManager microgameManager;
    private int placedPieces;
    private int totalPieces;

    private void Start()
    {
        SpawnPieces();
    }

    private void SpawnPieces()
    {
        List<Transform> availablePositions = new List<Transform>();
        totalPieces = slots.Count;
        foreach (Transform point in pieceSpawnPoints)
        {
            availablePositions.Add(point);
        }

        if (availablePositions.Count < slots.Count)
        {
            Debug.LogError("No hay suficientes puntos de aparicion para las piezas.");
            return;
        }

        foreach (ItemDropper slot in slots)
        {
            List<ItemDropper> matchingSlots = new List<ItemDropper>();

            foreach (ItemDropper otherSlot in slots)
            {
                if (otherSlot.SlotType == slot.SlotType)
                {
                    matchingSlots.Add(otherSlot);
                }
            }

            int randomIndex = Random.Range(0, availablePositions.Count);

            Transform spawnPosition = availablePositions[randomIndex];
            availablePositions.RemoveAt(randomIndex);

            ItemDragger spawnedPiece = Instantiate(
                piecePrefab,
                spawnPosition.position,
                Quaternion.identity
            );

            spawnedPiece.Init(matchingSlots, this);
        }
    }

        public void PiecePlaced()
    {
        placedPieces++;

        if (placedPieces >= totalPieces)
        {
            microgameManager.WinGame();
        }
    }
}

