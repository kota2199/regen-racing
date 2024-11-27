using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplaySwitcher : MonoBehaviour
{
    public Dropdown[] displayDropdowns; // 各Displayの設定UI

    private int[] selectedDisplays; // 各Displayの選択内容を保存

    public static DisplaySwitcher instance; // シングルトンとしてこのスクリプトを保持

    void Awake()
    {
        // シングルトン化して、シーンを超えてオブジェクトを保持
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        int availableDisplays = Display.displays.Length;

        // 利用可能なディスプレイを有効化
        for (int i = 1; i < availableDisplays; i++)
        {
            Display.displays[i].Activate();
        }

        // 各ドロップダウンの初期化
        selectedDisplays = new int[displayDropdowns.Length];
        for (int i = 0; i < displayDropdowns.Length; i++)
        {
            InitializeDropdown(displayDropdowns[i], i);
        }

        Debug.Log("SceneChanged");
    }

    void InitializeDropdown(Dropdown dropdown, int index)
    {
        dropdown.options.Clear();

        // 利用可能なディスプレイをリストに追加
        for (int i = 0; i < Display.displays.Length; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData("Physical Display " + (i + 1)));
        }

        // 初期値を設定
        dropdown.value = index;
        dropdown.RefreshShownValue();

        // ドロップダウン変更時のイベント登録
        dropdown.onValueChanged.AddListener((selectedDisplay) =>
        {
            selectedDisplays[index] = selectedDisplay; // 選択を保存
            Debug.Log($"Display {index + 1} assigned to Physical Display {selectedDisplay + 1}");

            // フルスクリーン設定を更新
            SetFullscreen(selectedDisplay);
        });

        // 初期選択を保存
        selectedDisplays[index] = dropdown.value;
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("01_Title");
    }

    public void ApplySettingsToCameras()
    {
        // シーン内のすべてのカメラを取得
        Camera[] sceneCameras = FindObjectsOfType<Camera>();

        for (int i = 0; i < sceneCameras.Length && i < selectedDisplays.Length; i++)
        {
            int displayIndex = selectedDisplays[i];
            if (displayIndex < Display.displays.Length)
            {
                sceneCameras[i].targetDisplay = displayIndex; // 対応するTarget Displayを設定
                Debug.Log($"Camera {sceneCameras[i].name} assigned to Physical Display {displayIndex + 1}");

                // フルスクリーン設定を適用
                SetFullscreen(displayIndex);
            }
        }
    }

    private void SetFullscreen(int displayIndex)
    {
        if (displayIndex < Display.displays.Length)
        {
            // フルスクリーンモードを設定
            Screen.SetResolution(
                Display.displays[displayIndex].systemWidth,
                Display.displays[displayIndex].systemHeight,
                true,
                displayIndex
            );
            Debug.Log($"Set Display {displayIndex + 1} to Fullscreen");
        }
        else
        {
            Debug.LogWarning("Invalid display index for fullscreen setting!");
        }
    }
}