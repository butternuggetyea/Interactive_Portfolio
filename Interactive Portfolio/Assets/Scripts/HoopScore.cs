using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;

public class HoopScore : MonoBehaviour
{
    public TMP_Text _scoreDisplay;
    private int _score;
    void Start()
    {
        _score = 0;
        LoadScore();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            _score++;
            _scoreDisplay.text = _score.ToString();
            SaveScore();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadScore() 
    {
        if (File.Exists(Application.persistentDataPath + "/" + gameObject.name + ".txt"))
        {
            string _json = File.ReadAllText(Application.persistentDataPath + "/" + gameObject.name + ".txt");
            BasketScore score = JsonUtility.FromJson<BasketScore>(_json);
            _score = score.score;

            _scoreDisplay.text = _score.ToString();
        }
    }

    private void SaveScore() 
    {
        BasketScore score = new BasketScore { score = _score, };
        string _json = JsonUtility.ToJson(score);

        File.WriteAllText(Application.persistentDataPath + "/" + gameObject.name + ".txt", _json);
        _score = score.score;
    }

    private class BasketScore
    {
        public int score;
    }

}
