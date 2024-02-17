using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PageType { Text, Texture }

[CreateAssetMenu(fileName = "New Page", menuName = "Notes System/new Page")]
public class Page : ScriptableObject
{
    [SerializeField] PageType type = PageType.Text;
    public PageType Type { get { return type; } }

    [TextArea(8, 16)]
    [SerializeField] string text = string.Empty;
    public string Text { get { return text; } }

    [SerializeField] Sprite texture = null;
    public Sprite Texture { get { return texture; } }

    [SerializeField] bool useSubscript = true;
    public bool UseSubscript { get { return useSubscript; } } 

    [SerializeField] bool displayLines = true;
    public bool DisplayLines { get { return displayLines; } }
}
