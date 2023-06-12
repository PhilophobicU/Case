using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FarmGrid : MonoBehaviour {

    [SerializeField] private GemProducer tile;
    [SerializeField] private int height = 2;
    [SerializeField] private int width = 2;
    [SerializeField] private float gapSize = 0.1f;
    [SerializeField] private Gem[] gems;
    private List<GemProducer> _gemList = new List<GemProducer>();
    public bool autoUpdate;
    private void Awake() {
        ResetGrid();
    }

    private void SpawnGrid() {
        float tileSize = tile.transform.localScale.x;

        float totalWidth = (width * tileSize) + ((width - 1) * gapSize);
        float totalHeight = (height * tileSize) + ((height - 1) * gapSize);

        float startX = -totalWidth / 2.0f + tileSize / 2.0f;
        float startZ = totalHeight / 2.0f - tileSize / 2.0f;



        for (float y = 0; y < height; y++) {
            for (float x = 0; x < width; x++) {
                Vector3 localSpawnPos = new Vector3(startX + x * (tileSize + gapSize), 0.01f, startZ - y * (tileSize + gapSize));
                GemProducer tileInstance = Instantiate(tile, transform.position + localSpawnPos, tile.transform.rotation, transform);
                _gemList.Add(tileInstance);
            }
        }
    }

    private void Update() {
        foreach (GemProducer gemProducer in _gemList) {
            if (gemProducer.emptyGemTile) {
                gemProducer.SetGem(gems[Random.Range(0, gems.Length)]);
            }
        }
    }

    public void ResetGrid() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
            _gemList.Clear();
        }
        SpawnGrid();
    }
}