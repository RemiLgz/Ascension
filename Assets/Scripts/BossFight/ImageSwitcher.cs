using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ImageSwitcher : MonoBehaviour
{
    public List<Image> images;
    public int requiredClicks = 5;
    public int bossHealth = 300;
    public int damagePerRound = 10;
    public TextMeshProUGUI bossHealthText;
    public RectTransform panel;
    public RectTransform secondaryPanel;

    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;
    public float flashIntensity = 0.5f;
    public float panelDestroyDelay = 0.5f;

    private Image currentImage;
    private int clickCount = 0;
    private int previousIndex = -1;
    private int displayedHealth;

    void Start()
    {
        if (images.Count == 0) return;
        displayedHealth = bossHealth;
        UpdateBossHealthUI();
        HideAllImages();
        ShowRandomImage();
    }

    public void OnImageClick()
    {
        clickCount++;
        StartCoroutine(ClickFeedback());

        if (clickCount >= requiredClicks)
        {
            clickCount = 0;
            ApplyDamageToBoss();
            StartCoroutine(HideAndShowNewImage());
        }
        else
        {
            UpdateFilledCircleSize();
        }
    }

    private void ApplyDamageToBoss()
    {
        bossHealth -= damagePerRound;
        UpdateBossHealthUI();
        StartCoroutine(ScreenShake(shakeDuration, shakeMagnitude));
        StartCoroutine(FlashEffect());

        if (bossHealth <= 0)
        {
            StartCoroutine(DestroyPanelsWithDelay());
        }
    }

    private IEnumerator DestroyPanelsWithDelay()
    {
        yield return new WaitForSeconds(panelDestroyDelay);

        Image panelImage = panel.GetComponent<Image>();
        Image secondaryPanelImage = secondaryPanel.GetComponent<Image>();

        Sequence destructionSequence = DOTween.Sequence();

        if (panelImage != null)
        {
            destructionSequence.Append(panel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack))
                               .Join(panelImage.DOFade(0, 0.5f))
                               .AppendCallback(() => Destroy(panel.gameObject));
        }

        destructionSequence.AppendInterval(0.2f);

        if (secondaryPanelImage != null)
        {
            destructionSequence.Append(secondaryPanel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack))
                               .Join(secondaryPanelImage.DOFade(0, 0.5f))
                               .AppendCallback(() => Destroy(secondaryPanel.gameObject));
        }

        destructionSequence.Play();
    }

    private void UpdateBossHealthUI()
    {
        if (bossHealthText != null)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateHealthDisplay());
        }
    }

    private IEnumerator UpdateHealthDisplay()
    {
        int startHealth = displayedHealth;
        int endHealth = Mathf.Max(bossHealth, 0);

        float duration = 0.5f;
        float time = 0;
        while (time < duration)
        {
            displayedHealth = (int)Mathf.Lerp(startHealth, endHealth, time / duration);
            bossHealthText.text = displayedHealth.ToString();
            time += Time.deltaTime;
            yield return null;
        }

        displayedHealth = endHealth;
        bossHealthText.text = displayedHealth.ToString();
    }

    private IEnumerator ClickFeedback()
    {
        Vector3 originalScale = currentImage.transform.localScale;
        Vector3 punchScale = originalScale * 1.2f;

        currentImage.transform.localScale = punchScale;
        yield return new WaitForSeconds(0.05f);
        currentImage.transform.localScale = originalScale;
    }

    private IEnumerator HideAndShowNewImage()
    {
        yield return ShrinkEffect(currentImage.transform, 0.2f);
        ShowRandomImage();
    }

    private void ShowRandomImage()
    {
        HideAllImages();

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, images.Count);
        } while (randomIndex == previousIndex);

        previousIndex = randomIndex;
        currentImage = images[randomIndex];
        currentImage.gameObject.SetActive(true);

        ResetFilledCircleSize();
        StartCoroutine(GrowEffect(currentImage.transform, 0.4f));
    }

    private void HideAllImages()
    {
        foreach (var image in images)
        {
            image.gameObject.SetActive(false);
        }
    }

    private void ResetFilledCircleSize()
    {
        Transform filledCircle = currentImage.transform.GetChild(0);
        filledCircle.localScale = Vector3.zero;
    }

    private void UpdateFilledCircleSize()
    {
        Transform filledCircle = currentImage.transform.GetChild(0);
        float targetScale = 1.0f;
        float scaleIncrement = targetScale / requiredClicks;
        StartCoroutine(SmoothScale(filledCircle, Vector3.one * (scaleIncrement * clickCount), 0.2f));
    }

    private IEnumerator SmoothScale(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 startScale = target.localScale;
        float time = 0;

        while (time < duration)
        {
            target.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        target.localScale = targetScale;
    }

    private IEnumerator GrowEffect(Transform target, float duration)
    {
        target.localScale = Vector3.zero;
        Vector3 targetScale = Vector3.one * 1.2f;
        float time = 0;

        while (time < duration)
        {
            float scaleValue = Mathf.Sin((time / duration) * Mathf.PI * 0.5f);
            target.localScale = Vector3.Lerp(Vector3.zero, targetScale, scaleValue);
            time += Time.deltaTime;
            yield return null;
        }
        target.localScale = targetScale / 1.2f;
    }

    private IEnumerator ShrinkEffect(Transform target, float duration)
    {
        Vector3 startScale = target.localScale;
        float time = 0;

        while (time < duration)
        {
            float scaleValue = 1 - Mathf.Sin((time / duration) * Mathf.PI * 0.5f);
            target.localScale = Vector3.Lerp(startScale, Vector3.zero, scaleValue);
            time += Time.deltaTime;
            yield return null;
        }
        target.localScale = Vector3.zero;
        target.gameObject.SetActive(false);
    }

    private IEnumerator ScreenShake(float duration, float magnitude)
    {
        Vector3 originalPosition = Camera.main.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Mathf.PerlinNoise(Time.time * 50, 0.0f) * 2 - 1;
            float y = Mathf.PerlinNoise(0.0f, Time.time * 50) * 2 - 1;

            x *= magnitude * 0.1f;
            y *= magnitude * 0.1f;

            Camera.main.transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPosition;
    }

    private IEnumerator FlashEffect()
    {
        Image flashImage = new GameObject("Flash", typeof(Image)).GetComponent<Image>();
        flashImage.color = new Color(1, 1, 1, 0);
        flashImage.transform.SetParent(this.transform, false);
        flashImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        float duration = 0.2f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            flashImage.color = new Color(1, 1, 1, Mathf.Lerp(0, flashIntensity, t / duration));
            yield return null;
        }

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            flashImage.color = new Color(1, 1, 1, Mathf.Lerp(flashIntensity, 0, t / duration));
            yield return null;
        }

        Destroy(flashImage.gameObject);
    }
}
