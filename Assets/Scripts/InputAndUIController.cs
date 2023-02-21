using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InputAndUIController : MonoBehaviour
{
    [SerializeField] GameofLifeController GameofLifeController;
    private Camera MainCamera;
    private UIDocument Doc;
    private Button RestartBtn;
    private Button PlayBtn;
    private Button PauseBtn;
    private GroupBox PlayPauseGroup;
    private Slider GenSpeedSlider;
    private bool IsLeftDown;
    private int LastX;
    private int LastY;

    void Awake()
    {
        MainCamera = Camera.main;

        Doc = GetComponent<UIDocument>();
        RestartBtn = Doc.rootVisualElement.Q<Button>("restart-btn");
        PlayPauseGroup = Doc.rootVisualElement.Q<GroupBox>("play-pause-group");
        PlayBtn = Doc.rootVisualElement.Q<Button>("play-btn");
        PauseBtn = Doc.rootVisualElement.Q<Button>("pause-btn");
        GenSpeedSlider = Doc.rootVisualElement.Q<Slider>("gen-speed-slider");

        RestartBtn.clicked += onClickRestart;
        PlayBtn.clicked += onClickPlay;
        PauseBtn.clicked += onClickPause;
        GenSpeedSlider.RegisterValueChangedCallback<float>(OnGenSpeedSliderChange);
    }

    private void Start()
    {
        onClickPause();
        GenSpeedSlider.value = 0.5f;
        LastX = -1;
        LastY = -1;
    }

    private void onClickPause()
    {
        PlayPauseGroup.Clear();
        PlayPauseGroup.Add(PlayBtn);
        GameofLifeController.IsPause = true;
        GameofLifeController.UpdateRenderTextureToPixel();
    }

    private void onClickPlay()
    {
        PlayPauseGroup.Clear();
        PlayPauseGroup.Add(PauseBtn);
        GameofLifeController.IsPause = false;
    }

    private void onClickRestart()
    {
        onClickPause();
        GameofLifeController.Reset();
    }

    private void OnGenSpeedSliderChange(ChangeEvent<float> evt)
    {
        GameofLifeController.PlaySpeed = evt.newValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLeftDown)
        {
            if (Input.GetMouseButtonUp(0))
            {
                IsLeftDown = false;
                GameofLifeController.ClearDrawingState();
            }
            else
            {
                // Cast a ray from the mouse position
                Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                // Check if the ray intersects with the quad collider
                if (Physics.Raycast(ray, out hitInfo) && (LastX != (int)hitInfo.point.x || LastY != (int)hitInfo.point.y))
                {
                    LastX = (int)hitInfo.point.x;
                    LastY = (int)hitInfo.point.y;
                    GameofLifeController.DrawPixel(LastX, LastY);
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            IsLeftDown = true;
        }
    }
}
