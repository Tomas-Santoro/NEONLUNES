using UnityEngine;
using MoreMountains.TopDownEngine;
using System.Collections.Generic;
using TMPro; // para TextMeshPro

[RequireComponent(typeof(Health))]
public class IdlePenaltyTDE : MonoBehaviour
{
    [Header("Detección de inactividad")]
    [Tooltip("Distancia mínima (en unidades de mundo) por frame para considerarlo movimiento")]
    public float movementEpsilon = 0.005f;

    [Tooltip("Segundos quieto antes de empezar a penalizar")]
    public float idleDelay = 2f;

    [Header("Penalización")]
    [Tooltip("Daño aplicado en cada tick mientras está quieto")]
    public float damagePerTick = 1f;

    [Tooltip("Intervalo (segundos) entre ticks de daño mientras esté quieto")]
    public float damageInterval = 1f;

    [Header("Parámetros de Damage (TDE)")]
    [Tooltip("Duración del flicker visual (si no querés, dejalo en 0)")]
    public float flickerDuration = 0f;

    [Tooltip("Invencibilidad breve tras el golpe (si no querés i-frames, dejalo en 0)")]
    public float invincibilityDuration = 0f;

    [Header("UI de advertencia (sobre la barra de vida)")]
    [Tooltip("Si está vacío, el script crea el label como hijo del Canvas del MMHealthBar")]
    public TextMeshProUGUI warningLabel;

    [TextArea]
    public string warningText = "You can't stay still. Move.";

    [Tooltip("Offset local en el Canvas world-space del MMHealthBar")]
    public Vector2 labelLocalOffset = new Vector2(0f, 0.25f);

    [Tooltip("Tamaño de fuente del aviso")]
    public float labelFontSize = 3.5f;

    [Tooltip("Mantener visible mientras dure la penalización")]
    public bool keepVisibleWhilePenalizing = true;

    private Health _health;
    private Vector3 _lastPos;
    private float _idleTime = 0f;
    private float _tickTimer = 0f;
    private bool _isPenalizing = false;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _lastPos = transform.position;

        // Si no hay label asignado, intentamos crear uno colgado del MMHealthBar (Canvas world-space)
        if (warningLabel == null)
        {
            Canvas hbCanvas = FindHealthBarCanvas();
            if (hbCanvas != null)
            {
                GameObject go = new GameObject("IdlePenaltyWarning_TMP");
                go.layer = hbCanvas.gameObject.layer;
                go.transform.SetParent(hbCanvas.transform, false);

                var rect = go.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 1f);  // top center
                rect.anchorMax = new Vector2(0.5f, 1f);
                rect.pivot = new Vector2(0.5f, 0f);  // crece hacia arriba
                rect.anchoredPosition = labelLocalOffset;

                warningLabel = go.AddComponent<TextMeshProUGUI>();
                warningLabel.text = "";
                warningLabel.alignment = TextAlignmentOptions.Center;
                warningLabel.enableWordWrapping = false;
                warningLabel.fontSize = labelFontSize;
                warningLabel.raycastTarget = false;
                // Si querés outline/sombra, usá un Material Preset de TMP con outline configurado.
            }
        }

        HideWarning();
    }

    private void OnEnable()
    {
        if (_health == null) { _health = GetComponent<Health>(); }
        if (_health != null)
        {
            _health.OnDeath += HandleDeath;
            _health.OnRevive += HandleRevive; // por si hay respawn
        }
    }

    private void OnDisable()
    {
        if (_health != null)
        {
            _health.OnDeath -= HandleDeath;
            _health.OnRevive -= HandleRevive;
        }
    }

    private void Update()
    {
        // Distancia recorrida este frame
        float moved = (transform.position - _lastPos).magnitude;
        _lastPos = transform.position;

        bool isMoving = moved > movementEpsilon;

        if (isMoving)
        {
            _idleTime = 0f;
            _tickTimer = 0f;

            if (_isPenalizing)
            {
                _isPenalizing = false;
                HideWarning();
            }
            return;
        }

        // Acumula tiempo quieto
        _idleTime += Time.deltaTime;

        // Inicia penalización
        if (!_isPenalizing && _idleTime >= idleDelay)
        {
            _isPenalizing = true;
            ShowWarning();
        }

        // Ticks de daño
        if (_isPenalizing)
        {
            _tickTimer += Time.deltaTime;
            if (_tickTimer >= damageInterval)
            {
                _tickTimer = 0f;

                if (_health != null)
                {
                    _health.Damage(
                        damagePerTick,
                        this.gameObject,      // instigator
                        flickerDuration,
                        invincibilityDuration,
                        Vector3.zero,
                        null                  // typed damages
                    );
                }

                if (!keepVisibleWhilePenalizing)
                {
                    HideWarning();  // flash en vez de persistente
                }
            }
        }
    }

    // Busca el Canvas world-space que crea el MMHealthBar para colgar ahí el texto
    private Canvas FindHealthBarCanvas()
    {
        var mmhb = GetComponentInParent<MoreMountains.Tools.MMHealthBar>();
        if (mmhb != null)
        {
            var canvas = mmhb.GetComponentInChildren<Canvas>(true);
            if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
            {
                return canvas;
            }
        }
        return null;
    }

    private void ShowWarning()
    {
        if (warningLabel != null)
        {
            warningLabel.gameObject.SetActive(true);
            warningLabel.text = warningText;

            // Aseguramos anclajes y offset cada vez (por si cambian en runtime)
            var rect = warningLabel.rectTransform;
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchoredPosition = labelLocalOffset;
        }
    }

    private void HideWarning()
    {
        if (warningLabel != null)
        {
            warningLabel.text = "";
            warningLabel.gameObject.SetActive(false);
        }
    }

    // --- Eventos de vida/muerte del TDE ---

    private void HandleDeath()
    {
        _isPenalizing = false;
        HideWarning();
        enabled = false; // opcional: dejamos de chequear hasta revive
    }

    private void HandleRevive()
    {
        _idleTime = 0f;
        _tickTimer = 0f;
        _isPenalizing = false;
        HideWarning();
        enabled = true;
    }
}
