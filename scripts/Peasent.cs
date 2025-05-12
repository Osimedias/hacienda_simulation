using System;
using Godot;



namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class Peasent : Node3D
    {
        Thoughts[] thoughts;
        string WorkingIn = "Firepit";
        Vector3[] WorkingLoopPositions =  [];
        Agent agent;
        TransportNode transportNode;
        FiniteStateMachine finiteStateMachine;



        public override void _Ready()
        {
            base._Ready();
            agent = GetNode<Agent>("Agent");
            transportNode = GetNode<TransportNode>("TransportationNode");
            finiteStateMachine = GetNode<FiniteStateMachine>("FiniteStateMachine");
        }

        



        private Thoughts GetRandomThough()
        {
            Random random = new Random();
            int randomIndex = random.Next(thoughts.Length);
            return thoughts[randomIndex];
        }
    }
}