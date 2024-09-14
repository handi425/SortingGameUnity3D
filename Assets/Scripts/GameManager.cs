using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject bottlePrefab;
    public GameObject ballPrefab;
    public List<Color> ballColors;

    public Button playAgainButton;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI winMessage;

    public int currentLevel = 1;
    public int maxLevel = 10;

    // Opsi level (jumlah warna)
    private int[] levelColors = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    private List<Bottle> bottles = new List<Bottle>();
    private Bottle selectedBottle = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Nonaktifkan tombol dan pesan kemenangan di awal
        playAgainButton.gameObject.SetActive(false);
        winMessage.gameObject.SetActive(false);

        CreateLevel();
    }

    public void OnBottleClicked(Bottle bottle)
    {
        if (selectedBottle == null)
        {
            if (bottle.BallCount() == 0)
                return;

            selectedBottle = bottle;
            HighlightBottle(bottle, true);
        }
        else
        {
            if (bottle == selectedBottle)
            {
                HighlightBottle(bottle, false);
                selectedBottle = null;
            }
            else
            {
                MoveBall(selectedBottle, bottle);
                HighlightBottle(selectedBottle, false);
                selectedBottle = null;
            }
        }
    }

    private void MoveBall(Bottle fromBottle, Bottle toBottle)
    {
        if (toBottle.BallCount() >= 4)
            return;

        GameObject ballToMove = fromBottle.PeekBall();
        GameObject targetBall = toBottle.PeekBall();

        if (ballToMove == null)
            return;

        if (targetBall == null || ballToMove.GetComponent<SpriteRenderer>().color == targetBall.GetComponent<SpriteRenderer>().color)
        {
            fromBottle.RemoveBall();
            toBottle.AddBall(ballToMove);
            CheckWinCondition();
        }
    }

    private void HighlightBottle(Bottle bottle, bool highlight)
    {
        SpriteRenderer renderer = bottle.GetComponent<SpriteRenderer>();
        if (highlight)
            renderer.color = Color.yellow;
        else
            renderer.color = Color.white;
    }

    // Parameter Grid
    int columnsPerRow = 5; // Sesuaikan sesuai kebutuhan
    float xSpacing = 2f;
    float ySpacing = 3f;


    private void CreateLevel()
    {
        // Hapus botol dan bola sebelumnya jika ada
        foreach (Bottle bottle in bottles)
        {
            // Hapus semua bola dalam botol
            foreach (GameObject ball in bottle.balls)
            {
                Destroy(ball);
            }
            bottle.balls.Clear();

            // Hapus botol
            Destroy(bottle.gameObject);
        }
        bottles.Clear();

        // Tentukan jumlah warna berdasarkan level saat ini
        int colorCount = levelColors[currentLevel - 1];

        // Jumlah botol adalah jumlah warna * 4 / kapasitas botol (4) + botol kosong (misalnya, 2)
        int bottleCount = colorCount + 2; // Setiap warna memiliki 4 bola, dan kapasitas botol adalah 4

        int columnsPerRow = 5; // Sesuaikan sesuai kebutuhan
        float baseXSpacing = 2f;
        float baseYSpacing = 3f;

        // Hitung total baris dan kolom
        int totalColumns = Mathf.Min(columnsPerRow, bottleCount);
        int totalRows = Mathf.CeilToInt((float)bottleCount / columnsPerRow);

        // Menghitung faktor skala berdasarkan jumlah baris
        float bottleScaleFactor = 1f;
        if (totalRows == 1)
        {
            bottleScaleFactor = 1f;
        }
        else if (totalRows == 2)
        {
            bottleScaleFactor = 0.5f;
        }
        else
        {
            // Untuk lebih dari 2 baris, Anda dapat menyesuaikan faktor skala sesuai kebutuhan
            bottleScaleFactor = 1f / totalRows; // Contoh sederhana
        }
        // Atur ukuran botol
        Vector3 bottleScale = new Vector3(bottleScaleFactor, bottleScaleFactor, 1f);

        // Sesuaikan spasi berdasarkan faktor skala
        float xSpacing = baseXSpacing * bottleScaleFactor * 2; // Kalikan dengan 2 agar spasi cukup
        float ySpacing = baseYSpacing * bottleScaleFactor * 2;



        // Hitung ukuran grid
        float gridWidth = (totalColumns - 1) * xSpacing;
        float gridHeight = (totalRows - 1) * ySpacing;

        // Posisi awal untuk memusatkan grid
        float startX = -gridWidth / 2;
        float startY = gridHeight / 2 - ySpacing / 2;

        // Membuat botol dan menempatkannya dalam grid
        for (int i = 0; i < bottleCount; i++)
        {
            int row = i / columnsPerRow;
            int column = i % columnsPerRow;

            float xPos = startX + column * xSpacing;
            float yPos = startY - row * ySpacing;

            GameObject bottleObj = Instantiate(bottlePrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            Bottle bottle = bottleObj.GetComponent<Bottle>();
            bottles.Add(bottle);
        }

        // Sesuaikan kamera agar sesuai dengan grid
        AdjustCamera(totalRows);

        // Membuat bola
        List<GameObject> ballsList = new List<GameObject>();
        for (int i = 0; i < colorCount; i++)
        {
            Color color = ballColors[i % ballColors.Count];
            for (int j = 0; j < 4; j++)
            {
                GameObject ballObj = Instantiate(ballPrefab);
                ballObj.GetComponent<SpriteRenderer>().color = color;
                ballsList.Add(ballObj);
            }
        }

        // Mengacak bola
        Shuffle(ballsList);

        // Menempatkan bola ke botol
        int ballIndex = 0;
        foreach (Bottle bottle in bottles)
        {
            // Skip botol kosong
            if (ballIndex >= ballsList.Count)
                break;

            // Kumpulkan bola untuk botol ini
            List<GameObject> bottleBalls = new List<GameObject>();

            for (int i = 0; i < 4 && ballIndex < ballsList.Count; i++)
            {
                bottleBalls.Add(ballsList[ballIndex]);
                ballIndex++;
            }

            // Menambahkan bola ke botol dari bawah ke atas
            foreach (GameObject ball in bottleBalls)
            {
                bottle.AddBall(ball);
            }
        }

        // Perbarui teks level
        levelText.text = "Level " + currentLevel;

        // Nonaktifkan pesan kemenangan
        winMessage.gameObject.SetActive(false);
    }

    private void AdjustCamera(int totalRows)
    {
        // Parameter Grid (harus sama dengan yang digunakan di CreateLevel)
        float ySpacing = 3f;

        // Mengatur ukuran ortografik kamera
        float requiredHeight = totalRows * ySpacing + 2f; // Tambahan ruang untuk UI
        Camera.main.orthographicSize = Mathf.Max(requiredHeight / 2f, 5f); // Minimal ukuran kamera 5

        // Pastikan kamera berada di tengah
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }



    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void CheckWinCondition()
    {
        bool isWin = true;
        foreach (Bottle bottle in bottles)
        {
            if (bottle.BallCount() == 0)
                continue;

            if (bottle.BallCount() != 4)
            {
                isWin = false;
                break;
            }

            // Mengambil warna bola pertama (paling bawah)
            Color firstColor = bottle.balls[0].GetComponent<SpriteRenderer>().color;

            // Memeriksa setiap bola dalam botol
            foreach (GameObject ball in bottle.balls)
            {
                if (ball.GetComponent<SpriteRenderer>().color != firstColor)
                {
                    isWin = false;
                    break;
                }
            }

            if (!isWin)
                break;
        }

        if (isWin)
        {
            // Tampilkan pesan kemenangan
            winMessage.gameObject.SetActive(true);
            winMessage.text = "Selamat! Anda memenangkan Level " + currentLevel + "!";

            // Aktifkan tombol Play Again
            playAgainButton.gameObject.SetActive(true);

            // Ubah teks tombol menjadi "Next Level"
            playAgainButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Level";
        }
    }

    public void NextLevel()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
        else
        {
            currentLevel = 1; // Kembali ke level 1 jika sudah mencapai level maksimum
        }

        // Nonaktifkan tombol Play Again
        playAgainButton.gameObject.SetActive(false);

        // Membuat level baru
        CreateLevel();
    }

    public void RestartLevel()
    {
        // Nonaktifkan tombol Play Again
        playAgainButton.gameObject.SetActive(false);

        // Membuat level saat ini lagi
        CreateLevel();
    }
}
