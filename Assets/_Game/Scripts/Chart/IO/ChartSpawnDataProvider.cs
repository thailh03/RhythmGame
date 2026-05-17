using System.Collections.Generic;
using UnityEngine;

public static class ChartSpawnDataProvider
{
    public static bool TryGetSpawnData(string fileName, out List<ChartNoteSpawnData> spawnDataList)
    {
        spawnDataList = new List<ChartNoteSpawnData>();

        if (!BeatmapParser.TryLoadChart(fileName, out ChartData loadedChart))
        {
            Debug.LogError($"Failed to get spawn data. Cannot load chart: {fileName}");
            return false;
        }

        spawnDataList = ChartRuntimeConverter.ConvertToSpawnData(loadedChart);

        if (spawnDataList.Count == 0)
        {
            Debug.LogWarning($"Spawn data is empty: {fileName}");
            return false;
        }

        Debug.Log($"Spawn data ready. File: {fileName} | Count: {spawnDataList.Count}");
        return true;
    }
    public static bool TryGetChartAndSpawnData(
    string fileName,
    out ChartData loadedChart,
    out List<ChartNoteSpawnData> spawnDataList)
    {
        loadedChart = null;
        spawnDataList = new List<ChartNoteSpawnData>();

        if (!BeatmapParser.TryLoadChart(fileName, out loadedChart))
        {
            Debug.LogError($"Failed to get chart and spawn data. Cannot load chart: {fileName}");
            return false;
        }

        spawnDataList = ChartRuntimeConverter.ConvertToSpawnData(loadedChart);

        if (spawnDataList.Count == 0)
        {
            Debug.LogWarning($"Spawn data is empty: {fileName}");
            return false;
        }

        Debug.Log($"Chart and spawn data ready. File: {fileName} | Count: {spawnDataList.Count}");
        return true;
    }
}