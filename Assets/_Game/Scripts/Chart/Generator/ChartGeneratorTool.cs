using UnityEngine;

public class ChartGeneratorTool : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] private ChartGenerationSettings generationSettings;
    [SerializeField] private AudioSource musicSource;

    [Header("Save")]
    [SerializeField] private string saveFileName = "test_chart";

    [Header("Preview Optional")]
    [SerializeField] private ChartVisualizer visualizer;

    public void GenerateAndSave()
    {
        if (generationSettings == null)
        {
            Debug.LogError("Generation settings is missing.");
            return;
        }

        if (musicSource == null || musicSource.clip == null)
        {
            Debug.LogError("Music source or AudioClip is missing.");
            return;
        }

        generationSettings.ApplyPreset();

        string songName = musicSource.clip.name;
        float songLength = musicSource.clip.length;

        ChartData chart = SimpleChartGenerator.Generate(
            songName,
            generationSettings.bpm,
            songLength,
            generationSettings.offset,
            generationSettings.laneCount,
            generationSettings.subdivision,
            generationSettings.noteChance
        );

        ChartSaveLoad.Save(chart, saveFileName);

        Debug.Log(
            $"Generated and saved chart: {chart.songName} | " +
            $"Notes: {chart.notes.Count} | " +
            $"BPM: {chart.bpm} | " +
            $"Offset: {chart.offset}"
        );

        if (visualizer != null)
        {
            visualizer.Draw(chart);
        }
    }
    public void LoadAndPreview()
    {
        if (visualizer == null)
        {
            Debug.LogError("Visualizer is missing.");
            return;
        }

        if (!BeatmapParser.TryLoadChart(saveFileName, out ChartData loadedChart))
        {
            Debug.LogError($"Failed to load chart preview: {saveFileName}");
            return;
        }

        visualizer.Draw(loadedChart);

        Debug.Log(
            $"Loaded preview chart: {loadedChart.songName} | " +
            $"Notes: {loadedChart.notes.Count} | " +
            $"BPM: {loadedChart.bpm} | " +
            $"Offset: {loadedChart.offset}"
        );
    }
}