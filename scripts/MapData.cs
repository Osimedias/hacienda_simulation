using Godot;
using System;
/*
    file: MapData.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:02 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    This script and the global scene is use to mantaing the data of the current selected map in SingleplayerScene.
*/

namespace Trinketos.HaciendaSimulator
{
    public partial class MapData : Node
    {
        public Texture2D heightmap;
        public Texture2D splatmap;
        public Texture2D watermask;
        public Texture2D treeDistMask;
    }
}