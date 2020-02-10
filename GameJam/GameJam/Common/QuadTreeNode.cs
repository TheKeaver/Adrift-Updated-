using Audrey;
using GameJam.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Common
{
    public class QuadTreeNode
    {
        public QuadTreeNode parent;
        public BoundingRect boundingRect;

        public bool allNodes = false;
        public List<Entity> leaves;
        public int maxLeavesBeforeSubTrees;
        public List<QuadTreeNode> subNodes;

        public QuadTreeNode(BoundingRect bounds)
        {
            parent = null;
            boundingRect = bounds;
            leaves = new List<Entity>();
            maxLeavesBeforeSubTrees = CVars.Get<int>("quad_tree_max_references");
            subNodes = new List<QuadTreeNode>(4);
        }
        public QuadTreeNode(BoundingRect bounds, QuadTreeNode parent)
        {
            this.parent = parent;
            boundingRect = bounds;
            leaves = new List<Entity>();
            maxLeavesBeforeSubTrees = CVars.Get<int>("quad_tree_max_references");
            subNodes = new List<QuadTreeNode>(4);
        }

        public void AddReference(Entity entity)
        {
            if (!allNodes && leaves.Count < maxLeavesBeforeSubTrees)
            {
                leaves.Add(entity);
                entity.GetComponent<QuadTreeReferenceComponent>().node = this;
            }
            else
            {
                if(!allNodes)
                {
                    subNodes.Add(new QuadTreeNode(new BoundingRect(boundingRect.Left,
                                                                    boundingRect.Top,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2),
                                                                    this));
                    subNodes.Add(new QuadTreeNode(new BoundingRect(boundingRect.Center.X,
                                                                    boundingRect.Top,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2),
                                                                    this));
                    subNodes.Add(new QuadTreeNode(new BoundingRect(boundingRect.Left,
                                                                    boundingRect.Center.Y,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2),
                                                                    this));
                    subNodes.Add(new QuadTreeNode(new BoundingRect(boundingRect.Center.X,
                                                                    boundingRect.Center.Y,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2),
                                                                    this));
                    allNodes = true;
                    AddReference(entity);
                    List<Entity> temp = leaves;
                    leaves.Clear();
                    foreach (Entity e in temp)
                    {
                        AddReference(e);
                    }
                }
                else
                {
                    TransformComponent xfrom = entity.GetComponent<TransformComponent>();
                    bool noIntersect = true;
                    foreach (QuadTreeNode qtn in subNodes)
                    {
                        if (qtn.boundingRect.Contains(entity.GetComponent<CollisionComponent>()
                                                        .GetAABB((float)Math.Cos(xfrom.Rotation),
                                                        (float)Math.Sin(xfrom.Rotation),
                                                        xfrom.Scale)));
                        {
                            qtn.AddReference(entity);
                            noIntersect = false;
                        }
                    }
                    if (noIntersect)
                    {
                        leaves.Add(entity);
                    }
                }
            }
        }
    }
}
