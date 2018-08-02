using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using Utility;

namespace PathSolution {
	public class Dijkstra {
		Vector2 originTile;
		Grid mMap;

		public Dijkstra(Grid p_map) {
			mMap = p_map;
		}

		//Find Walkable Node
		public List<Node> findConnectNode(BaseUnit p_target) {
			List<Node> closeNode  = new List<Node>();
			List<Node> openNode  = new List<Node>();

			Node startGrid = mMap.NodeFromWorldPoint(p_target.transform.position);
			startGrid.costSoFar = 0;
			openNode.Add(startGrid);

			int movePoint = p_target._view_range;


			while (openNode.Count > 0) {
				Node node = openNode.First();
				List<Node> neighborNodes = GetNeighbour(node);

				for (int i = 0; i < neighborNodes.Count; i++) {
					Node refilterN = neighborNodes[i];
					int p_costSoFar = node.costSoFar + refilterN.cost;

					if (closeNode.Contains(refilterN) || p_costSoFar > movePoint)  continue;
					if (openNode.Contains(neighborNodes[i])) {
						
					} else {
						refilterN.costSoFar = p_costSoFar;
						openNode.Add(neighborNodes[i]);
					}
				}

				openNode.Remove(node);
				closeNode.Add(node);
			}
			
			return closeNode;
		}

		private List<Node> GetNeighbour(Node dot) {
				List<Vector3> tempNodeList = new List<Vector3>();
				tempNodeList.Add(new Vector3(dot.worldPosition.x+1, dot.worldPosition.y,  dot.worldPosition.z));
				tempNodeList.Add(new Vector3(dot.worldPosition.x-1, dot.worldPosition.y, dot.worldPosition.z));
				tempNodeList.Add(new Vector3(dot.worldPosition.x, dot.worldPosition.y, dot.worldPosition.z+1));
				tempNodeList.Add(new Vector3(dot.worldPosition.x, dot.worldPosition.y, dot.worldPosition.z-1));

				List<Node> tempNodeList2 = new List<Node>();	

				//Check if the tempNode is valid and not exist in nodeStorage
				for (int i = 0; i < tempNodeList.Count; i++) {
					Node refilterN = mMap.NodeFromWorldPoint(tempNodeList[i]);

					if ( refilterN.cost < 0) {
						tempNodeList2.Add(refilterN);
					}
				}
			return tempNodeList2;
		}



	}
}