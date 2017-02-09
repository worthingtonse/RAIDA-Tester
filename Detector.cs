using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCoinCore
{
    public class Detector
    {

        //  instance variables - replace the example below with your own
        RAIDA raida;

        FileUtils fileUtils;

        public Detector(FileUtils fileUtils, int timeout)
        {
            this.raida = new RAIDA(timeout);
            this.fileUtils = fileUtils;
        }

        // end Detect constructor
        public int[] detectAll()
        {
            // LOAD THE .suspect COINS ONE AT A TIME AND TEST THEM
            int[] results = new int[3];
            // [0] Coins to bank, [1] Coins to fracked [1] Coins to Counterfeit
            String[] suspectFileNames = Directory.GetFiles(this.fileUtils.suspectFolder);
            int totalValueToBank = 0;
            int totalValueToCounterfeit = 0;
            int totalValueToFractured = 0;
            CloudCoin newCC;
            for (int j = 0; (j < suspectFileNames.Length); j++)
            {
                try
                {
                    // System.out.println("Construct Coin: "+rootFolder + suspectFileNames[j]);
                    newCC = this.fileUtils.loadOneCloudCoinFromJsonFile(this.fileUtils.suspectFolder + suspectFileNames[j]);
                    Console.Out.WriteLine("");
                    Console.Out.WriteLine("");
                    Console.Out.WriteLine("Detecting SN #" + newCC.sn + ", Denomination: " + newCC.getDenomination() );
                    CloudCoin detectedCC = this.raida.detectCoin(newCC);
                    // Checks all 25 GUIDs in the Coin and sets the status. 
                    // detectedCC.saveCoin( detectedCC.extension );//save coin as bank
                    detectedCC.consoleReport();
                    switch (detectedCC.extension)
                    {
                        case "bank":
                            totalValueToBank++;
                            this.fileUtils.writeTo( this.fileUtils.bankFolder, detectedCC );
                            break;
                        case "fractured":
                            totalValueToFractured++;
                            this.fileUtils.writeTo(this.fileUtils.frackedFolder, detectedCC);
                            break;
                        case "counterfeit":
                            totalValueToCounterfeit++;
                            this.fileUtils.writeTo(this.fileUtils.counterfeitFolder, detectedCC);
                            break;
                    }
                    // end switch on the place the coin will go 
                    File.Delete(this.fileUtils.suspectFolder + suspectFileNames[j]);
                }
                catch (FileNotFoundException ex)
                {
                    Console.Out.WriteLine(ex);
                }
                catch (IOException ioex)
                {
                    Console.Out.WriteLine(ioex);
                }// end try catch
            }// end for each coin to import
            // System.out.println("Results of Import:");
            results[0] = totalValueToBank;
            results[1] = totalValueToCounterfeit;
            // System.out.println("Counterfeit and Moved to trash: "+totalValueToCounterfeit);
            results[2] = totalValueToFractured;
            // System.out.println("Fracked and Moved to Fracked: "+ totalValueToFractured);
            return results;
        }
    }
}
