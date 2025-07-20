using UnityEngine;

namespace BattleSystem
{
    public class DamagePopup : MonoBehaviour
    {
        /// 기본 버전
        public static void Show(Vector3 position, int damage)
        {
            Debug.Log($"Damage {damage} at {position} (기본)");
        }

        /// 오버로드 버전 — isCritical, isElemental 받음
        public static void Show(Vector3 position, int damage, bool isCritical, bool isElemental)
        {
            string msg = $"Damage {damage} at {position}";
            if (isCritical) msg += " [CRIT]";
            if (isElemental) msg += " [ELEMENT]";
            Debug.Log(msg);
        }
    }
}
