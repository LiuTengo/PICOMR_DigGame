using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PICOMR.Scripts.ResourcesLoader.Interfaces
{
    public interface IEntityManager
    {
        //UniTask<T> LoadAsync<T>(string resourcePath) where T : Object;
        // UniTask LoadRoomEntities();
        // UniTask ClearRoomEntities();
        UniTask LoadGameEntities();
        UniTask SaveGameEntities();
        UniTask ClearGameEntities();
        UniTask<IEntity> CreateAndAddEntity(GameObject gameObject);
        UniTask DeleteEntityAndAnchor(IEntity entity);
    }
}