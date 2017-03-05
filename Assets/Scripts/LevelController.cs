using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyInfo
    {
        [SerializeField]
        public GameObject Prefab;
    }

    [SerializeField]
    public List<EnemyInfo> Enemies;

    public AnimationPlayer LevelStart;
    public AnimationPlayer LevelEnd;

    public List<NextLevelGate> Gates;

    public void OpenGates()
    {
        foreach (var gate in Gates)
        {
            gate.Open();
        }
    }

    public bool Entered()
    {
        return Gates.Any(g => g.Entered);
    }
}
