using UnityEngine;
using System.Collections.Generic;

public class Bottle : MonoBehaviour
{
    public List<GameObject> balls = new List<GameObject>();

    private void OnMouseDown()
    {
        GameManager.Instance.OnBottleClicked(this);
    }

    public void AddBall(GameObject ball)
    {
        balls.Add(ball); // Tambahkan bola ke akhir list
        ball.transform.SetParent(transform);
        UpdateBallPositions();
    }

    public GameObject RemoveBall()
    {
        if (balls.Count == 0)
            return null;

        int lastIndex = balls.Count - 1;
        GameObject ball = balls[lastIndex];
        balls.RemoveAt(lastIndex);
        UpdateBallPositions();
        return ball;
    }

    public GameObject PeekBall()
    {
        if (balls.Count == 0)
            return null;

        return balls[balls.Count - 1];
    }

    public int BallCount()
    {
        return balls.Count;
    }

    private void UpdateBallPositions()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            float yOffset = .5f; // Jarak awal dari dasar botol
            float ballHeight = 1f; // Tinggi bola (sesuai skala Y bola)
            float yPos = yOffset + i * ballHeight;
            balls[i].transform.localPosition = new Vector3(0, yPos, 0);
        }
    }
}
