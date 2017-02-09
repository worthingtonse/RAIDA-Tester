using System;
using System.Net;
using System.Text;
using System.IO;

namespace CloudCoinCore
{
    public class DetectionAgent
    {

        // instance variables
        /**
         * Time to allow the Detection Agent to respond before marking them as "error"
         */
        public int readTimeout;//how many mili seconds before saying fuck it, we are done waiting. 
                               /**
                                   * 0-24. RAIDA0 will be 0
                                */
        public int RAIDANumber;
        /**
         * The url of the detection agent's service including folder such as https://RADIA9.cloudcoin.global/service/
         */
        public String fullUrl;

        /**
         * The status of the last detection request (pass, fail, error or notdetected)
         */
        public String lastDetectStatus = "notdetected";//error, notdetected, pass, fail
                                                       /**
                                                        * Detection Milliseconds. How many milliseconds did it take to do the last operation
                                                        */
        public long dms = 0; //ms to detect

        //Ticket Status
        /**
         * The last "message" or ticket that was recieved from a ticket request
         */
        public String lastTicket = "empty";
        /**
         * Status returned from the last ticket request: "ticket" (Received a ticked), "fail" (Failed to authenticate
         */
        public String lastTicketStatus = "empty";//ticket, fail, error

        //Fix it status
        /**
         * Status returned from the last fix request: "success" (unit fixed) "fail" or "error"
         */
        public String lastFixStatus = "empty";// fail, error

        //General 
        /**
         * Last HTTPS GET request sent like: https://raida8.cloudcoin.global/service/echo
         */
        public String lastRequest = "empty";//Last GET command sent to RAIDA
                                            /**
                                             * Last HTTPS recieved from server. Will be in JSON form. 
                                             */
        public String lastResponse = "empty";//LAST JSON recieved from the RAIDA

        /**
         * DetectionAgent Constructor
         *
         * @param readTimeout A parameter that determines how many milliseconds each request will be allowed to take
         * @param RAIDANumber The number of the RAIDA server 0-24
         */
        public DetectionAgent(int RAIDANumber, int readTimeout)
        {
            this.RAIDANumber = RAIDANumber;
            this.fullUrl = "https://RAIDA" + RAIDANumber + ".cloudcoin.global/service/";
            this.readTimeout = readTimeout;
        }//Detection Agent Constructor




        public String echo(int raidaID)
        {
            this.lastRequest = this.fullUrl + "echo?b=t";
            // System.out.println(this.lastRequest);
            DateTime before = DateTime.Now;
            try
            {
                this.lastResponse = getHtml(this.lastRequest);
            }
            catch (Exception ex)
            {
                lastDetectStatus = "error";
                this.lastResponse += ex;
                return "error";
            }
            DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
            this.dms = ts.Milliseconds;
            return this.lastResponse;
        }//end detect


        /**
         * Method detect
         *
         * @param nn An int thatis the coin's Network Number 
         * @param sn An int that is the coin's Serial Number
         * @param an A String that is the coin's Authenticity Number (GUID)
         * @param pan A String that is the Proposed Authenticity Number to replace the AN.
         * @param d An int that is the Denomination of the Coin
         * @return Returns pass, fail or error. 
         */
        public String detect(int nn, int sn, String an, String pan, int d)
        {
            this.lastRequest = this.fullUrl + "detect?nn=" + nn + "&sn=" + sn + "&an=" + an + "&pan=" + pan + "&denomination=" + d +"&b=t";
            // System.out.println(this.lastRequest);
            DateTime before = DateTime.Now;
            try
            {
                this.lastResponse = getHtml(this.lastRequest);
            }
            catch (Exception ex)
            {
                lastDetectStatus = "error";
                return "error";
            }
            DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
            this.dms = ts.Milliseconds;
            if (this.lastResponse.Contains("pass"))
            {
                lastDetectStatus = "pass";
                return "pass";
            }
            else if (this.lastResponse.Contains("fail") && this.lastResponse.Length < 200)//less than 200 incase their is a fail message inside errored page
            {
                lastDetectStatus = "fail";
                return "fail";
            }
            else
            {
                lastDetectStatus = "error";
                return "error";
            }
        }//end detect

 
        public String get_ticket(int nn, int sn, String an, int d)
        { //Will only use ans to fix
            this.lastRequest = fullUrl + "get_ticket?nn=" + nn + "&sn=" + sn + "&an=" + an + "&pan=" + an + "&denomination=" + d;
            DateTime before = DateTime.Now;
            this.lastResponse = getHtml(this.lastRequest);

            if (this.lastResponse.Contains("ticket"))
            {
                String[] KeyPairs = this.lastResponse.Split(',');
                String message = KeyPairs[3];
                int startTicket = ordinalIndexOf(message, "\"", 3) + 2;
                int endTicket = ordinalIndexOf(message, "\"", 4) - startTicket;
                this.lastTicket = message.Substring(startTicket -1, endTicket + 1);
                this.lastTicketStatus = "ticket";
                DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
                this.dms = ts.Milliseconds;
                return this.lastTicket;
            }//end if
            return "error";
        }//end get ticket

