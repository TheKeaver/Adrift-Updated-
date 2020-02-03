using Audrey;
using GameJam.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Common
{
    public class QuadTreeNode
    {
        public BoundingRect boundingRect;

        public bool allNodes = false;
        public List<Entity> leaves;
        public List<QuadTreeNode> subNodes;

        public QuadTreeNode(BoundingRect bounds)
        {
            boundingRect = bounds;
            leaves = new List<Entity>(CVars.Get<int>("quad_tree_max_references"));
            subNodes = new List<QuadTreeNode>(4);
        }

        public void AddReference(Entity entity)
        {
            if (!allNodes && leaves.Count < leaves.Capacity)
            {
                leaves.Add(entity);
                entity.GetComponent<QuadTreeReferenceComponent>().node = this;
            }
            else
            {
                if(!allNodes)
                {
                    subNodes.Add(new QuadTreeNode(new BoundingRect(boundingRect.Center.X - boundingRect.Width / 2,
                                                                    boundingRect.Center.Y - boundingRect.Height / 2,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2)));
                    subNodes.Add(new QuadTreeNode(new BoundingRect(boundingRect.Center.X + boundingRect.Width / 2,
                                                                    boundingRect.Center.Y - boundingRect.Height / 2,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2)));
                    subNodes.Add(new QuadTreeNode(new BoundingRect(boundingRect.Center.X - boundingRect.Width / 2,
                                                                    boundingRect.Center.Y + boundingRect.Height / 2,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2)));
                    subNodes.Add(new QuadTreeNode(new BoundingRect(boundingRect.Center.X + boundingRect.Width / 2,
                                                                    boundingRect.Center.Y + boundingRect.Height / 2,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2)));
                    allNodes = true;
                    AddReference(entity);
                    foreach(Entity e in leaves)
                    {
                        AddReference(e);
                    }
                }
                else
                {
                    foreach (QuadTreeNode qtn in subNodes)
                    {
                        if (qtn.boundingRect.Contains(entity.GetComponent<TransformComponent>().Position))
                        {
                            AddReference(entity);
                        }
                    }
                }
            }
        }
    }
}
