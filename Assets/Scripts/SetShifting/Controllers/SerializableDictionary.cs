using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SetShifting.controllers
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<TValue> values = new List<TValue>();

        public void OnBeforeSerialize()
        {
            // Do Nothing
        }

        // Load Dictionary From Lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count == values.Count)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    try
                    {
                        this.Add(keys[i], values[i]);
                    }
                    catch
                    {
                        // Do Nothing , Do Not Want To Spam Exceptions In The Console When Editing The Dictionary
                    }
                }
            }
            else
            {
                //LoggerController.Instance.LogError("The Dictionary Keys Count Does Not SpadesSingleMatch The Values Count !, Please Fix Your Dictionary !");
            }
        }
    }

    [Serializable]
    public class Serialized_KeyStringValueTexture2D_Dictionary : SerializableDictionary<string, Texture2D>
    {

    }

    [Serializable]
    public class Serialized_KeyStringValueGameObject_Dictionary : SerializableDictionary<string, GameObject>
    {

    }

    [Serializable]
    public class Serialized_KeyStringValueSprite_Dictionary : SerializableDictionary<string, Sprite>
    {

    }


}