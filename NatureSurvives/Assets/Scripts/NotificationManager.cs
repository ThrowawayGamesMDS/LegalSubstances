using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{

    [SerializeField] private Text notificationText;
    [SerializeField] private float fadeTime;

    private static NotificationManager instance;

    private IEnumerator notificationCoroutine;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }

    }

    public void AdjustFadeTime(bool _bSpeedIncreased)
    {
        switch(_bSpeedIncreased)
        {
            case true:
                {
                    Instance.fadeTime = Instance.fadeTime * 3;

                    break;
                }
            case false:
                {
                    Instance.fadeTime = Instance.fadeTime / 3;
                    break;
                }
        }
    }

    public static NotificationManager Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }

            instance = FindObjectOfType<NotificationManager>();

            //CreateNewInstance();

            return instance;
        }
    }

    public static NotificationManager CreateNewInstance()
    {
        NotificationManager notificationManagerPrefab = Resources.Load<NotificationManager>("NotificationManager");
        instance = Instantiate(notificationManagerPrefab);

        return instance;
    } 

    /// <summary>
    /// Same as SetNewNotification, just didn't want to have to alter 23 references...
    /// </summary>
    /// <param name="_sMessage"></param>

    public void SetWarningNotification(string _sMessage)
    {
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        notificationCoroutine = FadeOutNotification(_sMessage, true);
        StartCoroutine(notificationCoroutine);
    }


    public void SetNewNotification(string message)
    {
        if(notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }
        notificationCoroutine = FadeOutNotification(message, false);
        StartCoroutine(notificationCoroutine);
    }

    private IEnumerator FadeOutNotification(string message, bool _bWarningMessage)
    {
        if (_bWarningMessage)
        {
            notificationText.color = Color.red;
            message = "WARNING: " + message;
        }
        notificationText.text = message;
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            notificationText.color = new Color(
                notificationText.color.r,
                notificationText.color.g,
                notificationText.color.b,
                Mathf.Lerp(1f, 0f, t / fadeTime));
            yield return null;
        }
    }
}
