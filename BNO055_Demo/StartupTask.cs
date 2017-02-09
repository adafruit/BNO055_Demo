/*------------------------------------------------------------------------
  Adafruit Bno055 Demo for Windows Core IoT: Bno055 IMU chip.

  Written by Rick Lesniak for Adafruit Industries.

  Adafruit invests time and resources providing this open source code,
  please support Adafruit and open-source hardware by purchasing products
  from Adafruit!

  ------------------------------------------------------------------------
  This file is intended to work with the Adafruit Class Library

  This code is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

  MIT license, all text above must be included in any redistribution.
  ------------------------------------------------------------------------*/using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using AdafruitClassLibrary;
using System.Threading.Tasks;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace BNO055_Demo
{
    public sealed class StartupTask : IBackgroundTask
    {
        Bno055 bno055 { get; set; }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            bno055 = new Bno055();

            await bno055.InitBNO055Async(Bno055.OperationMode.OPERATION_MODE_NDOF, "UART0", 18);

            Bno055.SystemStatus sysStatus = bno055.GetSystemStatus(true);
            System.Diagnostics.Debug.WriteLine(string.Format("System Status: Status {0}, Self-Test {1}, Error {2}", sysStatus.StatusReg, sysStatus.SelfTestResult, sysStatus.ErrorReg));

            Bno055.Revisions revision = bno055.GetRevision();
            System.Diagnostics.Debug.WriteLine(string.Format("Revision: Software {0}, Bootloader {1}, Accel ID {2}, Mag ID {3}, Gyro ID {4}", revision.Software, revision.Bootloader, revision.AccelID, revision.MagID, revision.GyroID));
            Task.Delay(1000).Wait();

            while (true)
            {
                Bno055.Euler euler = bno055.ReadEuler();
                if (null != euler)
                    System.Diagnostics.Debug.WriteLine(string.Format("Euler: Pitch {0}, Roll {1}, Heading {2}", euler.Pitch, euler.Roll, euler.Heading));
                Task.Delay(1000).Wait();
            }
        }
    }
}
