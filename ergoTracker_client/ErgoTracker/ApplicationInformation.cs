﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErgoTracker
{
    class ApplicationInformation
    {
        private static ApplicationInformation instance;

        private string username;
        private string password;
        private bool trainingModeOn;
        private bool diagnosticModeOn;

        private ApplicationInformation()
        { setUsername(""); setPassword(""); trainingModeOn = false; diagnosticModeOn = false; }

        public string getUsername() { return this.username; }
        public string getPassword() { return this.password; }
        public bool isDiagnosticModeOn() { return this.diagnosticModeOn; }
        public bool isTrainingModeOn() { return this.trainingModeOn; }

        public void setUsername(string username) { this.username = username; }
        public void setPassword(string password) { this.password = password; }
        public void setDiagnosticMode(bool diagnosticModeOn) { this.diagnosticModeOn = diagnosticModeOn; }
        public void setTrainingMode(bool trainingModeOn) { this.trainingModeOn = trainingModeOn; }
        
        public static ApplicationInformation Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ApplicationInformation();
                }

                return instance;
            }
        }
    }
}
