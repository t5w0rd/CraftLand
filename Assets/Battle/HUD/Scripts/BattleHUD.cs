using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleHUD : Singleton<BattleHUD>
{
    public DamageText dmgEffect;
    public Inventory inventory;
    public Inventory storehouse;
    public GameObject onScreenPad;

    private RectTransform rt;
    private Canvas canvas;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();

        inventory.InitInventory(ArchiveManager.player.inventories[0]);
        storehouse.InitInventory(ArchiveManager.player.inventories[1]);
    }

    public void OpenInventory()
    {
        inventory.Show(inventory.visible);
        storehouse.Show(storehouse.visible);
    }

    public void OnInventoryButtonClicked()
    {
        inventory.Show(!inventory.visible);
        storehouse.Show(!storehouse.visible);
    }

    public void OnScreenPadButtonClicked()
    {
        onScreenPad.SetActive(!onScreenPad.activeSelf);
    }

    public void PlayDamageText(int value, Transform unit)
    {
        var de = Instantiate(dmgEffect, rt);
        //float deltaY = 0.0f;
        Vector3 pos;
        Vector3 to;

        switch (canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                pos = Utils.WorldToScreenPointInScreenSpaceOverlayMode(unit.position, Camera.main);
                to = Utils.WorldToScreenPointInScreenSpaceOverlayMode(unit.position + Vector3.up, Camera.main);
                break;
            case RenderMode.ScreenSpaceCamera:
                pos = Utils.WorldToScreenPointInScreenSpaceCameraMode(unit.position, rt, Camera.main);
                to = Utils.WorldToScreenPointInScreenSpaceCameraMode(unit.position + Vector3.up, rt, Camera.main);
                break;
            default:
                pos = Vector3.zero;
                to = Vector3.zero;
                break;
        }
        de.transform.position = pos;
        de.FloatUp(value, (to - pos).y);
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        OnInventoryButtonClicked();
    }
}
