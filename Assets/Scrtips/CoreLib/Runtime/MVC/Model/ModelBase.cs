using UnityEngine;
using UnityEngine.Events;

namespace Corelib.Utils
{
    public class ModelBase : ISerializationCallbackReceiver
    {
        #region ========== Evnet ==========
        public UnityEvent<ModelBase> onUpdate;
        #endregion ====================

        protected virtual void InitializeEvents()
        {
            onUpdate = new();

            InitializeUpdateEvent();
        }
        protected virtual void InitializeUpdateEvent()
        {

        }

        public virtual ModelStateBase ToState()
        {
            return null;
        }

        #region ========== ISerializationCallbackReceiver ==========
        public virtual void OnBeforeSerialize() { }

        public virtual void OnAfterDeserialize()
        {
            InitializeEvents();
        }
        #endregion ====================
    }
}
