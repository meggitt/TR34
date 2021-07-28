using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace TR34RemoteKeyloading
{
    /// <summary>
    /// 
    /// </summary>
    public class TR34KeyLoad
    {
	
        private Timer unbindTimer = new Timer();

        /// <summary>
        /// Event handler for EPP Unbind Operation.
        /// </summary>
        private void EppUnboundEventHandler(object sender, CertificateUnboundEventArgs eventArgs)
        {
			// un-subcribe all unbind opeartion
            UnsubscribeUnbindEppEvents();

            if (eventArgs != null)
            {
				// if Unbind is succesful then timer will be started with 
				// specified time period. here in this logic we used 10 as the value.
				// Once specified time is elapsed then elapsed-event will be fired
                if (eventArgs.Result == 0)
                {                   
					unbindTimer.Interval = Convert.ToInt32(10) * 1000;
					unbindTimer.Elapsed += UnbindTimer_Elapsed;
					unbindTimer.Enabled = true;			   
				}
            }            
        }

		// Event handler for Timer-Elapsed event.
		// Once timer is elapsed the send response to host with EPP is unbound state. 
        private void UnbindTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SendResponse("I", "EPPUNBOUND");
        }

        /// <summary>
        /// Handler for Bind EPP successful event.
        /// </summary>
		/// Once EPP is bind with Host, then it will stop timer
		/// and not sending any subsequent messages.
        private void CertificateBoundEventHandler(object sender, CertificateBoundEventArgs eventArgs)
        {
            unbindTimer.Enabled = false;
            unbindTimer.Elapsed -= UnbindTimer_Elapsed;
        }

    }
}
