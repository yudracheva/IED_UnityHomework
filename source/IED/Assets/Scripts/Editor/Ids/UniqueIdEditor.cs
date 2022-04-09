﻿using System.Linq;
using Enemies.Spawn;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor.Ids
{
  [CustomEditor(typeof(UniqueId))]
  public class UniqueIdEditor : UnityEditor.Editor
  {
    private void OnEnable()
    {
      var uniqueId = (UniqueId) target;
      
      if(IsPrefab(uniqueId))
        return;
      
      if (string.IsNullOrEmpty(uniqueId.Id))
        Generate(uniqueId);
      else
      {
        var uniqueIds = FindObjectsOfType<UniqueId>();
        
        if(uniqueIds.Any(other => other != uniqueId && other.Id == uniqueId.Id))
          Generate(uniqueId);
      }
    }

    private bool IsPrefab(UniqueId uniqueId) => 
      uniqueId.gameObject.scene.rootCount == 0;

    private void Generate(UniqueId uniqueId)
    {
      uniqueId.GenerateId();

      if (!Application.isPlaying)
      {
        EditorUtility.SetDirty(uniqueId);
        EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
      }
    }
  }
}