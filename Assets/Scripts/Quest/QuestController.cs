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
    /// <param name="questName"></param>
    /// <exception cref="Exception"></exception>
    public void StartQuest(string questName)
    {
        Quest quest = inactiveQuests.Find(q => q.name == questName);

        if (quest == null) throw new Exception("Quest is not in QuestController"); ;

        inactiveQuests.Remove(quest);
        activeQuests.Add(quest);
        quest.onStart?.Invoke();
    }

    /// <summary>
    /// Marks a quest as complete
    /// </summary>
    /// <param name="questName"></param>
    /// <exception cref="Exception"></exception>
    public void CompleteQuest(string questName)
    {
        Quest quest = activeQuests.Find(q => q.name == questName);

        if (quest == null) throw new Exception("Quest is not in QuestController"); ;
        
        activeQuests.Remove(quest);
        completedQuests.Add(quest);
        quest.onComplete?.Invoke();
    }

    /// <summary>
    /// Returns the status of a quest.
    /// </summary>
    /// <param name="questName"></param>
    /// <returns>Status of quest</returns>
    /// <exception cref="Exception"></exception>
    public QuestStatus GetQuestStatus(string questName)
    {
        if (inactiveQuests.Find(q => q.name == questName) != null) return QuestStatus.Inactive;
        if (activeQuests.Find(q => q.name == questName) != null) return QuestStatus.Active;
        if (completedQuests.Find(q => q.name == questName) != null) return QuestStatus.Completed;

        throw new Exception("Quest is not in QuestController");
    }
}
