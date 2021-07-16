using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using FortalezaDesktop.Models;

namespace FortalezaDesktop.Utils
{
    class ErrorHandler
    {
        public static void ApiGenericErrorHandler(ApiException e)
        {
            string errorMessage = "("+ e.TargetSite + ") Error " + e.StatusCode.ToString() + ": " + e.Message;
            MessageBox.Show(errorMessage, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            Logger.Log(errorMessage + " Stack:" + e.StackTrace, Logger.LogType.Error);
        }
    }
}
