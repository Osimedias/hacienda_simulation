using Godot;
using System;

namespace Trinketos.HaciendaSimulator
{
    public partial class Actor : Node3D
    {
        [Export]
        private BoneAttachment3D[] BonesAttachments;

        [Export]
        private AnimationTree animationTree;



        public void SetAnimationTransition(string animationName)
        {
            animationTree.Set("parameters/Transition/transition_request", animationName);
        }

        public void SetObjectToBoneAttachment(int index, string scene)
        {
            PackedScene obj = GD.Load<PackedScene>(scene);
            Node3D newObj = obj.Instantiate() as Node3D;
            BonesAttachments[index].AddChild(newObj);
        }
    }
}
