using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCoinCore
{
    public class Frack_Fixer
    {

        //  instance variables - replace the example below with your own
        FileUtils fileUtils;

        int totalValueToBank;

        int totalValueToFractured;

        int totalValueToCounterfeit;

        RAIDA raida;

        public Frack_Fixer(FileUtils fileUtils, int timeout)
        {
            this.fileUtils = fileUtils;
            this.raida = new RAIDA(timeout);
        }

        public int[] fixAll()
        {
            int[] results = new int[3];
            String[] frackedFileNames = Directory.GetFiles(this.fileUtils.frackedFolder);
            int totalValueToBank = 0;
            int totalValueToCounterfeit = 0;
            int totalValueToFractured = 0;
            CloudCoin newCC;
            for (int j = 0; (j < frackedFileNames.Length); j++)
            {
                try
                {
                    // System.out.println("Construct Coin: "+rootFolder + suspectFileNames[j]);
                    newCC = fileUtils.loadOneCloudCoinFromJsonFile(this.fileUtils.frackedFolder + frackedFileNames[j]);
                    Console.WriteLine("UnFracking SN #" + newCC.sn + ", Denomination: " + newCC.getDenomination());
                    CloudCoin fixedCC = this.raida.fixCoin(newCC);
                    // Will attempt to unfrack the coin. 
                    fixedCC.consoleReport();
                    switch (fixedCC.extension)
                    {
                        case "bank":
                            this.totalValueToBank++;
                            this.fileUtils.writeTo( this.fileUtils.bankFolder, fixedCC );
                            break;
                        case "fractured":
                            this.totalValueToFractured++;
                            this.fileUtils.writeTo( this.fileUtils.frackedFolder, fixedCC );
                            break;
                        case "counterfeit":
                            this.totalValueToCounterfeit++;
                            this.fileUtils.writeTo( this.fileUtils.counterfeitFolder, fixedCC );
                            break;
                    }
                    // end switch on the place the coin will go 
                    this.deleteCoin( this.fileUtils.frackedFolder + frackedFileNames[j] );
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine(ex);
                }
                catch (IOException ioex)
                {
                    Console.WriteLine(ioex);
                }

                // end try catch
            }

            // end for each coin to import
            // System.out.println("Results of Import:");
            results[0] = this.totalValueToBank;
            results[1] = this.totalValueToCounterfeit;
            // System.out.println("Counterfeit and Moved to trash: "+totalValueToCounterfeit);
            results[2] = this.totalValueToFractured;
            // System.out.println("Fracked and Moved to Fracked: "+ totalValueToFractured);
            return results;
        }// end fix all
 

        // End select all file names in a folder
        public bool deleteCoin(String path)
        {
            bool deleted = false;
         
            // System.out.println("Deleteing Coin: "+path + this.fileName + extension);
            try
            {
               File.Delete(path);
            }catch (Exception e) {
                Console.WriteLine( e );
            }

            return deleted;
        }
    }
    // End Frack Fixer  
}
