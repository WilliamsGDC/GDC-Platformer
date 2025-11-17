using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Level currentLevel;
    public List<string> loadedScenes = new();

    public Queue<Level> levelsToLoadQueue = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void QueueLoadSurroundingLevels(Level level)
    {
        levelsToLoadQueue.Enqueue(level);
        StartCoroutine(ProcessLevels());
    }

    public IEnumerator ProcessLevels()
    {
        while (levelsToLoadQueue.Count > 0)
        {
            Level levelToLoad = levelsToLoadQueue.Dequeue();
            yield return StartCoroutine(LoadSurroundingLevels(levelToLoad));
        }
    }

    public IEnumerator LoadSurroundingLevels(Level level)
    {
        List<string> shouldBeLoaded = new();

        foreach (string scene in level.connectedScenes)
        {
            if (!SceneManager.GetSceneByName(scene).isLoaded)
            {
                // asyncrhonously load to avoid lag / stalling
                AsyncOperation op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                op.allowSceneActivation = true;

                while (!op.isDone) yield return null;
            }

            shouldBeLoaded.Add(scene);

            if (!loadedScenes.Contains(scene)) loadedScenes.Add(scene);
        }

        List<string> scenesToRemove = loadedScenes.FindAll(name => !shouldBeLoaded.Contains(name));

        foreach (string scene in scenesToRemove)
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(scene);

            while (!op.isDone) yield return null;

            loadedScenes.Remove(scene);
        }
    }
}