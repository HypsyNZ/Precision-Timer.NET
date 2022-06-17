﻿/*
*MIT License
*
*Copyright (c) 2022 S Christison
*
*Permission is hereby granted, free of charge, to any person obtaining a copy
*of this software and associated documentation files (the "Software"), to deal
*in the Software without restriction, including without limitation the rights
*to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
*copies of the Software, and to permit persons to whom the Software is
*furnished to do so, subject to the following conditions:
*
*The above copyright notice and this permission notice shall be included in all
*copies or substantial portions of the Software.
*
*THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
*IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
*FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
*AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
*LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
*OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
*SOFTWARE.
*/

using System;

namespace PrecisionTiming
{
    /// <summary>
    /// High Resolution Multimedia Timer Wrapper
    /// </summary>
    public class PrecisionTimer
    {
        /// <summary>
        /// Occurs when the <see cref="PrecisionTimer"/> has started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when the <see cref="PrecisionTimer"/> has stopped.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Set the Interval and Tick Event of the <see cref="PrecisionTimer"/> and decide if it should start automatically
        /// Start the timer with the default settings
        /// </summary>
        /// <param name="TimerTask">The Action</param>
        /// <param name="Interval">The Interval for the TimerTask in Milliseconds</param>
        /// <param name="Periodic">True if Periodic / False if OneShot</param>
        /// <param name="Start">True if the timer should start automatically with the default settings, false if you are going to configure/start it later</param>
        /// <param name="args">Optional user provided EventArgs</param>
        public void SetInterval(Action TimerTask, int Interval, bool Start = true, bool Periodic = true, EventArgs args = null)
        {
            Timer = new MMTimer();
            Timer.Tick += (sender, args) => { TimerTask(); };
            Timer.SetAutoReset = Periodic;
            Timer.SetPeriod = Interval;
            Timer.SetResolution = 0;
            Timer.SetArgs = args;

            if (Start)
            {
                Timer.Start();
            }
        }

        /// <summary>
        /// True if the Timer is running
        /// Will also be false if no <see cref="PrecisionTimer"/> is configured
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            return Timer != null ? Timer.IsRunning : false;
        }

        /// <summary>
        /// Start the <see cref="PrecisionTimer"/>
        /// </summary>
        public void Start(EventArgs args = null)
        {
            if (CheckTimerValid())
            {
                if (Timer.Start(args))
                {
                    if (Started is object)
                        Started(this, args);
                }
            }
        }

        /// <summary>
        /// Stop the <see cref="PrecisionTimer"/>
        /// </summary>
        public void Stop(EventArgs args = null)
        {
            if (Timer != null)
            {
                Timer.Stop();

                if (Stopped is object)
                    Stopped(this, args);
            }
        }

        /// <summary>
        /// Set the Action of the <see cref="PrecisionTimer"/> before you Start the Timer
        /// </summary>
        /// <param name="TimerTask">The Action</param>
        public void SetAction(Action TimerTask)
        {
            if (CheckTimerValid())
            {
                Timer.Tick += (sender, args) => { TimerTask(); };
            }
        }

        /// <summary>
        /// Set the Interval (Period) of the <see cref="PrecisionTimer"/> before you Start the Timer
        /// </summary>
        public void SetInterval(int Interval)
        {
            if (CheckTimerValid())
            {
                Timer.SetPeriod = Interval;
            }
        }

        /// <summary>
        /// Get the Interval (Period) of the <see cref="PrecisionTimer"/>
        /// </summary>
        public int GetInterval => Timer.GetPeriod;

        /// <summary>
        /// Set the Resolution of the <see cref="PrecisionTimer"/> before you Start the Timer
        /// <para>Default: 0</para>
        /// </summary>
        public void SetResolution(int Resolution)
        {
            if (CheckTimerValid())
            {
                Timer.SetResolution = Resolution;
            }
        }

        /// <summary>
        /// Get the Resolution of the <see cref="PrecisionTimer"/>
        /// <para>Default: 0</para>
        /// </summary>
        public int GetResolution => Timer.GetResolution;

        /// <summary>
        /// Set the Periodic/OneShot Mode of the <see cref="PrecisionTimer"/> before you Start the Timer
        /// <para>Default:True (Periodic)</para>
        /// </summary>
        public void SetPeriodic(bool periodic)
        {
            if (CheckTimerValid())
            {
                Timer.SetAutoReset = periodic;
            }
        }

        /// <summary>
        /// Set the Periodic/OneShot Mode of the <see cref="PrecisionTimer"/>
        /// <para>Default:True (Periodic)</para>
        /// </summary>
        public bool GetPeriodic => Timer.GetAutoReset;

        /// <summary>
        /// Set Event Args of the <see cref="PrecisionTimer"/> before you Start the Timer
        /// </summary>
        public void SetEventArgs(EventArgs args)
        {
            if (CheckTimerValid())
            {
                Timer.SetArgs = args;
            }
        }

        /// <summary>
        /// Get Event Args for the Timer
        /// </summary>
        public EventArgs GetEventArgs => Timer.GetArgs;

        /// <summary>
        /// Release all resources for this <see cref="PrecisionTimer"/>
        /// </summary>
        public void Dispose()
        {
            Timer?.Dispose();
        }

        internal volatile MMTimer Timer;

        internal bool CheckTimerValid()
        {
            if (Timer == null)
            {
                Timer = new MMTimer();
            }

            return true;
        }
    }
}
