using System.Collections.Generic;

namespace Data.Types
{
    public class ActorGraph
    {
        private BitMatrix _nodes;
        private List<Actor> _actors;

        private int _currentActorNode;

        public ActorGraph(int countActors)
        {
            _nodes = new BitMatrix(countActors);
            _currentActorNode = 0;

            _actors = new List<Actor>(countActors);

            for (int i = 0; i < countActors; i++)
            {
                _actors.Add(null);
            }
        }

        public void SetActorToGraph(Actor actor, int nodeNumber)
        {
            ClearNode(nodeNumber);

            _actors[nodeNumber] = actor;
        }

        public int GetActorsCount()
        {
            int count = 0;

            foreach (Actor actor in _actors)
            {
                if (actor != null) count++;
            }

            return count;
        }

        public bool TrySetActorToGraph(Actor actor, int nodeNumber)
        {
            if(nodeNumber > _actors.Count - 1)
            {
                return false;
            }

            SetActorToGraph(actor, nodeNumber);

            return true;
        }

        public bool TryAddActorToGraph(Actor actor)
        {
            if (_currentActorNode > _actors.Count - 1)
            {
                int emptySlotIndex = GetActorEmptySlot();

                if (emptySlotIndex == -1)
                    return false;

                ClearNode(emptySlotIndex);

                _actors[emptySlotIndex] = actor;
                actor.SetNodeIndex(emptySlotIndex);

                return true;

            }else
            {
                _actors[_currentActorNode] = actor;
                
                actor.SetNodeIndex(_currentActorNode);

                _currentActorNode++;

                return true;
            }
        }

        public bool TryActorConnect(int nodeA, int nodeB)
        {
            return _nodes.TrySetValue(nodeA, nodeB, true);
        }

        public int GetChainLegthByActor(int nodeTarget, int escapeNode, ref List<int> chainActors)
        {
            int count = 1;
            chainActors.Add(nodeTarget);

            for (int i = 0; i < _nodes.Dimensions; i++)
            {
                if (i == escapeNode || _actors[i] == null || _actors[nodeTarget] == null)
                    continue;

                if(_nodes.GetValue(nodeTarget, i))
                {
                    if(_actors[nodeTarget].ActorType == _actors[i].ActorType)
                    {
                        count += GetChainLegthByActor(i, nodeTarget, ref chainActors);
                    }
                }
            }

            return count;
        }

        public int GetConnectedCount(int node)
        {
            int count = 0;

            for (int i = 0; i < _nodes.Dimensions; i++)
            {
                if(_nodes.TryGetValue(node, i, out bool value))
                {
                    count++;
                }
            }

            return count;
        }

        public Actor TryGetActorByNodeIndex(int node)
        {
            if (node < _actors.Count)
                return _actors[node];

            return null;
        }

        public void DisconnectNodes(int nodeA, int nodeB)
        {
            _nodes.SetValue(nodeA, nodeB, false);
            _nodes.SetValue(nodeB, nodeA, false);
        }

        public void ClearNode(int row, bool actorCleaning = false)
        {
            _nodes.ClearAllRow(row);
            _nodes.ClearAllColumn(row);

            if (actorCleaning)
                _actors[row] = null;
        }

        private int GetActorEmptySlot()
        {
            for (int i = 0; i < _actors.Count; i++)
            {
                if (_actors[i] == null)
                    return i;
            }

            return -1;
        }

    }
}
