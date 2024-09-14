# **Sorting Balls in Bottles**

Game puzzle berbasis Unity di mana pemain harus menyortir bola berwarna ke dalam botol, mengikuti aturan tertentu, untuk menyelesaikan teka-teki di berbagai level dengan tingkat kesulitan yang meningkat.

## **Daftar Isi**

- [Pendahuluan](#pendahuluan)
- [Mekanisme Permainan](#mekanisme-permainan)
- [Fitur](#fitur)
- [Memulai](#memulai)
  - [Prasyarat](#prasyarat)
  - [Instalasi](#instalasi)
- [Struktur Proyek](#struktur-proyek)
- [Penjelasan Skrip](#penjelasan-skrip)
  - [GameManager.cs](#gamemanagercs)
  - [Bottle.cs](#bottlecs)
- [Penyesuaian](#penyesuaian)
  - [Mengatur Level](#mengatur-level)
  - [Mengubah Spasi Antar Bola](#mengubah-spasi-antar-bola)
- [Masalah yang Diketahui](#masalah-yang-diketahui)
- [Kontribusi](#kontribusi)
- [Lisensi](#lisensi)

---

## **Pendahuluan**

"Sorting Balls in Bottles" adalah game puzzle yang dikembangkan menggunakan Unity. Tujuan utama dari game ini adalah menyortir bola-bola berwarna ke dalam botol sehingga setiap botol hanya berisi bola dengan warna yang sama. Seiring dengan kemajuan pemain, level akan menjadi lebih menantang dengan menambahkan lebih banyak warna dan botol.

## **Mekanisme Permainan**

- **Botol**: Wadah yang dapat menampung hingga 4 bola.
- **Bola**: Objek dengan berbagai warna yang perlu disortir oleh pemain.
- **Interaksi Pemain**: Pemain dapat mengklik botol untuk memilih dan memindahkan bola paling atas ke botol lain, mengikuti aturan tertentu.

**Aturan:**

1. Hanya bola paling atas dari botol yang dapat dipindahkan.
2. Bola hanya dapat ditempatkan di atas bola dengan warna yang sama atau ke dalam botol kosong.
3. Setiap botol hanya dapat menampung maksimal 4 bola.

## **Fitur**

- **Level Bertingkat**: Game memiliki beberapa level dengan tingkat kesulitan yang meningkat.
- **Skala Dinamis**: Ukuran botol dan bola akan menyesuaikan berdasarkan jumlah baris untuk memastikan tampilan yang proporsional.
- **Desain Responsif**: Tampilan game menyesuaikan dengan berbagai ukuran dan resolusi layar.
- **Elemen UI**: Menampilkan level saat ini, pesan kemenangan, dan tombol "Next Level".
- **Penyesuaian Spasi**: Spasi antar bola dalam botol dapat disesuaikan.

## **Memulai**

### **Prasyarat**

- **Unity Editor** (disarankan versi 2019.4 atau lebih baru)
- Pengetahuan dasar tentang **C#** dan antarmuka **Unity**

### **Instalasi**

1. **Clone Repository:**

   ```bash
   git clone https://github.com/username/SortingBallsInBottles.git
   ```

2. **Buka Proyek di Unity:**

   - Buka **Unity Hub**.
   - Klik **Add** dan pilih direktori proyek yang telah di-clone.
   - Buka proyek tersebut.

3. **Siapkan Scene:**

   - Buka scene utama (misalnya, `MainScene.unity`) dari folder `Assets/Scenes`.

4. **Jalankan Game:**

   - Klik tombol **Play** di Unity Editor untuk memulai game.

## **Struktur Proyek**

```
Assets/
├── Scripts/
│   ├── GameManager.cs
│   └── Bottle.cs
├── Prefabs/
│   ├── Bottle.prefab
│   └── Ball.prefab
├── Sprites/
│   ├── BottleSprite.png
│   └── BallSprite.png
├── Scenes/
│   └── MainScene.unity
└── UI/
    ├── Canvas/
    │   ├── LevelText
    │   ├── WinMessage
    │   └── PlayAgainButton
    └── Fonts/
        └── (Font kustom opsional)
```

## **Penjelasan Skrip**

### **GameManager.cs**

Kelas `GameManager` mengelola logika utama game, termasuk pembuatan level, input pemain, dan kondisi kemenangan.

#### **Metode Utama:**

- `Start()`: Inisialisasi game dengan membuat level pertama dan mengatur elemen UI.
- `CreateLevel()`: Menghasilkan botol dan bola berdasarkan level saat ini, mengatur posisi mereka dalam grid, dan menyesuaikan skala berdasarkan jumlah baris.
- `OnBottleClicked(Bottle bottle)`: Mengelola pemilihan botol dan pemindahan bola antar botol.
- `MoveBall(Bottle fromBottle, Bottle toBottle)`: Memvalidasi dan mengeksekusi pemindahan bola dari satu botol ke botol lain.
- `CheckWinCondition()`: Memeriksa apakah pemain telah berhasil menyortir semua bola dan menangani skenario kemenangan.
- `NextLevel()`: Melanjutkan game ke level berikutnya dengan meregenerasi elemen game.
- `AdjustCamera(int totalRows, float ySpacing)`: Menyesuaikan ukuran kamera untuk memastikan semua botol terlihat di layar.

#### **Variabel Penting:**

- `public GameObject bottlePrefab;`: Referensi ke prefab botol.
- `public GameObject ballPrefab;`: Referensi ke prefab bola.
- `public List<Color> ballColors;`: Daftar warna yang digunakan untuk bola.
- `public Button playAgainButton;`: Referensi ke tombol UI untuk memulai ulang atau melanjutkan level.
- `public Text levelText;`: Menampilkan level saat ini.
- `public Text winMessage;`: Menampilkan pesan kemenangan setelah level selesai.
- `private List<Bottle> bottles;`: List instance botol dalam level saat ini.
- `private Bottle selectedBottle;`: Botol yang saat ini dipilih oleh pemain.

#### **Poin Penyesuaian:**

- **Konfigurasi Level**: Modifikasi array `levelColors` untuk menyesuaikan jumlah warna (dan dengan demikian, bola dan botol) per level.
- **Skala Botol**: Sesuaikan metode `GetBottleScaleFactor(int totalRows)` untuk mengubah cara botol diskalakan berdasarkan jumlah baris.
- **Spasi Bola**: Modifikasi variabel `ballSpacing` (jika diimplementasikan) untuk mengatur spasi vertikal antara bola.

### **Bottle.cs**

Kelas `Bottle` mewakili setiap botol dalam game dan mengelola bola yang dikandungnya.

#### **Metode Utama:**

- `OnMouseDown()`: Mendeteksi ketika botol diklik dan memberi tahu `GameManager`.
- `AddBall(GameObject ball)`: Menambahkan bola ke botol dan menjadikannya sebagai objek anak.
- `RemoveBall()`: Menghapus bola paling atas dari botol dan mengembalikannya.
- `PeekBall()`: Mengembalikan bola paling atas tanpa menghapusnya.
- `BallCount()`: Mengembalikan jumlah bola yang saat ini ada dalam botol.
- `UpdateBallScaleAndPosition(float bottleScaleFactor)`: Memperbarui skala dan posisi bola di dalam botol berdasarkan faktor skala.

#### **Variabel Penting:**

- `public List<GameObject> balls;`: List instance bola yang dikandung dalam botol.
- `public float spacingFactor;`: Menentukan spasi vertikal antara bola dalam botol (default adalah `0.25f`).

#### **Poin Penyesuaian:**

- **Spasi Bola**: Sesuaikan variabel `spacingFactor` untuk mengontrol spasi antara bola di dalam botol.

## **Penyesuaian**

### **Mengatur Level**

Untuk memodifikasi level:

1. **Mengubah Jumlah Warna:**

   - Di `GameManager.cs`, modifikasi array `levelColors`:

     ```csharp
     private int[] levelColors = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };
     ```

   - Setiap elemen mewakili jumlah warna untuk level tersebut. Sesuaikan angka-angka ini untuk mengubah tingkat kesulitan.

2. **Menambahkan Level Baru:**

   - Tingkatkan variabel `maxLevel` dan tambahkan entri yang sesuai dalam array `levelColors`.

### **Mengubah Spasi Antar Bola**

Untuk mengatur spasi antara bola di dalam botol:

1. **Modifikasi `spacingFactor` di `Bottle.cs`:**

   - Buka `Bottle.cs` dan temukan metode `UpdateBallScaleAndPosition()`.
   - Ubah nilai `spacingFactor`:

     ```csharp
     public float spacingFactor = 0.25f; // Nilai default
     ```

   - Tingkatkan nilai ini untuk menambah spasi, kurangi untuk mengurangi spasi.

2. **Sesuaikan per Botol (Opsional):**

   - Karena `spacingFactor` adalah variabel publik, Anda dapat menyesuaikannya secara individual untuk setiap botol di Unity Editor jika diperlukan.

## **Masalah yang Diketahui**

- **Bola Bertumpuk atau Melebihi Botol**: Jika `spacingFactor` diatur terlalu tinggi, bola mungkin melebihi batas botol.
  - **Solusi**: Pastikan bahwa total tinggi bola dan spasi tidak melebihi tinggi botol. Sesuaikan `spacingFactor` atau faktor skala botol.
- **Elemen UI Bertumpang Tindih dengan Botol**: Pada layar yang lebih kecil, elemen UI mungkin bertumpang tindih dengan objek game.
  - **Solusi**: Sesuaikan metode `AdjustCamera()` dan pengaturan anchor UI untuk memastikan spasi yang tepat.

## **Kontribusi**

Kontribusi sangat dihargai! Silakan fork repository ini dan kirim pull request untuk perbaikan atau peningkatan.

1. **Fork Repository**
2. **Buat Branch Fitur**

   ```bash
   git checkout -b fitur/FiturAnda
   ```

3. **Commit Perubahan Anda**

   ```bash
   git commit -m "Menambahkan fitur Anda"
   ```

4. **Push ke Branch**

   ```bash
   git push origin fitur/FiturAnda
   ```

5. **Buka Pull Request**

## **Lisensi**

Proyek ini dilisensikan di bawah Lisensi MIT - lihat file [LICENSE](LICENSE) untuk detailnya.

---

**Catatan:** Pastikan Anda memiliki hak dan izin yang diperlukan untuk menggunakan dan mendistribusikan aset apa pun (sprite, font, dll.) yang disertakan dalam proyek ini.

---

## **Komentar Kode dan Dokumentasi**

Baik `GameManager.cs` dan `Bottle.cs` telah diberi komentar secara rinci untuk menjelaskan fungsionalitas setiap metode dan variabel. Berikut adalah skrip dengan komentar lengkap.

---

### **GameManager.cs**

```csharp
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Mengelola logika utama game, termasuk pembuatan level,
/// input pemain, dan kondisi kemenangan.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Instance singleton

    [Header("Prefabs")]
    public GameObject bottlePrefab; // Prefab botol
    public GameObject ballPrefab;   // Prefab bola

    [Header("Colors")]
    public List<Color> ballColors; // Daftar warna untuk bola

    [Header("UI Elements")]
    public Button playAgainButton; // Tombol untuk memulai ulang atau melanjutkan level
    public Text levelText;         // Teks untuk menampilkan level saat ini
    public Text winMessage;        // Teks untuk menampilkan pesan kemenangan

    [Header("Game Settings")]
    public int currentLevel = 1; // Nomor level saat ini
    public int maxLevel = 10;    // Jumlah level maksimum

    // Array yang menentukan jumlah warna per level
    private int[] levelColors = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    // List instance botol dalam level saat ini
    private List<Bottle> bottles = new List<Bottle>();
    private Bottle selectedBottle = null; // Botol yang dipilih saat ini

    private void Awake()
    {
        // Implementasi singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Inisialisasi elemen UI
        playAgainButton.gameObject.SetActive(false);
        winMessage.gameObject.SetActive(false);

        // Membuat level awal
        CreateLevel();
    }

    /// <summary>
    /// Mengelola logika ketika botol diklik oleh pemain.
    /// </summary>
    /// <param name="bottle">Botol yang diklik.</param>
    public void OnBottleClicked(Bottle bottle)
    {
        if (selectedBottle == null)
        {
            // Pilih botol jika memiliki bola
            if (bottle.BallCount() == 0)
                return;

            selectedBottle = bottle;
            HighlightBottle(bottle, true);
        }
        else
        {
            if (bottle == selectedBottle)
            {
                // Batalkan pemilihan jika botol yang sama diklik lagi
                HighlightBottle(bottle, false);
                selectedBottle = null;
            }
            else
            {
                // Coba pindahkan bola ke botol baru
                MoveBall(selectedBottle, bottle);
                HighlightBottle(selectedBottle, false);
                selectedBottle = null;
            }
        }
    }

    /// <summary>
    /// Memindahkan bola dari satu botol ke botol lain jika gerakan valid.
    /// </summary>
    /// <param name="fromBottle">Botol asal.</param>
    /// <param name="toBottle">Botol tujuan.</param>
    private void MoveBall(Bottle fromBottle, Bottle toBottle)
    {
        // Periksa apakah botol tujuan penuh
        if (toBottle.BallCount() >= 4)
            return;

        GameObject ballToMove = fromBottle.PeekBall();
        GameObject targetBall = toBottle.PeekBall();

        if (ballToMove == null)
            return;

        // Periksa apakah gerakan valid (warna cocok atau botol kosong)
        if (targetBall == null || ballToMove.GetComponent<SpriteRenderer>().color == targetBall.GetComponent<SpriteRenderer>().color)
        {
            fromBottle.RemoveBall();
            toBottle.AddBall(ballToMove);
            CheckWinCondition();
        }
    }

    /// <summary>
    /// Menyorot botol ketika dipilih.
    /// </summary>
    /// <param name="bottle">Botol yang akan disorot.</param>
    /// <param name="highlight">Apakah botol disorot atau tidak.</param>
    private void HighlightBottle(Bottle bottle, bool highlight)
    {
        SpriteRenderer renderer = bottle.GetComponent<SpriteRenderer>();
        if (highlight)
            renderer.color = Color.yellow;
        else
            renderer.color = Color.white;
    }

    /// <summary>
    /// Membuat level baru berdasarkan pengaturan level saat ini.
    /// </summary>
    private void CreateLevel()
    {
        // Hapus botol dan bola sebelumnya
        foreach (Bottle bottle in bottles)
        {
            foreach (GameObject ball in bottle.balls)
            {
                Destroy(ball);
            }
            bottle.balls.Clear();
            Destroy(bottle.gameObject);
        }
        bottles.Clear();

        // Tentukan jumlah warna dan botol untuk level ini
        int colorCount = levelColors[currentLevel - 1];
        int bottleCount = colorCount + 2; // Tambahan botol kosong untuk gameplay

        // Parameter grid
        int columnsPerRow = 5; // Sesuaikan sesuai kebutuhan
        float baseXSpacing = 2f;
        float baseYSpacing = 3f;

        // Hitung total kolom dan baris
        int totalColumns = Mathf.Min(columnsPerRow, bottleCount);
        int totalRows = Mathf.CeilToInt((float)bottleCount / totalColumns);

        // Dapatkan faktor skala botol berdasarkan total baris
        float bottleScaleFactor = GetBottleScaleFactor(totalRows);

        // Atur skala botol
        Vector3 bottleScale = new Vector3(bottleScaleFactor, bottleScaleFactor, 1f);

        // Sesuaikan spasi berdasarkan skala
        float xSpacing = baseXSpacing * bottleScaleFactor * 2f;
        float ySpacing = baseYSpacing * bottleScaleFactor * 2f;

        // Hitung ukuran grid
        float gridWidth = (totalColumns - 1) * xSpacing;
        float gridHeight = (totalRows - 1) * ySpacing;

        // Posisi awal untuk memusatkan grid
        float startX = -gridWidth / 2f;
        float startY = gridHeight / 2f - ySpacing / 2f;

        // Membuat dan menempatkan botol
        for (int i = 0; i < bottleCount; i++)
        {
            int row = i / totalColumns;
            int column = i % totalColumns;

            float xPos = startX + column * xSpacing;
            float yPos = startY - row * ySpacing;

            GameObject bottleObj = Instantiate(bottlePrefab, new Vector3(xPos, yPos, 0f), Quaternion.identity);
            bottleObj.transform.localScale = bottleScale;

            Bottle bottle = bottleObj.GetComponent<Bottle>();
            bottles.Add(bottle);
        }

        // Sesuaikan kamera untuk menyesuaikan grid
        AdjustCamera(totalRows, ySpacing);

        // Membuat bola dan menetapkan warna
        List<GameObject> ballsList = new List<GameObject>();
        for (int i = 0; i < colorCount; i++)
        {
            Color color = ballColors[i % ballColors.Count];
            for (int j = 0; j < 4; j++)
            {
                GameObject ballObj = Instantiate(ballPrefab);
                ballObj.GetComponent<SpriteRenderer>().color = color;
                ballObj.transform.localScale = new Vector3(bottleScaleFactor, bottleScaleFactor, 1f);
                ballsList.Add(ballObj);
            }
        }

        // Mengacak bola untuk keacakan
        Shuffle(ballsList);

        // Membagikan bola ke dalam botol
        int ballIndex = 0;
        foreach (Bottle bottle in bottles)
        {
            // Lewati botol kosong jika semua bola telah dibagikan
            if (ballIndex >= ballsList.Count)
                break;

            List<GameObject> bottleBalls = new List<GameObject>();

            for (int i = 0; i < 4 && ballIndex < ballsList.Count; i++)
            {
                bottleBalls.Add(ballsList[ballIndex]);
                ballIndex++;
            }

            foreach (GameObject ball in bottleBalls)
            {
                bottle.AddBall(ball);
            }

            // Perbarui skala dan posisi bola dalam botol
            bottle.UpdateBallScaleAndPosition(bottleScaleFactor);
        }

        // Perbarui UI
        levelText.text = "Level " + currentLevel;
        winMessage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Menentukan faktor skala botol berdasarkan jumlah baris.
    /// </summary>
    /// <param name="totalRows">Jumlah total baris dalam grid.</param>
    /// <returns>Faktor skala untuk botol.</returns>
    private float GetBottleScaleFactor(int totalRows)
    {
        if (totalRows == 1)
            return 1f;
        else if (totalRows == 2)
            return 0.7f;
        else if (totalRows == 3)
            return 0.6f;
        else if (totalRows == 4)
            return 0.5f;
        else
            return 0.4f; // Untuk lebih dari 4 baris
    }

    /// <summary>
    /// Menyesuaikan kamera untuk memastikan semua botol terlihat.
    /// </summary>
    /// <param name="totalRows">Jumlah total baris dalam grid.</param>
    /// <param name="ySpacing">Spasi vertikal antara botol.</param>
    private void AdjustCamera(int totalRows, float ySpacing)
    {
        float requiredHeight = totalRows * ySpacing + 4f; // Tambahan ruang untuk UI
        Camera.main.orthographicSize = Mathf.Max(requiredHeight / 2f, 5f);

        // Posisikan kamera di tengah
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    /// <summary>
    /// Mengacak list menggunakan algoritma Fisher-Yates.
    /// </summary>
    /// <typeparam name="T">Tipe elemen list.</typeparam>
    /// <param name="list">List yang akan diacak.</param>
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

    /// <summary>
    /// Memeriksa apakah kondisi kemenangan terpenuhi dan menangani skenario kemenangan.
    /// </summary>
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

            // Dapatkan warna bola pertama
            Color firstColor = bottle.balls[0].GetComponent<SpriteRenderer>().color;

            // Periksa apakah semua bola dalam botol memiliki warna yang sama
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
            // Tampilkan pesan kemenangan dan tombol "Next Level"
            winMessage.gameObject.SetActive(true);
            winMessage.text = "Selamat! Anda menyelesaikan Level " + currentLevel + "!";
            playAgainButton.gameObject.SetActive(true);
            playAgainButton.GetComponentInChildren<Text>().text = "Next Level";
        }
    }

    /// <summary>
    /// Melanjutkan ke level berikutnya.
    /// </summary>
    public void NextLevel()
    {
        if (currentLevel < maxLevel)
            currentLevel++;
        else
            currentLevel = 1; // Kembali ke level 1 jika telah mencapai level maksimum

        playAgainButton.gameObject.SetActive(false);
        CreateLevel();
    }

    /// <summary>
    /// Memulai ulang level saat ini.
    /// </summary>
    public void RestartLevel()
    {
        playAgainButton.gameObject.SetActive(false);
        CreateLevel();
    }
}
```

---

### **Bottle.cs**

```csharp
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Mewakili botol dalam game dan mengelola bola yang dikandungnya.
/// </summary>
public class Bottle : MonoBehaviour
{
    // List bola yang saat ini ada dalam botol
    public List<GameObject> balls = new List<GameObject>();

    // Faktor untuk menentukan spasi antara bola
    [HideInInspector]
    public float spacingFactor = 0.25f;

    private void OnMouseDown()
    {
        // Memberi tahu GameManager ketika botol ini diklik
        GameManager.Instance.OnBottleClicked(this);
    }

    /// <summary>
    /// Menambahkan bola ke botol.
    /// </summary>
    /// <param name="ball">Bola yang akan ditambahkan.</param>
    public void AddBall(GameObject ball)
    {
        balls.Add(ball);
        ball.transform.SetParent(transform);
    }

    /// <summary>
    /// Menghapus bola paling atas dari botol.
    /// </summary>
    /// <returns>Bola yang dihapus.</returns>
    public GameObject RemoveBall()
    {
        if (balls.Count == 0)
            return null;

        int lastIndex = balls.Count - 1;
        GameObject ball = balls[lastIndex];
        balls.RemoveAt(lastIndex);
        return ball;
    }

    /// <summary>
    /// Mengembalikan bola paling atas tanpa menghapusnya.
    /// </summary>
    /// <returns>Bola paling atas.</returns>
    public GameObject PeekBall()
    {
        if (balls.Count == 0)
            return null;

        return balls[balls.Count - 1];
    }

    /// <summary>
    /// Mengembalikan jumlah bola dalam botol.
    /// </summary>
    /// <returns>Jumlah bola.</returns>
    public int BallCount()
    {
        return balls.Count;
    }

    /// <summary>
    /// Memperbarui skala dan posisi bola dalam botol.
    /// </summary>
    /// <param name="bottleScaleFactor">Faktor skala botol.</param>
    public void UpdateBallScaleAndPosition(float bottleScaleFactor)
    {
        // Sesuaikan faktor spasi jika diperlukan
        spacingFactor = 0.25f * bottleScaleFactor; // Modifikasi sesuai kebutuhan

        for (int i = 0; i < balls.Count; i++)
        {
            // Perbarui skala bola
            balls[i].transform.localScale = new Vector3(bottleScaleFactor, bottleScaleFactor, 1f);

            // Hitung posisi Y bola
            float yOffset = -0.5f + (i * spacingFactor) + (spacingFactor / 2f);
            balls[i].transform.localPosition = new Vector3(0, yOffset, 0);
        }
    }
}
```
