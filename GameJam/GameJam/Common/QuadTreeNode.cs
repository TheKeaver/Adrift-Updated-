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
            subNodes = new List<QuadTreeNode>(CVars.Get<int>("quad_tree_max_references"));
        }

        public void AddReference(Entity entity)
        {
            if (!allNodes && leaves.Count == leaves.Capacity)
            {
                leaves.Add(entity);
            }
            else
            {
                if(!allNodes)
                {
                    subNodes[0] = new QuadTreeNode(new BoundingRect(boundingRect.Center.X - boundingRect.Width / 2,
                                                                    boundingRect.Center.Y - boundingRect.Height / 2,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2));
                    subNodes[1] = new QuadTreeNode(new BoundingRect(boundingRect.Center.X + boundingRect.Width / 2,
                                                                    boundingRect.Center.Y - boundingRect.Height / 2,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2));
                    subNodes[2] = new QuadTreeNode(new BoundingRect(boundingRect.Center.X - boundingRect.Width / 2,
                                                                    boundingRect.Center.Y + boundingRect.Height / 2,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2));
                    subNodes[3] = new QuadTreeNode(new BoundingRect(boundingRect.Center.X + boundingRect.Width / 2,
                                                                    boundingRect.Center.Y + boundingRect.Height / 2,
                                                                    boundingRect.Width / 2,
                                                                    boundingRect.Height / 2));
                    allNodes = true;
                    AddReference(entity);
                    AddReference(leaves[0]);
                    AddReference(leaves[1]);
                    AddReference(leaves[2]);
                    AddReference(leaves[3]);
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
