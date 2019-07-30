using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementsScript : MonoBehaviour 
{
    public static AchievementsScript instance;

    #region STRUCTS
    [System.Serializable]
    public class Achieve
    {
        [HideInInspector] public GameObject _UIElement;
        public string wording;
        public int _value;
        public int _coinReward;
        public int _xpReward;
        public bool isHidden;
    }
    #endregion

    #region PUBLIC_VAR
    public List<Achieve> _levelAchievements;
    public List<Achieve> _killAchivements;

    public GameObject _achievementPrefab;
    public GameObject _achievementScroller;
    public LevelUIManager _levelUIManager;
    #endregion

    #region PRIVATE_VAR
    private PlayerInfoScript info;
    private List<Achieve> _completed = new List<Achieve>();
    #endregion

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        info = PlayerInfoScript.instance; 

        if (info == null)
            Debug.LogError("PlayerInfoScript not found");

        int levelIndex = UpdateAchievements(info.GetLevelAchievementIndex(), info.GetLevel(), _levelAchievements, 0);
        int killIndex = UpdateAchievements(info.GetKillAchievementIndex(), info.GetTotalKills(), _killAchivements, 1);

        _achievementScroller.GetComponent<ScrollRect>().verticalNormalizedPosition = 1; //Automatically scroll to the top

        if (!info.HasAchievementBeenLoaded())
        {
            info.SetAchievementLevel(levelIndex);
            info.SetAchievementKillLevel(killIndex);
        }
        
        _levelUIManager.UpdateUIStats();
        OrderAchievements();
    }

    private int UpdateAchievements(int completedIndex, int value, List<Achieve> achievement, int key = 0)
    {
        int xpToAdd = 0;
        int index = 0;
        for (int i = 0; i < achievement.Count; i++)
        {
            if (achievement[i]._UIElement == null)
            {
                GameObject achieve = Instantiate(_achievementPrefab, _achievementScroller.transform.GetChild(0));
                achievement[i]._UIElement = achieve;
                if (achievement[i].isHidden)
                    achieve.GetComponent<TextMeshProUGUI>().text = " HIDDEN";
                else
                    achieve.GetComponent<TextMeshProUGUI>().text = " " + achievement[i].wording;
            }

            if (value >= achievement[i]._value)
            {
                _completed.Add(achievement[i]);

                if (index > completedIndex - 1)
                {
                    print(achievement[i].wording);
                    info.AddCoins(achievement[i]._coinReward);
                    xpToAdd += achievement[i]._xpReward;
                }

                achievement[i]._UIElement.GetComponent<TextMeshProUGUI>().color = Color.green;
                achievement.RemoveAt(i);
                i--;
                index++;
            }
        }

        if (xpToAdd > 0)
        {
            info.AddXp(xpToAdd);
            switch (key)
            {
                case 0:
                    break;
                case 1:
                    UpdateAchievements(info.GetKillAchievementIndex(), info.GetTotalKills(), _killAchivements, 1);
                    break;
                default:
                    Debug.Log("KEY NOT FOUND");
                    break;
            };
            UpdateAchievements(info.GetLevelAchievementIndex(), info.GetLevel(), _levelAchievements, 0);
        }
        return index;
    }

    private void OrderAchievements()
    {
        foreach(Achieve complete in _completed)
        {
            complete._UIElement.transform.SetAsLastSibling();
        }
    }

}
