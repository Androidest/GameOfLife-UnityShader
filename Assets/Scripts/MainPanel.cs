using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MainPanel : MonoBehaviour
{
    [SerializeField] GameofLifeController GameofLifeController;
    private Camera MainCamera;
    private UIDocument Doc;
    private Button RestartBtn;
    private Button PlayBtn;
    private Button PauseBtn;
    private GroupBox PlayPauseGroup;
    private Slider GenSpeedSlider;

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
        // Check for a left mouse button click
        if (Input.GetMouseButtonDown(0) && 
            !EventSystem.current.IsPointerOverGameObject())
        {
            // Cast a ray from the mouse position
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // Check if the ray intersects with the quad collider
            if (Physics.Raycast(ray, out hitInfo))
            {
                // Use the world hit point for your game logic
                //Debug.Log("World hit point: " + hitInfo.point);

                GameofLifeController.DrawPixel((int)hitInfo.point.x, (int)hitInfo.point.y);
            }
        }
    }
}
