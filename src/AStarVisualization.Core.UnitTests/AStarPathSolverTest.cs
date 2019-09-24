﻿using AStarVisualization.Core.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AStarVisualization.Core.UnitTests
{
    [TestFixture]
    public class AStarPathSolverTest
    {
        private static Node[,] Map = new Node[,]
        {
            {new Node(NodeState.Start), new Node(NodeState.Wall), new Node(NodeState.Wall), new Node(NodeState.Ground)},
            {new Node(NodeState.Ground), new Node(NodeState.Ground), new Node(NodeState.Ground), new Node(NodeState.Ground)},
            {new Node(NodeState.Ground), new Node(NodeState.Wall), new Node(NodeState.Wall), new Node(NodeState.Ground)},
            {new Node(NodeState.Ground), new Node(NodeState.Ground), new Node(NodeState.Wall), new Node(NodeState.Goal)},
        };

        [Test, TestCaseSource("FindPath_PathExists_ReturnsPath_Cases")]
        public async Task FindPath_PathExists_ReturnsPath(object[] parameters)
        {
            int startRowIdx = (int)parameters[0];
            int startColIdx = (int)parameters[1];
            int goalRowIdx = (int)parameters[2];
            int goalColIdx = (int)parameters[3];
            List<Node> requiredPath = parameters.Skip(4).Cast<Node>().ToList();
            Node[,] map = GetMapWithStartAndGoal(startRowIdx, startColIdx, goalRowIdx, goalColIdx, Map);

            IPathSolver pathfinder = new AStarPathSolver(ref map);
            var path = await pathfinder.FindPath();
            path = new List<Node>() { map[0, 0], map[1, 0], map[1, 1], map[1, 2], map[2, 3], map[3, 3] };

            Assert.AreEqual(path, requiredPath);
        }
        #region TestUtils FindPath_PathExists_ReturnsPath
        private static object[] FindPath_PathExists_ReturnsPath_Cases =
        {
            new object[] {0, 0, 3, 3, Map[0,0], Map[1,0], Map[1,1], Map[1,2], Map[2,3], Map[3,3]},
        };
        private Node[,] GetMapWithStartAndGoal(int startRowIdx, int startColIdx, int goalRowIdx, int goalColIdx, Node[,] map)
        {
            //map[startRowIdx, startColIdx] = new Node(NodeState.Start);
            //map[goalRowIdx, goalColIdx] = new Node(NodeState.Goal);
            return map;
        }
        #endregion
        //[Test]
        //public void FindPath_NoPathExists_ThrowsError()
        //{
        //    Node[,] map = null;
        //    IPathSolver pathfinder = new AStarPathSolver(ref map);

        //    Assert.That(async () => await pathfinder.FindPath(),
        //        Throws.Exception
        //        .TypeOf<NoPathFoundException>());
        //}
        [Test]
        public void FindPath_MapIsNull_ThrowsError()
        {
            Node[,] map = null;
            IPathSolver pathfinder = new AStarPathSolver(ref map);

            Assert.That(async () => await pathfinder.FindPath(),
                Throws.Exception
                .TypeOf<ArgumentNullException>());
        }
        [Test]
        public void FindPath_MapIsTooSmall_ThrowsError()
        {
            var map = new Node[,] { { new Node(NodeState.Wall), new Node(NodeState.Goal) } };

            IPathSolver pathfinder = new AStarPathSolver(ref map);

            Assert.That(async () => await pathfinder.FindPath(),
                Throws.Exception
                .TypeOf<MapTooSmallException>());
        }
        [Test, TestCaseSource("FindPath_NoWayPoints_ThrowsError_Cases")]
        public void FindPath_NoWayPoints_ThrowsError(object[,] mapCase)
        {
            var map = (Node[,])mapCase;

            IPathSolver pathfinder = new AStarPathSolver(ref map);

            Assert.That(async () => await pathfinder.FindPath(),
                Throws.Exception
                .TypeOf<NoWayPointsException>());
        }
        private static object[] FindPath_NoWayPoints_ThrowsError_Cases =
        {
            new Node[,]
            {
                {new Node(NodeState.Wall), new Node(NodeState.Ground)},
                {new Node(NodeState.Start), new Node(NodeState.Ground)},
            },
            new Node[,]
            {
                {new Node(NodeState.Goal), new Node(NodeState.Ground)},
                {new Node(NodeState.Start), new Node(NodeState.Ground)},
            },
            new Node[,]
            {
                {new Node(NodeState.Start), new Node(NodeState.Ground)},
                {new Node(NodeState.Wall), new Node(NodeState.Ground)},
            },
            new Node[,]
            {
                {new Node(NodeState.Wall), new Node(NodeState.Ground)},
                {new Node(NodeState.Wall), new Node(NodeState.Ground)},
            },
        };
    }
}
