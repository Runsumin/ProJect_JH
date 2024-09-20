using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    public interface INode
    {
        public enum eNodeState { Running, Success, Failure }

        // 현재 노드 상태 반환
        public eNodeState Evaluate();
    }

    // 실제 행위 하는 노드
    public class ActionNode : INode
    {
        Func<INode.eNodeState> _onUpdate = null;

        public ActionNode(Func<INode.eNodeState> onUpdate)
        {
            _onUpdate = onUpdate;
        }

        public INode.eNodeState Evaluate() => _onUpdate?.Invoke() ?? INode.eNodeState.Failure;
    }

    public class SelectorNode : INode
    {
        List<INode> _childs;

        public SelectorNode(List<INode> childs)
        {
            _childs = childs;
        }

        public INode.eNodeState Evaluate()
        {
            if (_childs == null)
                return INode.eNodeState.Failure;

            foreach (var child in _childs)
            {
                switch (child.Evaluate())
                {
                    case INode.eNodeState.Running:
                        return INode.eNodeState.Running;
                    case INode.eNodeState.Success:
                        return INode.eNodeState.Success;
                }
            }

            return INode.eNodeState.Failure;
        }
    }

    public class SequenceNode : INode
    {
        List<INode> _childs;

        public SequenceNode(List<INode> childs)
        {
            _childs = childs;
        }

        public INode.eNodeState Evaluate()
        {
            if (_childs == null || _childs.Count == 0)
                return INode.eNodeState.Failure;

            foreach (var child in _childs)
            {
                switch (child.Evaluate())
                {
                    case INode.eNodeState.Running:
                        return INode.eNodeState.Running;
                    case INode.eNodeState.Success:
                        continue;
                    case INode.eNodeState.Failure:
                        return INode.eNodeState.Failure;
                }
            }

            return INode.eNodeState.Success;
        }
    }

    public class BehaviorTreeRunner
    {
        INode _rootNode;
        public BehaviorTreeRunner(INode rootNode)
        {
            _rootNode = rootNode;
        }

        public void Operate()
        {
            _rootNode.Evaluate();
        }
    }
}