        public String testHint( int nn, int sn, String an, int d)
        {
            Console.Out.WriteLine("Checking ticket...");
            get_ticket( nn, sn, an, d);
            Console.Out.WriteLine("Last ticket is: " + lastTicket);
            if ( lastTicketStatus == "ticket")
            {
                this.lastRequest = fullUrl + "hints?rn=" + this.lastTicket;         
                DateTime before = DateTime.Now;
                this.lastResponse = getHtml(this.lastRequest);
                DateTime after = DateTime.Now;
                TimeSpan ts = after.Subtract(before);
                this.dms = ts.Milliseconds;
                return lastTicketStatus;
            }
            else
            {//Getting ticket failed
                return "Get_ticket failed on this RAIDA so hints could not be checked.";
            }//end if ticket was got
        }//End test hints



        /**
         * Method fix
         *
         * @param triad The three trused server RAIDA numbers
         * @param m1 The ticket from the first trusted server
         * @param m2 The ticket from the second trusted server
         * @param m3 The ticket from the third trusted server
         * @param pan The proposed authenticity number (to replace the wrong AN the RAIDA has)
         * @return The status sent back from the server: sucess, fail or error. 
         */
        public String fix(int[] triad, String m1, String m2, String m3, String pan)
        {

            this.lastFixStatus = "error";
            int f1 = triad[0];
            int f2 = triad[1];
            int f3 = triad[2];
            this.lastRequest = fullUrl + "fix?fromserver1=" + f1 + "&message1=" + m1 + "&fromserver2=" + f2 + "&message2=" + m2 + "&fromserver3=" + f3 + "&message3=" + m3 + "&pan=" + pan;
            //System.out.println(this.lastRequest);

            try
            {
                DateTime before = DateTime.Now;

                this.lastResponse = getHtml(this.lastRequest);

                DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
                this.dms = ts.Milliseconds;
                // System.out.println(this.lastResponse + " " + this.dms );
            }
            catch (Exception ex)
            {//quit
             //  System.out.println(ex + " " +this.lastResponse);
                return "error";
            }
           

            if (this.lastResponse.Contains("success"))
            {
                this.lastFixStatus = "success";
                return "success";
            }

            return "error";
        }//end fixit


        /**
         * Method getHtml download a webpage or a web service
         *
         * @param url_in The URL to be downloaded
         * @return The text that was downloaded
         */
        private String getHtml(String urlAddress)
        {
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            request.ContinueTimeout = 2000;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    data = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return data;
        }//end get HTML

   

        /**
         * Method ordinalIndexOf used to parse cloudcoins. Finds the nth number of a character within a string
         *
         * @param str The string to search in
         * @param substr What to count in the string
         * @param n The nth number
         * @return The index of the nth number
         */
        public int ordinalIndexOf(string str, string substr, int n)
        {
            int pos = str.IndexOf(substr);
            while (--n > 0 && pos != -1)
            {
                pos = str.IndexOf(substr, (pos + 1));
            }
            return pos;
        }//end ordinal Index of

    }//End class Detection Agent
}//end namespace
