using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class PathFinding : MonoBehaviour
{
	private PathRequestManager requestManager;
	private Grids grid;

	private void Awake()
	{
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grids>();
	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos)
	{
		StartCoroutine(FindPath(startPos, targetPos));
	}

	private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Stopwatch sw = new Stopwatch();
		sw.Start();

		Vector3[] wayPoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		if (startNode.walkable && targetNode.walkable)
		{
			// open list and closed list
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node>();

			// add the start node to open list
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);


				// find path
				if (currentNode == targetNode)
				{
					sw.Stop();
					print("Path found : " + sw.ElapsedMilliseconds + "ms");
					pathSuccess = true;
					break;
				}

				foreach (Node neighbour in grid.GetNeighbours(currentNode))
				{
					// if neighbour is not traversable or neighbour is in closed list
					if (!neighbour.walkable || closedSet.Contains(neighbour))
						continue;

					// if new path to neighbour is shorter OR neighbour is not in open list
					int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;

						// if neighbour is not in open list
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else
							openSet.UpdateItem(neighbour);
					}
				}
			}
		}

		yield return null;

		if(pathSuccess)
			wayPoints = RetracePath(startNode, targetNode);

		requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
	}

	private Vector3[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] wayPoints = SimplifyPath(path);
		Array.Reverse(wayPoints);

		return wayPoints;
	}

	Vector3[] SimplifyPath(List<Node> path)
	{
		List<Vector3> wayPoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++)
		{
			Vector2 directionNew = new Vector2 (path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
			if(directionNew != directionOld)
			{
				wayPoints.Add(path[i - 1].worldPosition);
			}

			directionOld = directionNew;
		}

		return wayPoints.ToArray();
	}

	private int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}
