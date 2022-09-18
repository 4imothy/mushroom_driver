using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.SceneManagement;

public class LeaderboardController : MonoBehaviour
{
    bool synced = false;

    private Animator animator;

    public GameObject container;
    public TextMeshProUGUI title;
    public TextMeshProUGUI first;
    public TextMeshProUGUI second;
    public TextMeshProUGUI third;
    public TextMeshProUGUI fourth;
    public TextMeshProUGUI fifth;
    public TextMeshProUGUI sixth;
    public TextMeshProUGUI seventh;
    public TextMeshProUGUI eigth;
    public TextMeshProUGUI ninth;
    public TextMeshProUGUI tenth;

    private string weekly = "MDBoardWeekly";
    private string allTime = "MDBoardAllTime";


    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("/Canvas").GetComponent<Animator>();

        if (GameObject.Find("ServerManager") != null)
            ShowWeeklyScores();
        else
            StartCoroutine(LoginRoutineLeaderboard());
    }

    public void ShowWeeklyScores()
    {
        title.text = "Top Weekly Scores";
        StartCoroutine(FetchWeeklyScoresRoutine());
    }

    public void ShowAllTimeScores()
    {
        title.text = "Top All Time Scores";
        StartCoroutine(FethAllTimeScoresRoutine());
    }

    IEnumerator FetchWeeklyScoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(weekly, 10, 0, (response) =>
        {
            if (response.success)
            {
                int i = 0;
                LootLockerLeaderboardMember[] members = response.items;

                for (; i < members.Length; i++)
                {
                   GetText(i).text = members[i].rank + ". " + members[i].player.name + ": " + members[i].score;
                }
                for(; i< 10; i++)
                {
                    GetText(i).text = i + 1 + ". none";
                }
                done = true;
            }
            else
            {
                Debug.Log("failed");
            }
        });
        yield return new WaitWhile(() => !done);
    }

    IEnumerator FethAllTimeScoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(allTime, 10, 0, (response) =>
        {
            if (response.success)
            {
                int i = 0;
                LootLockerLeaderboardMember[] members = response.items;

                for (; i < members.Length; i++)
                {
                    GetText(i).text = members[i].rank + ". " + members[i].player.name + ": " + members[i].score;
                }
                for (; i < 10; i++)
                {
                    GetText(i).text = i + 1 + ". none";
                }
                done = true;
            }
            else
            {
                Debug.Log("failed");
            }
        });
        yield return new WaitWhile(() => !done);
    }

    private TextMeshProUGUI GetText(int index)
    {
        switch (index)
        {
            case 0:
                return first;
            case 1:
                return second;
            case 2:
                return third;
            case 3:
                return fourth;
            case 4:
                return fifth;
            case 5:
                return sixth;
            case 6:
                return seventh;
            case 7:
                return eigth;
            case 8:
                return ninth;
            case 9:
                return tenth;
        }
        return null;
    }
    IEnumerator LoginRoutineLeaderboard()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("guest session started: " + response.player_id);
                synced = true;
                ShowWeeklyScores();
            }
            else
            {
                Debug.Log("couldn't start session" + response.Error);
                synced = true;
            }
        });
        yield return new WaitWhile(() => !synced);
    }
}
