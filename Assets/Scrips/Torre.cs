using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Torre : MonoBehaviour
{
    [Header("É a torre final (de vitória)?")]
    public bool torreFinal = true;

    [Header("Discos em ordem (menor primeiro → maior por último)")]
    public List<Transform> discos = new List<Transform>();

    [Header("Evento quando vencer")]
    public UnityEvent onVictory;

    [Header("Debug")]
    public bool debugLogs = true;
    public float yTolerance = 0.001f; // tolerância de Y pra evitar empates por float

    private bool venceu = false;

    void Update()
    {
        if (!torreFinal || venceu) return;

        if (VerificarVitoria(out string motivoFalha))
        {
            venceu = true;
            if (debugLogs) Debug.Log("🎉 Vitória detectada pela torre: " + name);
            onVictory?.Invoke();
        }
        else
        {
            // Descomente se quiser spam de debug a cada frame
            // if (debugLogs) Debug.Log($"Ainda não venceu: {motivoFalha}");
        }
    }

    /// <summary>
    /// Faz a verificação completa e retorna false com motivo detalhado se falhar.
    /// </summary>
    bool VerificarVitoria(out string reason)
    {
        if (discos == null || discos.Count == 0)
        {
            reason = "Lista de discos vazia no Inspector.";
            return false;
        }

        // 1) Todos os discos precisam ser filhos desta torre
        for (int i = 0; i < discos.Count; i++)
        {
            var d = discos[i];
            if (d == null)
            {
                reason = $"O item discos[{i}] está vazio.";
                return false;
            }
            if (d.parent != transform)
            {
                reason = $"O disco '{d.name}' não é filho da torre '{name}'.";
                return false;
            }
        }

        // 2) Ordem correta: menor (índice 0) no topo → Y maior; maior (último) embaixo → Y menor
        for (int i = 0; i < discos.Count - 1; i++)
        {
            float yTopo = discos[i].position.y;
            float yAbaixo = discos[i + 1].position.y;

            // topo precisa estar estritamente acima (considerando tolerância)
            if (!(yTopo > yAbaixo + yTolerance))
            {
                reason = $"Ordem incorreta entre '{discos[i].name}' (Y={yTopo}) e '{discos[i + 1].name}' (Y={yAbaixo}).";
                return false;
            }
        }

        reason = null;
        return true;
    }

    // ====== Ferramentas de teste rápidas ======

    [ContextMenu("TESTE: Forçar Vitória (Invoke OnVictory)")]
    void TestarInvocarVitoria()
    {
        Debug.Log("🔔 Teste manual: invocando onVictory pela torre " + name);
        onVictory?.Invoke();
    }

    [ContextMenu("TESTE: Checar Estado e Logar Motivo")]
    void TestarChecagem()
    {
        if (VerificarVitoria(out string motivoFalha))
            Debug.Log("✅ Checagem: Vitória confirmada nesta torre (" + name + ")");
        else
            Debug.LogWarning("❌ Checagem: ainda não venceu. Motivo: " + motivoFalha);
    }
}
