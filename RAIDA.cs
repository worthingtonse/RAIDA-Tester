using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace CloudCoinCore
{
    public class RAIDA
    {//  instance variables
        public DetectionAgent[] agent;
        public int milliSecondsToTimeOut;
        public CloudCoin returnCoin;
        private String ticketStatus0 = "empty";
        private String ticketStatus1 = "empty";
        private String ticketStatus2 = "empty";

        private int working_nn;
        private int working_sn; 
        private String[] working_ans;
        private int working_getDenomination;
        private int[] working_triad = {0,1,2};//place holder



        public RAIDA(int milliSecondsToTimeOut)
        { //  initialise instance variables
            this.milliSecondsToTimeOut = milliSecondsToTimeOut;
            this.agent = new DetectionAgent[25];
            for (int i = 0; (i < 25); i++)
            {
                this.agent[i] = new DetectionAgent(i, milliSecondsToTimeOut);
            } // end for each Raida
        }//End Constructor

    
        public CloudCoin detectCoin(CloudCoin cc)
        {
            returnCoin = cc;
            ThreadStart detectThread0Start = new ThreadStart(detectThread0);
            ThreadStart detectThread1Start = new ThreadStart(detectThread1);
            ThreadStart detectThread2Start = new ThreadStart(detectThread2);
            ThreadStart detectThread3Start = new ThreadStart(detectThread3);
            ThreadStart detectThread4Start = new ThreadStart(detectThread4);
            ThreadStart detectThread5Start = new ThreadStart(detectThread5);
            ThreadStart detectThread6Start = new ThreadStart(detectThread6);
            ThreadStart detectThread7Start = new ThreadStart(detectThread7);
            ThreadStart detectThread8Start = new ThreadStart(detectThread8);
            ThreadStart detectThread9Start = new ThreadStart(detectThread9);
            ThreadStart detectThread10Start = new ThreadStart(detectThread10);
            ThreadStart detectThread11Start = new ThreadStart(detectThread1);
            ThreadStart detectThread12Start = new ThreadStart(detectThread12);
            ThreadStart detectThread13Start = new ThreadStart(detectThread13);
            ThreadStart detectThread14Start = new ThreadStart(detectThread14);
            ThreadStart detectThread15Start = new ThreadStart(detectThread15);
            ThreadStart detectThread16Start = new ThreadStart(detectThread16);
            ThreadStart detectThread17Start = new ThreadStart(detectThread17);
            ThreadStart detectThread18Start = new ThreadStart(detectThread18);
            ThreadStart detectThread19Start = new ThreadStart(detectThread19);
            ThreadStart detectThread20Start = new ThreadStart(detectThread20);
            ThreadStart detectThread21Start = new ThreadStart(detectThread21);
            ThreadStart detectThread22Start = new ThreadStart(detectThread22);
            ThreadStart detectThread23Start = new ThreadStart(detectThread23);
            ThreadStart detectThread24Start = new ThreadStart(detectThread24);

            Thread[] detectThread = new Thread[25];
            detectThread[0] = new Thread(detectThread0Start);
            detectThread[1] = new Thread(detectThread1Start);
            detectThread[2] = new Thread(detectThread2Start);
            detectThread[3] = new Thread(detectThread3Start);
            detectThread[4] = new Thread(detectThread4Start);
            detectThread[5] = new Thread(detectThread5Start);
            detectThread[6] = new Thread(detectThread6Start);
            detectThread[7] = new Thread(detectThread7Start);
            detectThread[8] = new Thread(detectThread8Start);
            detectThread[9] = new Thread(detectThread9Start);
            detectThread[10] = new Thread(detectThread10Start);
            detectThread[11] = new Thread(detectThread11Start);
            detectThread[12] = new Thread(detectThread12Start);
            detectThread[13] = new Thread(detectThread13Start);
            detectThread[14] = new Thread(detectThread14Start);
            detectThread[15] = new Thread(detectThread15Start);
            detectThread[16] = new Thread(detectThread16Start);
            detectThread[17] = new Thread(detectThread17Start);
            detectThread[18] = new Thread(detectThread18Start);
            detectThread[19] = new Thread(detectThread19Start);
            detectThread[20] = new Thread(detectThread20Start);
            detectThread[21] = new Thread(detectThread21Start);
            detectThread[22] = new Thread(detectThread22Start);
            detectThread[23] = new Thread(detectThread23Start);
            detectThread[24] = new Thread(detectThread24Start);

            foreach (Thread myThread in detectThread)
            {
                myThread.Start();
                if (!myThread.Join(TimeSpan.FromSeconds(milliSecondsToTimeOut)))
                {
                    myThread.Abort();
                }//End if timeout. 
            }//End start each thread for each

            returnCoin.setAnsToPansIfPassed();
            returnCoin.calculateHP();
            returnCoin.gradeCoin();
            // sets the grade and figures out what the file extension should be (bank, fracked, counterfeit, lost
            returnCoin.calcExpirationDate();
            returnCoin.grade();
            return returnCoin;
        }//end detectCoin

        public void detectThread0()
        {
            returnCoin.pastStatus[0] = agent[0].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[0], returnCoin.pans[0], returnCoin.getDenomination());
        }
        public void detectThread1()
        {
            returnCoin.pastStatus[1] = agent[1].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[1], returnCoin.pans[1], returnCoin.getDenomination());
        }
        public void detectThread2()
        {
            returnCoin.pastStatus[2] = agent[2].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[2], returnCoin.pans[2], returnCoin.getDenomination());
        }
        public void detectThread3()
        {
            returnCoin.pastStatus[3] = agent[3].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[3], returnCoin.pans[3], returnCoin.getDenomination());
        }
        public void detectThread4()
        {
            returnCoin.pastStatus[4] = agent[4].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[4], returnCoin.pans[4], returnCoin.getDenomination());
        }
        public void detectThread5()
        {
            returnCoin.pastStatus[5] = agent[5].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[5], returnCoin.pans[5], returnCoin.getDenomination());
        }
        public void detectThread6()
        {
            returnCoin.pastStatus[6] = agent[6].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[6], returnCoin.pans[6], returnCoin.getDenomination());
        }
        public void detectThread7()
        {
            returnCoin.pastStatus[7] = agent[7].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[7], returnCoin.pans[7], returnCoin.getDenomination());
        }
        public void detectThread8()
        {
            returnCoin.pastStatus[8] = agent[8].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[8], returnCoin.pans[8], returnCoin.getDenomination());
        }
        public void detectThread9()
        {
            returnCoin.pastStatus[9] = agent[9].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[9], returnCoin.pans[9], returnCoin.getDenomination());
        }
        public void detectThread10()
        {
            returnCoin.pastStatus[10] = agent[10].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[10], returnCoin.pans[10], returnCoin.getDenomination());
        }
        public void detectThread11()
        {
            returnCoin.pastStatus[11] = agent[11].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[11], returnCoin.pans[11], returnCoin.getDenomination());
        }
        public void detectThread12()
        {
            returnCoin.pastStatus[12] = agent[12].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[12], returnCoin.pans[12], returnCoin.getDenomination());
        }
        public void detectThread13()
        {
            returnCoin.pastStatus[13] = agent[13].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[13], returnCoin.pans[13], returnCoin.getDenomination());
        }
        public void detectThread14()
        {
            returnCoin.pastStatus[14] = agent[14].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[14], returnCoin.pans[14], returnCoin.getDenomination());
        }
        public void detectThread15()
        {
            returnCoin.pastStatus[15] = agent[15].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[15], returnCoin.pans[15], returnCoin.getDenomination());
        }
        public void detectThread16()
        {
            returnCoin.pastStatus[16] = agent[16].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[16], returnCoin.pans[16], returnCoin.getDenomination());
        }
        public void detectThread17()
        {
            returnCoin.pastStatus[17] = agent[17].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[17], returnCoin.pans[17], returnCoin.getDenomination());
        }
        public void detectThread18()
        {
            returnCoin.pastStatus[18] = agent[18].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[18], returnCoin.pans[18], returnCoin.getDenomination());
        }
        public void detectThread19()
        {
            returnCoin.pastStatus[19] = agent[19].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[19], returnCoin.pans[19], returnCoin.getDenomination());
        }
        public void detectThread20()
        {
            returnCoin.pastStatus[20] = agent[20].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[20], returnCoin.pans[20], returnCoin.getDenomination());
        }
        public void detectThread21()
        {
            returnCoin.pastStatus[21] = agent[21].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[21], returnCoin.pans[21], returnCoin.getDenomination());
        }
        public void detectThread22()
        {
            returnCoin.pastStatus[22] = agent[22].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[22], returnCoin.pans[22], returnCoin.getDenomination());
        }
        public void detectThread23()
        {
            returnCoin.pastStatus[23] = agent[23].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[23], returnCoin.pans[23], returnCoin.getDenomination());
        }
        public void detectThread24()
        {
            returnCoin.pastStatus[24] = agent[24].detect(returnCoin.nn, returnCoin.sn, returnCoin.ans[24], returnCoin.pans[24], returnCoin.getDenomination());
        }






        public CloudCoin fixCoin(CloudCoin brokeCoin)
        {
            returnCoin = brokeCoin;
            returnCoin.setAnsToPans();
            DateTime before = DateTime.Now;

            String fix_result = "";
            FixitHelper fixer;
            String[] trustedServerAns;
            String[] tickets = { "error", "error", "error" };
            int corner = 1;

            // Make sure we set the RAIDA to the cc ans and not new pans. 
            // For every guid, check to see if it is fractured
            for (int guid_id = 0; (guid_id < 25); guid_id++)
            {
                Console.Out.WriteLine("Inspecting RAIDA guid " + guid_id);
                if (returnCoin.pastStatus[guid_id] == "fail")
                { // This guid has failed, get tickets 
                    Console.Out.WriteLine("RAIDA" + guid_id + " failed.");
                    fixer = new FixitHelper(guid_id, returnCoin.ans);

                    trustedServerAns = new String[] { returnCoin.ans[fixer.currentTriad[0]], returnCoin.ans[fixer.currentTriad[1]], returnCoin.ans[fixer.currentTriad[2]] };
                    while (!fixer.finnished)
                    {
                        fix_result = "";
                        tickets = this.get_tickets(fixer.currentTriad, trustedServerAns, returnCoin.nn, returnCoin.sn, returnCoin.getDenomination());
                        // See if there are errors in the tickets                  
                        if (tickets[0] == "error" || tickets[2] == "error" || tickets[2] == "error")
                        {// No tickets, go to next triad corner 
                            Console.Out.WriteLine("Get ticket commands failed for guid " + guid_id);
                            corner++;
                            fixer.setCornerToCheck(corner);
                        }
                        else
                        {
                            // Has three good tickets   
                            fix_result = this.agent[guid_id].fix(fixer.currentTriad, tickets[0], tickets[1], tickets[2], returnCoin.ans[guid_id]);
                            if (fix_result == "success")
                            {
                                Console.Out.WriteLine("GUID fixed for guid " + guid_id);
                                returnCoin.pastStatus[guid_id] = "pass";
                                fixer.finnished = true;
                            }
                            else
                            { // command failed,  need to try another corner
                                corner++;
                                fixer.setCornerToCheck(corner);
                            }//end if else fix was successful
                        }//end if else one of the tickts has an error. 
                    }//end while fixer not finnihsed. 
                }// end if guid is fail
            }//end for all the guids
            DateTime after = DateTime.Now;
            TimeSpan ts = after.Subtract(before);
            Console.WriteLine("It took this many ms to fix the guid: " + ts.Milliseconds);

            returnCoin.calculateHP();
            returnCoin.gradeCoin();
            // sets the grade and figures out what the file extension should be (bank, fracked, counterfeit, lost
            returnCoin.calcExpirationDate();
            returnCoin.grade();
            return returnCoin;
        }// end fix coin


        public String[] get_tickets(int[] triad, String[] ans, int nn, int sn, int denomination)
        {
            string[] returnTicketsStatus = { "error", "error", "error" };

            working_nn = nn;
            working_sn = sn;
            working_ans = ans;
            working_getDenomination = denomination;
            working_triad = triad;

            ThreadStart detectThread0Start = new ThreadStart(fixThread0);
            ThreadStart detectThread1Start = new ThreadStart(fixThread1);
            ThreadStart detectThread2Start = new ThreadStart(fixThread2);


            Thread[] fixThread = new Thread[3];
            fixThread[0] = new Thread(detectThread0Start);
            fixThread[1] = new Thread(detectThread1Start);
            fixThread[2] = new Thread(detectThread2Start);


            foreach (Thread myThread in fixThread)
            {
                myThread.Start();
                if (!myThread.Join(TimeSpan.FromSeconds(milliSecondsToTimeOut)))
                {
                    myThread.Abort();
                }//End if timeout. 
            }//End start each thread for each

            returnTicketsStatus[0] = agent[working_triad[0]].lastTicketStatus;
            returnTicketsStatus[1] = agent[working_triad[1]].lastTicketStatus;
            returnTicketsStatus[2] = agent[working_triad[2]].lastTicketStatus;

            return returnTicketsStatus;
        }//end detectCoin


        public void fixThread0()
        {
            agent[working_triad[0]].get_ticket(working_nn, working_sn, working_ans[0], working_getDenomination );
            Console.Out.WriteLine(agent[working_triad[0]].lastRequest);
            Console.Out.WriteLine();
            if (agent[working_triad[0]].lastResponse.Contains("fail")) { Console.ForegroundColor = ConsoleColor.Red; } else { Console.ForegroundColor = ConsoleColor.Green; }
            Console.Out.WriteLine(agent[working_triad[0]].lastResponse);
            Console.Out.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void fixThread1()
        {
            agent[working_triad[1]].get_ticket(working_nn, working_sn, working_ans[1], working_getDenomination);
            Console.Out.WriteLine(agent[working_triad[1]].lastRequest);
            Console.Out.WriteLine();
            if (agent[working_triad[1]].lastResponse.Contains("fail")) { Console.ForegroundColor = ConsoleColor.Red; } else { Console.ForegroundColor = ConsoleColor.Green; }
            Console.Out.WriteLine(agent[working_triad[1]].lastResponse);
            Console.Out.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void fixThread2()
        {
            agent[working_triad[2]].get_ticket(working_nn, working_sn, working_ans[2], working_getDenomination);
            Console.Out.WriteLine(agent[working_triad[2]].lastRequest);
            Console.Out.WriteLine();
            if (agent[working_triad[2]].lastResponse.Contains("fail")) { Console.ForegroundColor = ConsoleColor.Red; } else { Console.ForegroundColor = ConsoleColor.Green; }
            Console.Out.WriteLine(agent[working_triad[2]].lastResponse);
            Console.Out.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }


    }//end class
}//end namespace
