using UnityEngine;
using UnityEngine.Serialization;

public class selectTile : MonoBehaviour
{
    public string currentTile;
    public bool tileSelected = false;
    public GameObject tileOptionsUI;

    [FormerlySerializedAs("selectedTileGM")]
    public GameObject selectedTileGm;
    // Start is called before the first frame update
    // void Start()
    // {
    //     // tileOptionsUI = GameObject.FindGameObjectWithTag("TileInfoUI");
    // }

    // Update is called once per frame
    void Update()
    {
        if (tileSelected)
        {
            tileOptionsUI.SetActive(true);
        }
        else
        {
            tileOptionsUI.SetActive(false);
        }
    }
}