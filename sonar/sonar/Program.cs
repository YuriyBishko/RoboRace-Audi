using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.Hardware;
using GHIElectronics.NETMF.FEZ;

namespace sonar
{
    public class Program
    {
        private static int ticks;

        private static InterruptPort EchoPin = new InterruptPort((Cpu.Pin)FEZ_Pin.Digital.Di7, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);
        private static OutputPort TriggerPin = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di6, false);

        public static void Main()
        {
            EchoPin.OnInterrupt += new NativeEventHandler(EchoPin_OnInterrupt);
            EchoPin.DisableInterrupt();
            while (true)
            {
                Distance();
             //   Debug.Print("distance = " + myDistance + " mm.");
                Thread.Sleep(1000);
            }
        }

       /* private static void EchoPin_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            //throw new NotImplementedException();
        }*/

        public static void Distance()
        {
            EchoPin.EnableInterrupt();
            TriggerPin.Write(false);
            Thread.Sleep(2);
            TriggerPin.Write(true);
            Thread.Sleep(10);
            TriggerPin.Write(false);
            Thread.Sleep(2);
        }

        private static void EchoPin_OnInterrupt(uint port, uint state, DateTime time)
        {
       
            if (state == 0) // falling edge, end of pulse
            {
              
                int pulseWidth = (int)time.Ticks - ticks;
                // valid for 20A°C
                int pulseWidthMilliSeconds = pulseWidth * 10 / 582;
                //valid for 24A°C
                //int pulseWidthMilliSeconds = (pulseWidth * 10 / (int)578.29);
                Debug.Print("Distance = " + pulseWidthMilliSeconds + " millimA?tres.");
              
            }
            else
            {
                ticks = (int)time.Ticks;
            }
            EchoPin.ClearInterrupt();
        }

    }
}