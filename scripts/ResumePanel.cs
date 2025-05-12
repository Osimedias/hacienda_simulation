using Godot;
using System;

public partial class ResumePanel : CenterContainer
{
	[Export]
	Tree ScorePanel;
	[Export]
	Tree BuildingsPanel;
	[Export]
	Tree UnitsPanel;
	[Export]
	Tree ResourcePanel;
	[Export]
	Tree MarketPanel;
	[Export]
	Tree OthersPanel;
	[Export]
	Control DiagramsPanel;

	string[] scoreHeaders = ["Player Name","Total Score","Economic Score","Military Score","Explore Score"];
	string[] buidingsHeaders = ["Player Name","Labor","Food and Farms","Civic","Military","Hacienda","Defence"];
	string[] unitsHeaders = ["Player Name","Peasents","Militaries","Faborite Military Unit"];
	string[] resourceHeaders = ["Player Name","Stockpile","Granery","Kichen","Fonda"];
	string[] marketHeaders = ["Player Name","Money by selling","Lose money by buying","Total money from Market"];
	string[] othersHeaders = [];

	public override void _Ready()
	{
		PopulateTreeWhitItems(scoreHeaders,ScorePanel);
		PopulateTreeWhitItems(buidingsHeaders,BuildingsPanel);
		PopulateTreeWhitItems(unitsHeaders,UnitsPanel);
		PopulateTreeWhitItems(resourceHeaders,ResourcePanel);
		PopulateTreeWhitItems(marketHeaders,MarketPanel);
		PopulateTreeWhitItems(othersHeaders,OthersPanel);
	}



	private void PopulateTreeWhitItems(string[] headers,Tree tree)
	{
		TreeItem root = tree.CreateItem();
		tree.HideRoot = true;
		tree.Columns = 10;
		for (int i = 0; i < headers.Length; i++)
		{
			TreeItem item = tree.CreateItem(root);
			item.SetText(0,headers[i]);
		}
	}


	public void OnContinuePressed()
	{
		Hide();
	}
}
