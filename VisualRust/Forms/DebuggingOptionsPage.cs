﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using VisualRust.Forms;
using VisualRust.Project;

namespace VisualRust.Options
{
    [ComVisible(true)]
    [Guid("93F42A39-0AF6-40EE-AE2C-1C44AB5F8B15")]
    public partial class DebuggingOptionsPage : DialogPage
    {
        public bool UseCustomGdbPath { get; set; }
        public string DebuggerLocation { get; set; }
        public string ExtraArgs { get; set; }

        private IWin32Window page;

        protected override IWin32Window Window
        {
            get
            {
                if(page != null)
                    return page;
                if(Feature.Gdb(this.Site))
                    page = new DebuggingOptionsPageControl(this);
                else
                    page = new DebuggingDisabledControl();
                return page;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            var debugControl = page as DebuggingOptionsPageControl;
            if(debugControl != null)
                debugControl.LoadSettings(this);
            base.OnClosed(e);
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            var debugControl = page as DebuggingOptionsPageControl;
            if (e.ApplyBehavior == ApplyKind.Apply && debugControl != null)
                debugControl.ApplySettings(this);
            base.OnApply(e);
        }
    }
}
