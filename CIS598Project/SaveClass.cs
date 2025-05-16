using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS598Project.Game_Entities;

namespace CIS598Project
{
    public class SaveClass
    {
        string fileName = Environment.CurrentDirectory + "\\savedata.txt";
        public void save(StringBuilder sr, Player player) 
        {
            using (StreamWriter sw = new StreamWriter(fileName, false)) // 'false' to overwrite the file
            {
                for (int j = 0; j < player.consecutivePlays.Length; j++)
                {
                    sr.Append(player.consecutivePlays[j] + "\n");
                }
                sr.Append(player.name + "\n");
                sr.Append(player.fruityMazeWins + "\n");
                sr.Append(player.ballpitPlays + "\n");
                sr.Append(player.ballpitTowerLosses + "\n");
                for (int j = 0; j < player.foundSecret.Length; j++)
                {
                    sr.Append(player.foundSecret[j] + "\n");
                }
                for (int j = 0; j < player.sawEnding.Length; j++)
                {
                    sr.Append(player.sawEnding[j] + "\n");
                }
                for (int j = 0; j < player.itemsUnlocked.Length; j++)
                {
                    for (int k = 0; k < player.itemsUnlocked[j].Length; k++)
                    {
                        sr.Append(player.itemsUnlocked[j][k] + "\n");
                    }
                }
                sr.Append(player.ticketAmount);
                string line = sr.ToString();
                sw.WriteLine(line);
                sr.Clear();
            }
        }
    }
}
