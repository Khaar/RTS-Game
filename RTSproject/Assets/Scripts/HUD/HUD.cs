using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

    private const int Orders_Bar_Width = 150, Resource_Bar_Height = 40; 
    public GUISkin resourceSkin, ordersSkin;
    private Player player;
    private const int Selection_Name_Height = 15;

	void Start () {
        player = transform.root.GetComponent<Player>();
	}
    void OnGUI()
    {
        if(player && player.human)
        {
            DrawOrdersBar();
            DrawResourceBar();
        }
    }

    private void DrawOrdersBar()
    {
        GUI.skin = ordersSkin;
        GUI.BeginGroup(new Rect(Screen.width - Orders_Bar_Width, Resource_Bar_Height, Orders_Bar_Width, Screen.height - Resource_Bar_Height));
        GUI.Box(new Rect(0, 0, Orders_Bar_Width, Screen.height - Resource_Bar_Height), "");

        string selectionName = "";
        if (player.SelectedObject)
        {
            selectionName = player.SelectedObject.objectName;
        }
        if (!selectionName.Equals(""))
        {
            GUI.Label(new Rect(0, 10, Orders_Bar_Width, Selection_Name_Height), selectionName);
        }
        GUI.EndGroup();
    }
    private void DrawResourceBar()
    {
        GUI.skin = resourceSkin;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Resource_Bar_Height));
        GUI.Box(new Rect(0, 0, Screen.width, Resource_Bar_Height), "");
        GUI.EndGroup();
    }

    public bool MouseInBounds()
    {
        Vector3 mousePos = Input.mousePosition;
        bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width - Orders_Bar_Width;
        bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height - Resource_Bar_Height;

        return insideWidth && insideHeight;
    }
}
