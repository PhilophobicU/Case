using System.Collections.Generic;
public interface IPathfindingHelper { // Try to simplfy more

    List<GridObject> EnvironmentGrids(List<GridObject> occupiedGrids);
    int CalculateDistanceCost(GridObject a, GridObject b); // calculate Cost between 2 grid

    int GetWidth();
    int GetHeight();
    GridObject GetNode(int x, int y); // while calculating neighbours 
    GridObject GetLowestFCostNode(List<GridObject> pathNodeList);   // while calculating path

    List<GridObject> GetNeighbourList(GridObject currentNode); // get  grid neighbors

    List<GridObject> CalculatePath(GridObject endNode);         // last calculation from end to start reversed

    GridObject CalculateClosestGrid(List<GridObject> occupiedGrids, GridObject currentGrid);
}
