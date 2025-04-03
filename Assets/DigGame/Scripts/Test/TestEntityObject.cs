using PICOMR.Scripts.ResourcesLoader.Interfaces;
using UnityEngine;

namespace DigGame.Scripts.Test
{
    public class TestEntityObject : MonoBehaviour,IItem
    {
        public ulong EntityID { get; set; }
    }
}