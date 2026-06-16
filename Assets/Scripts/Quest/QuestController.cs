using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum QuestStatus
{
    Inactive,
    Active,
    Completed
}

[Serializable]
public class Quest
{
    public string name;
    public UnityEvent onStart;
    public UnityEvent onComplete;
}

public class QuestController : MonoBehaviour
{
    public List<Quest> inactiveQuests;
    public List<Quest> activeQuests;
    public List<Quest> completedQuests;

    public static QuestController instance; // Singleton

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Make sure only one instance
        }
    }

    /// <summary>
    /// Starts a quest.
    /// </summary>
    /// <param name="q">Quest to start</param>
    public void StartQuest(Quest q)
    {
        inactiveQuests.Remove(q);
        activeQuests.Add(q);
        q.onStart?.Invoke();
    }

    /// <summary>
    /// Marks a quest as complete.
    /// </summary>
    /// <param name="q"></param>
    public void CompleteQuest(Quest q)
    {
        activeQuests.Remove(q);
        completedQuests.Add(q);
        q.onComplete?.Invoke();
    }

    public QuestStatus GetQuestStatus(Quest q)
    {
        if (inactiveQuests.Contains(q)) return QuestStatus.Inactive;
        if (activeQuests.Contains(q)) return QuestStatus.Active;
        if (completedQuests.Contains(q)) return QuestStatus.Completed;
        throw new Exception("Quest is not in QuestController");
    }
}
