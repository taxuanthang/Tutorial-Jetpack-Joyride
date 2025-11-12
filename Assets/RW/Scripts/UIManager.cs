using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Animator startButton;
    public Animator settingsButton;
    public Animator dialog;
    public Animator contentPanel;
    public Animator gearImage;

    public bool contentFlag;



    public void StartGame()
    {
        SceneManager.LoadScene("RocketMouse");

        SetStartButtonActive(flag: true, animator: startButton);
        contentFlag = false;
    }

    public void OpenSettings()
    {
        //startButton.SetBool("isHidden", true);
        SetStartButtonActive(flag : false,animator: startButton);
        //settingsButton.SetBool("isHidden", true);
        SetSettingsButtonActive(flag: false, animator: settingsButton);
        //dialog.SetBool("isHidden", false);
        SetDialogActive(true, dialog);
    }

    public void CloseSettings()
    {
        //startButton.SetBool("isHidden", false);
        SetStartButtonActive(flag: true, animator: startButton);
        //settingsButton.SetBool("isHidden", false);
        SetSettingsButtonActive(flag: true, animator: settingsButton);
        //dialog.SetBool("isHidden", true);
        SetDialogActive(false, dialog);

    }
    public void ToggleMenu()
    {
        contentFlag = !contentFlag;
        //gearImage.SetBool("isHidden", !isHidden);
        //contentPanel.SetBool("isHidden", !isHidden);
        SetContentPanelActive(contentFlag, contentPanel);
        SetGearImageActive(contentFlag,gearImage);
    }

    public void SetStartButtonActive(bool flag, Animator animator)
    {
        RectTransform transform = animator.GetComponent<RectTransform>();
        switch (flag)
        {
            case false:
                transform.DOAnchorMin(new Vector2(0.2f, 1f), 1f);
                transform.DOAnchorMax(new Vector2(0.8f, 1f), 1f);
                transform.DOAnchorPosY(60f, 1f);
                break;
            case true:
                transform.DOAnchorMin(new Vector2(0.2f, 0f), 1f);
                transform.DOAnchorMax(new Vector2(0.8f, 0f), 1f);
                transform.DOAnchorPosY(250f, 1f);
                break;
        }
    }

    public void SetSettingsButtonActive(bool flag, Animator animator)
    {
        RectTransform transform = animator.GetComponent<RectTransform>();
        switch (flag)
        {
            case true:
                transform.DOAnchorPosY(180f, 1f);
                break;

            case false:
                transform.DOAnchorPosY(-50f, 1f);
                break;

        }
    }

    public void SetDialogActive(bool flag, Animator animator)
    {
        RectTransform transform = animator.GetComponent<RectTransform>();
        switch (flag)
        {
            case true:
                transform.DOAnchorMin(new Vector2(0.5f, 0.5f), 1f);
                transform.DOAnchorMax(new Vector2(0.5f, 0.5f), 1f);
                transform.DOAnchorPosX(0f, 1f);
                break;

            case false:
                transform.DOAnchorMin(new Vector2(1f, 0.5f), 1f);
                transform.DOAnchorMax(new Vector2(1f, 0.5f), 1f);
                transform.DOAnchorPosX(220f, 1f);
                break;

        }
    }

    public void SetContentPanelActive(bool flag, Animator animator)
    {
        RectTransform transform = animator.GetComponent<RectTransform>();
        switch (flag)
        {
            case true:
                transform.DOAnchorMin(new Vector2(0f, 0f), 1f);
                transform.DOAnchorMax(new Vector2(1f, 1f), 1f);

                DOTween.To(
                    () => transform.offsetMin,
                    x => transform.offsetMin = x,
                    new Vector2(0, 0),
                    1f);
                break;

            case false:
                transform.DOAnchorMin(new Vector2(0f, 0f), 1f);
                transform.DOAnchorMax(new Vector2(1f, 0f), 1f);


                DOTween.To(
                    () => transform.offsetMin,
                    x => transform.offsetMin = x,
                    new Vector2(0, -192),
                    1f);

                break;

        }
    }

    public void SetGearImageActive(bool flag, Animator animator)
    {
        RectTransform transform = animator.GetComponent<RectTransform>();
        switch (flag)
        {
            case true:
                transform.DORotate(new Vector3(0f,0f,360f),1f, RotateMode.FastBeyond360);
                print("a");
                break;

            case false:
                transform.DORotate(new Vector3(0f, 0f, -360f), 1f, RotateMode.FastBeyond360);
                break;

        }
    }
}
