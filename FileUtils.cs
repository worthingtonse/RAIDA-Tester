using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace CloudCoinCore
{
    public class FileUtils
    {
        //  instance variables
        public String rootFolder;
        public String importFolder;
        public String importedFolder;
        public String trashFolder;
        public String suspectFolder;
        public String frackedFolder;
        public String bankFolder;
        public String templateFolder;
        public String counterfeitFolder;
        public String directoryFolder;
        public String exportFolder;

        public FileUtils(String rootFolder, String importFolder, String importedFolder, String trashFolder, String suspectFolder, String frackedFolder, String bankFolder, String templateFolder, String counterfeitFolder, String directoryFolder, String exportFolder)
        {
            //  initialise instance variables
            this.rootFolder = rootFolder;
            this.importFolder = importFolder;
            this.importedFolder = importedFolder;
            this.trashFolder = trashFolder;
            this.suspectFolder = suspectFolder;
            this.frackedFolder = frackedFolder;
            this.bankFolder = bankFolder;
            this.templateFolder = templateFolder;
            this.counterfeitFolder = counterfeitFolder;
            this.directoryFolder = directoryFolder;
            this.exportFolder = exportFolder;
        }  // End constructor



        /*
         This loads a JSON file (.stack) from the hard drive that contains only one CloudCoin and turns it into an object. 
             */
        public CloudCoin loadOneCloudCoinFromJsonFile(String loadFilePath)
        {

            //Load file as JSON
            String incomeJson = this.importJSON(loadFilePath);
            //STRIP UNESSARY test
            int secondCurlyBracket = ordinalIndexOf(incomeJson, "{", 2) -1;
            int firstCloseCurlyBracket = ordinalIndexOf(incomeJson, "}", 0) - secondCurlyBracket;
            // incomeJson = incomeJson.Substring(secondCurlyBracket, firstCloseCurlyBracket);
            incomeJson = incomeJson.Substring( secondCurlyBracket, firstCloseCurlyBracket +1 );
           // Console.Out.WriteLine(incomeJson);
            //Deserial JSON
            SimpleCoin cc = JsonConvert.DeserializeObject<SimpleCoin>(incomeJson);
            //Make Coin
            String[] strAoid = cc.aoid.ToArray();
            Dictionary<string, string> aoid_dictionary = new Dictionary<string, string>();
            for (int j = 0; j < strAoid.Length; j++)
            { //"fracked=ppppppppppppppppppppppppp"
                if (strAoid[j].Contains("="))
                {//see if the string contains an equals sign
                    String[] keyvalue = strAoid[j].Split('=');
                    aoid_dictionary.Add(keyvalue[0], keyvalue[1]);//index 0 is the key index 1 is the value.
                }
                else
                { //There is something there but not a key value pair. Treak it like a memo
                    aoid_dictionary.Add("memo", strAoid[j]);
                }//end if cointains an = 
            }//end for each aoid


            CloudCoin returnCC = new CloudCoin(cc.nn, cc.sn, cc.an.ToArray(), cc.ed, aoid_dictionary, "suspect");
            for (int i = 0; (i < 25); i++)
            {//All newly created loaded coins get new PANs. 
                returnCC.pans[i] = returnCC.generatePan();
                returnCC.pastStatus[i] = "undetected";
            } // end for each pan
              //Return Coin

            returnCC.fileName = (returnCC.getDenomination() + (".CloudCoin." + (returnCC.nn + ("." + (returnCC.sn + ".")))));
            returnCC.json = "";
            returnCC.jpeg = null;

            return returnCC;
        }//end load one CloudCoin from JSON

        public CloudCoin loadOneCloudCoinFromJPEGFile(String loadFilePath)
        { // JPEG
            String wholeString = "";
            byte[] jpegHeader = ReadFile(loadFilePath);
            wholeString = this.toHexadecimal(jpegHeader);
            //  System.out.println(wholeString);
            CloudCoin returnCC = this.parseJpeg(wholeString);

            return returnCC;
        }//end load one CloudCoin from JSON



        public byte[] ReadFile(string filePath)
        {
            byte[] buffer = new byte[455];
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, 455 - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }//end Read File



        // end loadCloudCoin
        public String importJSON(String jsonfile)
        {
            String jsonData = "";
            String line;

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.

                using (StreamReader sr = new StreamReader( jsonfile ))
                {
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null)
                        {
                            break;
                        }//End if line is null
                        jsonData = (jsonData + line + "\n");
                    }//end while true
                }//end using
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file "+ jsonfile + " could not be read:");
                Console.WriteLine(e.Message);
            }
            return jsonData;
        }//end importJSON

        // en d json test
        public String setJSON(CloudCoin cc)
        {
            const string quote = "\"";
            const string tab = "t\\";
            String json = (tab + tab + "{ " + Environment.NewLine);// {
            json = json + tab + tab + quote + "nn" + quote + ":" + quote + cc.nn + quote + ", " + Environment.NewLine;// "nn":"1",
            json += tab + tab + quote + "sn" + quote + ":" + quote + cc.sn + quote + ", " + Environment.NewLine;// "sn":"367544",
            json += tab + tab + quote + "an" + quote + ": [" + quote;// "an": ["
            for (int i = 0; (i < 25); i++)
            {
                json += cc.ans[i];// 8551995a45457754aaaa44
                if (i == 4 || i == 9 || i == 14 || i == 19)
                {
                    json += quote + "," + Environment.NewLine + tab + tab + tab; //", 
                }
                else if (i == 24)
                {
                    // json += "\""; last one do nothing
                }
                else
                { // end if is line break
                    json += quote + ", " + quote;
                }

                // end else
            }// end for 25 ans

            json += quote + "]," + Environment.NewLine;//"],
            // End of ans
            json += tab + tab + quote + "ed" + quote + ":" + quote + "9-2016" + quote + "," + Environment.NewLine; // "ed":"9-2016",
            json += tab + tab + quote + "aoid" + quote + ": [";

            int count = 0;
            foreach (KeyValuePair<string, string> entry in cc.aoid)
            {
                if ((count != 0))
                {
                    json += ",";
                }

                json += quote + entry.Key + quote + "=" + quote + entry.Value + quote;
                count++;
            }//end for each

            json += "]" + Environment.NewLine;
            json += tab + tab + "}" + Environment.NewLine;
            // Keep expiration date when saving (not a truley accurate but good enought )
            return json;
        }

        // end get JSON
        public byte[] makeJpeg(CloudCoin cc)
        {
            byte[] returnBytes = null;
            // Make byte array from CloudCoin
            String cloudCoinStr = "";
            for (int i = 0; (i < 25); i++)
            {
                cloudCoinStr = (cloudCoinStr + cc.ans[i]);
            }

            // end for each an
            //  cloudCoinStr +="Defeat tyrants and obey God0"; //27 AOID and comments
            cloudCoinStr += "204f42455920474f4420262044454645415420545952414e54532000";
            // Hex for " OBEY GOD & DEFEAT TYRANTS "
            cloudCoinStr += "00";
            // LHC = 100%
            cloudCoinStr += "97E2";
            // 0x97E2;//Expiration date Sep. 2018
            cloudCoinStr += "01";
            //  cc.nn;//network number
            String hexSN = cc.sn.ToString("X6");
            String fullHexSN = "";
            switch (hexSN.Length)
            {
                case 1:
                    fullHexSN = ("00000" + hexSN);
                    break;
                case 2:
                    fullHexSN = ("0000" + hexSN);
                    break;
                case 3:
                    fullHexSN = ("000" + hexSN);
                    break;
                case 4:
                    fullHexSN = ("00" + hexSN);
                    break;
                case 5:
                    fullHexSN = ("0" + hexSN);
                    break;
                case 6:
                    fullHexSN = hexSN;
                    break;
            }
            cloudCoinStr = (cloudCoinStr + fullHexSN);

            switch (cc.getDenomination())
            {
                case 1:
                    returnBytes = Read500Bytes(this.templateFolder + "jpeg1.jpg");
                    break;
                case 5:
                    returnBytes = Read500Bytes(this.templateFolder + "jpeg5.jpg");
                    break;
                case 25:
                    returnBytes = Read500Bytes(this.templateFolder + "jpeg25.jpg");
                    break;
                case 100:
                    returnBytes = Read500Bytes(this.templateFolder + "jpeg100.jpg");
                    break;
                case 250:
                    returnBytes = Read500Bytes(this.templateFolder + "jpeg250.jpg");
                    break;
            }
            // end switch
            byte[] ccArray = this.hexStringToByteArray(cloudCoinStr);
            int offset = 20;
            //  System.out.println("ccArray length " + ccArray.length);
            for (int j = 0; (j < ccArray.Length); j++)
            {
                returnBytes[(offset + j)] = ccArray[j];
            }

            // end for each byte in the ccArray
            return returnBytes;
        }

        public byte[] Read500Bytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, 500);
            }
            return buffer;
        }


        // end get jpeg
        public void moveToTrashFolder(String fileName)
        {
            String source = (this.importFolder + fileName);
            String target = (this.trashFolder + fileName);
            File.Move(source, target);
        }

        public void moveToImportedFolder(String fileName)
        {
            String source = (this.importFolder + fileName);
            String target = (this.importedFolder + fileName);
            File.Move(source, target);
        }

        public void writeToSuspectFolder(String fileName)
        {
            String source = (this.importFolder + fileName);
            String target = (this.suspectFolder + fileName);
            File.Move(source, target);
        }




        // end if file Exists
        public String loadFileToString(String filePath)
        {
            string text;
            using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }

            return text;
        } // end load file to string

        public int IndexOfNth( string str, string value, int nth )
        {
            if (nth <= 0)
                throw new ArgumentException("Can not find the zeroth index of substring in string. Must start with 1");
            int offset = str.IndexOf(value);
            for (int i = 0; i < nth; i++)
            {
                if (offset == -1) return -1;
                offset = str.IndexOf(value, offset + 1);
            }
            return offset;
        }//end IndexOfNth


        // end string to file 
        private String toHexadecimal(byte[] digest)
        {
            return BitConverter.ToString(digest);
        }

        public bool writeTo(String folder, CloudCoin cc)
        {
            const string quote = "\"";
            const string tab = "t\\";

            bool goodSave = false;
            String json = this.setJSON(cc);
            if (!File.Exists(folder + cc.fileName + ".stack"))
            {
                using (FileStream fs = File.Create(folder + cc.fileName + ".stack"))
                {
                    String wholeJson = "{" + Environment.NewLine; //{
                    wholeJson += tab + quote + "cloudcoin" + quote + ": [" + Environment.NewLine; // "cloudcoin" : [
                    wholeJson += json;
                    wholeJson += tab + Environment.NewLine;
                    wholeJson += "}";
                    File.WriteAllText(folder + cc.fileName + ".stack", wholeJson);
                    goodSave = true;
                    return goodSave;
                }
            }
            else
            {
                Console.WriteLine("A coin with that SN already exists in the folder.");
                return goodSave;
            }//File Exists
        }

        private CloudCoin parseJpeg(String wholeString)
        {
            CloudCoin cc = new CloudCoin();
            int startAn = 40;
            int endAn = 72;
            for (int i = 0; (i < 25); i++)
            {
                cc.ans[i] = wholeString.Substring((startAn
                                + (i * 32)), (endAn
                                + (i * 32)));
                //  System.out.println(i +": " +ans[i]);
            }

            // end for
            cc.aoid = null;
            // wholeString.substring( 840, 895 );
            cc.hp = 25;
            // Integer.parseInt(wholeString.substring( 896, 896 ), 16);
            cc.ed = wholeString.Substring(898, 902);
            cc.nn = Convert.ToInt32(wholeString.Substring(902, 904), 16);
            cc.sn = Convert.ToInt32(wholeString.Substring(904, 910), 16);
            for (int i = 0; (i < 25); i++)
            {
                cc.pans[i] = cc.generatePan();
                cc.pastStatus[i] = "undetected";
            }

            // end for each pan
            return cc;
        }// end parse Jpeg



        // en d json test
        public byte[] hexStringToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }//End hex string to byte array

       public int ordinalIndexOf(String str, String substr, int n)
        {
            int pos = str.IndexOf(substr);
            while (--n > 0 && pos != -1)
                pos = str.IndexOf(substr, pos + 1);
            return pos;
        }



    }//end class


}//End namespace
