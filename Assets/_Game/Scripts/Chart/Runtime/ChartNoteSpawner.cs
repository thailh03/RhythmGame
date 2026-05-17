using System.Collections.Generic;
using UnityEngine;

public class ChartNoteSpawner : MonoBehaviour
{
    [Header("Chart")]
    [SerializeField] private string chartFileName = "test_chart";

    [Header("Clock")]
    [SerializeField] private ChartPlaybackClock playbackClock;

    [Header("Spawn")]
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Transform noteParent;
    [SerializeField] private NoteManager noteManager;
    [Header("Layout")]
    [SerializeField] private float laneSpacing = 1.2f;
    [SerializeField] private float spawnY = 3f;

    [Header("Timing")]
    [SerializeField] private float preSpawnTime = 2f;

    [Header("Debug")]
    [SerializeField] private bool logSpawnedNotes = false;
    [SerializeField] private bool logMissingGameplayBridge = false;
    private List<ChartNoteSpawnData> _spawnDataList;
    private int _nextSpawnIndex;
    private bool _isReady;

    private void Start()
    {
        if (!ChartSpawnDataProvider.TryGetChartAndSpawnData(
                chartFileName,
                out ChartData loadedChart,
                out _spawnDataList))
        {
            Debug.LogError("ChartNoteSpawner failed to get chart and spawn data.");
            return;
        }

        if (playbackClock != null)
        {
            playbackClock.SetOffset(loadedChart.offset);
        }
        Debug.Log($"Applied chart offset: {loadedChart.offset}");

        ClearSpawnedNotes();

        _nextSpawnIndex = 0;
        _isReady = true;

        Debug.Log($"ChartNoteSpawner ready. Notes: {_spawnDataList.Count}");
    }

    private void Update()
    {
        if (!_isReady)
        {
            return;
        }

        if (playbackClock == null)
        {
            Debug.LogError("Playback clock is missing.");
            _isReady = false;
            return;
        }

        if (!playbackClock.IsPlaying)
        {
            return;
        }

        SpawnDueNotes(playbackClock.SongTime);
    }

    private void SpawnDueNotes(float songTime)
    {
        while (_nextSpawnIndex < _spawnDataList.Count)
        {
            ChartNoteSpawnData data = _spawnDataList[_nextSpawnIndex];

            float spawnTime = data.hitTime - preSpawnTime;

            if (songTime < spawnTime)
            {
                break;
            }

            SpawnNote(data);
            _nextSpawnIndex++;
        }
    }

    private void SpawnNote(ChartNoteSpawnData data)
    {
        if (notePrefab == null)
        {
            Debug.LogError("Note prefab is missing.");
            return;
        }

        Vector3 position = new Vector3(
            data.laneIndex * laneSpacing,
            spawnY,
            0f
        );

        GameObject noteObject = Instantiate(notePrefab, position, Quaternion.identity, noteParent);
        noteObject.name = $"Note_{data.noteId}_{data.noteType}_Lane{data.laneIndex}_Time{data.hitTime:F2}";

        TryRegisterToNoteManager(noteObject, data);

        if (logSpawnedNotes)
        {
            Debug.Log($"Spawned note ID {data.noteId}. HitTime: {data.hitTime:F2}");
        }

        Debug.Log($"Spawned note ID {data.noteId}. HitTime: {data.hitTime:F2}");
    }
    private void TryRegisterToNoteManager(GameObject noteObject, ChartNoteSpawnData data)
    {
        if (noteManager == null)
        {
            if (logMissingGameplayBridge)
            {
                Debug.Log($"NoteManager is not assigned. Skipped register for note ID {data.noteId}.");
            }

            return;
        }

        NoteBase noteBase = noteObject.GetComponent<NoteBase>();

        if (noteBase == null)
        {
            if (logMissingGameplayBridge)
            {
                Debug.Log($"Note ID {data.noteId} has no NoteBase. Skipped NoteManager register.");
            }

            return;
        }

        noteManager.RegisterNote(noteBase);

        if (logMissingGameplayBridge)
        {
            Debug.Log($"Registered note ID {data.noteId} to NoteManager.");
        }
    }
    private void ClearSpawnedNotes()
    {
        if (noteParent == null)
        {
            return;
        }

        for (int i = noteParent.childCount - 1; i >= 0; i--)
        {
            Destroy(noteParent.GetChild(i).gameObject);
        }
    }
}