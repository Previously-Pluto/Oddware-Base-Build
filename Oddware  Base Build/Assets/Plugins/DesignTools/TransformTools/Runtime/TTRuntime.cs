﻿/*
Copyright (c) 2020 Omar Duarte
Unauthorized copying of this file, via any medium is strictly prohibited.
Writen by Omar Duarte, 2020.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PluginMaster.Runtime
{
    public static class TTRuntime
    {
        #region BOUNDS
        private static readonly Vector3 MIN_VECTOR3 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        private static readonly Vector3 MAX_VECTOR3 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        public enum Bound { MIN, CENTER, MAX }

        public enum RelativeTo
        {
            LAST_SELECTED,
            FIRST_SELECTED,
            BIGGEST_OBJECT,
            SMALLEST_OBJECT,
            SELECTION,
            CANVAS
        }
        public enum Axis { X, Y, Z }

        public enum ObjectProperty
        {
            BOUNDING_BOX,
            CENTER,
            PIVOT
        }
        
        public static Bounds GetBounds(Transform transform, ObjectProperty property = ObjectProperty.BOUNDING_BOX)
        {
            var renderer = transform.GetComponent<Renderer>();
            var rectTransform = transform.GetComponent<RectTransform>();
            
            if(rectTransform == null)
            {
                if(renderer == null || property == ObjectProperty.PIVOT) return new Bounds(transform.position, Vector3.zero);
                if(property == ObjectProperty.CENTER) return new Bounds(renderer.bounds.center, Vector3.zero);
                return renderer.bounds;
            }
            else
            {
                if (property == ObjectProperty.PIVOT) return new Bounds(rectTransform.position, Vector3.zero);
                return new Bounds(rectTransform.TransformPoint(rectTransform.rect.center), rectTransform.TransformVector(rectTransform.rect.size));
            }
        }

        private static Bounds GetBoundsRecursive(Transform transform, bool recursive = true, ObjectProperty property = ObjectProperty.BOUNDING_BOX)
        {
            if(!recursive) return GetBounds(transform, property);
            var children = transform.GetComponentsInChildren<Transform>(true);
            var min = MAX_VECTOR3;
            var max = MIN_VECTOR3;
            var emptyHierarchy = true;
            foreach (var child in children)
            {
                if (child.GetComponent<Renderer>() == null && child.GetComponent<RectTransform>() == null) continue;
                emptyHierarchy = false;
                var bounds = GetBounds(child, property);
                min = Vector3.Min(bounds.min, min);
                max = Vector3.Max(bounds.max, max);
            }
            if (emptyHierarchy) return new Bounds(transform.position, Vector3.zero);
            var size = max - min;
            var center = min + size / 2f;
            return new Bounds(center, size);
        }
        private static Vector3 GetBound(Bounds bounds, Bound bound)
        {
            switch (bound)
            {
                case Bound.MIN: return bounds.min;
                case Bound.CENTER: return bounds.center;
                case Bound.MAX: return bounds.max;
                default: return bounds.center;
            }
        }

        private static GameObject GetAnchorObject(List<GameObject> selection, RelativeTo relativeTo, Axis axis, bool recursive = true)
        {
            if (selection.Count == 0) return null;
            switch (relativeTo)
            {
                case RelativeTo.LAST_SELECTED: return selection.Last<GameObject>();
                case RelativeTo.FIRST_SELECTED: return selection[0];
                case RelativeTo.BIGGEST_OBJECT:
                    GameObject biggestObject = null;
                    var maxSize = float.MinValue;
                    foreach (var obj in selection)
                    {
                        var bounds = GetBoundsRecursive(obj.transform, recursive);
                        switch (axis)
                        {
                            case Axis.X:
                                if (bounds.size.x <= maxSize) break;
                                maxSize = bounds.size.x;
                                biggestObject = obj;
                                break;
                            case Axis.Y:
                                if (bounds.size.y <= maxSize) break;
                                maxSize = bounds.size.y;
                                biggestObject = obj;
                                break;
                            case Axis.Z:
                                if (bounds.size.z <= maxSize) break;
                                maxSize = bounds.size.z;
                                biggestObject = obj;
                                break;
                        }
                    }
                    return biggestObject;
                case RelativeTo.SMALLEST_OBJECT:
                    GameObject smallestObject = null;
                    var minSize = float.MaxValue;
                    foreach (var obj in selection)
                    {
                        var bounds = GetBoundsRecursive(obj.transform, recursive);
                        switch (axis)
                        {
                            case Axis.X:
                                if (bounds.size.x >= minSize) break;
                                minSize = bounds.size.x;
                                smallestObject = obj;
                                break;
                            case Axis.Y:
                                if (bounds.size.y >= minSize) break;
                                minSize = bounds.size.y;
                                smallestObject = obj;
                                break;
                            case Axis.Z:
                                if (bounds.size.z >= minSize) break;
                                minSize = bounds.size.z;
                                smallestObject = obj;
                                break;
                        }
                    }
                    return smallestObject;
                default: return null;
            }
        }

        private static Bounds GetSelectionBounds(List<GameObject> selection, bool recursive = true)
        {
            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            foreach (var obj in selection)
            {
                var bounds = GetBoundsRecursive(obj.transform, recursive);
                max = Vector3.Max(bounds.max, max);
                min = Vector3.Min(bounds.min, min);
            }
            var size = max - min;
            var center = min + size / 2f;
            return new Bounds(center, size);
        }

        private static Tuple<GameObject, Bounds> GetSelectionBounds(List<GameObject> selection, RelativeTo relativeTo, Axis axis, bool recursive = true, ObjectProperty property = ObjectProperty.BOUNDING_BOX)
        {
            if (selection.Count == 0) return new Tuple<GameObject, Bounds>(null, new Bounds());
            var anchor = GetAnchorObject(selection, relativeTo, axis);
            if (anchor != null) return new Tuple<GameObject, Bounds>(anchor, GetBoundsRecursive(anchor.transform, recursive));
            if (relativeTo == RelativeTo.CANVAS)
            {
                var canvasBounds = GetCanvasBounds(selection);
                if(canvasBounds.size != Vector3.zero) return new Tuple<GameObject, Bounds>(null, GetCanvasBounds(selection));
            }
            return new Tuple<GameObject, Bounds>(null, GetSelectionBounds(selection));
        }

        private static Bounds GetCanvasBounds(List<GameObject> selection)
        {
            if (selection.Count == 0) return new Bounds();
            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            bool noCanvasFound = true;
            foreach(var obj in selection)
            {
                var canvas = GetTopmostCanvas(obj);
                if (canvas == null) continue;
                noCanvasFound = false;
                var rectTransform = canvas.GetComponent<RectTransform>();
                var halfSize = rectTransform.sizeDelta / 2;
                max = Vector3.Max(max, rectTransform.position + (Vector3)halfSize);
                min = Vector3.Min(min, rectTransform.position - (Vector3)halfSize);
            }
            if(noCanvasFound) return new Bounds();
            var size = max - min;
            var center = min + size / 2f;
            return new Bounds(center, size);
        }

        private static Bounds GetCanvasBounds(Canvas canvas)
        {
            var rectTransform = canvas.GetComponent<RectTransform>();
            return new Bounds(rectTransform.position, rectTransform.sizeDelta);
        }

        private static Canvas GetTopmostCanvas(GameObject obj)
        {
            var canvasesInParent = obj.GetComponentsInParent<Canvas>();
            if (canvasesInParent.Length == 0) return null;
            if (canvasesInParent.Length == 1) return canvasesInParent[0];
            foreach(var canvasInParent in canvasesInParent)
            {
                var canvasCount = canvasInParent.GetComponentsInParent<Canvas>().Length;
                if (canvasCount == 1) return canvasInParent;
            }
            return null;
        }
        #endregion
        #region ALIGN
        public static void Align(List<GameObject> selection, RelativeTo relativeTo, Axis axis, Bound bound, bool AlignToAnchor, bool filterByTopLevel = true, ObjectProperty property = ObjectProperty.BOUNDING_BOX)
        {
            if (selection.Count == 0) return;
            if (bound == Bound.CENTER && AlignToAnchor) return;

            var selectionBoundsTuple = GetSelectionBounds(selection, relativeTo, axis, filterByTopLevel);
            var selectionBound = GetBound(selectionBoundsTuple.Item2, AlignToAnchor ? (bound == Bound.MAX ? Bound.MIN : Bound.MAX) : bound);
            var anchor = selectionBoundsTuple.Item1;

            for (int i = 0; i < selection.Count; ++i)
            {
                var obj = selection[i];
                if (obj == anchor && relativeTo != RelativeTo.SELECTION) continue;
                var objBound = GetBound( GetBoundsRecursive(obj.transform, filterByTopLevel, property), bound);
                var alignedPosition = obj.transform.position;
                
                switch (axis)
                {
                    case Axis.X:
                        alignedPosition.x = obj.transform.position.x + selectionBound.x - objBound.x;
                        break;
                    case Axis.Y:
                        alignedPosition.y = obj.transform.position.y + selectionBound.y - objBound.y;
                        break;
                    case Axis.Z:
                        alignedPosition.z = obj.transform.position.z + selectionBound.z - objBound.z;
                        break;
                }
                var delta = alignedPosition - obj.transform.position;
                obj.transform.position = alignedPosition;
                if(anchor != null && anchor.transform.parent == obj.transform) anchor.transform.position -= delta;
            }
        }
        #endregion
        #region DISTRIBUTE
        public static void Distribute(List<GameObject> selection, Axis axis, Bound bound)
        {
            if (selection.Count < 2) return;
            var sortedList = new List<GameObject>(selection);
            switch (axis)
            {
                case Axis.X:
                    sortedList.Sort((obj1, obj2) => GetBound(GetBoundsRecursive(obj1.transform), bound).x.CompareTo(GetBound(GetBoundsRecursive(obj2.transform), bound).x));
                    break;
                case Axis.Y:
                    sortedList.Sort((obj1, obj2) => GetBound(GetBoundsRecursive(obj1.transform), bound).y.CompareTo(GetBound(GetBoundsRecursive(obj2.transform), bound).y));
                    break;
                case Axis.Z:
                    sortedList.Sort((obj1, obj2) => GetBound(GetBoundsRecursive(obj1.transform), bound).z.CompareTo(GetBound(GetBoundsRecursive(obj2.transform), bound).z));
                    break;
            }

            var min = GetBound(GetBoundsRecursive(sortedList.First<GameObject>().transform), bound);
            var max = GetBound(GetBoundsRecursive(sortedList.Last<GameObject>().transform), bound);

            var objDistance = 0f;
            switch (axis)
            {
                case Axis.X:
                    objDistance = (max.x - min.x) / (float)(selection.Count - 1);
                    break;
                case Axis.Y:
                    objDistance = (max.y - min.y) / (float)(selection.Count - 1);
                    break;
                case Axis.Z:
                    objDistance = (max.z - min.z) / (float)(selection.Count - 1);
                    break;
            }

            for(int i = 0; i < sortedList.Count; ++i)
            {
                var transform = sortedList[i].transform;
                var distributedPosition = transform.position;
                var objBound = GetBound(GetBoundsRecursive(transform), bound);
                switch (axis)
                {
                    case Axis.X:
                        distributedPosition.x += min.x - objBound.x + objDistance * i;
                        break;
                    case Axis.Y:
                        distributedPosition.y += min.y - objBound.y + objDistance * i;
                        break;
                    case Axis.Z:
                        distributedPosition.z += min.z - objBound.z + objDistance * i;
                        break;
                }
                transform.position = distributedPosition;
            }
        }

        public static void DistributeGaps(List<GameObject> selection, Axis axis)
        {
            if (selection.Count < 2) return;

            var selectionBounds = GetSelectionBounds(selection, RelativeTo.SELECTION, axis).Item2;
            var gapSize = selectionBounds.size;
            foreach (var obj in selection)
            {
                gapSize -= GetBoundsRecursive(obj.transform).size;
            }
            gapSize /= (float)(selection.Count - 1);

            var sortedList = new List<GameObject>(selection);
            switch (axis)
            {
                case Axis.X:
                    sortedList.Sort((obj1, obj2) => GetBoundsRecursive(obj1.transform).center.x.CompareTo(GetBoundsRecursive(obj2.transform).center.x));
                    break;
                case Axis.Y:
                    sortedList.Sort((obj1, obj2) => GetBoundsRecursive(obj1.transform).center.y.CompareTo(GetBoundsRecursive(obj2.transform).center.y));
                    break;
                case Axis.Z:
                    sortedList.Sort((obj1, obj2) => GetBoundsRecursive(obj1.transform).center.z.CompareTo(GetBoundsRecursive(obj2.transform).center.z));
                    break;
            }

            var prevMax = GetBoundsRecursive(sortedList.First<GameObject>().transform).min - gapSize;

            foreach (var obj in sortedList)
            {
                var transform = obj.transform;
                var distributedPosition = transform.position;
                var objBounds = GetBoundsRecursive(transform);
                var objMin = objBounds.min;
                switch (axis)
                {
                    case Axis.X:
                        distributedPosition.x += prevMax.x + gapSize.x - objMin.x;
                        break;
                    case Axis.Y:
                        distributedPosition.y += prevMax.y + gapSize.y - objMin.y;
                        break;
                    case Axis.Z:
                        distributedPosition.z += prevMax.z + gapSize.z - objMin.z;
                        break;
                }
                transform.position = distributedPosition;
                prevMax = GetBoundsRecursive(transform).max;
            }
        }
        #endregion
        #region GRID ARRANGE
        public class ArrangeAxisData
        {
            private bool _overwrite = true;
            private int _direction = 1;
            private int _priority = 0;
            private int _cells = 1;
            private CellSizeType _cellSizeType = CellSizeType.BIGGEST_OBJECT;
            private float _cellSize = 0f;
            private Bound _aligment = Bound.CENTER;
            private float _spacing = 0f;

            public ArrangeAxisData(int priority)
            {
                _priority = priority;
            }

            public int direction { get => _direction; set => _direction = value; }
            public int priority { get => _priority; set => _priority = value; }
            public int cells { get => _cells; set => _cells = value; }
            public Bound aligment { get => _aligment; set => _aligment = value; }
            public float spacing { get => _spacing; set => _spacing = value; }
            public CellSizeType cellSizeType { get => _cellSizeType; set => _cellSizeType = value; }
            public float cellSize
            {
                get => _cellSize;
                set
                {
                    if (value < 0 || _cellSize == value) return;
                    _cellSize = value;
                }
            }
            public bool overwrite
            {
                get => _overwrite;
                set
                {
                    if (_overwrite == value) return;
                    _overwrite = value;
                    if (!_overwrite)
                    {
                        _cells = 1;
                        _priority = 2;
                    }
                }
            }
        }

        public enum SortBy
        {
            SELECTION,
            POSITION,
            HIERARCHY
        }

        public enum CellSizeType
        {
            BIGGEST_OBJECT_PER_GROUP,
            BIGGEST_OBJECT,
            CUSTOM
        }

        public class ArrangeData
        {
            private ArrangeAxisData _x = new ArrangeAxisData(0);
            private ArrangeAxisData _y = new ArrangeAxisData(1);
            private ArrangeAxisData _z = new ArrangeAxisData(2);
            private SortBy _sortBy = SortBy.POSITION;
            private List<Axis> _priorityList = new List<Axis> { Axis.X, Axis.Y, Axis.Z };

            public ArrangeAxisData x { get => _x; set => _x = value; }
            public ArrangeAxisData y { get => _y; set => _y = value; }
            public ArrangeAxisData z { get => _z; set => _z = value; }
            public SortBy sortBy
            {
                get => _sortBy;
                set
                {
                    if (_sortBy == value) return;
                    _sortBy = value;
                    if (_sortBy == SortBy.POSITION)
                    {
                        x.priority = 0;
                        y.priority = 1;
                        z.priority = 2;
                        z.direction = y.direction = x.direction = +1;
                    }
                }
            }

            public ArrangeAxisData GetData(Axis axis)
            {
                return axis == Axis.X ? x : axis == Axis.Y ? y : z;
            }
            public void UpdatePriorities(Axis axis)
            {
                var activeAxes = Convert.ToInt32(x.overwrite) + Convert.ToInt32(y.overwrite) + Convert.ToInt32(z.overwrite);
                if (activeAxes > 0)
                {
                    if (x.overwrite) x.priority = Mathf.Min(x.priority, activeAxes - 1);
                    if (y.overwrite) y.priority = Mathf.Min(y.priority, activeAxes - 1);
                    if (z.overwrite) z.priority = Mathf.Min(z.priority, activeAxes - 1);
                }
                _priorityList.Remove(axis);
                _priorityList.Insert(GetData(axis).priority, axis);

                for (int priority = 0; priority < 3; ++priority)
                {
                    switch (_priorityList[priority])
                    {
                        case Axis.X:
                            x.priority = priority;
                            break;
                        case Axis.Y:
                            y.priority = priority;
                            break;
                        case Axis.Z:
                            z.priority = priority;
                            break;
                    }
                }
            }

        }

        private static int GetNextCellIndex(int currentIndex, int direction, int cellCount)
        {
            return IsLastCell(currentIndex, direction, cellCount) ? (direction > 0 ? 0 : cellCount - 1) : currentIndex + direction;
        }

        private static bool IsFirstCell(int currentIndex, int direction, int cellCount)
        {
            return direction > 0 ? currentIndex == 0 : currentIndex == cellCount - 1;
        }

        private static bool IsLastCell(int currentIndex, int direction, int cellCount)
        {
            return IsFirstCell(currentIndex, -direction, cellCount);
        }

        private static Dictionary<(int i, int j, int k), GameObject> SortBySelectionOrder(List<GameObject> selection, ArrangeData data)
        {
            int i = data.x.direction == 1 ? 0 : data.x.cells - 1;
            int j = data.y.direction == 1 ? 0 : data.y.cells - 1;
            int k = data.z.direction == 1 ? 0 : data.z.cells - 1;

            var dataList = new List<ArrangeAxisData>() { data.x, data.y, data.z };
            dataList.Sort((data1, data2) => data1.priority.CompareTo(data2.priority));

            var p0 = dataList[0] == data.x ? i : dataList[0] == data.y ? j : k;
            var p1 = dataList[1] == data.x ? i : dataList[1] == data.y ? j : k;
            var p2 = dataList[2] == data.x ? i : dataList[2] == data.y ? j : k;

            var objDictionary = new Dictionary<(int i, int j, int k), GameObject>();

            foreach (var obj in selection)
            {
                objDictionary.Add((
                    dataList[0] == data.x ? p0 : dataList[1] == data.x ? p1 : p2,
                    dataList[0] == data.y ? p0 : dataList[1] == data.y ? p1 : p2,
                    dataList[0] == data.z ? p0 : dataList[1] == data.z ? p1 : p2), obj);

                p0 = GetNextCellIndex(p0, dataList[0].direction, dataList[0].cells);
                if (!IsFirstCell(p0, dataList[0].direction, dataList[0].cells)) continue;
                p1 = GetNextCellIndex(p1, dataList[1].direction, dataList[1].cells);
                if (!IsFirstCell(p1, dataList[1].direction, dataList[1].cells)) continue;
                p2 = GetNextCellIndex(p2, dataList[2].direction, dataList[2].cells);
            }
            return objDictionary;
        }

        private static Dictionary<(int i, int j, int k), GameObject> SortByPosition(List<GameObject> selection, ArrangeData data, Bounds selectionBounds)
        {
            var maxSize = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            var averageSize = Vector3.zero;
            foreach (var obj in selection)
            {
                var objBounds = GetBoundsRecursive(obj.transform);
                maxSize = Vector3.Max(maxSize, objBounds.size);
                averageSize += objBounds.size;
            }
            averageSize /= selection.Count;
            var cellSize = new Vector3(
                data.x.cellSizeType == CellSizeType.BIGGEST_OBJECT ? maxSize.x : data.x.cellSizeType == CellSizeType.BIGGEST_OBJECT_PER_GROUP ? averageSize.x : data.x.cellSize,
                data.y.cellSizeType == CellSizeType.BIGGEST_OBJECT ? maxSize.y : data.y.cellSizeType == CellSizeType.BIGGEST_OBJECT_PER_GROUP ? averageSize.y : data.y.cellSize,
                data.z.cellSizeType == CellSizeType.BIGGEST_OBJECT ? maxSize.z : data.z.cellSizeType == CellSizeType.BIGGEST_OBJECT_PER_GROUP ? averageSize.z : data.z.cellSize);

            var firstCellCenter = selectionBounds.min + cellSize / 2f;

            var cellDict = new Dictionary<(int i, int j, int k), Bounds>();

            for (int k = 0; k < data.z.cells; ++k)
            {
                for (int j = 0; j < data.y.cells; ++j)
                {
                    for (int i = 0; i < data.x.cells; ++i)
                    {
                        var cellCenter = firstCellCenter + new Vector3(cellSize.x * i, cellSize.y * j, cellSize.z * k);
                        var cellBounds = new Bounds(cellCenter, cellSize);
                        cellDict.Add((i, j, k), cellBounds);
                    }
                }
            }
            var unsorted = new List<GameObject>(selection);
            var objDict = new Dictionary<(int i, int j, int k), GameObject>();

            while (unsorted.Count > 0)
            {
                var cellObjectsDict = new Dictionary<(int i, int j, int k), List<(GameObject obj, float sqrDistanceToCorner, float sqrDistanceToCenter)>>();
                foreach (var obj in unsorted)
                {
                    var objBounds = GetBoundsRecursive(obj.transform);
                    var minSqrDistanceToCorner = float.MaxValue;
                    var minSqrDistanceToCenter = float.MaxValue;
                    var closestCell = new KeyValuePair<(int i, int j, int k), Bounds>();
                    foreach (var cell in cellDict)
                    {
                        var objToCorner = new Vector3(
                            objBounds.min.x - cell.Value.min.x,
                            objBounds.min.y - cell.Value.min.y,
                            objBounds.min.z - cell.Value.min.z);
                        var sqrDistanceToCorner = Vector3.SqrMagnitude(objToCorner);
                        var sqrDistanceToCenter = Vector3.SqrMagnitude(objBounds.center - cell.Value.center);
                        if (sqrDistanceToCorner < minSqrDistanceToCorner)
                        {
                            minSqrDistanceToCorner = sqrDistanceToCorner;
                            minSqrDistanceToCenter = sqrDistanceToCenter;
                            closestCell = cell;
                        }
                        else if (minSqrDistanceToCorner == sqrDistanceToCorner && sqrDistanceToCenter < minSqrDistanceToCenter)
                        {
                            minSqrDistanceToCenter = sqrDistanceToCenter;
                            closestCell = cell;
                        }
                    }
                    if (cellObjectsDict.ContainsKey((closestCell.Key)))
                    {
                        cellObjectsDict[closestCell.Key].Add((obj, minSqrDistanceToCorner, minSqrDistanceToCenter));
                    }
                    else
                    {
                        cellObjectsDict.Add(closestCell.Key, new List<(GameObject obj, float sqrDistanceToCorner, float sqrDistanceToCenter)>());
                        cellObjectsDict[closestCell.Key].Add((obj, minSqrDistanceToCorner, minSqrDistanceToCenter));
                    }
                }

                foreach (var cellObjs in cellObjectsDict)
                {
                    var minSqrDistanceToCorner = cellObjs.Value[0].sqrDistanceToCorner;
                    var minSqrDistanceToCenter = cellObjs.Value[0].sqrDistanceToCenter;
                    GameObject closestObj = cellObjs.Value[0].obj;
                    for (int i = 1; i < cellObjs.Value.Count; ++i)
                    {
                        var objData = cellObjs.Value[i];
                        if (objData.sqrDistanceToCorner < minSqrDistanceToCorner)
                        {
                            minSqrDistanceToCorner = objData.sqrDistanceToCorner;
                            minSqrDistanceToCenter = objData.sqrDistanceToCenter;
                            closestObj = objData.obj;
                        }
                        else if (minSqrDistanceToCorner == objData.sqrDistanceToCorner && objData.sqrDistanceToCenter < minSqrDistanceToCenter)
                        {
                            minSqrDistanceToCenter = objData.sqrDistanceToCenter;
                            closestObj = objData.obj;
                        }
                    }
                    objDict.Add(cellObjs.Key, closestObj);
                    unsorted.Remove(closestObj);
                    cellDict.Remove(cellObjs.Key);
                }
            }
            return objDict;
        }

        public static bool Arrange(List<GameObject> selection, ArrangeData data)
        {
            if (selection.Count < 2 || selection.Count > data.x.cells * data.y.cells * data.z.cells) return false;
            var selectionBounds = GetSelectionBounds(selection);
            if (data.sortBy == SortBy.HIERARCHY)
            {
                selection = SortByHierarchy(selection);
            }
            Dictionary<(int i, int j, int k), GameObject> objDictionary = data.sortBy == SortBy.POSITION
                ? SortByPosition(selection, data, selectionBounds) : SortBySelectionOrder(selection, data);

            var maxSize = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            foreach (var obj in selection)
            {
                var objBounds = GetBoundsRecursive(obj.transform);
                maxSize = Vector3.Max(objBounds.size, maxSize);
            }

            maxSize = new Vector3(
                data.x.cellSizeType == CellSizeType.CUSTOM ? data.x.cellSize : maxSize.x,
                data.y.cellSizeType == CellSizeType.CUSTOM ? data.y.cellSize : maxSize.y,
                data.z.cellSizeType == CellSizeType.CUSTOM ? data.z.cellSize : maxSize.z);

            /////// X
            if (data.x.overwrite)
            {
                var prevMaxX = selectionBounds.min.x - data.x.spacing;
                for (int i = 0; i < data.x.cells; ++i)
                {
                    List<GameObject> objList = new System.Collections.Generic.List<GameObject>();
                    for (int j = 0; j < data.y.cells; ++j)
                    {
                        for (int k = 0; k < data.z.cells; ++k)
                        {
                            if (objDictionary.ContainsKey((i, j, k))) objList.Add(objDictionary[(i, j, k)]);
                        }
                    }
                    Align(objList, RelativeTo.SELECTION, Axis.X, data.x.aligment, false);
                    var columnBounds = GetSelectionBounds(objList, RelativeTo.SELECTION, Axis.X).Item2;
                    foreach (var obj in objList)
                    {
                        var arrangedPosition = obj.transform.position;
                        var objBounds = GetBoundsRecursive(obj.transform);
                        var objMin = objBounds.min;
                        arrangedPosition.x = obj.transform.position.x - columnBounds.min.x + prevMaxX + data.x.spacing;
                        if (data.x.cellSizeType != CellSizeType.BIGGEST_OBJECT_PER_GROUP && i > 0)
                        {
                            switch (data.x.aligment)
                            {
                                case Bound.CENTER:
                                    arrangedPosition.x += (maxSize.x - columnBounds.size.x) / 2f;
                                    break;
                                case Bound.MAX:
                                    arrangedPosition.x += maxSize.x - columnBounds.size.x;
                                    break;
                                default: break;
                            }
                        }
                        obj.transform.position = arrangedPosition;
                    }
                    columnBounds = GetSelectionBounds(objList, RelativeTo.SELECTION, Axis.X).Item2;
                    if (data.x.cellSizeType != CellSizeType.BIGGEST_OBJECT_PER_GROUP)
                    {
                        prevMaxX += maxSize.x + data.x.spacing;
                        if (i != 0) continue;
                        switch (data.x.aligment)
                        {
                            case Bound.CENTER:
                                prevMaxX -= (maxSize.x - columnBounds.size.x) / 2f;
                                break;
                            case Bound.MAX:
                                prevMaxX -= (maxSize.x - columnBounds.size.x);
                                break;
                            default: break;
                        }
                    }
                    else
                    {
                        prevMaxX = columnBounds.max.x;
                    }
                }
            }
            ///// Y
            if (data.y.overwrite)
            {
                var prevMaxY = selectionBounds.min.y - data.y.spacing;
                for (int j = 0; j < data.y.cells; ++j)
                {
                    List<GameObject> objList = new System.Collections.Generic.List<GameObject>();
                    for (int i = 0; i < data.x.cells; ++i)
                    {
                        for (int k = 0; k < data.z.cells; ++k)
                        {
                            if (objDictionary.ContainsKey((i, j, k)))
                            {
                                objList.Add(objDictionary[(i, j, k)]);
                            }
                        }
                    }
                    Align(objList, RelativeTo.SELECTION, Axis.Y, data.y.aligment, false);
                    var rowBounds = GetSelectionBounds(objList, RelativeTo.SELECTION, Axis.Y).Item2;

                    foreach (var obj in objList)
                    {
                        var arrangedPosition = obj.transform.position;
                        var objBounds = GetBoundsRecursive(obj.transform);
                        var objMin = objBounds.min;
                        arrangedPosition.y = obj.transform.position.y - rowBounds.min.y + prevMaxY + data.y.spacing;
                        if (data.y.cellSizeType != CellSizeType.BIGGEST_OBJECT_PER_GROUP && j > 0)
                        {
                            switch (data.y.aligment)
                            {
                                case Bound.CENTER:
                                    arrangedPosition.y += (maxSize.y - rowBounds.size.y) / 2f;
                                    break;
                                case Bound.MAX:
                                    arrangedPosition.y += maxSize.y - rowBounds.size.y;
                                    break;
                                default: break;
                            }
                        }
                        obj.transform.position = arrangedPosition;
                    }
                    rowBounds = GetSelectionBounds(objList, RelativeTo.SELECTION, Axis.Y).Item2;
                    if (data.y.cellSizeType != CellSizeType.BIGGEST_OBJECT_PER_GROUP)
                    {
                        prevMaxY += maxSize.y + data.y.spacing;
                        if (j != 0) continue;
                        switch (data.y.aligment)
                        {
                            case Bound.CENTER:
                                prevMaxY -= (maxSize.y - rowBounds.size.y) / 2f;
                                break;
                            case Bound.MAX:
                                prevMaxY -= (maxSize.y - rowBounds.size.y);
                                break;
                            default: break;
                        }
                    }
                    else
                    {
                        prevMaxY = rowBounds.max.y;
                    }
                }
            }
            ///// Z
            if (data.z.overwrite)
            {
                var prevMaxZ = selectionBounds.min.z - data.z.spacing;
                for (int k = 0; k < data.z.cells; ++k)
                {
                    List<GameObject> objList = new System.Collections.Generic.List<GameObject>();
                    for (int j = 0; j < data.y.cells; ++j)
                    {
                        for (int i = 0; i < data.x.cells; ++i)
                        {
                            if (objDictionary.ContainsKey((i, j, k)))
                            {
                                objList.Add(objDictionary[(i, j, k)]);
                            }
                        }
                    }
                    Align(objList, RelativeTo.SELECTION, Axis.Z, data.z.aligment, false);
                    var columnBounds = GetSelectionBounds(objList, RelativeTo.SELECTION, Axis.Z).Item2;

                    foreach (var obj in objList)
                    {
                        var arrangedPosition = obj.transform.position;
                        var objBounds = GetBoundsRecursive(obj.transform);
                        var objMin = objBounds.min;
                        arrangedPosition.z = obj.transform.position.z - columnBounds.min.z + prevMaxZ + data.z.spacing;
                        if (data.z.cellSizeType != CellSizeType.BIGGEST_OBJECT_PER_GROUP && k > 0)
                        {
                            switch (data.z.aligment)
                            {
                                case Bound.CENTER:
                                    arrangedPosition.z += (maxSize.z - columnBounds.size.z) / 2f;
                                    break;
                                case Bound.MAX:
                                    arrangedPosition.z += maxSize.z - columnBounds.size.z;
                                    break;
                                default: break;
                            }
                        }
                        obj.transform.position = arrangedPosition;
                    }
                    columnBounds = GetSelectionBounds(objList, RelativeTo.SELECTION, Axis.Z).Item2;
                    if (data.z.cellSizeType != CellSizeType.BIGGEST_OBJECT_PER_GROUP)
                    {
                        prevMaxZ += maxSize.z + data.z.spacing;
                        if (k != 0) continue;
                        switch (data.z.aligment)
                        {
                            case Bound.CENTER:
                                prevMaxZ -= (maxSize.z - columnBounds.size.z) / 2f;
                                break;
                            case Bound.MAX:
                                prevMaxZ -= (maxSize.z - columnBounds.size.z);
                                break;
                            default: break;
                        }
                    }
                    else
                    {
                        prevMaxZ = columnBounds.max.z;
                    }
                }
            }
            return true;
        }
        #endregion
        #region REARRANGE
        public static void Rearrange(List<GameObject> selection, ArrangeBy arrangeBy)
        {
            if (selection.Count < 2) return;
            if (arrangeBy == ArrangeBy.HIERARCHY_ORDER)
            {
                selection = SortByHierarchy(selection);
            }
            var firstPosition = selection[0].transform.position;
            for (int i = 0; i < selection.Count - 1; ++i)
            {
                selection[i].transform.position = selection[i + 1].transform.position;
            }
            selection[selection.Count - 1].transform.position = firstPosition;
        }
        #endregion
        #region RADIAL ARRANGE
        public enum RotateAround
        {
            SELECTION_CENTER,
            TRANSFORM_POSITION,
            OBJECT_BOUNDS_CENTER,
            CUSTOM_POSITION
        }

        public enum Shape
        {
            CIRCLE,
            CIRCULAR_SPIRAL,
            ELLIPSE,
            ELLIPTICAL_SPIRAL
        }

        public class RadialArrangeData
        {
            private ArrangeBy _arrangeBy = ArrangeBy.SELECTION_ORDER;
            private RotateAround _rotateAround = RotateAround.SELECTION_CENTER;
            private Transform _centerTransform = null;
            private Vector3 _center = Vector3.zero;
            private Vector3 _axis = Vector3.forward;
            private Shape _shape = Shape.CIRCLE;
            private Vector2 _startEllipseAxes = Vector2.one;
            private Vector2 _endEllipseAxes = Vector2.one;
            private float _startAngle = 0f;
            private float _maxArcAngle = 360f;
            private bool _orientToRadius = false;
            private Vector3 _orientDirection = Vector3.right;
            private Vector3 _parallelDirection = Vector3.up;
            private bool _overwriteX = true;
            private bool _overwriteY = true;
            private bool _overwriteZ = true;
            private bool _lastSpotEmpty = false;

            public ArrangeBy arrangeBy { get => _arrangeBy; set => _arrangeBy = value; }
            public Vector3 axis { get => _axis; set => _axis = value; }
            public Shape shape { get => _shape; set => _shape = value; }
            public Vector2 startEllipseAxes { get => _startEllipseAxes; set => _startEllipseAxes = value; }
            public Vector2 endEllipseAxes { get => _endEllipseAxes; set => _endEllipseAxes = value; }
            public float startAngle { get => _startAngle; set => _startAngle = value; }
            public float maxArcAngle { get => _maxArcAngle; set => _maxArcAngle = value; }
            public bool orientToRadius { get => _orientToRadius; set => _orientToRadius = value; }
            public Vector3 center { get => _center; set => _center = value; }
            public Vector3 orientDirection { get => _orientDirection; set => _orientDirection = value; }
            public Vector3 parallelDirection { get => _parallelDirection; set => _parallelDirection = value; }
            public Transform centerTransform
            {
                get => _centerTransform;
                set
                {
                    if (_centerTransform == value) return;
                    _centerTransform = value;
                    UpdateCenter();
                }
            }
            public RotateAround rotateAround
            {
                get => _rotateAround;
                set
                {
                    if (_rotateAround == value) return;
                    _rotateAround = value;
                    UpdateCenter();
                }
            }

            public bool overwriteX { get => _overwriteX; set => _overwriteX = value; }
            public bool overwriteY { get => _overwriteY; set => _overwriteY = value; }
            public bool overwriteZ { get => _overwriteZ; set => _overwriteZ = value; }
            public bool LastSpotEmpty { get => _lastSpotEmpty; set => _lastSpotEmpty = value; }

            public void UpdateCenter()
            {
                if (_centerTransform == null &&
                    (_rotateAround == RotateAround.TRANSFORM_POSITION
                    || _rotateAround == RotateAround.OBJECT_BOUNDS_CENTER))
                {
                    _center = Vector3.zero;
                }
                else if (_rotateAround == RotateAround.TRANSFORM_POSITION)
                {
                    _center = _centerTransform.transform.position;
                }
                else if (_rotateAround == RotateAround.OBJECT_BOUNDS_CENTER)
                {
                    _center = GetBoundsRecursive(_centerTransform).center;
                }
            }

            public void UpdateCenter(List<GameObject> selection)
            {
                if (_rotateAround != RotateAround.SELECTION_CENTER) return;
                if (selection.Count == 0)
                {
                    _center = Vector3.zero;
                }
                else
                {
                    _center = GetSelectionBounds(selection).center;
                }
            }
        }

        private static float GetEllipseRadius(Vector2 ellipseAxes, float angle)
        {
            if (ellipseAxes.x == ellipseAxes.y) return ellipseAxes.x;
            var a = ellipseAxes.x;
            var b = ellipseAxes.y;
            var sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            var cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            return a * b / Mathf.Sqrt(a * a * sin * sin + b * b * cos * cos);
        }

        private static Vector3 GetRadialPosition(Vector3 center, Vector3 axis, float radius, float angle)
        {
            var radiusDirection = Vector3.right;
            if (axis.x > 0 || axis.y < 0) radiusDirection = Vector3.forward;
            else if (axis.x < 0 || axis.z > 0) radiusDirection = Vector3.up;
            return center + Quaternion.AngleAxis(angle, axis) * radiusDirection * radius;
        }

        public static void RadialArrange(List<GameObject> selection, RadialArrangeData data)
        {
            if (data.arrangeBy == ArrangeBy.HIERARCHY_ORDER) selection = SortByHierarchy(selection);
            data.UpdateCenter();
            var angle = data.startAngle;

            var deltaAngle = data.maxArcAngle / ((float)selection.Count - (data.LastSpotEmpty ? 0f : 1f));
            var ellipseAxes = data.startEllipseAxes;
            var deltaEllipseAxes = (data.endEllipseAxes - data.startEllipseAxes) / ((float)selection.Count - 1);
            foreach (var obj in selection)
            {
                var radius = GetEllipseRadius(ellipseAxes, angle);
                var position = GetRadialPosition(data.center, data.axis, radius, angle);
                obj.transform.position = new Vector3(
                    data.overwriteX ? position.x : obj.transform.position.x,
                    data.overwriteY ? position.y : obj.transform.position.y,
                    data.overwriteZ ? position.z : obj.transform.position.z);
                if (data.orientToRadius)
                {
                    obj.transform.rotation = Quaternion.identity;
                    LookAtCenter(obj.transform, data.center, data.axis, data.orientDirection, data.parallelDirection);
                }
                angle += deltaAngle;
                ellipseAxes += deltaEllipseAxes;
            }
        }
        #endregion
        #region PROGRESSION
        public enum IncrementalDataType
        {
            CONSTANT_DELTA,
            CURVE,
            OBJECT_SIZE
        }

        public enum ArrangeBy
        {
            SELECTION_ORDER,
            HIERARCHY_ORDER
        }

        public class ProgressionAxisData
        {
            private float _constantDelta = 0f;
            private AnimationCurve _curve = AnimationCurve.Constant(0, 1, 0);
            private float _curveRangeMin = 0f;
            private float _curveRangeSize = 0f;
            private Rect _curveRange = new Rect(0, 0, 1, 1);
            private bool _overwrite = true;

            public float constantDelta { get => _constantDelta; set => _constantDelta = value; }
            public AnimationCurve curve { get => _curve; set => _curve = value; }
            public float curveRangeMin { get => _curveRangeMin; set => _curveRangeMin = value; }
            public float curveRangeSize { get => _curveRangeSize; set => _curveRangeSize = value; }
            public Rect curveRange { get => _curveRange; set => _curveRange = value; }
            public bool overwrite { get => _overwrite; set => _overwrite = value; }
        }

        public class ProgressionData
        {
            private ArrangeBy _arrangeOrder = ArrangeBy.HIERARCHY_ORDER;
            private IncrementalDataType _type = IncrementalDataType.CONSTANT_DELTA;
            private ProgressionAxisData _x = new ProgressionAxisData();
            private ProgressionAxisData _y = new ProgressionAxisData();
            private ProgressionAxisData _z = new ProgressionAxisData();

            public ArrangeBy arrangeOrder { get => _arrangeOrder; set => _arrangeOrder = value; }
            public IncrementalDataType type { get => _type; set => _type = value; }
            public Vector3 constantDelta
            {
                get => new Vector3(_x.constantDelta, _y.constantDelta, _z.constantDelta);
                set
                {
                    _x.constantDelta = value.x;
                    _y.constantDelta = value.y;
                    _z.constantDelta = value.z;
                }
            }
            public Vector3 curveRangeMin
            {
                get => new Vector3(_x.curveRangeMin, _y.curveRangeMin, _z.curveRangeMin);
                set
                {
                    if (new Vector3(_x.curveRangeMin, _y.curveRangeMin, _z.curveRangeMin) == value) return;
                    var rangeX = _x.curveRange;
                    rangeX.yMin = _x.curveRangeMin = value.x;
                    _x.curveRange = rangeX;
                    var rangeY = _y.curveRange;
                    rangeY.yMin = _y.curveRangeMin = value.y;
                    _y.curveRange = rangeY;
                    var rangeZ = _z.curveRange;
                    rangeZ.yMin = _z.curveRangeMin = value.z;
                    _z.curveRange = rangeZ;
                    UpdateRanges();
                }
            }
            public Vector3 curveRangeSize
            {
                get => new Vector3(_x.curveRangeSize, _y.curveRangeSize, _z.curveRangeSize);
                set
                {
                    if (new Vector3(_x.curveRangeSize, _y.curveRangeSize, _z.curveRangeSize) == value) return;
                    _x.curveRangeSize = value.x;
                    _y.curveRangeSize = value.y;
                    _z.curveRangeSize = value.z;
                    UpdateRanges();
                }
            }

            public ProgressionAxisData x { get => _x; set => _x = value; }
            public ProgressionAxisData y { get => _y; set => _y = value; }
            public ProgressionAxisData z { get => _z; set => _z = value; }

            private void UpdateRanges()
            {
                var rangeX = _x.curveRange;
                rangeX.yMax = _x.curveRangeMin + _x.curveRangeSize;
                _x.curveRange = rangeX;
                var rangeY = _y.curveRange;
                rangeY.yMax = _y.curveRangeMin + _y.curveRangeSize;
                _y.curveRange = rangeY;
                var rangeZ = _z.curveRange;
                rangeZ.yMax = _z.curveRangeMin + _z.curveRangeSize;
                _z.curveRange = rangeZ;
            }

            public Vector3 EvaluateCurve(float t)
            {
                return new Vector3(
                    _x.overwrite ? _x.curve.Evaluate(t) : 0f,
                    _y.overwrite ? _y.curve.Evaluate(t) : 0f,
                    _z.overwrite ? _z.curve.Evaluate(t) : 0f);
            }

            public Rect GetRect(Axis axis)
            {
                switch (axis)
                {
                    case Axis.X: return _x.curveRange;
                    case Axis.Y: return _y.curveRange;
                    default: return _z.curveRange;
                }
            }
        }
        private static int[] GetHierarchyIndex(GameObject obj)
        {
            var idxList = new List<int>();
            var parent = obj.transform;
            do
            {
                idxList.Insert(0, parent.transform.GetSiblingIndex());
                parent = parent.transform.parent;
            }
            while (parent != null);
            return idxList.ToArray();
        }

        public static void PositionProgression(List<GameObject> selection, ProgressionData data, bool orientToPath, Vector3 orientation)
        {
            if (selection.Count < 2) return;
            if (data.arrangeOrder == ArrangeBy.HIERARCHY_ORDER)
            {
                selection = SortByHierarchy(selection);
            }
            var position = selection[0].transform.position;
            var t = 0f;
            var delta = 1f / ((float)selection.Count - 1f);
            var i = 0;
            GameObject prevObj = null;
            foreach (var obj in selection)
            {
                var bounds = GetBoundsRecursive(obj.transform);
                var centerLocalPos = obj.transform.TransformVector(obj.transform.InverseTransformPoint(bounds.center));

                if (i > 0 && data.type == IncrementalDataType.OBJECT_SIZE)
                {
                    position += bounds.size / 2f - centerLocalPos;
                }
                ++i;
                obj.transform.position = new Vector3(
                    data.x.overwrite ? position.x : obj.transform.position.x,
                    data.y.overwrite ? position.y : obj.transform.position.y,
                    data.z.overwrite ? position.z : obj.transform.position.z);
                t += delta;

                position = data.type == IncrementalDataType.CONSTANT_DELTA
                    ? position + data.constantDelta
                    : data.type == IncrementalDataType.CURVE
                        ? selection[0].transform.position + data.EvaluateCurve(t)
                        : position + centerLocalPos + bounds.size / 2f;

                if (!orientToPath) continue;
                if (data.type != IncrementalDataType.OBJECT_SIZE)
                {
                    LookAtNext(obj.transform, position, orientation);
                }
                else if(i > 1)
                {
                    LookAtNext(prevObj.transform, obj.transform.position, orientation);
                }
                if (data.type == IncrementalDataType.OBJECT_SIZE && i == selection.Count)
                {
                    obj.transform.eulerAngles = prevObj.transform.eulerAngles;
                }
                prevObj = obj;
            }
        }

        private static void LookAtNext(Transform transform, Vector3 next, Vector3 orientation)
        {
            var objToCenter = next - transform.position;
            transform.rotation = Quaternion.FromToRotation(orientation, objToCenter);
        }

        public static void RotationProgression(List<GameObject> selection, ProgressionData data)
        {
            if (selection.Count < 2) return;
            if (data.arrangeOrder == ArrangeBy.HIERARCHY_ORDER)
            {
                selection = SortByHierarchy(selection);
            }
            var eulerAngles = selection[0].transform.rotation.eulerAngles;
            var firstObjEulerAngles = eulerAngles;
            var t = 0f;
            foreach (var obj in selection)
            {
                if (data.type == IncrementalDataType.CURVE)
                {
                    eulerAngles = firstObjEulerAngles + data.EvaluateCurve(t);
                    t += 1f / ((float)selection.Count - 1f);
                }
                obj.transform.rotation = Quaternion.Euler(
                    data.x.overwrite ? eulerAngles.x : obj.transform.rotation.eulerAngles.x,
                    data.y.overwrite ? eulerAngles.y : obj.transform.rotation.eulerAngles.y,
                    data.z.overwrite ? eulerAngles.z : obj.transform.rotation.eulerAngles.z);
                if (data.type == IncrementalDataType.CONSTANT_DELTA)
                {
                    eulerAngles += data.constantDelta;
                }
            }
        }

        public static void ScaleProgression(List<GameObject> selection, ProgressionData data)
        {
            if (selection.Count < 2) return;
            if (data.arrangeOrder == ArrangeBy.HIERARCHY_ORDER)
            {
                selection = SortByHierarchy(selection);
            }
            var scale = selection[0].transform.localScale;
            var firstObjScale = scale;
            var t = 0f;
            foreach (var obj in selection)
            {
                if(data.type == IncrementalDataType.CURVE)
                {
                    scale = firstObjScale + data.EvaluateCurve(t);
                    t += 1f / ((float)selection.Count - 1f);
                }
                
                obj.transform.localScale = new Vector3(
                    data.x.overwrite ? scale.x : obj.transform.localScale.x,
                    data.y.overwrite ? scale.y : obj.transform.localScale.y,
                    data.z.overwrite ? scale.z : obj.transform.localScale.z);

                if(data.type == IncrementalDataType.CONSTANT_DELTA)
                {
                    scale += data.constantDelta;
                }
            }
        }
        #endregion
        #region RANDOMIZE
        public static class RandomUtils
        {
            [Serializable]
            public class Range
            {
                [SerializeField] private float _min = -1f;
                [SerializeField] private float _max = 1f;

                public Range() { }
                public Range(Range other) => (_min, _max) = (other._min, other.max);
                public Range(float min, float max) => (_min, _max) = (min, max);

                public float min
                {
                    get => _min;
                    set
                    {
                        if (_min == value) return;
                        _min = value;
                        if (_min > _max)
                        {
                            _max = _min;
                        }
                    }
                }
                public float max
                {
                    get => _max;
                    set
                    {
                        if (_max == value) return;
                        _max = value;
                        if (_max < _min)
                        {
                            _min = _max;
                        }
                    }
                }

                public override int GetHashCode()
                {
                    int hashCode = -1605643878;
                    hashCode = hashCode * -1521134295 + _min.GetHashCode();
                    hashCode = hashCode * -1521134295 + _max.GetHashCode();
                    return hashCode;
                }
                public override bool Equals(object obj) => obj is Range range && _min == range._min;
                public static bool operator ==(Range value1, Range value2) => Equals(value1, value2);
                public static bool operator !=(Range value1, Range value2) => !Equals(value1, value2);

                public float randomValue => UnityEngine.Random.Range(min, max);

            }

            [Serializable]
            public class Range3
            {
                public Range x = new Range(0, 0);
                public Range y = new Range(0, 0);
                public Range z = new Range(0, 0);

                public Range3(Vector3 min, Vector3 max)
                {
                    x = new Range(min.x, max.x);
                    y = new Range(min.y, max.y);
                    z = new Range(min.z, max.z);
                }

                public Range3(Range3 other)
                {
                    x = new Range(other.x);
                    y = new Range(other.y);
                    z = new Range(other.z);
                }
                public Vector3 min
                {
                    get => new Vector3(x.min, y.min, z.min);
                    set
                    {
                        x.min = value.x;
                        y.min = value.y;
                        z.min = value.z;
                    }
                }

                public Vector3 max
                {
                    get => new Vector3(x.max, y.max, z.max);
                    set
                    {
                        x.max = value.x;
                        y.max = value.y;
                        z.max = value.z;
                    }
                }

                public override int GetHashCode()
                {
                    int hashCode = 373119288;
                    hashCode = hashCode * -1521134295 + x.GetHashCode();
                    hashCode = hashCode * -1521134295 + y.GetHashCode();
                    hashCode = hashCode * -1521134295 + z.GetHashCode();
                    return hashCode;
                }
                public override bool Equals(object obj) => obj is Range3 range3 && x == range3.x && y == range3.y && z == range3.z;
                public static bool operator ==(Range3 value1, Range3 value2) => Equals(value1, value2);
                public static bool operator !=(Range3 value1, Range3 value2) => !Equals(value1, value2);

                public Vector3 randomVector => new Vector3(x.randomValue, y.randomValue, z.randomValue);
            }
        }

        public class RandomizeAxisData
        {
            private bool _randomizeAxis = true;
            private RandomUtils.Range _offset = new RandomUtils.Range();
            public bool randomizeAxis { get => _randomizeAxis; set => _randomizeAxis = value; }
            public RandomUtils.Range offset { get => _offset; set => _offset = value; }

        }
        public class RandomizeData
        {
            private RandomizeAxisData _x = new RandomizeAxisData();
            private RandomizeAxisData _y = new RandomizeAxisData();
            private RandomizeAxisData _z = new RandomizeAxisData();

            public RandomizeAxisData x { get => _x; set => _x = value; }
            public RandomizeAxisData y { get => _y; set => _y = value; }
            public RandomizeAxisData z { get => _z; set => _z = value; }
        }
        public static void RandomizePositions(GameObject[] selection, RandomizeData data)
        {
            foreach (var obj in selection)
            {
                obj.transform.position += new Vector3(
                    data.x.randomizeAxis ? data.x.offset.randomValue : 0f,
                    data.y.randomizeAxis ? data.y.offset.randomValue : 0f,
                    data.z.randomizeAxis ? data.z.offset.randomValue : 0f);
            }
        }

        public static void RandomizeRotations(GameObject[] selection, RandomizeData data)
        {
            foreach (var obj in selection)
            {
                obj.transform.Rotate(
                    data.x.randomizeAxis ? data.x.offset.randomValue : 0f,
                    data.y.randomizeAxis ? data.y.offset.randomValue : 0f,
                    data.z.randomizeAxis ? data.z.offset.randomValue : 0f);
            }
        }

        public static void RandomizeScales(GameObject[] selection, RandomizeData data, bool separateAxes)
        {
            foreach (var obj in selection)
            {
                if (separateAxes)
                {
                    obj.transform.localScale += new Vector3(
                        data.x.randomizeAxis ? data.x.offset.randomValue : obj.transform.localScale.x,
                        data.y.randomizeAxis ? data.y.offset.randomValue : obj.transform.localScale.y,
                        data.z.randomizeAxis ? data.z.offset.randomValue : obj.transform.localScale.z);
                }
                else
                {
                    var value = data.x.offset.randomValue;
                    obj.transform.localScale += new Vector3(value, value, value);
                }
            }
        }
        #endregion
        #region PLACE ON SURFACE
        public class PlaceOnSurfaceData
        {
            private Space _projectionDirectionSpace = Space.Self;
            private Vector3 _projectionDirection = Vector3.down;
            private bool _rotateToSurface = true;
            private Vector3 _objectOrientation = Vector3.down;
            private float _surfaceDistance = 0f;
            private LayerMask _mask = ~0;
            public bool rotateToSurface { get => _rotateToSurface; set => _rotateToSurface = value; }
            public Vector3 objectOrientation { get => _objectOrientation; set => _objectOrientation = value; }
            public float surfaceDistance { get => _surfaceDistance; set => _surfaceDistance = value; }
            public Vector3 projectionDirection { get => _projectionDirection; set => _projectionDirection = value; }
            public Space projectionDirectionSpace { get => _projectionDirectionSpace; set => _projectionDirectionSpace = value; }
            public LayerMask mask { get => _mask; set => _mask = value; }
        }

        private static (Vector3 vertex, Transform transform)[] GetDirectionVertices(Transform target, Vector3 worldProjDir)
        {
            var children = Array.FindAll(target.GetComponentsInChildren<MeshFilter>(), filter => filter != null && filter.sharedMesh != null).Select(filter => (filter.transform, filter.sharedMesh)).ToArray();
            var maxSqrDistance = float.MinValue;
            var bounds = GetBoundsRecursive(target);
            var vertices = new List<(Vector3 vertex, Transform transform)>() { (bounds.center, target) };
            foreach (var child in children)
            {
                foreach (var vertex in child.sharedMesh.vertices)
                {
                    var centerToVertex = child.transform.TransformPoint(vertex) - bounds.center;
                    var projection = Vector3.Project(centerToVertex, worldProjDir);
                    var sqrDistance = projection.sqrMagnitude * (projection.normalized != worldProjDir.normalized ? -1 : 1);
                    var vertexTrans = (vertex, child.transform);
                    if (sqrDistance > maxSqrDistance)
                    {
                        vertices.Clear();
                        maxSqrDistance = sqrDistance;
                        vertices.Add(vertexTrans);
                    }
                    else if (sqrDistance + 0.001 >= maxSqrDistance)
                    {
                        if (vertices.Exists(item => item.vertex == vertexTrans.vertex)) continue;
                        vertices.Add(vertexTrans);
                    }
                }
            }
            return vertices.ToArray();
        }

        private static void PlaceOnSurface(Transform target, PlaceOnSurfaceData data, MeshFilter[] filters)
        {
            var worldProjDir = (data.projectionDirectionSpace == Space.World
                ? data.projectionDirection
                : target.TransformDirection(data.projectionDirection)).normalized;

            var originalPosition = target.position;
            var originalRotation = target.rotation;
            if (data.rotateToSurface)
            {
                var worldOrientDir = target.TransformDirection(data.objectOrientation);
                var orientAngle = Vector3.Angle(worldOrientDir, worldProjDir);
                var cross = Vector3.Cross(worldOrientDir, worldProjDir);
                if (cross == Vector3.zero)
                {
                    cross = target.TransformDirection(data.objectOrientation.y != 0 ? Vector3.forward : data.objectOrientation.z != 0 ? Vector3.right : Vector3.up);
                    orientAngle = worldOrientDir == worldProjDir ? 0 : 180;
                }
                target.Rotate(cross, orientAngle);
            }

            var dirVert = GetDirectionVertices(target, worldProjDir);
            var minDistance = float.MaxValue;
            var closestVertexInfoList = new List<((Vector3 vertex, Transform transform), RaycastHit hitInfo)>();
            foreach (var vertexTransform in dirVert)
            {
                RaycastHit hitInfo;
                var rayOrigin = vertexTransform.transform.TransformPoint(vertexTransform.vertex);
                if (!Physics.Raycast(rayOrigin, worldProjDir, out hitInfo, float.MaxValue, data.mask)) continue;
                if (hitInfo.distance < minDistance)
                {
                    minDistance = hitInfo.distance;
                    closestVertexInfoList.Clear();
                    closestVertexInfoList.Add((vertexTransform, hitInfo));
                }
                else if (hitInfo.distance - 0.001 <= minDistance)
                {
                    closestVertexInfoList.Add((vertexTransform, hitInfo));
                }
            }
            if (closestVertexInfoList.Count == 0)
            {
                target.SetPositionAndRotation(originalPosition, originalRotation);
                return;
            }
            var averageWorldVertex = Vector3.zero;
            var averageHitPoint = Vector3.zero;
            var averageNormal = Vector3.zero;
            foreach (var vertInfo in closestVertexInfoList)
            {
                averageWorldVertex += vertInfo.Item1.transform.TransformPoint(vertInfo.Item1.vertex);
                averageHitPoint += vertInfo.hitInfo.point;
                averageNormal += vertInfo.hitInfo.normal;
            }
            averageWorldVertex /= closestVertexInfoList.Count;
            var averageVertex = target.InverseTransformPoint(averageWorldVertex);
            averageHitPoint /= closestVertexInfoList.Count;
            averageNormal /= closestVertexInfoList.Count;

            if (data.rotateToSurface)
            {
                var worldOrientDir = target.TransformDirection(-data.objectOrientation);
                var angle = Vector3.Angle(worldOrientDir, averageNormal);
                var cross = Vector3.Cross(worldOrientDir, averageNormal);
                if (cross != Vector3.zero)
                {
                    target.RotateAround(target.TransformPoint(averageVertex), cross, angle);
                }
            }

            target.position = averageHitPoint - target.TransformVector(averageVertex) - worldProjDir * data.surfaceDistance;
        }

        public static void PlaceOnSurface(GameObject[] selection, PlaceOnSurfaceData data)
        {
            var ignoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
            var layerDictionary = new Dictionary<GameObject, int>();
            foreach (var obj in selection)
            {
                var children = obj.transform.GetComponentsInChildren<Transform>(true);
                foreach (var child in children)
                {
                    layerDictionary.Add(child.gameObject, child.gameObject.layer);
                    child.gameObject.layer = ignoreRaycast;
                }
            }
            foreach (var obj in selection)
            {
                PlaceOnSurface(obj.transform, data, null);
            }

            foreach (var item in layerDictionary)
            {
                item.Key.layer = item.Value;
            }
        }
        #endregion
        #region UTILS
        private static int CompareHierarchyIndex(GameObject obj1, GameObject obj2)
        {
            var idx1 = GetHierarchyIndex(obj1);
            var idx2 = GetHierarchyIndex(obj2);
            var depth = 0;
            do
            {
                if (idx1.Length <= depth)
                {
                    return -1;
                }
                if (idx2.Length <= depth)
                {
                    return 1;
                }
                var result = idx1[depth].CompareTo(idx2[depth]);
                if (result != 0)
                {
                    return result;
                }
                ++depth;
            }
            while (true);
        }

        private static List<GameObject> SortByHierarchy(List<GameObject> selection)
        {
            selection.Sort((obj1, obj2) => CompareHierarchyIndex(obj1, obj2));
            return selection;
        }

        private static void LookAtCenter(Transform transform, Vector3 center, Vector3 axis,  Vector3 orientation, Vector3 parallelAxis)
        {
            transform.rotation = Quaternion.FromToRotation(parallelAxis, axis);
            var worldOrientation = transform.TransformDirection(orientation);
            var objToCenter = center - transform.position;
            var angle = Vector3.Angle(worldOrientation, objToCenter);
            var cross = Vector3.Cross(worldOrientation, objToCenter);
            if(cross == Vector3.zero) cross = axis;
            transform.Rotate(cross, angle, Space.World);
        }
        #endregion
    }
}