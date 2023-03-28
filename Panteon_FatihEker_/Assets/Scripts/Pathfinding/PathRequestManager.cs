using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour {
   private Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
   private PathRequest _currentPathRequest;

   public static PathRequestManager Instance;
   private bool _isProcessingPath;

   private void Awake() {
      Instance = this;
   }

   public static void RequestPath(int startX, int startY, int endX, int endY, Action<List<GridObject>,bool> callback) {
      PathRequest newRequest = new PathRequest(startX, startY, endX, endY, callback);
      Instance._pathRequestQueue.Enqueue(newRequest);
      Instance.TryProcessNext();
   }

   private void TryProcessNext() {
      if(!_isProcessingPath && _pathRequestQueue.Count > 0) {
         _currentPathRequest = _pathRequestQueue.Dequeue();
         _isProcessingPath = true;
         Pathfinding.Instance.StartFindPath(_currentPathRequest.xStart, _currentPathRequest.yStart, _currentPathRequest.xEnd, _currentPathRequest.yEnd);
      }
   }
   public void FinishedProcessingPath(List<GridObject> path,bool success) {
      _currentPathRequest._callback(path, success);
      _isProcessingPath = false;
      TryProcessNext();
   }

   struct PathRequest {
      public int xStart;
      public int yStart;
      public int xEnd;
      public int yEnd;
      public Action<List<GridObject>, bool> _callback;

      public PathRequest(int startX, int startY, int endX, int endY, Action<List<GridObject>, bool> callback) {
         xStart = startX;
         yStart = startY;
         xEnd = endX;
         yEnd = endY;
         _callback = callback;
      }
   }
   
}
